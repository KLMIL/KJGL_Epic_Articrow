using UnityEngine;

[CreateAssetMenu(fileName = "RushAttackAction", menuName = "Enemy/Action/Rush Attack")]
public class RushAttackActionSO : EnemyActionSO
{
    public float rushSpeed = 6f;
    public float attackDuration = 0.5f;
    float elapsed = 0f;

    public override void Act(EnemyController controller)
    {
        if (controller.Player == null) return;

        if (elapsed < attackDuration)
        {
            Vector3 dir = (controller.Player.position - controller.transform.position).normalized;
            controller.transform.Translate(dir * rushSpeed * Time.deltaTime);
            controller.Animation.Play("RushAttack");
            elapsed += Time.deltaTime;
        }
    }

    public void ResetAttack() => elapsed = 0f;
}
