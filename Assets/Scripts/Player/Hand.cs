using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

namespace YSJ
{
    public class Hand : MonoBehaviour
    {
        Camera _camera;

        private void Update()
        {
            SpriteSort();
            SpriteRotation();
        }

        protected void SpriteSort()
        {
            Transform parent = transform.root;

            if (!parent)
            {
                Debug.LogError("손의 부모 없음!");
            }

            if (TryGetComponent<SortingGroup>(out SortingGroup sortingGroup) && parent.TryGetComponent<SpriteRenderer>(out SpriteRenderer parentSprite))
            {
                float handY = this.transform.position.y + 0.2f;

                if (handY > parent.position.y)
                {
                    sortingGroup.sortingOrder = parentSprite.sortingOrder - 1;
                }
                else if (handY < parent.position.y)
                {
                    sortingGroup.sortingOrder = parentSprite.sortingOrder + 1;
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