using UnityEngine;

namespace Code
{
    public class OrientationReducer : Reducer<PlayerOrientationChanged>
    {
        public override GameState Act(GameState gameState, PlayerOrientationChanged action)
        {
            var state = gameState.players.Find(p => p.id == action.id);

            state.yaw += action.yaw * 140f * Time.deltaTime;

            state.pitch = Mathf.Clamp01(state.pitch - action.pitch * 3f * Time.deltaTime);

            return gameState;
        }
    }
}