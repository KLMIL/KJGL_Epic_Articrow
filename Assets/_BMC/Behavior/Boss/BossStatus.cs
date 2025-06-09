using TMPro;
using Unity.Behavior;
using UnityEngine;
using YSJ;

public class BossStatus : MonoBehaviour, IDamagable
{
    Animator _anim;
    GameObject DamageTextPrefab;
    BehaviorGraphAgent _behaviorGraphAgent;
    [field: SerializeField] public bool IsDead { get; set; }

    [Header("일반 스테이터스")]
    [field: SerializeField] public float Health { get; set; }

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        Init();
        DamageTextPrefab = Managers.Resource.Load<GameObject>("Text/DamageText");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(10f);
            Debug.Log("보스 데미지 10 입음");
        }
    }

    public void Init()
    {
        Health = 100;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead)
            return;

        // 애니메이션 재생
        AnimatorStateInfo currentStateInfo = _anim.GetCurrentAnimatorStateInfo(0);
        if(currentStateInfo.IsName("Hit"))
            return;
        _anim.Play("Hit");

        Health -= damage;
        GameObject spawnedObj = Instantiate(DamageTextPrefab, transform.position, Quaternion.identity);
        //spawnedObj.transform.SetParent(transform,false);
        spawnedObj.GetComponent<TextMeshPro>().text = damage.ToString();
        UI_InGameEventBus.OnHpSliderValueUpdate?.Invoke(Health);

        if (Health <= 0)
        {
            IsDead = true;
            _behaviorGraphAgent.SetVariableValue("IsDead", IsDead);
            _behaviorGraphAgent.SetVariableValue("CurrentState", BossState.Die);
        }
    }
}