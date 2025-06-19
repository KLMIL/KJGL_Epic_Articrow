using UnityEngine;

public class DoingPoison : MonoBehaviour
{
    public float time;
    public float damage;
    public float interval;

    float elapsedTime = 0f;
    float elapsedDeltaTime = 0f;
    public Color defaultColor { get; private set; } = Color.white; // 기본 색상

    Color poisonColor = Color.green;
    public float hitTimeRecord { get; private set; } // 맞았을 때 시간기록


    public void Initialize(float time, float damage, float interval)
    {
        this.time = time;
        this.damage = damage;
        this.interval = interval;
    }

    private void Awake()
    {
        // 시간기록
        hitTimeRecord = Time.time;
    }

    private void Start()
    {
        // SpriteRenderer가 있는지 확인하고, 있다면 색상변경
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite)) 
        {
            DoingPoison[] otherPoisons = GetComponents<DoingPoison>();

            // 독이 나만있으면
            if (otherPoisons.Length == 1)
            {
                defaultColor = sprite.color;
                sprite.color = poisonColor;
                return;
            }

            // 다른 DoingPoison이 있으면
            DoingPoison firstPoision = this; // 일단 나로 임시 초기화 ㅎ;
            foreach (DoingPoison poison in otherPoisons)
            {
                if (firstPoision.hitTimeRecord > poison.hitTimeRecord) 
                {
                    firstPoision = poison; // 젤 작은 hitTimeRecord 찾기
                }
            }
            defaultColor = firstPoision.defaultColor;
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        elapsedDeltaTime += Time.deltaTime;
        if (elapsedTime > time) 
        {
            Destroy(this);
        }

        if (elapsedDeltaTime >= interval) 
        {
            GetComponent<IDamagable>().TakeDamage(damage);
            elapsedDeltaTime = 0f;
        }
    }

    private void OnDestroy()
    {
        DoingPoison[] otherPoisons = GetComponents<DoingPoison>();
        if (otherPoisons.Length > 1) 
        {
            return; // 다른 독이 존재하면 색상 변경하지 않음
        }
        GetComponent<SpriteRenderer>().color = defaultColor;
    }
}
