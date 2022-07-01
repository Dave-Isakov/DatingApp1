namespace API.Interfaces
{
    public interface IMessageRepository
    {
        Task<Group> GetGroupForConnection(string connectionId);
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername);
    }
}