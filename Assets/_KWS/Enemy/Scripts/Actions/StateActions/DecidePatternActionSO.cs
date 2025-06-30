using UnityEngine;

/*
 * 지정된 패턴 중, 랜덤으로 선택
 */
namespace Game.Enemy
{
    [CreateAssetMenu(
        fileName = "DecidePatternAction",
        menuName = "Enemy/Action/State/Decide Pattern"
    )]
    public class DecidePatternActionSO : EnemyActionSO
    {


        public override void Act(EnemyController controller)
        {
            
        }
    }
}

