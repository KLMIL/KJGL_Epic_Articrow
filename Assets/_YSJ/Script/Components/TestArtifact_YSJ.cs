using System.Collections;
using UnityEngine;

public class TestArtifact_YSJ : Artifact_YSJ
{
    bool isNormalAttackClicked;
    bool isSkillAttackClicked;

    Coroutine normalAttackCoroutine;
    Coroutine skillAttackCoroutine;

    private void Start()
    {
        ArtifactInitialize();
    }

    public override void NormalAttackTriggered()
    {
        isNormalAttackClicked = true;
        if (normalAttackCoroutine == null)
        {
            normalAttackCoroutine = StartCoroutine(NormalAttackCorutine());
        }
    }

    public override void NormalAttackCancled()
    {
        isNormalAttackClicked = false;
    }

    public override void SkillAttackTriggered()
    {
        if (skillAttackCoroutine == null) 
        {
            skillAttackCoroutine = StartCoroutine(SkillAttackCorutine());
        }
    }
    public override void SkillAttackCancled()
    {
        isSkillAttackClicked = false;
    }

    IEnumerator NormalAttackCorutine() 
    {
        while (true) 
        {
            // 누른상태고 쿨타임도 없다면 일반공격 발사
            if (isNormalAttackClicked && elapsedNormalCoolTime == 0)
            {
                // 발사전 행동
                BeforeFireNormalAttack?.Invoke(this);

                // 쿨타임 적용
                elapsedNormalCoolTime = Current_NormalAttackCoolTime;

                // 일반공격 발사
                Direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position).normalized;
                GameObject spawnedBullet = Instantiate(NormalAttackPrefab, firePosition.position, Quaternion.Euler(0, 0, Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg));
                MagicRoot_YSJ magicRoot = spawnedBullet.GetComponent<MagicRoot_YSJ>();
                magicRoot.BulletInitialize(this);

                // 일반공격 성공 후 액션
                AfterFireNormalAttack?.Invoke(this, spawnedBullet);
            }

            // 매 프레임 쿨타임 감소
            if (elapsedNormalCoolTime > 0)
            {
                elapsedNormalCoolTime -= Time.deltaTime;
                if (elapsedNormalCoolTime < 0)
                {
                    elapsedNormalCoolTime = 0;
                }
            }

            // 클릭중도 아니고 쿨타임도 끝났다면 루프 종료
            if (!isNormalAttackClicked && elapsedNormalCoolTime == 0) 
            {
                break;
            }

            yield return null;
        }

        normalAttackCoroutine = null;
    }

    IEnumerator SkillAttackCorutine() 
    {

        skillAttackCoroutine = null;
        yield return null;
    }
}
