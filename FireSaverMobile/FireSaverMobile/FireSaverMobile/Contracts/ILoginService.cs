using FireSaverMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Contracts
{
    public interface ILoginService
    {
        Task<AuthentificationResponse> AuthUser(AuthentificationInput authInput);
        Task<AuthentificationResponse> AuthGuest();
        Task<AuthentificationResponse> ReadDataFromStorage();
        Task<bool> CheckTokenValidity();
        Task Logout();
        void ClearStorage();

    }
}
