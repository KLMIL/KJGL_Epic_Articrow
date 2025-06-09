using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform projectileSpawnPoint;
    protected Transform target;
    protected float damage;
    float maxCooldownTime;
    float currentCooldownTime = 0f;
    bool isSkillAvailable = true;

    public void Setup(Transform target, float damage, float cooldownTime)
    {
        this.target = target;
        this.damage = damage;
        maxCooldownTime = cooldownTime;
    }

    void Update()
    {
        if(isSkillAvailable == false && Time.time - currentCooldownTime > maxCooldownTime)
        {
            isSkillAvailable = true;
        }
    }

    public void TryAttack()
    {
        if(isSkillAvailable == true)
        {
            OnAttack();
            isSkillAvailable = false;
            currentCooldownTime = Time.time;
        }
    }

    public abstract void OnAttack();
}