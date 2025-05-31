using UnityEngine;

[CreateAssetMenu(fileName = "RandomMoveAction", menuName = "Enemy/Action/RandomMove")]
public class RandomMoveActionSO : EnemyActionSO
{
    private Vector3 _randomDirection = Vector3.zero;
    private float _changeDirectionCooldown = 0f;

    public override void Act(EnemyController controller)
    {
        if (_changeDirectionCooldown <= 0f)
        {
            _randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            _changeDirectionCooldown = Random.Range(1f, 3f);
        }

        controller.transform.Translate(_randomDirection * controller.Status.moveSpeed * Time.deltaTime);
        controller.Animation.Play("RandomMove");
        _changeDirectionCooldown -= Time.deltaTime;
    }
}
