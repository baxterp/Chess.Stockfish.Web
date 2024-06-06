using Chess.StockfishPlayer.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Stockfish.NET.Models;
using Stockfish.NET.Core;
using Stockfish.NET;
using System.IO;


namespace Chess.StockfishPlayer.Web.Controllers
{
    public class HomeController : Controller
    {
        private IStockfish Stockfish { get; set; }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = currentDirectory +"\\BinaryFiles\\stockfish_20090216_x64.exe";

            Stockfish = new Stockfish.NET.Core.Stockfish(path, depth: 2);

            return View();
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
