using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamagable
{
    public void TakeDamage(float damage)
    {
        print("Damage : " + damage);
    }
}
