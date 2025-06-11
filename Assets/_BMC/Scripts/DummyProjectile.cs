using UnityEngine;

namespace BMC
{
    public class DummyProjectile : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<IDamagable>()?.TakeDamage(5f);
                Destroy(gameObject);
            }
        }
    }
}