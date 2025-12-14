using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float spawnTime;
    [SerializeField] public float lifeTime = 5f;
    [SerializeField] public float speed = 2f;
    public int damage;

    public Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartCoroutine(timerKill());
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate((new Vector3(direction.x, 0, direction.y)).normalized * speed * Time.deltaTime, Space.World);
    }

    // Called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>()?.PoopNei(damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator timerKill()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}

