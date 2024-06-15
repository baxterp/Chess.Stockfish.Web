using Stockfish.NET.Models;
using Stockfish.NET.Core;
using Stockfish.NET;
using System.IO;

namespace Chess.StockfishPlayer.Web.Helpers
{
    public class StockfishHelper
    {
        private IStockfish Stockfish { get; set; }

        public StockfishHelper()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = currentDirectory + "\\BinaryFiles\\stockfish_20090216_x64.exe";

            Stockfish = new Stockfish.NET.Core.Stockfish(path, depth: 2);
        }

        public async Task StartGame(int difficulty)
        {
            await Task.Run(() => 
                Stockfish.SkillLevel = difficulty
            );
        }

        public async Task<string> MakeMoveWithFEN(string fenString)
        {
            try
            {
                var bestMove = string.Empty;
                await Task.Run(() =>
                {
                    Stockfish.SetFenPosition(fenString);
                    bestMove = Stockfish.GetBestMove().Substring(0, 2) + "-" + Stockfish.GetBestMove().Substring(2, 2);
                    var visual = Stockfish.GetBoardVisual();
                    var fen = Stockfish.GetFenPosition();
                });
                return bestMove;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public async Task<string> GetBoardVisual()
        {
            var visual = string.Empty;
            await Task.Run(() =>
            {
                visual = Stockfish.GetBoardVisual();
            });
            return visual;
        }
    }
}
