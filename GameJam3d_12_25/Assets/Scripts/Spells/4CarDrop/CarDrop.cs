using UnityEngine;

public class CarDrop: Spell
{
    [SerializeField] float distance = 5f;

    Vector3 targetPosition;
    bool hasLanded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPosition = transform.position + new Vector3(direction.x, 0, direction.y).normalized * distance;
        transform.position = new Vector3(targetPosition.x, 5, targetPosition.z);

        PlaySpawnsSound();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 10f * Time.deltaTime);
        if (!hasLanded && transform.position == targetPosition)
        {
            PlayImpactSound();
            hasLanded = true;
        }
    }

    // Called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("CarDrop hit " + other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy") && !hasLanded)
        {
            other.gameObject.GetComponent<Enemy>().Kill();
        }
    }
}
