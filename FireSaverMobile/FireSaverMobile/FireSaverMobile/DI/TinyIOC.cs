using FireSaverMobile.Contracts;
using FireSaverMobile.Services;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Text;

namespace FireSaverMobile.DI
{
    public class TinyIOC
    {
        private static TinyIoCContainer _container;
        public static TinyIoCContainer Container 
        {
            get 
            {
                if(_container == null)
                    _container = new TinyIoCContainer();
                return _container; 
            }
        }

        public static void InitContainer() 
        {
            Container.Register<ILoginService, LoginService>().AsSingleton();
            Container.Register<IUserService, UserService>().AsSingleton();
        }
    }
}
