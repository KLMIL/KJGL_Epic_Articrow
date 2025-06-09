using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPlusSin : MonoBehaviour
{
    Light2D light2D;

    public float Speed = 1.0f;
    public float valueMultifly = 1.0f;
    float sinValue;
    float defaultIntensity;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        defaultIntensity = light2D.intensity;
    }

    void Update()
    {
        this.sinValue += Time.deltaTime * Speed;
        float sinValue = Mathf.Sin(this.sinValue) * valueMultifly;
        light2D.intensity = defaultIntensity + sinValue;
    }
}
