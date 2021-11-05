using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FireSaverApi.Helpers
{
    public class GuestsCleaner : IHostedService, IDisposable
    {
        private Timer timer = null;
        private readonly IServiceScopeFactory scopeFactory;
        private IServiceScope scope;

        public GuestsCleaner(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public void Dispose()
        {
            timer?.Dispose();
            scope?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            scope = scopeFactory.CreateScope();
            IAuthUserService authService = scope.ServiceProvider.GetRequiredService<IAuthUserService>();

            timer = new Timer(async o =>
                  {
                      var allGuests = await authService.GetAllGuests();
                      foreach (var guest in allGuests)
                      {
                          var currentTime = DateTime.Now;
                          TimeSpan timeDiff = currentTime - guest.DOB;
                          if (timeDiff.Hours >= 24)
                          {
                              await authService.LogoutGuest(guest.Id);
                          }
                      }

                  }, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}