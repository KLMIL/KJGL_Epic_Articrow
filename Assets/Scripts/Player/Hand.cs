using UnityEngine;
using UnityEngine.Rendering;

namespace YSJ
{
    public class Hand : MonoBehaviour
    {
        Transform _body;
        SpriteRenderer _playerSprite;

        PlayerHand _hand; // YSJ

        void Awake()
        {
            _playerSprite = transform.root.GetComponentInChildren<SpriteRenderer>();

            _hand = GetComponentInParent<PlayerHand>();
        }

        void FixedUpdate()
        {
            if (_hand && !_hand.CanHandling) 
            {
                return;
            }

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

            if (TryGetComponent<SortingGroup>(out SortingGroup sortingGroup) && _playerSprite != null)
            {
                bool isRight = (this.transform.position.x >_body.position.x);
                float handY = this.transform.position.y + (isRight ? -0.08f : -0.62f);

                if (handY > _body.position.y)
                {
                    sortingGroup.sortingOrder = _playerSprite.sortingOrder - 1;
                }
                else if (handY < _body.position.y)
                {
                    sortingGroup.sortingOrder = _playerSprite.sortingOrder + 1;
                }
            }
            else
            {
                //Debug.LogError("손 또는 부모의 스프라이트랜더러 없음!");
            }
        }

        protected void SpriteRotation()
        {
            Vector2 mousePos = Managers.Input.MouseWorldPos;
            Vector2 mouseDir = (mousePos - (Vector2)this.transform.position).normalized;
            float angleZ = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angleZ);
        }
    }
}