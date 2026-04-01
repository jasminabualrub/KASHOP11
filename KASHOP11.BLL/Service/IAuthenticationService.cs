using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest req);
        Task<LoginResponse> LoginAsync(LoginRequest req);
        Task<bool> ConfirmedEmailAsync(string token, string userid);
        Task<ForgotPasswordResponse> RequestPasswordReset(ForgotPasswordRequest request);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest req);
    }
}