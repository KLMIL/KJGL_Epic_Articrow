using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamagable
{
    public void TakeDamage(float damage, Transform attacker = null)
    {
        print("Damage : " + damage);
    }
}
