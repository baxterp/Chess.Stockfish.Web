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

        public void StartGame(int difficulty)
        {
            Stockfish.SkillLevel = difficulty;
        }

        public string MakeMoveWithFEN(string fenString)
        {
            try
            {
                Stockfish.SetFenPosition(fenString);
                var bestMove = Stockfish.GetBestMove().Substring(0, 2) + "-" + Stockfish.GetBestMove().Substring(2, 2);
                var visual = Stockfish.GetBoardVisual();
                var fen = Stockfish.GetFenPosition();

                return bestMove;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public string GetBoardVisual()
        {
            var visual = Stockfish.GetBoardVisual();
            return visual;
        }

        public string MakeMove(string move)
        {
            Stockfish.SetPosition(move);
            var visual = Stockfish.GetBoardVisual();

            var bestMove = Stockfish.GetBestMove();
            Stockfish.SetPosition(bestMove);
            visual = Stockfish.GetBoardVisual();

            bestMove = bestMove.Substring(0, 2) + "-" + bestMove.Substring(2, 2);

            return bestMove;

        }

        public bool TestMove(string move)
        {
            var result = Stockfish.IsMoveCorrect(move);
            return result;
        }

    }
}
