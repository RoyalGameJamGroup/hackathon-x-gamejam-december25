using UnityEngine;
using System.Collections;

public class Nuclear : Spell
{
    [SerializeField] float distance = 5f;
    [SerializeField] float height = 40f;

    [SerializeField] GameObject model;

    Vector3 targetPosition;
    bool falling = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPosition = transform.position + new Vector3(direction.x, 0, direction.y).normalized * distance;
        transform.position = new Vector3(targetPosition.x, height, targetPosition.z);

        PlaySpawnsSound();
        StartCoroutine(WaitForFall());
    }

    IEnumerator WaitForFall()
    {
        yield return new WaitForSeconds(11.5f);
        falling = true;
        model.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (falling){
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 10f * Time.deltaTime);
        }
    }

    // Called when the collider other enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("GAME HAS ENDED");
    }
}
