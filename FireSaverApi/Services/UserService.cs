using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Helpers;
using FireSaverApi.Helpers.ExceptionHandler.CustomExceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FireSaverApi.Services
{
    public class UserService : IUserService, IAuthUserService
    {
        private readonly DatabaseContext context;
        private readonly IMapper mapper;
        private readonly AppSettings appSettings;

        public UserService(DatabaseContext context,
                            IMapper mapper,
                            IOptions<AppSettings> appsettings)
        {
            this.appSettings = appsettings.Value;
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<UserInfoDto> CreateNewUser(RegisterUserDto newUserInfo)
        {
            User newUser = mapper.Map<User>(newUserInfo);

            newUser.RolesList = UserRole.AUTHORIZED_USER;
            newUser.Password = ComputeSha256Hash(newUser.Password);

            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();

            return mapper.Map<UserInfoDto>(newUser);
        }

        public async Task<UserContextInfo> GetUserContext(int userId)
        {
            try
            {
                var user = await GetUserById(userId);
                var allUserInfo = mapper.Map<UserInfoDto>(user);
                var contextUseInfo = mapper.Map<UserContextInfo>(allUserInfo);
                return contextUseInfo;
            }
            catch (UserNotFoundException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserInfoDto> GetUserInfoById(int userId)
        {
            var user = await GetUserById(userId);
            return mapper.Map<UserInfoDto>(user);
        }

        public async Task<UserInfoDto> UpdateUserInfo(UserInfoDto newUserInfo)
        {
            var user = await GetUserById(newUserInfo.Id);
            user = mapper.Map<User>(newUserInfo);
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return mapper.Map<UserInfoDto>(user);

        }

        public async Task<UserAuthResponseDto> AuthUser(AuthUserDto userAuth)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Mail == userAuth.Mail);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            var cntxUserPassword = user.Password;
            var cachedUserInputPassword = ComputeSha256Hash(userAuth.Password);
            if (cachedUserInputPassword != user.Password)
            {
                throw new WrongPasswordException();
            }

            var authToken = generateJwtToken(user);
            
            return new UserAuthResponseDto()
            {
                Token = authToken,
                UserId = user.Id
            };
        }

        async Task<User> GetUserById(int userId)
        {
            var foundUser = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (foundUser == null)
            {
                throw new UserNotFoundException();
            }

            return foundUser;
        }

        string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                     new Claim("id", user.Id.ToString()),
                     new Claim(ClaimTypes.Role, user.RolesList)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}