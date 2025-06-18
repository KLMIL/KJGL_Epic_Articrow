using UnityEngine;
using YSJ;

public class installedBarrier : MonoBehaviour, IDamagable
{
    public void TakeDamage(float damage)
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerStatus>(out PlayerStatus player)) 
        {
            player.installedBarriers.Add(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerStatus>(out PlayerStatus player)) 
        {
            player.installedBarriers.Remove(this);
        }
    }
}
