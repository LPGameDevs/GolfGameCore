using System;
using System.Collections.Generic;
using GameCore;
using GameCore.Players;
using Newtonsoft.Json;

namespace GamePlayer
{
    public class Game
    {
        private GameState _gameState;

        public Game()
        {
            _gameState = DebugGameState();
        }

        private GameState DebugGameState()
        {
            string json =
                "{\"id\": \"ceca4\",\"users\": [\"c1fb2cd2-77bb-42a4-87f8-f3d3dca3044e\"],\"currentUser\": \"c1fb2cd2-77bb-42a4-87f8-f3d3dca3044e\",      \"deck\": [        {          \"cardName\": 1,          \"suit\": 0        },        {          \"cardName\": 2,          \"suit\": 0        },        {          \"cardName\": 11,          \"suit\": 3        },        {          \"cardName\": 1,          \"suit\": 1        },        {          \"cardName\": 0,          \"suit\": 1        },        {          \"cardName\": 0,          \"suit\": 2        },        {          \"cardName\": 6,          \"suit\": 3        },        {          \"cardName\": 8,          \"suit\": 2        },        {          \"cardName\": 10,          \"suit\": 2        },        {          \"cardName\": 8,          \"suit\": 3        },        {          \"cardName\": 7,          \"suit\": 3        },        {          \"cardName\": 6,          \"suit\": 2        },        {          \"cardName\": 1,          \"suit\": 3        },        {          \"cardName\": 7,          \"suit\": 2        },        {          \"cardName\": 5,          \"suit\": 0        },        {          \"cardName\": 0,          \"suit\": 0        },        {          \"cardName\": 2,          \"suit\": 3        },        {          \"cardName\": 3,          \"suit\": 2        },        {          \"cardName\": 3,          \"suit\": 3        },        {          \"cardName\": 4,          \"suit\": 2        },        {          \"cardName\": 2,          \"suit\": 2        },        {          \"cardName\": 9,          \"suit\": 1        },        {          \"cardName\": 9,          \"suit\": 0        },        {          \"cardName\": 8,          \"suit\": 1        },        {          \"cardName\": 3,          \"suit\": 1        },        {          \"cardName\": 5,          \"suit\": 2        },        {          \"cardName\": 1,          \"suit\": 2        },        {          \"cardName\": 10,          \"suit\": 3        },        {          \"cardName\": 5,          \"suit\": 3        },        {          \"cardName\": 9,          \"suit\": 2        },        {          \"cardName\": 3,          \"suit\": 0        },        {          \"cardName\": 6,          \"suit\": 1        },        {          \"cardName\": 4,          \"suit\": 1        },        {          \"cardName\": 7,          \"suit\": 0        },        {          \"cardName\": 4,          \"suit\": 3        },        {          \"cardName\": 10,          \"suit\": 1        },        {          \"cardName\": 9,          \"suit\": 3        },        {          \"cardName\": 11,          \"suit\": 0        },        {          \"cardName\": 11,          \"suit\": 1        },        {          \"cardName\": 8,          \"suit\": 0        },        {          \"cardName\": 2,          \"suit\": 1        },        {          \"cardName\": 12,          \"suit\": 0        },        {          \"cardName\": 0,          \"suit\": 3        },        {          \"cardName\": 4,          \"suit\": 0        },        {          \"cardName\": 6,          \"suit\": 0        },        {          \"cardName\": 12,          \"suit\": 1        },        {          \"cardName\": 7,          \"suit\": 1        },        {          \"cardName\": 12,          \"suit\": 3        }      ],      \"discard\": [              ],      \"hands\": {        \"c1fb2cd2-77bb-42a4-87f8-f3d3dca3044e\": [          {            \"cardName\": 5,            \"suit\": 1          },          {            \"cardName\": 10,            \"suit\": 0          },          {            \"cardName\": 11,            \"suit\": 2          },          {            \"cardName\": 12,            \"suit\": 2          }        ]      },      \"turn\": 0    }";
            GameState state = JsonConvert.DeserializeObject<GameState>(json);
            return state;
        }

        public void StartListeners()
        {
            GolfStateMachine.OnTransitionState += LogStateTransition;
            TurnManager.OnTransitionPlayerTurn += LogPlayerTransition;
            PlayerManager.OnFetchPlayers += FetchPlayers;

            TurnManager.Instance.StartListening();
        }

        private void LogStateTransition(string arg1, string arg2)
        {
            Console.WriteLine($"Transitioning from {arg1} to {arg2}");
        }

        private void LogPlayerTransition(PlayerId arg1, PlayerId arg2)
        {
            Console.WriteLine($"Moving turn from {arg1.ToString()} to {arg2.ToString()}");
        }

        private void FetchPlayers(List<Player> players)
        {
            int i = 1;
            foreach (var user in _gameState.users)
            {
                IPlayerBrain brain = new AIBrain();
                players.Add(new Player(brain, (PlayerId) i));

                i++;
            }
        }

        public void StopListeners()
        {
            GolfStateMachine.OnTransitionState -= LogStateTransition;
            TurnManager.OnTransitionPlayerTurn -= LogPlayerTransition;
            PlayerManager.OnFetchPlayers -= FetchPlayers;

            GameManager.Instance.StopListening();
            TurnManager.Instance.StopListening();
        }

        public GameState GetState()
        {
            return _gameState;
        }
    }
}
