using System;
using GameCore.States;

namespace GameCore.Players
{
    public class AIBrain : IPlayerBrain
    {
        private Player _player;
        private bool _canGo = true;
        public void Action()
        {
            switch (GameManager.Instance.GetCurrentState())
            {
                default:
                case nameof(Waiting):
                    Console.WriteLine($"AI Brain taking action: Nothing.");
                    GameManager.Instance.GameOver();
                    _canGo = false;
                    break;

                case nameof(ViewCards):
                    Console.WriteLine($"AI Brain taking action: View card.");
                    PlayerEvents.ViewCards();
                    break;

                case nameof(DrawCard):
                    Console.WriteLine($"AI Brain taking action: Draw card.");
                    PlayerEvents.DrawCard();
                    break;

                case nameof(DiscardCard):
                    Console.WriteLine($"AI Brain taking action: Discard card.");
                    PlayerEvents.DiscardCard();
                    break;

                case nameof(CompleteTurn):
                    if (!GameManager.Instance.IsLastRound && _player.TurnsPlayed > 3)
                    {
                        Console.WriteLine($"AI Brain taking action: Call last round.");
                        PlayerEvents.CallLastRound();
                    }
                    else
                    {
                        Console.WriteLine($"AI Brain taking action: Complete turn.");
                        PlayerEvents.CompleteTurn();
                    }

                    _canGo = false;
                    break;

                case nameof(ShowCards):
                    Console.WriteLine($"AI Brain taking action: Show cards.");
                    break;
            }
        }

        public bool CanGo()
        {
            return _canGo;
        }

        public void Reset()
        {
            _canGo = true;
        }

        public void SetPlayer(Player player)
        {
            _player = player;
        }
    }
}
