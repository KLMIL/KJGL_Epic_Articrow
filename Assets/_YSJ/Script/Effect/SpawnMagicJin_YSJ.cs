using UnityEngine;

public class SpawnMagicJin_YSJ : MonoBehaviour
{
    Transform MagicJinTransform;
    SpriteRenderer MagicJin;
    Color magicJinColor;

    Transform MagicJinLightTransform;
    SpriteRenderer MagicJinLight;
    Color magicJinLightColor;

    public GameObject SpawnExplosion;

    public float SpawnDelay = 1f;
    float elapsedTime = 0f;

    void Awake()
    {
        MagicJinTransform = transform.GetChild(0);
        MagicJinTransform.localScale = new Vector3(0,0,1);
        MagicJin = MagicJinTransform.GetComponent<SpriteRenderer>();

        MagicJinLightTransform = transform.GetChild(1);
        MagicJinLightTransform.localScale = new Vector3(0, 0, 1);
        MagicJinLight = MagicJinLightTransform.GetComponent<SpriteRenderer>();

        magicJinColor = MagicJin.color;
        magicJinLightColor = MagicJinLight.color;

        magicJinColor.a = 0;
        magicJinLightColor.a = 0;

        MagicJin.color = magicJinColor;
        MagicJinLight.color = magicJinLightColor;
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;

        float scaleValue = Mathf.Lerp(MagicJinTransform.localScale.x, 1, elapsedTime / SpawnDelay);
        MagicJinTransform.localScale = new Vector3(scaleValue, scaleValue, 1);
        MagicJinLightTransform.localScale = new Vector3(scaleValue, scaleValue, 1);

        magicJinColor.a = Mathf.Lerp(0, 1, elapsedTime / (SpawnDelay*0.5f) );
        MagicJin.color = magicJinColor;

        magicJinLightColor.a = Mathf.Lerp(0, 1, elapsedTime / SpawnDelay);
        MagicJinLight.color = magicJinLightColor;

        if (elapsedTime > SpawnDelay) 
        {
            GameObject SpawnedExplosion = Instantiate(SpawnExplosion, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
