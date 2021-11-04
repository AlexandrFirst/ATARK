using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Dtos.IoTDtos;
using FireSaverApi.Dtos.TestDtos;
using FireSaverApi.Helpers;
using FireSaverApi.Helpers.ExceptionHandler.CustomExceptions;
using FireSaverApi.hub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FireSaverApi.Services
{
    public class UserService : IUserService, IAuthUserService, IUserHelper
    {
        private readonly DatabaseContext context;
        private readonly IMapper mapper;
        private readonly IOptions<AppSettings> appsettings;
        private readonly ILocationService locationService;
        private readonly IRoutebuilderService routebuilderService;
        private readonly ITestService testService;
        private readonly ISocketService socketService;
        private readonly IGuestStorage guestStorage;
        private readonly AppSettings appSettings;

        public UserService(DatabaseContext context,
                            IMapper mapper,
                            IOptions<AppSettings> appsettings,
                            ILocationService locationService,
                            IRoutebuilderService routebuilderService,
                            ITestService testService,
                            ISocketService socketService,
                            IGuestStorage guestStorage)
        {
            this.appSettings = appsettings.Value;
            this.context = context;
            this.mapper = mapper;
            this.appsettings = appsettings;
            this.locationService = locationService;
            this.routebuilderService = routebuilderService;
            this.testService = testService;
            this.socketService = socketService;
            this.guestStorage = guestStorage;
        }
        public async Task<UserInfoDto> CreateNewUser(RegisterUserDto newUserInfo, string Role)
        {
            User newUser = mapper.Map<User>(newUserInfo);

            newUser.RolesList = Role;
            newUser.Password = CalcHelper.ComputeSha256Hash(newUser.Password);

            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();

            return mapper.Map<UserInfoDto>(newUser);
        }

        public async Task<MyHttpContext> GetUserContext(int userId)
        {
            try
            {
                var user = await GetUserById(userId);
                var allUserInfo = mapper.Map<UserInfoDto>(user);
                var contextUseInfo = mapper.Map<MyHttpContext>(allUserInfo);
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

        public async Task<AuthResponseDto> AuthUser(AuthUserDto userAuth)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Mail == userAuth.Mail);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (compareInputAndUserPasswords(userAuth.Password, user.Password))
            {

                // var authToken = generateJwtTokenForAuthUsers(user);
                var authToken = TokenGenerator.generateJwtToken(user.Id, TokenGenerator.UserJWTType, user.RolesList, appSettings.Secret);

                return new AuthResponseDto()
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

        public async Task<AuthResponseDto> AuthGuest()
        {
            var guestUser = new RegisterUserDto()
            {
                DOB = DateTime.Now,
                Mail = "",
                Name = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString(),
                Patronymic = "",
                RolesList = new List<string>(),
                Surname = "",
                TelephoneNumber = "",
            };

            var response = await CreateNewUser(guestUser, UserRole.GUEST);

            await guestStorage.AddGuest(response.Id);

            var token = TokenGenerator.generateJwtToken(response.Id, TokenGenerator.UserJWTType, UserRole.GUEST, appSettings.Secret);
            return new AuthResponseDto()
            {
                Token = token,
                UserId = response.Id
            };
        }

        public async Task LogoutGuest(int guestId)
        {
            var userToLogout = await context.Users.FirstOrDefaultAsync(u => u.Id == guestId);
            if (userToLogout != null)
            {
                await guestStorage.RemoveGuest(guestId);
                context.Remove(userToLogout);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new UserNotFoundException();
            }
        }

        public async Task ChangeOldPassword(int userId, NewUserPasswordDto newUserPassword)
        {
            var user = await GetUserById(userId);

            if (compareInputAndUserPasswords(newUserPassword.OldPassword, user.Password))
            {
                var hashedNewPassword = CalcHelper.ComputeSha256Hash(newUserPassword.NewPassword);
                user.Password = hashedNewPassword;
                await context.SaveChangesAsync();
            }
            else
            {
                throw new InorrectOldPasswordException();
            }
        }


        public async Task<List<RoutePoint>> BuildEvacuationRootForCompartment(int userId)
        {
            var user = await GetUserById(userId);

            if (user.CurrentCompartment == null)
            {
                throw new Exception("Current compartment for user is not set");
            }

            var compartmentPoints = await context.RoutePoints.Include(p => p.MapPosition)
                                                             .Where(p => p.Compartment.Id == user.CurrentCompartment.Id)
                                                             .ToListAsync();

            if (compartmentPoints.Count == 0)
            {
                throw new Exception("No points for the compartment found");
            }

            var worldPosition = mapper.Map<PositionDto>(user.LastSeenBuildingPosition);
            var mappedWorldPostion = await locationService.WorldToImgPostion(worldPosition, user.CurrentCompartment.Id);

            RoutePoint closestPoint = GetClosestPoint(compartmentPoints, mappedWorldPostion);

            var rootPointFotCurrentRoutePoint = await routebuilderService.GetRootPointForRoutePoint(closestPoint.Id);

            var exitPoints = compartmentPoints.Where(p => p.RoutePointType == RoutePointType.EXIT ||
                                                          p.RoutePointType == RoutePointType.ADDITIONAL_EXIT).ToList();
            if (exitPoints.Count == 0)
            {
                return new List<RoutePoint>() { await routebuilderService.GetAllRoute(rootPointFotCurrentRoutePoint.Id) };
            }
            else
            {
                var possibleEvacuationList = new List<RoutePoint>();
                for (int i = 0; i < exitPoints.Count; i++)
                {
                    var evacuationRoute = await routebuilderService.GetRouteBetweenPoints(closestPoint.Id, exitPoints[i].Id);
                    possibleEvacuationList.Add(evacuationRoute);
                }
                return possibleEvacuationList;
            }

        }

        private RoutePoint GetClosestPoint(List<RoutePoint> compartmentPoints, PositionDto mappedWorldPostion)
        {
            double minDistance = double.MaxValue;
            var closestPoint = compartmentPoints.First();

            for (int i = 0; i < compartmentPoints.Count; i++)
            {
                var currentPointPostion = mapper.Map<PositionDto>(compartmentPoints[i].MapPosition);
                double distance = CalcHelper.ComputeDistanceBetweenPoints(mappedWorldPostion, currentPointPostion);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = compartmentPoints[i];
                }
            }

            return closestPoint;
        }

        public async Task<User> GetUserById(int userId)
        {
            var foundUser = await context.Users.Include(b => b.ResponsibleForBuilding)
                                               .Include(c => c.CurrentCompartment)
                                               .Include(p => p.LastSeenBuildingPosition)
                                               .FirstOrDefaultAsync(u => u.Id == userId);
            if (foundUser == null)
            {
                throw new UserNotFoundException();
            }

            return foundUser;
        }

        public async Task<TestOutputDto> EnterCompartmentById(int userId, int compartmentId, int? iotId)
        {
            var compartment = await context.Compartment
                            .Include(t => t.CompartmentTest)
                            .ThenInclude(q => q.Questions)
                            .FirstOrDefaultAsync(c => c.Id == compartmentId);

            if (compartment.CompartmentTest != null)
            {
                var testToComplete = await testService.GetTestInfo(compartment.CompartmentTest.Id);
                return mapper.Map<TestOutputDto>(testToComplete);
            }
            else
            {
                var user = await GetUserById(userId);
                //send to iot if there is such an open signal
                if (iotId != null)
                {
                    await socketService.OpenDoorWithIot(iotId.Value);
                }

                user.CurrentCompartment = compartment;
                context.Update(user);
                await context.SaveChangesAsync();

                return null;
            }
        }

        public async Task SetAlaramForBuilding(int userId)
        {
            var user = await GetUserById(userId);
            if (user.ResponsibleForBuilding != null)
            {
                await socketService.SetAlarmForBuilding(user.ResponsibleForBuilding.Id);
            }
            else
            {
                throw new Exception("Illegal action");
            }
        }

        bool compareInputAndUserPasswords(string inputPassword, string userPassword)
        {
            var hashedInputPassword = CalcHelper.ComputeSha256Hash(inputPassword);
            return hashedInputPassword == userPassword;
        }


    }
}