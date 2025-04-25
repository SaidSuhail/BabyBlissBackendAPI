using AutoMapper;
using BabyBlissBackendAPI.Data;
using BabyBlissBackendAPI.Dto;
using BabyBlissBackendAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace BabyBlissBackendAPI.Services.UserServices
{
    public class UserService:IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserViewDto>>ListUsers()
        {
            try
            {
                var users = await _context.users.Where(c => c.Role != User.UserRole.Admin).ToListAsync();
                var user = _mapper.Map<List<UserViewDto>>(users);
                return user;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserViewDto>GetUser(int id)
        {
            try
            {
                var user = await _context.users.FirstOrDefaultAsync(u => u.Id == id &&  u.Role != User.UserRole.Admin);
                var use = _mapper.Map<UserViewDto>(user);
                return use;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BlockUnblockDto>BlockUnblockUser(int id)
        {
            var user = await _context.users.FirstOrDefaultAsync(a => a.Id == id);
            if(user  == null)
            {
                throw new Exception("User not found");
            }
            user.IsBlocked = !user.IsBlocked;
            await _context.SaveChangesAsync();

            return new BlockUnblockDto
            {
                isBlocked = user.IsBlocked,
                Msg = user.IsBlocked ? "User id blocked" : "User Unblocked"
            };
        }
    }
}
