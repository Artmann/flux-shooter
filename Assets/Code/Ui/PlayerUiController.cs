using UnityEngine;

namespace Code.Ui
{
    public class PlayerUiController : MonoBehaviour
    {
        [SerializeField] private ProgressBarController staminaBar;
        
        private GameManager gameManager;
        
        private string id;
        
        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();

            gameManager.onGameStateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            gameManager.onGameStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(GameState gameState)
        {
            Render(gameState);
        }

        private void Render(GameState gameState)
        {
            if (id == null)
            {
                id = FindObjectOfType<PlayerController>().id;
            }

            var state = gameState.players.Find(p => p.id == id);
            
            staminaBar.SetProgress(state.stamina / 100f);
        }
    }
}