using UnityEngine;


[CreateAssetMenu(fileName = "DieAction", menuName = "Enemy/Action/Die")]
public class DieActionSO : EnemyActionSO
{
    public override void Act(EnemyController controller)
    {
        // 행동 멈추기 정도?
        //controller.Animation.Play("Die");
    }
}
