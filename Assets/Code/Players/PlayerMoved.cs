using UnityEngine;

namespace Code
{
    public class PlayerMoved : Action
    {
        public PlayerController controller;
        public Vector3 direction;
        public bool isMoving;
        public bool isSprinting;
    }
}