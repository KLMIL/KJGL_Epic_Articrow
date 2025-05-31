using UnityEngine;

[CreateAssetMenu(fileName = "RandomMoveAction", menuName = "Enemy/Action/Random Move")]
public class RandomMoveActionSO : EnemyActionSO
{
    public Rect moveBounds = new Rect(-8, -4, 16,  8);

    private Vector3 _randomDirection = Vector3.zero;
    private float _changeDirectionCooldown = 0f;

    public override void Act(EnemyController controller)
    {
        if (_changeDirectionCooldown <= 0f)
        {
            _randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            _changeDirectionCooldown = Random.Range(1f, 3f);
        }

        controller.transform.Translate(_randomDirection * controller.Status.moveSpeed * Time.deltaTime);

        Vector3 pos = controller.transform.position;
        pos.x = Mathf.Clamp(pos.x, moveBounds.xMin, moveBounds.xMax);
        pos.y = Mathf.Clamp(pos.y, moveBounds.yMin, moveBounds.yMax);
        pos.z = 0;
        controller.transform.position = pos;


        controller.Animation.Play("RandomMove");
        _changeDirectionCooldown -= Time.deltaTime;
    }
}
