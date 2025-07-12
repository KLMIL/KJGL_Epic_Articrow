using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/*
 * 몬스터의 Attack Ready 상태. 아무것도 하지 않음.
 */
namespace Game.Enemy
{
    public enum IndicatorType
    {
        Line, Circle, Cone
    }

    public enum AttackType
    {
        Melee_Basic = 100,
        Melee_Rush,
        Melee_Smash,

        Projectile_ScatterLinearAttack = 200,
        Projectile_LinearAttack,
        Projectile_LinearSpawn,
        Projectile_ParabolaAttack,
        Projectile_ParabolaSpawn,

        Special_Summon = 300,
        Special_Boom
    }

    [CreateAssetMenu(
    fileName = "AttackReadyAction",
    menuName = "Enemy/Action/State/AttackReady"
)]
    public class AttackReadyActionSO : EnemyActionSO
    {
        public GameObject IndicatorPrefab;
        public IndicatorType IndicatorType;
        public AttackType AttackType;
        public Vector2 IndicatorScale;
        public bool AlwaysMaxScale = false;

        public float angle = 0;
        public int count = 1;

        public float duration = 0.7f;

        public override void OnEnter(EnemyController controller)
        {
            controller.FSM.isSuperArmor = true;

            // 목표 위치 갱신 및 인디케이터 프리펩 미할당시 반환
            controller.FSM.AttackTargetPosition = controller.Player.position;
            if (IndicatorPrefab == null) return;

            // 크기 선언 및 기본값 할당
            Vector2 scale = IndicatorScale;
            float dist = (controller.Player.position - controller.transform.position).magnitude;// * 2;

            // 크기 결정
            switch  (IndicatorType)
            {
                case IndicatorType.Line:
                    float hOrigin = scale.y;
                    scale = AlwaysMaxScale ? IndicatorScale : Vector2.one * dist;
                    scale.y = hOrigin;
                    break;
                case IndicatorType.Circle:
                case IndicatorType.Cone:
                    scale = IndicatorScale;
                    break;
            }

            controller.FSM.IndicatorScale = scale;

            // 기준 방향 계산
            //Vector2 standardDir = (controller.FSM.AttackTargetPosition - (Vector2)controller.transform.position).normalized;
            Vector2 standardDir = (controller.FSM.AttackTargetPosition - (Vector2)controller.AttackIndicator.transform.position).normalized;

            List<Vector2> dirs = new();
            List<Vector2> lens = new();

            // 공격 타입에 따라 프리펩 생성
            switch (AttackType)
            {
                case AttackType.Melee_Basic: break;
                case AttackType.Melee_Rush:
                    dirs.Add(standardDir);
                    lens.Add(scale);
                    break;
                case AttackType.Melee_Smash:
                    dirs.Add(standardDir);
                    lens.Add(scale);
                    break;

                case AttackType.Projectile_ScatterLinearAttack:
                    {
                        float spreadAngle = 0;
                        float halfCount = (count - 1) * 0.5f;

                        dirs.Add(standardDir);
                        lens.Add(scale);
                        for (int i = 0; i < halfCount; i++)
                        {
                            spreadAngle += angle;

                            Vector2 newDir = Quaternion.Euler(0, 0, spreadAngle) * standardDir;
                            dirs.Add(newDir);
                            lens.Add(scale);

                            newDir = Quaternion.Euler(0, 0, -spreadAngle) * standardDir;
                            dirs.Add(newDir);
                            lens.Add(scale);
                        }
                    }
                    break;
                case AttackType.Projectile_LinearAttack:
                    dirs.Add(standardDir);
                    lens.Add(scale);
                    break;
                case AttackType.Projectile_LinearSpawn: break;
                case AttackType.Projectile_ParabolaAttack: break;
                case AttackType.Projectile_ParabolaSpawn: break;

                case AttackType.Special_Summon: break;
                case AttackType.Special_Boom:
                    dirs.Add(standardDir);
                    lens.Add(scale);
                    break;
            }

            //controller.AttackIndicator.SetIndicators(IndicatorType.Line, dirs, lens, IndicatorPrefab);
            // 인디케이터 생성 코루틴 호출 -> 몬스터 따라가면서 계속 갱신
            if (controller.IsIndicatorCoroutineGo()) // 코루틴이 실행중이라면 중지
            {
                controller.StopCoroutine(controller.GetIndicatorCoroutine());
                controller.EndIndicatorCoroutine();
            }
            controller.StartIndicatorCoroutine(controller.StartCoroutine(UpdateIndicatorCoroutine(controller, IndicatorType.Line, dirs, lens, IndicatorPrefab, duration)));
        }

        private IEnumerator BlinkAndHideCoroutine(EnemyController controller, float duration)
        {
            yield return new WaitForSeconds(duration);

            controller.AttackIndicator.BlinkAndHide();
        }

        private IEnumerator UpdateIndicatorCoroutine(EnemyController controller, IndicatorType type, List<Vector2> dirs, List<Vector2> lens, GameObject IndicatorPrefab, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration - 0.1f)
            {
                controller.AttackIndicator.SetIndicators(IndicatorType.Line, dirs, lens, IndicatorPrefab);
                elapsed += Time.deltaTime;
                yield return null;
            }

            controller.StartCoroutine(BlinkAndHideCoroutine(controller, 0.1f));
        }

        public override void Act(EnemyController controller)
        {
            /* Do Nothing */
        }
    }
}
