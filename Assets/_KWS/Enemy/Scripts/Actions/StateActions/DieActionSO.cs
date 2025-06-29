using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * 몬스터가 죽을 때 -> 액션 종료시 오브젝트 제거
 */
namespace Game.Enemy
{
    [CreateAssetMenu(
    fileName = "DieAction",
    menuName = "Enemy/Action/State/Die"
)]
    public class DieActionSO : EnemyActionSO
    {
        float deadDuration = 1.0f;
        Color deadColor = new Color(0.7f, 0.7f, 0.7f);

        public override void Act(EnemyController controller)
        {
            controller.FSM.isSuperArmor = false;

            if (!controller.FSM.isDied)
            {
                controller.FSM.isDied = true;

                // TODO: 몬스터가 죽을 때마다 Wave 진행해야하는지 확인
                BMC.EnemySpawner.OnEnemyDie?.Invoke();

                // 죽는 소리 재생
                YSJ.Managers.Sound.PlaySFX(Define.SFX.sfx_slime_die);

                // 몬스터가 죽을 때 카메라 흔들림
                GameManager.Instance.CameraController.ShakeCamera(0.5f, 0.1f);

                // 콜라이더 및 트리거 비활성화
                foreach (var collider in controller.GetComponents<Collider2D>())
                {
                    collider.enabled = false;
                }
                controller.StopMove();

                // 몬스터 스프라이트 색 변경
                controller.SpriteRenderer.color = deadColor;

                // 몬스터 넉백 방향 움직임 -> DEP
                //Vector2 knockBackDir = (controller.transform.position - controller.Player.position).normalized;
                //controller.StartCoroutine(DeadKnockback(controller, deadDuration / 3));

                // 몬스터 죽음 루틴 시작
                controller.StartCoroutine(DeadSequence(controller, deadDuration));

                // 오브젝트 제거 예약 및 사운드 재생
                Object.Destroy(controller.gameObject, deadDuration);
            }
        }

        private IEnumerator DeadSequence(EnemyController controller, float deadDuration)
        {
            // 넉백
            yield return controller.StartCoroutine(DeadKnockback(controller, deadDuration / 3));

            // 투명화
            yield return controller.StartCoroutine(FadeOut(controller, deadDuration * 2 / 3));

            // 제거
            Object.Destroy(controller.gameObject);
        }

        private IEnumerator DeadKnockback(EnemyController controller, float duration)
        {
            if (controller == null || controller.Player == null)
            {
                yield break;
            }

            // 넉백 방향 계산
            Vector2 knockbackDir = (controller.transform.position - controller.Player.position).normalized;
            float maxDistance = 1.0f;
            float skin = 0.02f;

            // 넉백 거리 계산 -> 넉백 방향으로 TilemapCollider까지의 거리 검출해서 작은 값 선택
            RaycastHit2D hit = Physics2D.Raycast(
                    controller.transform.position,
                    knockbackDir,
                    maxDistance + skin
                );

            float moveDistance = maxDistance;
            if (hit.collider != null)
            {
                if (hit.collider is TilemapCollider2D)
                {
                    moveDistance = Mathf.Min(hit.distance - skin, moveDistance);
                }
            }

            Vector2 start = controller.transform.position;
            Vector2 end = start + (Vector2)(knockbackDir * moveDistance);

            // duration동안 해당 거리 이동
            float elapsed = 0f;
            while (elapsed < duration)
            {
                controller.transform.position = Vector2.Lerp(start, end, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            controller.transform.position = end;
        }

        private IEnumerator FadeOut(EnemyController controller, float duration)
        {
            Color startColor = controller.SpriteRenderer.color;
            float elasped = 0f;

            while (elasped < duration)
            {
                float t = elasped / duration;
                Color fadedColor = startColor;
                fadedColor.a = Mathf.Lerp(1f, 0f, t);
                controller.SpriteRenderer.color = fadedColor;

                elasped += Time.deltaTime;
                yield return null;
            }

            Color finalColor = startColor;
            finalColor.a = 0f;
            controller.SpriteRenderer.color = finalColor;
        }
    }
}
