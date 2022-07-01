using AutoMapper.QueryableExtensions;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups.Include(c => c.Connections).Where(c => c.Connections.Any(x => x.ConnectionID == connectionId))
            .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
                    .Include(u => u.Sender)
                    .Include(u => u.Recipient)
                    .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups.Include(x => x.Connections).FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages.OrderByDescending(message => message.MessageSent).ProjectTo<MessageDTO>(_mapper.ConfigurationProvider).AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(user => user.RecipientUsername == messageParams.Username && user.RecipientDeleted == false),
                "Outbox" => query.Where(user => user.SenderUsername == messageParams.Username && user.SenderDeleted == false),
                _ => query.Where(user => user.RecipientUsername == messageParams.Username && user.DateRead == null && user.RecipientDeleted == false)
            };

            return await PagedList<MessageDTO>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var messages = await _context.Messages
                .Include(user => user.Sender).ThenInclude(photo => photo.Photos)
                .Include(user => user.Recipient).ThenInclude(photo => photo.Photos)
                .Where(message => 
                    message.Recipient.UserName == currentUsername && message.Sender.UserName == recipientUsername && message.RecipientDeleted == false
                    || message.Recipient.UserName == recipientUsername && message.Sender.UserName == currentUsername && message.SenderDeleted == false)
                .OrderBy(message => message.MessageSent)
                .ProjectTo<MessageDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var unreadMessages = messages.Where(message => message.DateRead == null && message.RecipientUsername == currentUsername).ToList();

            if(unreadMessages.Any())
            {
                foreach(var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }
            }

            return messages;
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }
    }
}