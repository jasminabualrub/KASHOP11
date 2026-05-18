using KASHOP11.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public interface IUserManagerService
    {
        public Task<List<UserListResponse>> GetAllUsers();
        public Task<UserDetailsResponse?> GetUser(string userId);
        public Task<bool> ChangeRole(string userId,string role);
        public Task<bool> ToggleBlockUser(string userId);
        public Task<bool> DeleteUser(string userId);
    }
}
