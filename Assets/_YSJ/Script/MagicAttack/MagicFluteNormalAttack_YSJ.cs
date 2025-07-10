using UnityEngine;

public class MagicFluteNormalAttack_YSJ : MagicRoot_YSJ
{
    //float AttackSize = 1f;

    SpriteRenderer[] spriteRenderers;
    Color[] spriteColors;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        spriteColors = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteColors[i] = spriteRenderers[i].color;
        }
    }

    private void Start()
    {
        //AttackSize = transform.localScale.x;
    }

    private void Update()
    {
        FlyingAction?.Invoke(ownerArtifact, gameObject);

        //transform.localScale += Vector3.one * Speed * Time.deltaTime * AttackSize;
        transform.localScale += Vector3.one * Speed * Time.deltaTime * base.LifeTime * base.Speed;

        for (int i = 0;i < spriteRenderers.Length; i++)
        {
            spriteColors[i].a = Mathf.Lerp(1, 0, elapsedTime / LifeTime);
            foreach (Transform child in transform)
            {
                Color color = child.GetComponent<SpriteRenderer>().color;
                color.a = Mathf.Lerp(1, 0, elapsedTime / LifeTime);
                child.GetComponent<SpriteRenderer>().color = color;
            }
            spriteRenderers[i].color = spriteColors[i];
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamagable>() != null)
        {
            OnHit(collision);
            collision.GetComponent<IDamagable>().TakeDamage(AttackPower);
            //CheckDestroy();
        }
    }
}
