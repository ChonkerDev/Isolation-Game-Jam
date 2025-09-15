using UnityEngine;

namespace Chonker.Scripts.Player {
    public abstract class InputManager : MonoBehaviour {
        protected IA_Player inputActions;
        protected PlatformerPlayerComponentContainer platformerPlayerComponentContainer;

        public void Initialize(IA_Player inputActions, PlatformerPlayerComponentContainer platformerPlayerComponentContainer) {
            this.inputActions = inputActions;
            this.platformerPlayerComponentContainer = platformerPlayerComponentContainer;
        }
    }
}