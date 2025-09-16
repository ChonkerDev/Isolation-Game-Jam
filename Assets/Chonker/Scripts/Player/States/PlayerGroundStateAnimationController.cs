using System;
using Chonker.Scripts.Player;
using Chonker.Scripts.Player.States;
using UnityEngine;

public class PlayerGroundStateAnimationController : MonoBehaviour {
    private PlatformerPlayerComponentContainer platformerPlayerComponentContainer;
    private PlatformerPlayerAnimationManager playerAnimationManager => platformerPlayerComponentContainer.platformerPlayerAnimationManager;
    private PlatformerCharacterController characterController => platformerPlayerComponentContainer.PlatformerCharacterController;

    private PlatformerPlayerAnimationManager.GroundStates currentGroundState = PlatformerPlayerAnimationManager.GroundStates.Idle;
    private bool wasStationaryLastFrame;
    private void Awake() {
        platformerPlayerComponentContainer = GetComponentInParent<PlatformerPlayerComponentContainer>();
    }

    private void Start() {
        
    }

    public void ProcessAnimations(bool isStationary) {
        bool stationaryStateChanged = isStationary != wasStationaryLastFrame;
        Debug.Log(isStationary);
        if (stationaryStateChanged) {
            if (isStationary) {
                crossFadeToIdle();
            }
            else {
                crossFadeToMoving();
            }
        }

        wasStationaryLastFrame = isStationary;
    }

    private void crossFadeToIdle() {
        if (playerAnimationManager.isCurrentState(PlatformerPlayerAnimationManager.GroundStates.Land)) return;
        if (playerAnimationManager.isCurrentState(PlatformerPlayerAnimationManager.GroundStates.Idle)) return;
        playerAnimationManager.CrossFadeToGround(PlatformerPlayerAnimationManager.GroundStates.MoveStop);
    }

    private void crossFadeToMoving() {
        if (playerAnimationManager.isCurrentState(PlatformerPlayerAnimationManager.GroundStates.MoveLoop)) return;
        playerAnimationManager.CrossFadeToGround(PlatformerPlayerAnimationManager.GroundStates.MoveStart);
    }

    public void OnGroundStateEnter(PlatformerPlayerMovementStateId previousState) {
        wasStationaryLastFrame = true;
        playerAnimationManager.CrossFadeToGround(PlatformerPlayerAnimationManager.GroundStates.Land);
    }
}
