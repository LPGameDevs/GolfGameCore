
using System;
using System.Collections.Generic;
using GameCore;
using GameCore.Players;

namespace GamePlayer
{
    public class Program
    {

        static void Main()
        {
            // Setup
            SetupGame();

            PlayGame();

            EndGame();
        }

        private static void SetupGame()
        {
            Console.WriteLine("Setup Game");
            GolfStateMachine.OnTransitionState += LogStateTransition;
            TurnManager.OnTransitionPlayerTurn += LogPlayerTransition;
            PlayerManager.OnFetchPlayers += FetchPlayers;

            TurnManager.Instance.StartListening();
        }

        private static void PlayGame()
        {
            Console.WriteLine("Play Game");

            // Starts game in ViewCards state.
            GameManager.Instance.StartNewGame();

            // @todo Handle viewing cards.

            // Moves to player 1 and Draw cards state.
            TurnManager.Instance.NextPlayerStartTurn();

            while (!GameManager.Instance.IsGameOver)
            {
                TurnManager.Instance.TakeTurn();
            }
        }

        private static void EndGame()
        {
            Console.WriteLine("End Game");
            GolfStateMachine.OnTransitionState -= LogStateTransition;
            TurnManager.OnTransitionPlayerTurn -= LogPlayerTransition;
            PlayerManager.OnFetchPlayers -= FetchPlayers;

            GameManager.Instance.StopListening();
            TurnManager.Instance.StopListening();
        }

        private static void LogStateTransition(string arg1, string arg2)
        {
            Console.WriteLine($"Transitioning from {arg1} to {arg2}");
        }
        private static void LogPlayerTransition(PlayerId arg1, PlayerId arg2)
        {
            Console.WriteLine($"Moving turn from {arg1.ToString()} to {arg2.ToString()}");
        }

        private static void FetchPlayers(List<Player> players)
        {
            IPlayerBrain brain1 = new AIBrain();
            players.Add(new Player(brain1, PlayerId.Player1));
            IPlayerBrain brain2 = new AIBrain();
            players.Add(new Player(brain2, PlayerId.Player2));
        }
    }
}
