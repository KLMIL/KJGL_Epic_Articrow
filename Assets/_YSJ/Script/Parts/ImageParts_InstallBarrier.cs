using CKT;
using System.Collections;
using UnityEngine;

public class ImageParts_InstallBarrier : ImageParts, ISkillable
{
    GameObject barrier;

    public override Define.SkillType SkillType => Define.SkillType.Cast;

    public override string SkillName => "InstallBarrier";

    private void Start()
    {
        barrier = Resources.Load<GameObject>("SkillPool/InstalledBarrier");
        if (!barrier)
        {
            print("베리어 못찾음!");
        }
    }

    public IEnumerator SkillCoroutine(Vector3 position, Vector3 directoin, int level, SkillManager skillManager)
    {
        //YSJ.Managers.Sound.PlaySFX(Define.SFX.InstallBarrier);
        for (int i = 0; i < level; i++) 
        {
            if (!barrier) 
            {
                print("베리어 못찾음!");
                break;
            }
            GameObject installedBarrier = Instantiate(barrier); //YSJ.Managers.TestPool.Get<GameObject>(Define.PoolID.InstallBarrier);
            installedBarrier.transform.position = position;
        }

        yield return null;
    }
}
