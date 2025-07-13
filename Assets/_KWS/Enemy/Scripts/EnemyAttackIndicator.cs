using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyAttackIndicator : MonoBehaviour
    {
        List<GameObject> _rendererPrefabs = new();
        Coroutine _blinkCoroutine = null;
        Color _originColor;


        public void SetIndicators(IndicatorType type, List<Vector2> dirs, List<Vector2> lens, GameObject prefab)
        {
            Hide();

            switch (type)
            {
                case IndicatorType.Line:
                    {
                        int count = dirs.Count;
                        for (int i = 0; i < count; i++)
                        {
                            GameObject indicator = Instantiate(prefab, transform);
                            LineRenderer lr = indicator.GetComponent<LineRenderer>();

                            Vector2 origin = (Vector2)transform.position;
                            Vector2 direction = dirs[i].normalized;
                            float maxDistance = lens[i].x;

                            float finalDistance = maxDistance;
                            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, LayerMask.GetMask("Obstacle"));
                            if (hit.collider != null)
                            {
                                finalDistance = hit.distance;
                            }


                            lr.positionCount = 2;
                            lr.SetPosition(0, origin);
                            //lr.SetPosition(1, (Vector2)transform.position + dirs[i].normalized * lens[i].x);
                            lr.SetPosition(1, origin + direction * finalDistance);

                            lr.startWidth = lens[i].y;
                            lr.endWidth = lens[i].y;

                            _rendererPrefabs.Add(indicator);
                        }

                        break;
                    }
                case IndicatorType.Circle:
                    {
                        int segments = 64; // 인디케이터를 구성하는 조각 수
                        float angleStep = 360f / segments;
                        float radius = lens[0].x; // 첫 값을 반지름으로
                        
                        Vector2 origin = (Vector2)transform.position;

                        for (int i = 0; i < segments; i++)
                        {
                            float angle = i * angleStep;
                            Vector2 dir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
                            float maxDist = radius;
                            float dist = maxDist;

                            // Raycast로 벽까지 거리 체크
                            RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxDist, LayerMask.GetMask("Obstacle"));
                            if (hit.collider != null)
                                dist = hit.distance;
                            

                            GameObject indicator = Instantiate(prefab, transform);
                            LineRenderer lr = indicator.GetComponent<LineRenderer>();

                            lr.positionCount = 2;

                            lr.SetPosition(0, origin);
                            lr.SetPosition(1, origin + dir * dist);

                            float width = (2 * Mathf.PI * dist) / segments;

                            lr.startWidth = 0;
                            lr.endWidth = width;

                            _rendererPrefabs.Add(indicator);
                        }

                        break;
                    }
                case IndicatorType.Cone:
                    {
                        Vector2 origin = (Vector2)transform.position;
                        Vector2 leftDir = dirs[0].normalized;
                        Vector2 rightDir = dirs[1].normalized;
                        float maxDist = lens[0].x;

                        float coneAngle = Vector2.Angle(leftDir, rightDir);

                        float stepAngle = 1f;
                        int segments = Mathf.Max(2, Mathf.CeilToInt(coneAngle / stepAngle));


                        Vector2 forward = (leftDir + rightDir).normalized;

                        float startAngle = Vector2.SignedAngle(forward, leftDir);

                        for (int i = 0; i < segments; i++)
                        {
                            float t = (segments == 1) ? 0.5f : (float)i / (segments - 1);
                            float angle = Mathf.Lerp(-coneAngle / 2, coneAngle / 2, t);

                            Vector2 dir = Quaternion.Euler(0, 0, angle) * forward;

                            float dist = maxDist;
                            RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxDist, LayerMask.GetMask("Obstacle"));
                            if (hit.collider != null)
                            {
                                dist = hit.distance;
                            }

                            GameObject indicator = Instantiate(prefab, transform);
                            LineRenderer lr = indicator.GetComponent<LineRenderer>();

                            lr.positionCount = 2;
                            lr.SetPosition(0, origin);
                            lr.SetPosition(1, origin + dir * dist);

                            float width = Mathf.Deg2Rad * stepAngle * dist;
                            //(2 * Mathf.PI * dist) / segments;

                            lr.startWidth = 0;
                            lr.endWidth = width;

                            _rendererPrefabs.Add(indicator);
                        }

                        break;
                    }
            }


            // Blink를 위해 기본 원본 색 저장
            if (_rendererPrefabs.Count > 0 && _rendererPrefabs[0] != null)
            {
                _originColor = _rendererPrefabs[0].GetComponent<LineRenderer>().startColor;
            }
        }


        public void DEP_SetIndicators(List<Vector2> dirs, List<Vector2> lens, GameObject prefab)
        {
            Hide();

            int count = dirs.Count;
            for (int i = 0; i < count; i++)
            {
                GameObject indicator = Instantiate(prefab, transform);

                Vector2 dir = dirs[i];
                Vector2 scale = lens[i];

                //Debug.LogError($"인디케이터 생성 {i}번째: ({dirs[i].x}, {dirs[i].y})");

                indicator.transform.right = dir;
                indicator.transform.localScale = new Vector3(scale.x, scale.y, 1f);

                _rendererPrefabs.Add(indicator);
            }

            _originColor = prefab.GetComponentInChildren<SpriteRenderer>().color;
        }


        private void Hide()
        {
            if (_rendererPrefabs == null || _rendererPrefabs.Count == 0) return;

            // 인디케이터 제거
            foreach (var p in _rendererPrefabs)
            {
                Destroy(p);
            }
            _rendererPrefabs.Clear();
            _originColor = Color.white;
        }

        public void BlinkAndHide(float duration = 0.1f, int count = 2)
        {
            if (_rendererPrefabs == null || _rendererPrefabs.Count == 0) return;

            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
            }
            _blinkCoroutine = StartCoroutine(BlinkCoroutine(duration, count));
        }

        private IEnumerator BlinkCoroutine(float duration, int count)
        {
            if (_rendererPrefabs == null || _rendererPrefabs.Count == 0) yield break;

            //Color originColor = _renderer.color;
            Color blinkColor = Color.white;

            for (int i = 0; i < count; i++)
            {
                // 1. 모든 SpriteRenderer를 흰색으로
                foreach (var p in _rendererPrefabs)
                {
                    //var srs = p.GetComponentsInChildren<SpriteRenderer>();
                    //foreach (var sr in srs)
                    //    sr.color = blinkColor;
                    var lr = p.GetComponent<LineRenderer>();
                    if (lr != null)
                    {
                        lr.startColor = blinkColor;
                        lr.endColor = blinkColor;
                    }
                }
                yield return new WaitForSeconds(duration / (count * 2));

                // 2. 원래 색으로 복구
                foreach (var p in _rendererPrefabs)
                {
                    //var srs = p.GetComponentsInChildren<SpriteRenderer>();
                    //foreach (var sr in srs)
                    //    sr.color = _originColor;
                    var lr = p.GetComponent<LineRenderer>();
                    if (lr != null)
                    {
                        lr.startColor = _originColor;
                        lr.endColor = _originColor;
                    }
                }
                yield return new WaitForSeconds(duration / (count * 2));
            }

            // 마지막에 다시 원상복귀(혹시 이상하게 깜빡였을 때)
            foreach (var p in _rendererPrefabs)
            {
                //var srs = p.GetComponentsInChildren<SpriteRenderer>();
                //foreach (var sr in srs)
                //    sr.color = _originColor;
                var lr = p.GetComponent<LineRenderer>();
                if (lr != null)
                {
                    lr.startColor = _originColor;
                    lr.endColor = _originColor;
                }
            }

            Hide();
        }

        public void StopCoroutines()
        {
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
            }
        }
    }
}
