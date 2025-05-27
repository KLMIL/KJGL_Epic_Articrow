using System;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action A_Dead;

    public float maxHealth;
    public float currentHealthPoint;
    GameObject DamageTextPrefab;

    private void Awake()
    {
        currentHealthPoint = maxHealth;
        DamageTextPrefab = Resources.Load<GameObject>("Text/DamageText");
        A_Dead += Death;
    }

    public void TakeDamage(float damage) 
    {
        currentHealthPoint -= damage;
        GameObject spawnedObj = Instantiate(DamageTextPrefab,transform.position, Quaternion.identity);
        //spawnedObj.transform.SetParent(transform,false);
        spawnedObj.GetComponent<TextMeshPro>().text = damage.ToString();

        if (currentHealthPoint <= 0) 
        {
            A_Dead.Invoke();
        }
    }

    void Death() 
    {
        Destroy(gameObject);
    }
}
