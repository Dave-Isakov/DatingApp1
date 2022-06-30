using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DataContext _context;
        public PhotoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Photo> GetPhotoById(int id)
        {
            return await _context.Photos.IgnoreQueryFilters().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PhotoForApprovalDTO>> GetUnapprovedPhotos()
        {
            return await _context.Photos.Where(p => p.IsApproved == false).Select(user => new PhotoForApprovalDTO
            {
                Id = user.Id,
                Url = user.Url,
                Username = user.AppUser.UserName,
                IsApproved = user.IsApproved
            }).IgnoreQueryFilters().ToListAsync();
        }

        public void RemovePhoto(Photo removedPhoto)
        {
            _context.Photos.Remove(removedPhoto);
        }
    }
}