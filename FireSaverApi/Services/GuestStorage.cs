using System.Collections.Generic;
using System.Threading.Tasks;
using FireSaverApi.Contracts;

namespace FireSaverApi.Services
{
    public class GuestStorage : IGuestStorage
    {
        List<int> guestIds;
        public GuestStorage()
        {
            guestIds = new List<int>();
        }

        public Task AddGuest(int userId)
        {
            guestIds.Add(userId);
            return Task.CompletedTask;
        }

        public Task RemoveGuest(int userId)
        {
            guestIds.Remove(userId);
            return Task.CompletedTask;
        }
    }
}