using UnityEngine;

[CreateAssetMenu(fileName = "Player Platformer Physics Config",
    menuName = "ScriptableObjects/PlatformerPlayerPhysicsConfig", order = 1)]
public class PlatformerPlayerPhysicsConfigSO : ScriptableObject {
    [Header("Ground")] public float MaxMovementSpeed = 10;
    public float GroundAcceleration = 1;
    public float GroundDeceleration = 1;

    [Header("Air")] public float GravityRate = 9.8f;
    public float AirInputAcceleration = 1;

    [Header("Jump")] public float JumpPower = 10;
    public float HighJumpGravityRate = 10;
    [Range(0, .5f)] public float CoyoteTime = .1f;
    [Range(0, .5f)] public float JumpInputBufferTimeInSeconds = .1f;

    [Header("Dash")] [Range(0, 1)] public float DashAccelerationTime = .1f;
    [Range(0, 1)] public float DashConstantSpeedTime = .5f;
    [Range(0, 1)] public float DashDecelerationTime = .1f;
    [Range(0, 30)] public float DashTopSpeed = 5;
    [Range(0, .2f)] public float DirectionInputBufferInSeconds = .05f;
    public bool AllowOmniDirectionalDash;

    [Header("Wall Slide")] public bool AllowWallSlide;
    public bool AllowSameSideWallSlide = false;
    public float WallSlideGravityRate = 10;
    public float MaxDistanceFromGroundToPreventWallSlide;
    public float WallSlideVerticalJumpPower = 10;
    public float WallSlideHorizontalJumpPower = 10;

    [Tooltip("Hold opposite of facing direction to slow down descent")] [Space]
    public bool AllowWallGripAbility;

    [Range(0, 1)] public float WallGripGravityMultiplier = .5f;


    [Header("Other")] public float CeilingPassthroughMargin = .1f;
    public float SlipperySurfaceCoefficient = 2;
    public float GlobalTerminalVelocity = 20;
}