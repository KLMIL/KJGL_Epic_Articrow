using CKT;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class ImageParts_Poison : ImageParts, ISkillable
{
    public override Define.SkillType SkillType => Define.SkillType.Hit;

    public override string SkillName => "Poison";

    float time = 3;
    float damage = 5;
    float interval = 0.5f;

    public IEnumerator SkillCoroutine(Vector3 position, Vector3 directoin, int level, SkillManager skillManager)
    {
        // position은 맞은 위치, direction은 날라가는방향, Level은 파츠개수
        //GameObject findtarget = Instantiate( Resources.Load<GameObject>("SkillPool/FindPoisonTarget"), position, Quaternion.identity);
        //findtarget.GetComponent<FindPoisonTarget>().RayCastShooot(directoin, time, damage + level, interval);

        // 쏜 방향으로 레이캐스트 쏴서 몬스터가 맞으면 DoingPoison 컴포넌트 추가
        RaycastHit2D hit = Physics2D.Raycast(position, (Vector2)directoin, .5f, LayerMask.GetMask("EnemyHurtBox"));
        Debug.DrawLine(position, position + directoin * .5f, Color.yellow, 1.5f);
        if (hit.collider != null)
        {
            print(hit.collider.name);
            DoingPoison poison = hit.collider.AddComponent<DoingPoison>();
            poison.Initialize(time, level * damage, interval);
        }
        else
        {
            print("독맞을타겟없음");
        }

        yield return null;
    }
}
