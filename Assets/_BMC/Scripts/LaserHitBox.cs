using UnityEngine;

public class LaserHitBox : MonoBehaviour
{
    float _damage = 15f;

    void OnTriggerStay2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer) == LayerMask.GetMask("PlayerHurtBox"))
        {
            if (collision.transform.parent.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                damagable.TakeDamage(_damage);
            }
        }
    }
}
