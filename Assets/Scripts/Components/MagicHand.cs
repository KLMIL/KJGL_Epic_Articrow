using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicHand : MonoBehaviour
{
    public List<BuffSkill> DefaultBuffs = new();

    // 왼손
    public List<CanEnterSlot> LeftHand = new();
    public float L_Delay = 0.1f;        // 루프 도는 속도
    public float L_CoolTime = 1f;       // 왼손 쿨타임
    public float L_Accuracy = 0;        // 정확도(0에 가까울 수록 정확해짐)
    public int L_FireNumber = 1;        // 발사 횟수
    float L_Timer = 0f;                 // 왼손 쿨타임 타이머


    // 오른손
    public List<CanEnterSlot> RightHand = new();
    public float R_Delay = 0.1f;
    public float R_CoolTime = 1f;
    public float R_Accuracy = 0;
    public int R_FireNumber = 1;
    float R_Timer = 0f;

    PlayerStatus _playerStatus;

    void Awake()
    {
        _playerStatus = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        TwoHandUpdate();

        // 쿨타임이 남아있으면 줄이기
        if (L_Timer > 0)
        {
            L_Timer -= Time.deltaTime;
        }
        // 없으면 0으로 만들기
        else 
        {
            L_Timer = 0;
        }
        if (R_Timer > 0)
        {
            R_Timer -= Time.deltaTime;
        }
        else 
        {
            R_Timer = 0;
        }

        // 남은 쿨타임 캔버스에 업데이트하기
        UIManager.Instance.leftHandCoolTime.CoolTimeUpdate(L_Timer);
        UIManager.Instance.rightHandCoolTime.CoolTimeUpdate(R_Timer);

        // 키 누르고 있으면 마법발싸 시도
        if (Managers.Input.InputSystemActions.Player.LeftHandFire.IsPressed()) 
        {
            L_FireMagic();
        }
        if (Managers.Input.InputSystemActions.Player.RightHandFire.IsPressed())
        {
            R_FireMagic();
        }
    }

    void TwoHandUpdate()
    {
        // 리스트 초기화
        LeftHand.Clear();
        RightHand.Clear();

        // 양손 UI에 붙어있는 스킬 정보들 가져오기
        foreach (Transform transform in UIManager.Instance.LeftHand)
        {
            CanEnterSlot skill = transform.GetComponentInChildren<CanEnterSlot>();
            if (skill)
            {
                LeftHand.Add(skill);
            }
        }
        foreach (Transform transform in UIManager.Instance.RightHand)
        {
            CanEnterSlot skill = transform.GetComponentInChildren<CanEnterSlot>();
            if (skill)
            {
                RightHand.Add(skill);
            }
        }
    }

    void FireMagic(bool isLeft)
    {
        List<CanEnterSlot> hand = isLeft ? LeftHand : RightHand;

        // 스킬 리스트에 아무것도 없을 때
        if (hand.Count == 0)
        {
            string debugText = isLeft ? "왼손에 스킬없음" : "오른손 스킬없음";
            Debug.Log(debugText);
            return;
        }
        // 쿨타임이 남아있을 때
        if (L_Timer > 0)
        {
            return;
        }

        // 루프 한번 돌기 시도
        StartCoroutine(Fire(LeftHand, L_Delay, L_FireNumber, L_Accuracy));
        // 클타임 설정해주기
        L_Timer = L_CoolTime;
    }


    void L_FireMagic()
    {
        // 스킬 리스트에 아무것도 없을 때
        if (LeftHand.Count == 0) 
        {
            Debug.Log("왼손에 스킬없음");
            return;
        }
        // 쿨타임이 남아있을 때
        if (L_Timer > 0) 
        {
            return;
        }

        // 루프 한번 돌기 시도
        StartCoroutine(Fire(LeftHand, L_Delay, L_FireNumber, L_Accuracy));
        // 클타임 설정해주기
        L_Timer = L_CoolTime;
    }

    void R_FireMagic()
    {
        // 스킬 리스트에 아무것도 없을 때
        if (RightHand.Count == 0)
        {
            Debug.Log("오른손 스킬없음");
            return;
        }
        // 쿨타임이 남아있을 때
        if (R_Timer > 0)
        {
            return;
        }

        // 루프 한번 돌기 시도
        StartCoroutine(Fire(RightHand, R_Delay, R_FireNumber, R_Accuracy));
        // 클타임 설정해주기
        R_Timer = R_CoolTime;
    }

    IEnumerator Fire(List<CanEnterSlot> skillList, float delay, int FireNumber, float accuracy)
    {
        List<BuffSkill> buffStack = new List<BuffSkill>(DefaultBuffs);
        // 리스트 한번 쓱 돌기
        for (int index = 0; index < skillList.Count; index++) 
        {
            // 마나 null이면 돌아가
            if (!_playerStatus)
            {
                break;
            }
            // 마법 발사체면
            if (skillList[index].TryGetComponent<MagicSkill>(out MagicSkill magic))
            {
                skillList[index]?.EnterSlot();

                // 설정된 발사 횟수만큼 발사
                for (int i = FireNumber; i > 0; i--)
                {
                    magic.TryFire(transform, buffStack, _playerStatus);
                }

                //버프스택 초기화
                buffStack = new List<BuffSkill>(DefaultBuffs);

                // 루프 딜레이만큼 기다려주기
                yield return new WaitForSeconds(delay);

                if (index < skillList.Count)
                {
                    skillList[index]?.ExitSlot();
                }
            }
            // 버프 마법이면
            else if (skillList[index].TryGetComponent<BuffSkill>(out BuffSkill buff))
            {
                buffStack.Add(buff);
            }
        }
    }
}
