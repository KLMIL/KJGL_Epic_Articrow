using System;
using UnityEngine;
using TMPro;

public class PlayerStatus : MonoBehaviour, IDamagable
{
    public Action A_Dead;
    GameObject DamageTextPrefab;

    [Header("업그레이드 가능한 스테이터스")]
    public float MaxHealth { get; set; } = 200;
    public float MaxMana { get; set; } = 100;
    public float ManaRecoverySpeed { get; set; } = 5f;
    public int MoveSpeed { get; set; } = 10;
    public float LeftHandCollTime { get; set; } = 1f;
    public float RightHandCollTime { get; set; } = 1f;

    [Header("일바 스테이터스")]
    public float Health { get; set; }
    public float Mana { get; set; }

    void Awake()
    {
        Init();
        DamageTextPrefab = Managers.Resource.Load<GameObject>("Text/DamageText");
        A_Dead += Death;
    }

    void Update()
    {
        //RecoveryMana(ManaRecoverySpeed * Time.deltaTime);
        //UIManager.Instance.manaBar.ManabarUpdate(Mana / MaxMana);
    }

    public void Init()
    {
        Health = MaxHealth;
        Mana = MaxMana;
    }


    #region 마나 관련
    public void RecoveryMana(float value)
    {
        Mana += value;
        Mana = Mathf.Clamp(Mana, 0, MaxMana);
    }

    public bool UseMana(float value)
    {
        if (Mana < value)
        {
            return false;
        }
        Mana -= value;
        return true;
    }
    #endregion

    public void TakeDamage(float damage)
    {
        Health -= damage;
        GameObject spawnedObj = Instantiate(DamageTextPrefab, transform.position, Quaternion.identity);
        //spawnedObj.transform.SetParent(transform,false);
        spawnedObj.GetComponent<TextMeshPro>().text = damage.ToString();

        if (Health <= 0)
        {
            A_Dead.Invoke();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}