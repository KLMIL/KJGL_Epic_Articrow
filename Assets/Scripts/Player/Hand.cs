using UnityEngine;
using UnityEngine.Rendering;

namespace YSJ
{
    public class Hand : MonoBehaviour
    {
        Camera _camera;
        Transform _body;

        private void FixedUpdate()
        {
            SpriteSort();
            SpriteRotation();
        }

        protected void SpriteSort()
        {
            _body = _body ?? transform.root.GetComponentInChildren<Animator>().transform;

            if (!_body)
            {
                Debug.LogError("손의 부모 없음!");
            }

            if (TryGetComponent<SortingGroup>(out SortingGroup sortingGroup) && _body.TryGetComponent<SpriteRenderer>(out SpriteRenderer bodySprite))
            {
                bool isRight = (this.transform.position.x >_body.position.x);
                float handY = this.transform.position.y + (isRight ? 0.4f : -0.1f);

                if (handY > _body.position.y)
                {
                    sortingGroup.sortingOrder = bodySprite.sortingOrder - 1;
                }
                else if (handY < _body.position.y)
                {
                    sortingGroup.sortingOrder = bodySprite.sortingOrder + 1;
                }
            }
            else
            {
                Debug.LogError("손 또는 부모의 스프라이트랜더러 없음!");
            }
        }

        protected void SpriteRotation()
        {
            _camera = _camera ?? Camera.main;

            Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (mousePos - (Vector2)this.transform.position).normalized;
            float angleZ = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

            this.transform.rotation = Quaternion.Euler(0, 0, angleZ - 90f);
        }
    }
}