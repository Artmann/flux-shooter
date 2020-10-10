using UnityEngine;

namespace Code
{
    public class StaminaRecovery : Reducer<TickAction>
    {
        public override GameState Act(GameState gameState, TickAction action)
        {
            foreach (var state in gameState.players)
            {
                state.stamina = Mathf.Clamp(
                    state.stamina + PlayerConstants.sprintingCost * 0.5f * action.deltaTime, 
                    0, 
                    100f
                );
            }

            return gameState;
        }
    }
}