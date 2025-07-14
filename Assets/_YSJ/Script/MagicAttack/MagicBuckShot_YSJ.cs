using UnityEngine;

public class MagicBuckShot_YSJ : MagicRoot_YSJ
{
    Rigidbody2D rb2d;
    public int splinterCount = 3;
    public float spreadAngle = 5f;
    public GameObject SplinterPrefab;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        FlyingAction?.Invoke(ownerArtifact, gameObject);

        rb2d.linearVelocity = transform.right * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamagable>() != null)
        {
            OnHit(collision);
            collision.GetComponent<IDamagable>().TakeDamage(AttackPower);

            Vector2 Direction = transform.right.normalized;
            float currentAngle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            float firstAngle = currentAngle - (( spreadAngle * (splinterCount - 1) ) / 2);

            if (!SplinterPrefab) 
            {
                print("파편 프리팹 연결 안돼있음!");
                return;
            }

            // 개수만큼 파편 생성
            for (int SpawnCount = 0; SpawnCount < splinterCount; SpawnCount++)
            {
                GameObject SpawnedSplinter = Instantiate(SplinterPrefab, transform.position, Quaternion.Euler(0, 0, firstAngle + spreadAngle * SpawnCount)); // 첫 각도부터 시작해서 퍼지는 각도 더하면서 탄 생성
                // 액션 넘겨주기, 맞은 애 기록해주기
                MagicSplinter_YSJ splinterMagicRoot = SpawnedSplinter.GetComponent<MagicSplinter_YSJ>();
                splinterMagicRoot.SplinterInitialize(this, collision);
                splinterMagicRoot.AttackPower = base.AttackPower * 0.5f;
            }

            CheckDestroy();
        }
    }
}
