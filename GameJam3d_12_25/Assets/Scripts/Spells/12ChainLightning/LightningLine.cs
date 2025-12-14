using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LightningLine : MonoBehaviour
{
    [Header("Visual Settings")]
    public float thickness = 0.1f;   // How thick the line is
    public Color boltColor = Color.cyan; // The color of the lightning

    [Header("Timing")]
    public float duration = 0.2f;    // How long it stays visible
    public float updateInterval = 0.05f; // Updates shape every 0.05s

    [Header("Jaggedness")]
    public int segments = 10;        // Higher = more zig-zags
    public float randomness = 0.5f;  // Higher = messier/wider lightning

    private LineRenderer _lr;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private bool _active = false;
    private float _timer;

    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
    }

    public void DrawLine(Vector3 start, Vector3 end)
    {
        // --- AUTO CONFIGURATION ---
        // This ensures you don't have to touch LineRenderer settings manually
        _lr.startWidth = thickness;
        _lr.endWidth = thickness;

        _lr.startColor = boltColor;
        _lr.endColor = boltColor;

        // Standard settings for visual effects
        _lr.useWorldSpace = true;
        _lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _lr.receiveShadows = false;
        // --------------------------

        _startPoint = start;
        _endPoint = end;
        _active = true;
        _timer = 0f;

        // Draw immediately so we don't wait 0.05s for the first frame
        CalculateJaggedLine();

        StartCoroutine(Fade());
    }

    void Update()
    {
        if (!_active) return;

        _timer += Time.deltaTime;

        if (_timer >= updateInterval)
        {
            CalculateJaggedLine();
            _timer = 0f;
        }
    }

    void CalculateJaggedLine()
    {
        _lr.positionCount = segments + 2;

        _lr.SetPosition(0, _startPoint);
        _lr.SetPosition(segments + 1, _endPoint);

        for (int i = 1; i <= segments; i++)
        {
            float t = (float)i / (float)(segments + 1);
            Vector3 pointOnLine = Vector3.Lerp(_startPoint, _endPoint, t);

            // Random 3D offset
            Vector3 jitter = Random.insideUnitSphere * randomness;

            _lr.SetPosition(i, pointOnLine + jitter);
        }
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}