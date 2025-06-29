using System.Collections;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyAttackIndicator : MonoBehaviour
    {
        SpriteRenderer _renderer;
        Coroutine _blinkCoroutine = null;

        private void Awake()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            if (_renderer == null)
            {
                Debug.LogWarning($"{gameObject.name}: SpriteRenderer missing");
                return;
            }
            Hide();
        }

        public void SetDirection(Vector2 dir, float len)
        {
            if (_renderer == null) return;
            if (dir == Vector2.zero) return;

            transform.right = dir;
            transform.localScale = new Vector2(len, transform.localScale.y);
        }

        public void Show()
        {
            if (_renderer == null) return;
            _renderer.enabled = true;
        }

        public void Hide()
        {
            if (_renderer == null) return;
            _renderer.enabled = false;
        }

        /// <summary>
        /// 인디케이터를 빠르게 깜빡이는 함수. 공격 시작 직전임을 표시.
        /// </summary>
        public void BlinkAndHide(float duration = 0.1f, int count = 2)
        {
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
            }
            _blinkCoroutine = StartCoroutine(BlinkCoroutine(duration, count));
        }

        private IEnumerator BlinkCoroutine(float duration, int count)
        {
            if (_renderer == null) yield break;

            Color originColor = _renderer.color;
            Color blinkColor = Color.white;

            for (int i = 0; i < count; i++)
            {
                _renderer.color = blinkColor;
                yield return new WaitForSeconds(duration / count);
                _renderer.color = originColor;
                yield return new WaitForSeconds(duration / count);
            }
            _renderer.color = originColor;

            Hide();
        }
    }
}
