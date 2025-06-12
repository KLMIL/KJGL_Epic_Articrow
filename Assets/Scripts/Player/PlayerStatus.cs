using System;
using UnityEngine;
using TMPro;

namespace YSJ
{
    public class PlayerStatus : MonoBehaviour, IDamagable
    {
        public Action A_Dead;

        [Header("업그레이드 가능한 스테이터스")]
        public float MaxHealth { get; set; } = 200;
        public int MoveSpeed { get; set; } = 10;
        public float LeftHandCollTime { get; set; } = 1f;
        public float RightHandCollTime { get; set; } = 1f;

        [Header("일반 스테이터스")]
        public float Health { get; set; }

        void Awake()
        {
            Init();
            A_Dead += Death;
        }

        public void Init()
        {
            Health = MaxHealth;
        }

        public void TakeDamage(float damage)
        {
            Debug.Log("플레이어 맞음");

            Debug.Log("Player Hit");
            Health -= damage;

            TextMeshPro damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
            damageText.transform.position = transform.position;
            //TextMeshPro damageText = Instantiate(_damageTextPrefab, transform.position, Quaternion.identity);
            //spawnedObj.transform.SetParent(transform,false);
            damageText.text = damage.ToString();
            UI_InGameEventBus.OnPlayerHpSliderValueUpdate?.Invoke(Health);

            if (Health <= 0)
            {
                A_Dead.Invoke();
            }
        }

        void Death()
        {
            Destroy(gameObject);
        }

        public void IncreaseHealth(float amount) 
        {
            Health += amount;
            Health = Mathf.Clamp(Health, 0, MaxHealth);

            UI_InGameEventBus.OnPlayerHpSliderValueUpdate?.Invoke(Health);
        }
    }
}