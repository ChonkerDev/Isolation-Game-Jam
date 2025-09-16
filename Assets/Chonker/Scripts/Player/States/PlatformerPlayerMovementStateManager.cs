using Chonker.Runtime.Core.StateMachine;

namespace Chonker.Scripts.Player.States {
    public class PlatformerPlayerMovementStateManager : StateMachineManager<PlatformerPlayerMovementStateId, PlatformerPlayerMovementState> {

        public void UpdateState(PlatformerPlayerMovementStateId stateId) {
            base.UpdateState(stateId);
        }
        public void UpdateStateToGround() {
            UpdateState(PlatformerPlayerMovementStateId.Ground);
        }

        public void UpdateStateToAir() {
            UpdateState(PlatformerPlayerMovementStateId.Air);
        }

        public void UpdateStateToDash() {
            UpdateState(PlatformerPlayerMovementStateId.Dash);
        }
    }
}