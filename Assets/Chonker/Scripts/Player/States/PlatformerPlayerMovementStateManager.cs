using Chonker.Runtime.Core.StateMachine;

namespace Chonker.Scripts.Player.States {
    public class PlatformerPlayerMovementStateManager : StateMachineManager<PlatformerPlayerMovementStateId, PlatformerPlayerMovementState> {

        public void UpdateState(PlatformerPlayerMovementStateId stateId) {
            base.UpdateState(stateId);
        }
    }
}