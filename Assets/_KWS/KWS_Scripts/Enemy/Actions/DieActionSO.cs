using UnityEngine;


[CreateAssetMenu(fileName = "DieAction", menuName = "Enemy/Action/Die")]
public class DieActionSO : EnemyActionSO
{
    public override void Act(EnemyController controller)
    {
        // 오브젝트 파괴
        //controller.Animation.Play("Die");
    }
}
