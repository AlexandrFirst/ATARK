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
    public class UserService : IUserService, IAuthUserService, IUserHelper
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
            newUser.Password = HashHelper.ComputeSha256Hash(newUser.Password);

            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();

            return mapper.Map<UserInfoDto>(newUser);
        }

        public async Task<HttpUserContext> GetUserContext(int userId)
        {
            try
            {
                var user = await GetUserById(userId);
                var allUserInfo = mapper.Map<UserInfoDto>(user);
                var contextUseInfo = mapper.Map<HttpUserContext>(allUserInfo);
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

            if (compareInputAndUserPasswords(userAuth.Password,user.Password))
            {

                var authToken = generateJwtToken(user);

                return new UserAuthResponseDto()
                {
                    Token = authToken,
                    UserId = user.Id
                };
            }
            else
            {
                throw new WrongPasswordException();
            }
        }

        public async Task ChangeOldPassword(int userId, NewUserPasswordDto newUserPassword)
        {
            var user = await GetUserById(userId);

            if (compareInputAndUserPasswords(newUserPassword.OldPassword, user.Password))
            {
                var hashedNewPassword = HashHelper.ComputeSha256Hash(newUserPassword.NewPassword);
                user.Password = hashedNewPassword;
                await context.SaveChangesAsync();
            }
            else
            {
                throw new InorrectOldPasswordException();
            }
        }

        public async Task<User> GetUserById(int userId)
        {
            var foundUser = await context.Users.Include(b => b.ResponsibleForBuilding).FirstOrDefaultAsync(u => u.Id == userId);
            if (foundUser == null)
            {
                throw new UserNotFoundException();
            }

            return foundUser;
        }

        bool compareInputAndUserPasswords(string inputPassword, string userPassword)
        {
            var hashedInputPassword = HashHelper.ComputeSha256Hash(inputPassword);
            return hashedInputPassword == userPassword;
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