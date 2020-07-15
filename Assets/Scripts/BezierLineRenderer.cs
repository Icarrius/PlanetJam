using UnityEngine;

///<summary>
///	This component represents a Bézier Quadratic Curve applied to a Line Renderer points.
///</summary>
[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public sealed class BezierLineRenderer : MonoBehaviour
{
    public Transform start;
    public Transform control;
    public Transform end;

    [SerializeField, Range(3, 100), Tooltip("Increase this value to smooth the line trajectory.")]
    private int _lineLength = 12;
    [SerializeField] private LineRenderer _line;

    //[Range(0f, 1f)] public float t = 0.5f;

    private Vector3[] _linePositions;

    private void Reset()
    {
        _line = GetComponent<LineRenderer>();

        SetupInicialLinePoints();
    }
    private void Awake()
    {
        SetLineLength(_lineLength);
    }
    private void Update()
    {
        UpdateLinePoints();
    }
    private void OnValidate()
    {
        SetLineLength(_lineLength);
    }

    private void SetupInicialLinePoints()
    {
        start.position = Vector3.left;
        control.position = Vector3.up;
        end.position = Vector3.right;
    }
    private void UpdateLinePoints()
    {
        if (start == null || control == null || end == null)
        {
            SetupInicialLinePoints();
            return;
        }

        for (int i = 0; i < _linePositions.Length; i++)
        {
            float t = i * 1f / (_linePositions.Length - 1f);
            Vector3 middlePoint1 = Vector3.Lerp(start.position, control.position, t);
            Vector3 middlePoint2 = Vector3.Lerp(control.position, end.position, t);
            Vector3 bezierPoint = Vector3.Lerp(middlePoint1, middlePoint2, t);
            _linePositions[i] = bezierPoint;
        }
        _line.SetPositions(_linePositions);
    }

    /// <summary>
    /// Represents the line positionCount. Increase this value to smooth the line trajectory.
    /// <para>For optimization purposes, only values between 3 and 100 will be accepted.</para>
    /// </summary>
    /// <param name="length">Lenght of the line. Increase to smooth the line trajectory.</param>
    public void SetLineLength(int length)
    {
        _linePositions = new Vector3[Mathf.Clamp(length, 3, 100)];
        _line.positionCount = _linePositions.Length;
    }
}