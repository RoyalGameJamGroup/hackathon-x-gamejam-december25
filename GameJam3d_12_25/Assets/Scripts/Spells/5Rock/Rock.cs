using UnityEngine;

public class Rock : Spell
{
    [SerializeField] float speed = 3f;

    [Header("Rock Rotation")]
    [SerializeField] float rotationSpeed = 500f;
    private Transform childToRotate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (transform.childCount > 0)
        {
            childToRotate = transform.GetChild(0);
        }
        PlaySpawnsSound();
    }

    // Update is called once per frame
    void Update()
    {
        if (childToRotate != null)
        {
            childToRotate.Rotate(Vector3.right, rotationSpeed * Time.deltaTime, Space.Self);
        }
        transform.Translate((new Vector3(direction.x, 0, direction.y)).normalized * speed * Time.deltaTime);
    }

    // Called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Rock hit " + other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        PlayImpactSound();
        Destroy(gameObject);
    }
}
