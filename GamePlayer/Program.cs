using System;
using GameCore;

namespace GamePlayer
{
    public class Program
    {
        static void Main()
        {
            // Setup
            Game game = SetupGame();

            PlayGame(game);

            EndGame(game);
        }

        private static Game SetupGame()
        {
            Console.WriteLine("Setup Game");

            Game game = new Game();
            game.StartListeners();

            return game;
        }

        private static void PlayGame(Game game)
        {
            Console.WriteLine("Play Game");

            // Starts game in ViewCards state.
            GameManager.Instance.StartNewGame(game.GetState());

            // @todo Handle viewing cards.

            // Moves to player 1 and Draw cards state.
            TurnManager.Instance.NextPlayerStartTurn();

            while (!GameManager.Instance.IsGameOver)
            {
                TurnManager.Instance.TakeTurn();
            }
        }

        private static void EndGame(Game game)
        {
            Console.WriteLine("End Game");
            game.StopListeners();
        }
    }
}
