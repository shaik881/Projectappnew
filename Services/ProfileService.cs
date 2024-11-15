using DefaultIdentityColumnRename.Data;
using DefaultIdentityColumnRename.Models;

namespace DefaultIdentityColumnRename.Services
{
    public class ProfileService
    {
        private readonly ApplicationDbContext _context;
        public ProfileService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<byte[]> ConvertToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public UserProfile FindUserProfile(User user)
        {
            var u = _context.UserProfiles.Where(x=>x.UserId.Equals(user.Id)).FirstOrDefault();
            if (u == null)
                return null;
            return u;
        }
        public void Login(UserProfile profile)
        {
            _context.UserProfiles.Add(profile);
            Console.WriteLine(_context.SaveChanges());
        }
        public async Task<bool> UpdateProfile(UserProfile profile)
        {
            var user = _context.UserProfiles.Where(x => x.UserId.Equals(profile.UserId)).FirstOrDefault();
            if (user == null)
                Console.WriteLine("user is null");
            user.Pincode = profile.Pincode;
            user.Address = profile?.Address;
            user.Pic = profile?.Pic;
            user.UserId = profile?.UserId;
            user.State = profile?.State;
            user.Gender = profile?.Gender;
             _context.Update(user);
            if(await _context.SaveChangesAsync()!=0)
            {
                return true;
            }
            return false;
        }
    }
}
