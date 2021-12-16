using FireSaverMobile.Models;
using FireSaverMobile.Models.TestCompartmentModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Contracts
{
    public interface ICompartmentEnterService
    {
        Task<TestCompModel> SendCompartmentData(UserEnterCompartmentDto compartmentEnterData);
        Task<ServerResponse> SendTestAnswers(AnswerListModel compartmentEnterData);
        Task<CompartmentCommonInfo> GetCompartmentById(int compartmentId);
    }
}
