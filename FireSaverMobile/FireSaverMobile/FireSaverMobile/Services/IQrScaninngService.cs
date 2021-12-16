using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FireSaverMobile.Services
{
    public interface IQrScaninngService
    {
        Task<string> ScanAsync();
    }
}
