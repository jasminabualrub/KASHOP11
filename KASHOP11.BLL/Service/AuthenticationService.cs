using Azure.Core;
using KASHOP11.DAL.DTO.Request;
using KASHOP11.DAL.DTO.Response;
using KASHOP11.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IEmailSender _EmailSender;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpcontextaccessor;

        public AuthenticationService(UserManager<ApplicationUser> UserManager, IEmailSender EmailSender, IConfiguration configuration, IHttpContextAccessor httpcontextaccessor)
        {
            _UserManager = UserManager;
            _EmailSender = EmailSender;
            _configuration = configuration;
            _httpcontextaccessor = httpcontextaccessor;
        }

        public async Task<bool> ConfirmedEmailAsync(string token, string userId)
        {
            var user = await _UserManager.FindByIdAsync(userId);
            if (user == null) return false;
            // var result = await _UserManager.ConfirmEmailAsync(user, token);
            token = Uri.UnescapeDataString(token);
            var result = await _UserManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return false;
            return true;
        }


        public async Task<LoginResponse> LoginAsync(LoginRequest req)
        {
            var user = await _UserManager.FindByEmailAsync(req.Email);
            if (user is null)
            {
                return new LoginResponse() { Success = false, Message = "InvalidEmail" };
            }
            if (!await _UserManager.IsEmailConfirmedAsync(user))
                return new LoginResponse() { Success = false, Message = "Email is not confirmed" };
            var result = await _UserManager.CheckPasswordAsync(user, req.Password);
            if (!result)
                return new LoginResponse() { Success = false, Message = "Invaliadpassward" };
            return new LoginResponse() { Success = true, Message = "success", AcessToken = await GenerateAccessToken(user) };

        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest req)
        {
            var user = req.Adapt<ApplicationUser>();
            var result = await _UserManager.CreateAsync(user, req.Password);
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Code);
                Console.WriteLine(error.Description);
            }
           
            if (!result.Succeeded)
            {
                return new RegisterResponse()
                {
                    Success = false,
                    Message = "error",
                    Errors = result.Errors.Select(p => p.Description).ToList()
                };
            }


            await _UserManager.AddToRoleAsync(user, "User");
            var token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);
            var confirmationLink = $"{_httpcontextaccessor.HttpContext.Request.Scheme}://{_httpcontextaccessor.HttpContext.Request.Host}/api/Account/ConfirmEmail?userId={user.Id}&token={token}";
            await _EmailSender.SendEmailAsync(
                req.Email,
                "Welcome",
                $"<h1>Welcome {req.UserName}</h1>" +
                $"<a href='{confirmationLink}'>Confirm Email</a>"
            );
            return new RegisterResponse() { Success = true, Message = "success" };


        }
        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email
                ),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
       issuer: _configuration["Jwt:Issuer"],
       audience: _configuration["Jwt:Audience"],
       claims: userClaims,
       expires: DateTime.Now.AddMinutes(15),
       signingCredentials: credentials
       );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<ForgotPasswordResponse> RequestPasswordReset(ForgotPasswordRequest request)
        {
            var user = await _UserManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ForgotPasswordResponse()
                {
                    Success = false,
                    Message = " Email Not Found "
                };
            }
            var random = new Random();
           var code= random.Next(1000, 9999).ToString();
            user.CodeResetPassword = code;
            user.PasswordResetExpire =DateTime.UtcNow.AddMinutes(15);
            await _UserManager.UpdateAsync(user);
            await _EmailSender.SendEmailAsync(request.Email, "resetpassword", $"<p>Code Is{code}</p>");

            return new ForgotPasswordResponse()
            {
                Success = true,
                Message = " code sent to your Email "
            };
        }
    public async Task <ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest req)
        {
            var user = await _UserManager.FindByEmailAsync(req.Email);
            if (user is null)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = " Email Not Found "
                }; }
            else if (user.CodeResetPassword != req.Code)
            {

                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = " Invalid Code "
                };
            }
            else if (user.PasswordResetExpire < DateTime.UtcNow)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "  Code expired"
                };
            }
           var isSamedPassword= await _UserManager.CheckPasswordAsync(user, req.NewPassword);
            if (isSamedPassword)
            {
                return new ResetPasswordResponse()
                {
                    Success = false,
                    Message = "  new password must be different from the old password"
                };
            }
            var token = await _UserManager.GeneratePasswordResetTokenAsync(user);
           var result= await _UserManager.ResetPasswordAsync(user,token , req.NewPassword);
            if (!result.Succeeded)
            {
                return new ResetPasswordResponse() {
                    Success = false,
                    Message = "   password reset failed"
                };

            }
       
            await _EmailSender.SendEmailAsync(req.Email, "changepassword",$"<p> your password is changed</p>");

            return new ResetPasswordResponse()
            {
                Success = true,
                Message = "   password reset successfully"
            };
        }
        
    }
    } 