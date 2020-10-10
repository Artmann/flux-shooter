
using UnityEngine;

namespace Code
{
    public class MovementReducer : Reducer<PlayerMoved>
    {
        public override GameState Act(GameState gameState, PlayerMoved action)
        {
            var state = gameState.players.Find(player => player.id == action.controller.id);
            
            if (state == null) {
                return gameState;
            }

            const float baseMovementSpeed = 7f;
            
            var speedMultiplier = action.isSprinting && state.stamina > 0 ? 1.5f : 1f;
            var movementSpeed = baseMovementSpeed * speedMultiplier;

            var movement = Time.deltaTime * movementSpeed * action.direction;

            var position = action.controller.Move(movement);

            state.direction = action.direction;
            state.isMoving = action.isMoving;
            state.position = position;
            
            if (action.isSprinting)
            {
                state.stamina = Mathf.Clamp(state.stamina - PlayerConstants.sprintingCost * Time.deltaTime, 0, 100f);    
            }
            
            return gameState;
        }
    }
}