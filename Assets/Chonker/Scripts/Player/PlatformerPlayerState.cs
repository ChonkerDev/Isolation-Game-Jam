using System;
using Chonker.Scripts.Player.Collider_Checks;
using UnityEngine;

namespace Chonker.Scripts.Player {
    public class PlatformerPlayerState : MonoBehaviour {

        private PlatformerPlayerPhysicsConfigSO PlatformerPlayerPhysicsConfig => PlatformerPlayerComponentContainer.PhysicsConfigSO;
        private PlatformerPlayerComponentContainer PlatformerPlayerComponentContainer;
        private PlatformerPlayerMoveablePlatformCheck PlatformerPlayerMoveablePlatformCheck  => PlatformerPlayerComponentContainer.PlatformerPlayerMoveablePlatformCheck;
        public bool DoubleJumpUnlocked;
        public int MaxNumberOfJumps => DoubleJumpUnlocked ? 2 : 1;
        public int MaxNumberOfDashes => 1;
        [SerializeField] private bool omniDrirectionDashUnlocked;
        public int NumDashesAvailable;
        public int NumJumpsAvailable { get; private set; }

        public bool DebugInifiniteDashes;

        public SurfaceType CurrentSurfaceType;

        [SerializeField] private bool wallSlideAbilityUnlocked;
        [SerializeField] private bool wallGripAbilityUnlocked;
        public float CurrentSurfaceAccelerationCoefficient {
            get {
                switch (CurrentSurfaceType) {
                    case SurfaceType.None:
                        return 1;
                    case SurfaceType.Ice:
                        return PlatformerPlayerPhysicsConfig.SlipperySurfaceCoefficient;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Vector2 CurrentMoveablePlatformPositionDiff => PlatformerPlayerMoveablePlatformCheck.CurrentMovablePlatformPositionDiff;

        private void Awake() {
            PlatformerPlayerComponentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
        }

        private void Start() {
            ResetNumDashes();
            ResetNumJumps();
        }

        public void ResetNumJumps() {
            NumJumpsAvailable = MaxNumberOfJumps;
        }

        public void DecrementNumJumps() {
            NumJumpsAvailable--;
            NumJumpsAvailable = Mathf.Max(0, NumJumpsAvailable);
        }

        public void ResetNumDashes() {
            NumDashesAvailable = MaxNumberOfDashes;
        }

        public void DecrementNumberOfDashes() {
            NumDashesAvailable--;
            NumDashesAvailable = Mathf.Max(0, NumJumpsAvailable);
        }

        public bool AllowedToDash() {
            return NumDashesAvailable > 0 || DebugInifiniteDashes;
        }

        public bool AllowedToOmniDirectionalDash() {
            return omniDrirectionDashUnlocked && PlatformerPlayerPhysicsConfig.AllowOmniDirectionalDash;
        }

        public bool WallSlideAbilityUnlocked() {
            return wallSlideAbilityUnlocked && PlatformerPlayerPhysicsConfig.AllowWallSlide;
        }

        public bool WallGripAbilityUnlocked() {
            return wallGripAbilityUnlocked && PlatformerPlayerPhysicsConfig.AllowWallGripAbility;
        } 
    }
}