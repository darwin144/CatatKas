using CatatKas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CatatKas.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CatatKasContext db;
        private IWebHostEnvironment _env;

        public HomeController(CatatKasContext context, IWebHostEnvironment env, ILogger<HomeController> logger)
        {
            _logger = logger;
            db = context;
            _env = env;
        }

        public IActionResult Index()
        {
            int filterMonth = DateTime.Now.Month;
            int filterYear = DateTime.Now.Year;
    
            ViewBag.SelectedMonth = filterMonth;
            ViewBag.SelectedYear = filterYear;
            return View();
        }

        public JsonResult GetDataChart(int? month, int? year)
        {
            int filterMonth = month ?? DateTime.Now.Month;
            int filterYear = year ?? DateTime.Now.Year;

            var query = (from a in db.transactions
                            where a.transaction_date.Month == filterMonth && a.transaction_date.Year == filterYear
                            group a by a.category.name into g
                            select new
                            {
                                category = g.Key,
                                total = g.Sum(x => x.amount),
                            });

            var expenses = query.ToList();

            return Json(expenses);
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
