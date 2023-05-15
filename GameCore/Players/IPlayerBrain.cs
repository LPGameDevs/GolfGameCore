namespace GameCore.Players
{
    public interface IPlayerBrain
    {
        void SetPlayer(Player player);

        /**
         * Perform next action for this game state.
         */
        void Action();

        bool CanGo();
        void Reset();
    }
}
