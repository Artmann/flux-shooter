using System;
using UnityEngine;

namespace Code
{
    [Serializable]
    public class PlayerState : BlueprintBasedState
    {
        public float stamina = 100f;

        public Vector3 direction;
        public bool isMoving;
        public float pitch;
        public float yaw;
    }
}