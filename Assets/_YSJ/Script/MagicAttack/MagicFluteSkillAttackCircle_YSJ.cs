using UnityEngine;

public class MagicFluteSkillAttackCircle_YSJ : MagicRoot_YSJ
{
    SpriteRenderer spriteRenderer;
    Color spriteColor;
    Collider2D col;

    private void OnEnable()
    {
        LifeTime = .2f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;
        spriteColor.a = 1;
        spriteRenderer.color = spriteColor;

        col = GetComponent<Collider2D>();

        CheckDamageable();
    }

    void CheckDamageable() 
    {
        Collider2D[] hits = new Collider2D[10];
        ContactFilter2D mask = new();
        mask.useLayerMask = true;
        mask.useTriggers = true;
        mask.SetLayerMask(LayerMask.GetMask("EnemyHurtBox", "BreakableObj"));
        int count = Physics2D.OverlapCollider(col, mask, hits);
        for (int i = 0; i < count; i++) 
        {
            if (hits[i].TryGetComponent<IDamagable>(out IDamagable damageable)) 
            {
                transform.parent.GetComponent<MagicRoot_YSJ>().OnHit(hits[i]);
                damageable.TakeDamage(AttackPower);
            }
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        spriteColor.a = Mathf.Lerp(1, 0, elapsedTime/LifeTime);
        spriteRenderer.color = spriteColor;

        if (elapsedTime > LifeTime)
        {
            gameObject.SetActive(false);
            // 내가 마지막 자식이면
            if (transform.parent.GetChild(transform.parent.childCount - 1) == transform) 
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.GetComponent<IDamagable>() != null)
    //    {
    //        OnHit(collision);
    //        collision.GetComponent<IDamagable>().TakeDamage(AttackPower);
    //        //CheckDestroy();
    //    }
    //}

    public void AttackCircleInitialize(float AttackPower)
    {
        this.AttackPower = AttackPower;
    }
}
