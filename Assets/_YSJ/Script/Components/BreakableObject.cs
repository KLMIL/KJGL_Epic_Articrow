using System.Collections.Generic;
using UnityEngine;
using YSJ;

public class BreakableObject : MonoBehaviour, IDamagable
{
    [SerializeField] GameObject Parts;

    public void TakeDamage(float damage)
    {
        Break();
    }

    void Awake()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>())
        {
            Break(collision);
        }
    }

    void Break(Collider2D collision) 
    {
        GameObject spawnedParts = Instantiate(Parts, transform.position, transform.rotation);
        spawnedParts.transform.SetParent(transform.parent, true);

        foreach (Transform part in spawnedParts.transform)
        {
            Rigidbody2D rb2d = part.GetComponent<Rigidbody2D>();
            Vector2 direction = (rb2d.position - (Vector2)collision.transform.position).normalized;
            rb2d.AddForce(direction * Random.Range(3f, 10f), ForceMode2D.Impulse);

            rb2d.AddTorque(Random.Range(-1f, 1f), ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }

    void Break() 
    {
        GameObject spawnedParts = Instantiate(Parts, transform.position, transform.rotation);
        spawnedParts.transform.SetParent(transform.parent, true);

        foreach (Transform part in spawnedParts.transform)
        {
            Rigidbody2D rb2d = part.GetComponent<Rigidbody2D>();
            Vector2 direction = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
            rb2d.AddForce(direction * Random.Range(3f, 10f), ForceMode2D.Impulse);

            rb2d.AddTorque(Random.Range(-1f, 1f), ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }
}
