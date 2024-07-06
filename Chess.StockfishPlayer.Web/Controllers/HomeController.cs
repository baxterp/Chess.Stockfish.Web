using Chess.StockfishPlayer.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Stockfish.NET.Models;
using Stockfish.NET.Core;
using Stockfish.NET;
using System.IO;
using Chess.StockfishPlayer.Web.Helpers;
using System.Net;


namespace Chess.StockfishPlayer.Web.Controllers
{
    
    public class HomeController : Controller
    {
        private StockfishHelper stockfishHelper { get; set; }
        public HomeController()
        {
            stockfishHelper = new StockfishHelper();
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        [Route("StartGame")]
        public async Task<IActionResult> StartGame([FromBody] string difficulty)
        {
            await stockfishHelper.StartGame(int.Parse(difficulty));
            return Json(200);
        }

        [HttpPost]
        [Route("MakeMoveWithFEN")]
        public async Task<IActionResult> MakeMoveWithFEN([FromBody] string fenString)
        {
            var result = await stockfishHelper.MakeMoveWithFEN(fenString);

            if (!string.IsNullOrEmpty(result))
                return Json(result);
            else
                return Json(null);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
