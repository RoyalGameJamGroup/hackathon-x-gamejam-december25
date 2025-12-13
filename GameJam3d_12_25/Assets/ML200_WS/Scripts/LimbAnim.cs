using UnityEngine;

public class LimbAnim : MonoBehaviour
{
    [Header("References")]
    public Transform leftFoot;
    public Transform rightFoot;

    [Tooltip("Arm pivot transforms (rotate around local X).")]
    public Transform leftArmPivot;
    public Transform rightArmPivot;

    [Header("Motion")]
    [Tooltip("Local Y offset for each foot at peak stride (units).")]
    public float footStrideDistance = 0.06f;

    [Tooltip("Arm swing angle at peak stride (degrees) around local X.")]
    public float armSwingDegrees = 18f;

    [Tooltip("Base gait frequency in cycles/second when velocityScale=0.")]
    public float baseFrequency = 2.2f;

    [Tooltip("Constant multiplier for animation speed.")]
    public float speedMultiplier = 1.0f;

    [Tooltip("How strongly velocity magnitude affects animation frequency.")]
    public float velocityToFrequency = 1.0f;

    [Tooltip("Minimum speed (units/sec) required to animate.")]
    public float moveThreshold = 0.02f;

    [Header("Idle behavior")]
    public bool returnToNeutralWhenIdle = true;
    public float neutralReturnSpeed = 10f;

    [Header("Phase")]
    public float phaseOffset = 0f;

    // Cached neutral local states
    private Vector3 leftFootNeutralLocalPos;
    private Vector3 rightFootNeutralLocalPos;
    private Quaternion leftArmNeutralLocalRot;
    private Quaternion rightArmNeutralLocalRot;

    private float phase;

    // Transform-derived velocity
    private Vector3 lastWorldPos;
    private float speed; // units/sec

    private void Awake()
    {
        if (leftFoot != null) leftFootNeutralLocalPos = leftFoot.localPosition;
        if (rightFoot != null) rightFootNeutralLocalPos = rightFoot.localPosition;

        if (leftArmPivot != null) leftArmNeutralLocalRot = leftArmPivot.localRotation;
        if (rightArmPivot != null) rightArmNeutralLocalRot = rightArmPivot.localRotation;

        lastWorldPos = transform.position;
    }

    private void Update()
    {
        UpdateSpeedFromTransform();

        if (speed < moveThreshold)
        {
            if (returnToNeutralWhenIdle)
                ReturnToNeutral();
            return;
        }

        // Frequency scales with velocity and constant multiplier
        float frequency = (baseFrequency + speed * velocityToFrequency) * Mathf.Max(0f, speedMultiplier);

        // Advance phase (radians)
        phase += (Mathf.PI * 2f) * frequency * Time.deltaTime;

        float s = Mathf.Sin(phase + phaseOffset);

        ApplyFootMotion(s);
        ApplyArmMotion(s);
    }

    private void UpdateSpeedFromTransform()
    {
        // World space distance per second
        Vector3 current = transform.position;
        Vector3 delta = current - lastWorldPos;

        float dt = Time.deltaTime;
        speed = (dt > 0f) ? (delta.magnitude / dt) : 0f;

        lastWorldPos = current;
    }

    private void ApplyFootMotion(float s)
    {
        if (leftFoot != null)
        {
            Vector3 p = leftFootNeutralLocalPos;
            p.z += (-s) * footStrideDistance;
            leftFoot.localPosition = p;
        }

        if (rightFoot != null)
        {
            Vector3 p = rightFootNeutralLocalPos;
            p.z += s * footStrideDistance;
            rightFoot.localPosition = p;
        }
    }

    private void ApplyArmMotion(float s)
    {
        // Typical gait: arms swing opposite their corresponding leg.
        float leftAngle = (-s) * armSwingDegrees;
        float rightAngle = (s) * armSwingDegrees;

        if (leftArmPivot != null)
            leftArmPivot.localRotation = leftArmNeutralLocalRot * Quaternion.Euler(leftAngle, 0f, 0f);

        if (rightArmPivot != null)
            rightArmPivot.localRotation = rightArmNeutralLocalRot * Quaternion.Euler(rightAngle, 0f, 0f);
    }

    private void ReturnToNeutral()
    {
        float t = 1f - Mathf.Exp(-neutralReturnSpeed * Time.deltaTime); // smooth, framerate-independent

        if (leftFoot != null)
            leftFoot.localPosition = Vector3.Lerp(leftFoot.localPosition, leftFootNeutralLocalPos, t);

        if (rightFoot != null)
            rightFoot.localPosition = Vector3.Lerp(rightFoot.localPosition, rightFootNeutralLocalPos, t);

        if (leftArmPivot != null)
            leftArmPivot.localRotation = Quaternion.Slerp(leftArmPivot.localRotation, leftArmNeutralLocalRot, t);

        if (rightArmPivot != null)
            rightArmPivot.localRotation = Quaternion.Slerp(rightArmPivot.localRotation, rightArmNeutralLocalRot, t);
    }
}
