using System;
using UnityEngine;

namespace Code
{
    public class PlayerController : MonoBehaviour
    {
        public string id;

        private Animator animator;
        private new Camera camera;
        private CharacterController characterController;
        private GameManager gameManager;
        
        private static readonly int horizontalMovementKey = Animator.StringToHash("HorizontalMovement");
        private static readonly int verticalMovementKey = Animator.StringToHash("VerticalMovement");

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            camera = GetComponentInChildren<Camera>();
            characterController = GetComponent<CharacterController>();
            
            gameManager = FindObjectOfType<GameManager>();

            gameManager.onGameStateChanged += OnGameStateChanged;
            
            Cursor.lockState = CursorLockMode.Locked; 
        }

        private void OnDisable()
        {
            gameManager.onGameStateChanged -= OnGameStateChanged;
        }

        private void Update()
        {
            var mx = Input.GetAxis("Horizontal");
            var my = Input.GetAxis("Vertical");
            var input = new Vector3(mx, 0 , my);
            var direction = input;
            var isMoving = Mathf.Abs(mx) + Mathf.Abs(my) > 0;

            gameManager.Dispatch(new PlayerMoved()
            {
                controller = this,
                direction = direction,
                isMoving = isMoving, 
                isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)
            });

            var mouseX = Input.GetAxis("Mouse X");
            var mouseY = Input.GetAxis("Mouse Y");

            if (Mathf.Abs(mouseX) + Mathf.Abs(mouseY) > 0)
            {
                gameManager.Dispatch(new PlayerOrientationChanged()
                {
                    id = id,
                    pitch = mouseY,
                    yaw = mouseX
                }); 
            }
        }

        private void OnGameStateChanged(GameState gameState)
        {
            var state = gameState.players.Find(p => p.id == id);

            if (state == null)
            {
                return;
            }

            var playerTransform = transform;
            var playerPosition = playerTransform.position;
            var cameraTransform = camera.transform;
                
            playerTransform.localRotation = Quaternion.Euler(0, state.yaw, 0);

            const float cameraDistance = 1.7f;
            
            var closeCameraPosition = playerPosition + playerTransform.right * 1.2f + new Vector3(0, 2.3f, 0);
            var farCameraPosition = closeCameraPosition - playerTransform.forward * cameraDistance;

            var cameraPosition = Physics.Linecast(closeCameraPosition, farCameraPosition, out var hit)
                ? hit.point
                : farCameraPosition;

            var cameraRotation = Mathf.Lerp(-40f, 40f, state.pitch);

            cameraTransform.position = cameraPosition;
            cameraTransform.localRotation = Quaternion.Euler(cameraRotation, 0, 0);
            
            animator.SetFloat(horizontalMovementKey, state.direction.x);
            animator.SetFloat(verticalMovementKey, state.direction.y);
        }
        
        public Vector3 GetCameraPosition()
        {
            return camera.transform.position;
        }

        public Vector3 GetCameraDirection()
        {
            return camera.transform.forward;
        }

        public Vector3 Move(Vector3 movement)
        {
            characterController.Move(transform.TransformDirection(movement));

            return transform.position;
        }
    }
}