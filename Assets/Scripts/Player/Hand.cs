using UnityEngine;
using UnityEngine.Rendering;

namespace YSJ
{
    public class Hand : MonoBehaviour
    {
        //protected CheckPlayerDirection checkDirection;
        Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

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
            /*if (checkDirection)
            {
                transform.rotation = Quaternion.Euler(0, 0, checkDirection.Angle);
            }*/
            
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3 mouseDir = (mousePos - this.transform.position).normalized;
            this.transform.up = mouseDir;
            this.transform.localRotation = Quaternion.Euler(0, 0, this.transform.localRotation.z);
        }
    }
}