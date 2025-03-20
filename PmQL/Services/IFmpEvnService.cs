using PmQL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PmQL.Services
{
    public interface IFmpEvnService
    {
        Task CalculateAndSaveFmpEvnAsync();
        Task<List<FmpEvn>> GetFmpEvnDataAsync();
        Task CalculateAndSavePmEvnAsync();
        Task<List<PmEvn>> GetPmEvnDataAsync();
        Task CalculateAndSaveFmpSaiAsync();
        Task<List<FmpSai>> GetFmpSaiDataAsync();
        Task CalculateAndSavePmSaiAsync(); 
        Task<List<PmSai>> GetPmSaiDataAsync();
    }
}