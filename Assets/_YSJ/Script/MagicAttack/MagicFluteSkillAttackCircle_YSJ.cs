using UnityEngine;

public class MagicFluteSkillAttackCircle_YSJ : MagicRoot_YSJ
{
    SpriteRenderer spriteRenderer;
    Color spriteColor;

    private void OnEnable()
    {
        LifeTime = 1;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;
        spriteColor.a = 1;
        spriteRenderer.color = spriteColor;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamagable>() != null)
        {
            OnHit(collision);
            collision.GetComponent<IDamagable>().TakeDamage(AttackPower);
            //CheckDestroy();
        }
    }

    public void AttackCircleInitialize(float AttackPower)
    {
        this.AttackPower = AttackPower;
    }
}
