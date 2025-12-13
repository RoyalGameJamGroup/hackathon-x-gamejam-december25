using UnityEngine;

public class MarcelSeeker : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (target == null) return;

        Vector3 lookAtTarget = target.transform.position;
        lookAtTarget.y = transform.position.y; // keep only horizontal rotation
        transform.LookAt(target.transform);
        
        moveDir = (target.transform.position - transform.position).normalized;
        step = speed * Time.deltaTime;

        if(!movementBlocked){
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.transform.position,
                step
            );
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>()?.PoopNei(damage);
            Destroy(gameObject);
        }
    }
}
