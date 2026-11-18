using UnityEngine;
using UnityEngine.Events;
using Oculus.Interaction;

public class WheelMagneticDetent : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag WheelRootSpin (the parent with Grabbable and OneGrabRotateTransformer) here.")]
    [SerializeField] private Transform _grabPivot;
    [SerializeField] private Grabbable _grabbable;
    public RotatingDoorPuzzle rotatingDoorPuzzle;

    [Header("Detent Settings")]
    [SerializeField] private float _snapIncrement = 45f;
    [Tooltip("How close (in degrees) to a 45° notch before the magnet pulls it in.")]
    [SerializeField] private float _magnetZone = 14f;
    [Tooltip("How fast the wheel snaps into the notch.")]
    [SerializeField] private float _snapSpeed = 22f;

    // Set to (0, 0, 1) for Z-axis, (0, 1, 0) for Y-axis, etc.
    [SerializeField] private Vector3 _localRotationAxis = Vector3.forward;

    [Tooltip("Check this if Clockwise (Right) and Counter-Clockwise (Left) are flipped for your wheel.")]
    [SerializeField] private bool _invertClockwiseDirection = false;

    [Header("45-Degree Turn Events")]
    public UnityEvent OnTurnedRight45;
    public UnityEvent OnTurnedLeft45;

    private Quaternion _lastPivotRotation;
    private float _continuousHandAngle;
    private float _currentVisualAngle;
    private int _lastSnappedStep;

    public AudioSource snapSound;

    private void Start()
    {
        if (_grabPivot == null && transform.parent != null)
            _grabPivot = transform.parent;

        if (_grabbable == null && _grabPivot != null)
            _grabbable = _grabPivot.GetComponent<Grabbable>();

        if (_grabPivot != null)
            _lastPivotRotation = _grabPivot.localRotation;

        _continuousHandAngle = 0f;
        _currentVisualAngle = 0f;
        _lastSnappedStep = 0;
    }

    private void LateUpdate()
    {
        if (_grabPivot == null) return;

        // 1. Measure exact signed rotation delta around _localRotationAxis using Quaternions (no Euler 180° flips)
        Quaternion currentPivotRot = _grabPivot.localRotation;
        Quaternion deltaRot = currentPivotRot * Quaternion.Inverse(_lastPivotRotation);
        _lastPivotRotation = currentPivotRot;

        float deltaAngle = GetSignedAngleAroundAxis(deltaRot, _localRotationAxis.normalized);
        _continuousHandAngle += deltaAngle;

        // 2. Calculate nearest 45-degree notch and unwrapped distance from hand angle
        float nearestNotch = Mathf.Round(_continuousHandAngle / _snapIncrement) * _snapIncrement;
        float distanceToNotch = Mathf.Abs(_continuousHandAngle - nearestNotch);

        bool isGrabbed = _grabbable != null && _grabbable.SelectingPointsCount > 0;

        // 3. Snap to notch inside magnet zone (or when released); otherwise follow hand
        float targetContinuousAngle = (!isGrabbed || distanceToNotch <= _magnetZone)
            ? nearestNotch
            : _continuousHandAngle;

        _currentVisualAngle = Mathf.Lerp(_currentVisualAngle, targetContinuousAngle, Time.deltaTime * _snapSpeed);

        // 4. Set visual rotation cleanly using Quaternions so it always sits at _currentVisualAngle
        Quaternion targetWorldLocalRot = Quaternion.AngleAxis(_currentVisualAngle, _localRotationAxis.normalized);
        transform.localRotation = Quaternion.Inverse(currentPivotRot) * targetWorldLocalRot;

        // 5. Check if the visual wheel has reached a new 45° step
        CheckFor45DegreeStep();
    }

    private float GetSignedAngleAroundAxis(Quaternion deltaRotation, Vector3 axis)
    {
        // Pick a reference vector perpendicular to the rotation axis
        Vector3 perpendicular = Mathf.Abs(Vector3.Dot(axis, Vector3.up)) < 0.9f
            ? Vector3.Cross(axis, Vector3.up).normalized
            : Vector3.Cross(axis, Vector3.right).normalized;

        Vector3 rotatedPerpendicular = deltaRotation * perpendicular;
        // Project onto plane perpendicular to axis to eliminate any floating-point drift
        rotatedPerpendicular = Vector3.ProjectOnPlane(rotatedPerpendicular, axis).normalized;

        return Vector3.SignedAngle(perpendicular, rotatedPerpendicular, axis);
    }

    private void CheckFor45DegreeStep()
    {
        int currentStep = Mathf.RoundToInt(_currentVisualAngle / _snapIncrement);
        float distanceFromExactStep = Mathf.Abs(_currentVisualAngle - (currentStep * _snapIncrement));

        if (currentStep != _lastSnappedStep && distanceFromExactStep < 1.0f)
        {
            int stepDifference = currentStep - _lastSnappedStep;
            _lastSnappedStep = currentStep;

            snapSound.Play();

            bool isClockwise = _invertClockwiseDirection ? (stepDifference < 0) : (stepDifference > 0);
            int stepsMoved = Mathf.Abs(stepDifference);

            for (int i = 0; i < stepsMoved; i++)
            {
                if (isClockwise)
                {
                    if (rotatingDoorPuzzle != null) rotatingDoorPuzzle.TurnLeftOnce();
                    OnTurnedRight45?.Invoke();
                }
                else
                {
                    if (rotatingDoorPuzzle != null) rotatingDoorPuzzle.TurnRightOnce();
                    OnTurnedLeft45?.Invoke();
                }
            }
        }
    }
}