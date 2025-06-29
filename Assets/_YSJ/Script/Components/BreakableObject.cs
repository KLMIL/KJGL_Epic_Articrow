using BMC;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamagable
{
    bool _isBreak;                             // 파편이 많이 생기는 것을 막기 위함

    [SerializeField] GameObject _partsPrefab;  // 파편 오브젝트 모음

    // 충돌 시, 파편 생성
    void Break(Collider2D collision) 
    {
        _isBreak = true;
        RandomManaRegenerate();

        GameObject spawnedParts = Instantiate(_partsPrefab, transform.position, transform.rotation);
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

    // 피격 시, 파편 생성
    void Break() 
    {
        GameObject spawnedParts = Instantiate(_partsPrefab, transform.position, transform.rotation);
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

    // 랜덤 마나 회복
    void RandomManaRegenerate()
    {
        float randomAmount = Random.Range(0f, 10f);
        PlayerManager.Instance.PlayerStatus.RegenerateMana(randomAmount);
    }

    public void TakeDamage(float damage)
    {
        Break();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isBreak)
        {
            return;
        }

        if (collision.GetComponent<Rigidbody2D>())
        {
            Break(collision);
        }
    }
}