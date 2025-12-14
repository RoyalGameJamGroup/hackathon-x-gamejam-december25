using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class NukeDeto : MonoBehaviour
{
    public Light light;
    public GameObject detonationEffect;

    public Gradient lightIntensity;
    public float maxLightInensity = 200.0f;
    public float maxLightExponent = 2.0f;

    public float lightDuration = 3.0f;
    public float startTimeEffect = 2.0f;
    float startTime;

    private void Start()
    {
        startTime = Time.time;
        StartCoroutine("wixxBums");

    }

    private void Update()
    {
        if (Time.time-startTime < lightDuration)
        {
            light.color = lightIntensity.Evaluate((Time.time - startTime) / lightDuration);
            light.intensity = maxLightInensity * (1.0f / Mathf.Max(Mathf.Pow(Time.time - startTime, maxLightExponent), 0.00001f));
        }
    }

    IEnumerator wixxBums()
    {
        yield return new WaitForSeconds(startTimeEffect);
        detonationEffect.SetActive(true);
    }

}
