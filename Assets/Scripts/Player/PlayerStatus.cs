using System;
using UnityEngine;
using TMPro;

public class PlayerStatus : MonoBehaviour, IDamagable
{
    public Action A_Dead;
    GameObject DamageTextPrefab;

    [Header("업그레이드 가능한 스테이터스")]
    public float MaxHealth { get; set; } = 200;
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
        if (Input.GetKeyDown(KeyCode.G))
            TakeDamage(10f);
    }

    public void Init()
    {
        Health = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        GameObject spawnedObj = Instantiate(DamageTextPrefab, transform.position, Quaternion.identity);
        //spawnedObj.transform.SetParent(transform,false);
        spawnedObj.GetComponent<TextMeshPro>().text = damage.ToString();
        BMC.UI_InGameEventBus.OnHpSliderValueUpdate?.Invoke(Health);

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