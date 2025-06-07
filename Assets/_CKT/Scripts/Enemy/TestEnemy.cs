using UnityEngine;

namespace CKT
{
    public class TestEnemy : MonoBehaviour, IDamagable
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TakeDamage(float damage)
        {
            Debug.Log($"{this.name} : TakeDamage");
        }
    }
}