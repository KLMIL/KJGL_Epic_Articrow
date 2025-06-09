using UnityEngine;

public class ScalePlusSin_YSJ : MonoBehaviour
{
    public float Speed = 1.0f;
    public float valueMultifly = 1.0f;
    float sinValue;
    Vector3 defaultSize;

    private void Awake()
    {
        defaultSize = transform.localScale;
    }

    void Update()
    {
        this.sinValue += Time.deltaTime * Speed;
        float sinValue = Mathf.Sin(this.sinValue) * valueMultifly;
        transform.localScale = defaultSize + new Vector3(sinValue, sinValue, sinValue);
    }
}
