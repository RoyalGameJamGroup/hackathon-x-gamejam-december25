using UnityEngine;

public class Rock : Spell
{
    [SerializeField] float speed = 3f;
    [SerializeField] float lifetime = 5f;

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
            Vector3 movementVector = (new Vector3(direction.x, 0, direction.y)).normalized;
            Vector3 rollAxis = Vector3.Cross(movementVector, Vector3.up);
            childToRotate.Rotate(rollAxis, -rotationSpeed * Time.deltaTime, Space.World);
        }
        transform.Translate((new Vector3(direction.x, 0, direction.y)).normalized * speed * Time.deltaTime);
    }

    // Called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        PlayImpactSound();
        Debug.Log("Rock hit " + other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Projectile"))
        {
            other.gameObject.GetComponent<Enemy>().Kill();
        }else if(!other.gameObject.CompareTag("Spell")){
            Destroy(gameObject);
        }
    }
}
