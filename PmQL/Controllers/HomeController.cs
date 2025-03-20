using Microsoft.AspNetCore.Mvc;
using PmQL.Data;
using PmQL.Models;
using PmQL.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PmQL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFmpEvnService _fmpEvnService;

        public HomeController(IFmpEvnService fmpEvnService)
        {
            _fmpEvnService = fmpEvnService;
        }

        public async Task<IActionResult> Index()
        {
            await _fmpEvnService.CalculateAndSaveFmpEvnAsync();
            var fmpEvnData = await _fmpEvnService.GetFmpEvnDataAsync();
            return View(fmpEvnData);
        }

        public async Task<IActionResult> PmEvn()
        {
            await _fmpEvnService.CalculateAndSavePmEvnAsync();
            var pmEvnData = await _fmpEvnService.GetPmEvnDataAsync();
            return View(pmEvnData);
        }

        public async Task<IActionResult> FmpSai()
        {
            await _fmpEvnService.CalculateAndSaveFmpSaiAsync();
            var fmpSaiData = await _fmpEvnService.GetFmpSaiDataAsync();
            return View(fmpSaiData);
        }

        public async Task<IActionResult> PmSai()
        {
            // Tính toán và lưu Pm_Sai
            await _fmpEvnService.CalculateAndSavePmSaiAsync();

            // Lấy dữ liệu Pm_Sai để hiển thị
            var pmSaiData = await _fmpEvnService.GetPmSaiDataAsync();
            return View(pmSaiData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}