using BMC;
using CKT;
using System.Collections;
using UnityEngine;

public class ImageParts_FollowBarrier : ImageParts, ISkillable
{
    GameObject barrier;

    public override Define.SkillType SkillType => Define.SkillType.Cast;

    public override string SkillName => "FollowBarrier";

    private void Start()
    {
        barrier = Resources.Load<GameObject>("SkillPool/FollowBarrier");
        if (!barrier)
        {
            print("보호막 못찾음!");
        }
    }

    public IEnumerator SkillCoroutine(Vector3 position, Vector3 directoin, int level, SkillManager skillManager)
    {
        //YSJ.Managers.Sound.PlaySFX(Define.SFX.InstallBarrier);
        for (int i = 0; i < level; i++)
        {
            if (!barrier)
            {
                print("보호막 못찾음!");
                break;
            }
            GameObject followBarrier = Instantiate(barrier); //YSJ.Managers.TestPool.Get<GameObject>(Define.PoolID.InstallBarrier);
            followBarrier.transform.SetParent(PlayerManager.Instance.transform);
            followBarrier.transform.localPosition = Vector3.zero;
        }

        yield return null;
    }
}
