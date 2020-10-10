using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class GameManager : MonoBehaviour
    {
        public ClientType clientType = ClientType.Server;

        public delegate void GameStateChanged(GameState gameState);

        public GameStateChanged onGameStateChanged;

        [SerializeField] private GameState gameState;

        private readonly Dictionary<Type, List<Func<Action, GameState>>> listeners =
            new Dictionary<Type, List<Func<Action, GameState>>>();

        private void Start()
        {
            gameState = DefaultGameState();

            AddReducer(new OrientationReducer());
            AddReducer(new MovementReducer());
            AddReducer(new StaminaRecovery());
            
            SpawnThings();
        }

        private void Update()
        {
            Dispatch(new TickAction()
            {
                deltaTime = Time.deltaTime
            });
        }
        
        public void Dispatch(Action action)
        {
            var type = action.GetType();

            if (!listeners.ContainsKey(type))
            {
                return;
            }
            
            foreach (var listener in listeners[type])
            {
                gameState = listener.Invoke(action);
            }
            
            onGameStateChanged.Invoke(gameState);
        }
        
        public GameState GetState()
        {
            return gameState;
        }

        private void AddReducer<T>(Reducer<T> reducer) where T : Action
        {
            var type = typeof(T);

            if (!listeners.ContainsKey(type))
            {
                listeners.Add(type, new List<Func<Action, GameState>>());
            }

            listeners[type].Add(action =>
            {
                if (clientType == ClientType.Client && reducer.IsServerSide())
                {
                    return gameState;
                }

                if (action.GetType() != reducer.GetActionType())
                {
                    return gameState;
                }

                return reducer.Act(gameState, action as T);
            });
        }
        
        private void SpawnThings()
        {
            // Spawn players

            foreach (var player in gameState.players)
            {
                var playerObject = SpawnFromBlueprint(player);

                playerObject.GetComponent<PlayerController>().id = player.id;
            }
        }

        private static GameObject SpawnFromBlueprint(BlueprintBasedState state)
        {
            var blueprint = Resources.Load<Blueprint>($"Blueprints/BP_{state.blueprint}");

            var gameObject = Instantiate(blueprint.prefab);

            gameObject.transform.position = state.position;

            return gameObject;
        }

        private static GameState DefaultGameState()
        {
            var gameState = new GameState();

            gameState.players.Add(new PlayerState()
            {
                blueprint = "Player",
                position = new Vector3(0, 0, 0)
            });

            return gameState;
        }
    }
}