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


        public void SetIndicators(List<Vector2> dirs, List<Vector2> lens, GameObject prefab)
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
            if (_rendererPrefabs == null || _rendererPrefabs.Count == 0 ) yield break;

            //Color originColor = _renderer.color;
            Color blinkColor = Color.white;

            for (int i = 0; i < count; i++)
            {
                // 1. 모든 SpriteRenderer를 흰색으로
                foreach (var p in _rendererPrefabs)
                {
                    var srs = p.GetComponentsInChildren<SpriteRenderer>();
                    foreach (var sr in srs)
                        sr.color = blinkColor;
                }
                yield return new WaitForSeconds(duration / (count * 2));

                // 2. 원래 색으로 복구
                foreach (var p in _rendererPrefabs)
                {
                    var srs = p.GetComponentsInChildren<SpriteRenderer>();
                    foreach (var sr in srs)
                        sr.color = _originColor;
                }
                yield return new WaitForSeconds(duration / (count * 2));
            }

            // 마지막에 다시 원상복귀(혹시 이상하게 깜빡였을 때)
            foreach (var p in _rendererPrefabs)
            {
                var srs = p.GetComponentsInChildren<SpriteRenderer>();
                foreach (var sr in srs)
                    sr.color = _originColor;
            }

            Hide();
        }
    }
}
