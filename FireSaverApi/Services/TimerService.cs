using System;
using System.Collections.Generic;
using System.Linq;
using FireSaverApi.Contracts;
using FireSaverApi.Helpers.SceduledTaskManager;

namespace FireSaverApi.Services
{
    class UserFailedTestKey
    {
        public int userId { get; set; }
        public int testId { get; set; }

        public override int GetHashCode()
        {
            return userId.GetHashCode() ^ testId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is UserFailedTestKey)
            {
                UserFailedTestKey compositeKey = (UserFailedTestKey)obj;

                return ((this.userId == compositeKey.userId) &&
                        (this.testId == compositeKey.testId));
            }

            return false;
        }
    }

    public class TimerService : ITimerService
    {
        Dictionary<UserFailedTestKey, Action> userFailedTestList;
        Dictionary<UserFailedTestKey, int> userFailedTestCount;

        Scheduler blockUserScheduler;
        public TimerService()
        {
            userFailedTestList = new Dictionary<UserFailedTestKey, Action>();
            userFailedTestCount = new Dictionary<UserFailedTestKey, int>();

            blockUserScheduler = new Scheduler();
        }


        public void IncreaseFailedUserTestFaledCount(int userId, int testId, int testFailedThreshold)
        {
            UserFailedTestKey userTestKey = new UserFailedTestKey()
            {
                testId = testId,
                userId = userId
            };

            if (userFailedTestList.ContainsKey(userTestKey))
                throw new Exception("Use is already blocked");

            if (!userFailedTestCount.ContainsKey(userTestKey))
            {
                userFailedTestCount.Add(userTestKey, 1);
            }
            else
            {
                userFailedTestCount[userTestKey]++;
            }


            if (userFailedTestCount[userTestKey] >= testFailedThreshold)
            {
                AddFailedTestUsers(userTestKey);
            }

        }

        private void AddFailedTestUsers(UserFailedTestKey userTestKey)
        {
            Action releasingAction = () =>
            {
                DeleteFailures(userTestKey);
            };

            int fiveMinutesInMilliseconds = 300000;

            blockUserScheduler.Execute(releasingAction, fiveMinutesInMilliseconds);

            if (!userFailedTestList.ContainsKey(userTestKey))
            {
                userFailedTestList.Add(userTestKey, releasingAction);
            }
        }

        public void ClearUSerFailedTest(int userId, int testId)
        {
            UserFailedTestKey userTestKey = new UserFailedTestKey()
            {
                testId = testId,
                userId = userId
            };

            DeleteFailures(userTestKey);
        }

        private void DeleteFailures(UserFailedTestKey userTestKey)
        {
            userFailedTestList.Remove(userTestKey);
            userFailedTestCount.Remove(userTestKey);
        }

        public List<int> GetFailedTestUsers()
        {
            return userFailedTestList.Keys.Select(k => k.userId).ToList();
        }

        public bool IsUserForTestBanned(int userId, int testId)
        {
            return userFailedTestList.Keys.Where(k => k.testId == testId).Select(k => k.userId).ToList().Contains(userId);
        }
    }
}