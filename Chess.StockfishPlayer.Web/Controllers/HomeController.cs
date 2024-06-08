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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("StartGame")]
        public IActionResult StartGame([FromBody] string difficulty)
        {
            stockfishHelper.StartGame(int.Parse(difficulty));
            return Json(200);
        }


        [HttpPost]
        [Route("MakeMoveWithTest")]
        public IActionResult MakeMoveWithTest([FromBody] string moveString)
        {
            var result = stockfishHelper.MakeMove(moveString);

            if (!string.IsNullOrEmpty(result))
                return Json(result);
            else
                return Json(null);
        }

        [HttpPost]
        [Route("MakeMoveWithFEN")]
        public IActionResult MakeMoveWithFEN([FromBody] string fenString)
        {
            var result = stockfishHelper.MakeMoveWithFEN(fenString);

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
