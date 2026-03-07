using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        public AuthenticationService(UserManager<ApplicationUser> UserManager)
        {
            _UserManager = UserManager;
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest req)
        {
            var user = req.Adapt<ApplicationUser>();
            var result = await _UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return new RegisterResponse() { Success = false, Message = "error" };
            }
            return new RegisterResponse() { Success = true, Message = "success" };


        }
    }

}