using System;
using System.Collections;
using UnityEngine;

namespace Chonker.Scripts.Player {
    public class InputMovementWrapper : MonoBehaviour {
        IA_Player IA_Player;
        PlatformerPlayerComponentContainer platformerPlayerComponentContainer;
        public JumpInputManager jumpInputManager;
        
        private void Awake() {
            IA_Player = new IA_Player();
            IA_Player.Enable();
            platformerPlayerComponentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
            jumpInputManager = GetComponentInChildren<JumpInputManager>();
            jumpInputManager.Initialize(IA_Player, platformerPlayerComponentContainer);

        }

        public float ReadHorizontalMovementInput() {
           return IA_Player.PlayerControl.HorizontalMovement.ReadValue<float>();
        }
        
        public float ReadVerticalMovementInput() {
            return IA_Player.PlayerControl.VerticalMovement.ReadValue<float>();
        }

        public bool WasDashPressedThisFrame() {
            return IA_Player.PlayerControl.Dash.WasPressedThisFrame();
        }
        
    }
}