using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos.TestDtos;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Services
{
    public class TestService : ITestService
    {
        private readonly DatabaseContext dataContext;
        private readonly IMapper mapper;
        private readonly ICompartmentHelper compartmentHelper;
        private readonly ITimerService timerService;
        private readonly IUserContextService userContextService;
    

        public TestService(DatabaseContext dataContext,
                           IMapper mapper,
                           ICompartmentHelper compartmentHelper,
                           ITimerService timerService,
                           IUserContextService userContextService)
        {
            this.compartmentHelper = compartmentHelper;
            this.timerService = timerService;
            this.userContextService = userContextService;
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<TestInputDto> AddTestToCompartment(int compartmentId, TestInputDto newTestInfo)
        {

            var compartment = await compartmentHelper.GetCompartmentById(compartmentId);

            var testToAdd = await AddTestToDb(newTestInfo);
            await AddTestToCompartment(compartment, testToAdd);

            return mapper.Map<TestInputDto>(testToAdd);
        }

        public async Task<TestInputDto> UpdateTestToCompartment(int testId, TestInputDto newTestInfo)
        {
            var test = await dataContext.Tests.Include(t => t.Questions).FirstOrDefaultAsync(t => t.Id == testId);
            dataContext.RemoveRange(test.Questions);
            await dataContext.SaveChangesAsync();

            mapper.Map(newTestInfo, test);
            dataContext.Update(test);
            await dataContext.SaveChangesAsync();

            return mapper.Map<TestInputDto>(test);
        }

        private async Task AddTestToCompartment(Compartment compartment, Test testToAdd)
        {
            compartment.CompartmentTest = testToAdd;
            dataContext.Update(compartment);
            await dataContext.SaveChangesAsync();
        }

        private async Task<Test> AddTestToDb(TestInputDto newTestInfo)
        {
            var testToAdd = mapper.Map<Test>(newTestInfo);
            await dataContext.AddAsync(testToAdd);
            await dataContext.SaveChangesAsync();

            return testToAdd;
        }

        public async Task<bool> CheckTestAnwears(AnswerListDto answears)
        {
            int userId = userContextService.GetUserContext().Id;
            CheckIfUserIsNotBanned(userId, answears.TestId);

            var test = await dataContext.Tests.Include(t => t.Questions).Include(c => c.Compartment).FirstOrDefaultAsync(t => t.Id == answears.TestId);
            
            if (test == null)
            {
                throw new System.Exception("Test is not found");
            }

            if (answears.Answears.Count != test.Questions.Count)
            {
                throw new System.Exception("Take test again please");
            }
            int wrongCount = 0;
            for (int i = 0; i < answears.Answears.Count; i++)
            {
                var trueAnswer = test.Questions.Where(q => q.Id == answears.Answears[i].QuestionId).FirstOrDefault();
                if (trueAnswer == null)
                {
                    throw new System.Exception("Take test again please");
                }

                var realAnswers = trueAnswer.AnswearsList.ToLower().Split(',');
                var inputAnswers = answears.Answears[i].Answear.ToLower().Split(',');
                Array.Sort(realAnswers);
                Array.Sort(inputAnswers);

                if (!realAnswers.SequenceEqual(inputAnswers))
                {
                    wrongCount++;
                }
            }

            double passThreshold = 80;
            double currentThreshold = ((double)(answears.Answears.Count - wrongCount) / (double)answears.Answears.Count) * 100d;
            if (currentThreshold < passThreshold)
            {
                timerService.IncreaseFailedUserTestFaledCount(userId, answears.TestId, test.TryCount);

                return false;
            }

            timerService.ClearUSerFailedTest(userId, answears.TestId);

            var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.CurrentCompartment = test.Compartment;
            dataContext.Update(user);
            await dataContext.SaveChangesAsync();

            return true;
        }

        public bool CheckIfUserIsNotBanned(int userId, int testId)
        {
            if (timerService.IsUserForTestBanned(userId, testId))
            {
                throw new System.Exception("You have limited upu test ty count. Try again in 5 minutes");
            }

            return true;
        }

        public async Task<Test> GetTestInfo(int testId)
        {
            var testToReturn = await dataContext.Tests.Include(t => t.Questions).FirstOrDefaultAsync(t => t.Id == testId);
            return testToReturn;
        }

        public async Task RemoveTestFromCompartment(int compartmentId)
        {
            var compartment = await compartmentHelper.GetCompartmentById(compartmentId);
            dataContext.RemoveRange(compartment.CompartmentTest.Questions);
            dataContext.Remove(compartment.CompartmentTest);
            await dataContext.SaveChangesAsync();
        }
    }
}