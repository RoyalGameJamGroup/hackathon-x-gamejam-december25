using UnityEngine;

public class MainMenuButtonMovement : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private float minSpeed = 80f;
    [SerializeField] private float maxSpeed = 160f;
    [SerializeField] private RectTransform movementBounds;

    private Vector2[] velocities;
    

    private void Awake()
    {
       
        velocities = new Vector2[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            velocities[i] = Random.insideUnitCircle.normalized *
                            Random.Range(minSpeed, maxSpeed);
        }
    }

    private void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            MoveButton(i);
            BounceOffBounds(i);
        }

        HandleButtonCollisions();
    }

    private void MoveButton(int index)
    {
        buttons[index].anchoredPosition += velocities[index] * Time.deltaTime;
    }

    private void BounceOffBounds(int index)
    {
        RectTransform button = buttons[index];

        Vector2 pos = button.anchoredPosition;
        Vector2 halfSize = button.rect.size * 0.5f;

        Rect boundsRect = movementBounds.rect;
        Vector2 boundsCenter = movementBounds.anchoredPosition;

        Vector2 min = boundsCenter + boundsRect.min + halfSize;
        Vector2 max = boundsCenter + boundsRect.max - halfSize;

        if (pos.x < min.x || pos.x > max.x)
        {
            velocities[index].x *= -1f;
            pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        }

        if (pos.y < min.y || pos.y > max.y)
        {
            velocities[index].y *= -1f;
            pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        }

        button.anchoredPosition = pos;
    }

    private void HandleButtonCollisions()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            for (int j = i + 1; j < buttons.Length; j++)
            {
                if (RectOverlaps(buttons[i], buttons[j]))
                {
                    // Simple elastic collision: swap velocities
                    Vector2 temp = velocities[i];
                    velocities[i] = velocities[j];
                    velocities[j] = temp;
                }
            }
        }
    }

    private bool RectOverlaps(RectTransform a, RectTransform b)
    {
        Rect rectA = GetWorldRect(a);
        Rect rectB = GetWorldRect(b);

        return rectA.Overlaps(rectB);
    }

    private Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        return new Rect(
            corners[0].x,
            corners[0].y,
            corners[2].x - corners[0].x,
            corners[2].y - corners[0].y
        );
    }


}
