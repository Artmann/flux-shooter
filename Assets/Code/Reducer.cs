using System;

namespace Code
{
    public abstract class Reducer<T> where T : Action
    {
        public abstract GameState Act(GameState gameState, T action);

        public Type GetActionType()
        {
            return typeof(T);
        }
        
        public virtual bool IsServerSide()
        {
            return true;
        }
    }
}