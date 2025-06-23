using UnityEngine;

namespace Game.Enemy
{
    public class EnemyAttackIndicator : MonoBehaviour
    {
        SpriteRenderer _renderer;

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
    }
}
