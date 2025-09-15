using System;
using UnityEngine;

namespace Chonker.Scripts.Player {
    public class PlatformerPlayerPhysicsConfig : MonoBehaviour {
        public float JumpForce = 10;
        public float MaxMovementSpeed = 10;
        public float GravityRate = 9.8f;
        public float GroundAcceleration = 1;
        public float GroundDeceleration = 1;
        public float CoyoteTime = .1f;
        public float DistanceToGroundToTreatAirJumpAsGrounded = .3f;

    }
}