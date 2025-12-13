using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playertransform;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(playertransform.position.x, transform.position.y, playertransform.position.z);
    }
}
