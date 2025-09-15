using System;
using UnityEngine;

namespace Chonker.Scripts.Player {
    public class PlatformerPlayerPhysicsConfig : MonoBehaviour {
        [Header("Ground")]
        public float MaxMovementSpeed = 10;
        public float GroundAcceleration = 1;
        public float GroundDeceleration = 1;

        [Header("Air")]
        public float GravityRate = 9.8f;
        public float AirInputAcceleration = 1;

        [Header("Jump")]
        public float JumpPower = 10;
        public float HighJumpGravityRate = 10;
        [Range(0, .5f)]public float CoyoteTime = .1f;
        [Range(0, .5f)] public float JumpInputBufferTimeInSeconds = .1f;

    }
}