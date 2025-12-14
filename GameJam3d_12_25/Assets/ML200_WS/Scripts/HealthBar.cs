using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Setup")]
    public Enemy monitor;
    public Transform barPivot; // The point on the enemy (e.g., above head)
    public float offset = 1.0f;
    
    [Tooltip("Drag the foreground 'Fill' GameObject's RectTransform here")]
    public RectTransform healthFillRect; 

    private Camera _mainCam;
    private RectTransform _mainRectTransform;
    private CanvasGroup _canvasGroup;

    void Awake()
    {
        _mainRectTransform = GetComponent<RectTransform>();
        _mainCam = Camera.main;
        
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetMonitor(Enemy enemy, Transform pivot, float offset)
    {
        monitor = enemy;
        barPivot = pivot;
        this.offset = offset;
    }

    float FetchHealthFraction(Enemy enemy)
    {
        if (enemy.maxhealth == 0) return 0;
        return Mathf.Clamp01((float)enemy.health / (float)enemy.maxhealth);
    }

    void LateUpdate()
    {
        if (monitor == null)
        {
            Destroy(gameObject);
            return;
        }

        // 1. Move the whole container
        MoveToTarget();

        // 2. Scale the fill bar
        UpdateHealthScale();
    }

    void MoveToTarget()
    {
        Vector3 screenPos = _mainCam.WorldToScreenPoint(barPivot.position + offset * Vector3.up);

        if (screenPos.z < 0)
        {
            _canvasGroup.alpha = 0; // Behind camera -> Hide
        }
        else
        {
            _canvasGroup.alpha = 1; // In front -> Show
            _mainRectTransform.position = screenPos;
        }
    }

    void UpdateHealthScale()
    {
        if (healthFillRect != null)
        {
            float healthPct = FetchHealthFraction(monitor);
            
            // Keep existing Y and Z scale, only change X
            Vector3 newScale = healthFillRect.localScale;
            newScale.x = healthPct;
            healthFillRect.localScale = newScale;
        }
    }
}