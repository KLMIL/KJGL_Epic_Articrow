using UnityEngine;

/*
 * 몬스터가 죽을 때 -> 액션 종료시 오브젝트 제거
 */
[CreateAssetMenu(
    fileName = "DieAction", 
    menuName = "Enemy/Action/State/Die"
)]
public class DieActionSO : EnemyActionSO
{
    public override void Act(EnemyController controller)
    {
        if (!controller.FSM.isDied)
        {
            // (이팩트가 필요하다면 여기서 추가)
            foreach (var collider in controller.GetComponents<Collider2D>())
            {
                collider.enabled = false;
            }
            controller.StopMove();

            Object.Destroy(controller.gameObject, 1.0f);
            YSJ.Managers.Sound.PlaySFX(Define.SFX.sfx_slime_die);

            controller.FSM.isDied = true;
        }
    }

    public override void OnExit(EnemyController controller)
    {
        //Destroy(controller.gameObject);
    }

}
