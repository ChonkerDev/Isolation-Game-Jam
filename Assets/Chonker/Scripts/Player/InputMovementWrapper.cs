using System;
using UnityEngine;

namespace Chonker.Scripts.Player {
    public class InputMovementWrapper : MonoBehaviour {
        IA_Player IA_Player;

        private void Awake() {
            IA_Player = new IA_Player();
            IA_Player.Enable();
        }

        public Vector2 ReadMovementInput() {
           return IA_Player.PlayerControl.Movement.ReadValue<Vector2>();
        }

        public bool WasJumpPressedThisFrame() {
            return IA_Player.PlayerControl.Jump.WasPressedThisFrame();
        }
    }
}