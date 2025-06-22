using UnityEngine;
using UnityEngine.UI;

namespace BMC
{
    public class UI_Room : MonoBehaviour
    {
        [SerializeField] int _roomIndex; // 방 인덱스
        Image _roomCell;
        Image _roomTypeIcon;

        void Awake()
        {
            _roomCell = GetComponent<Image>();
            _roomTypeIcon = gameObject.GetComponentInDirectChildren<Image>();
            _roomIndex = transform.GetSiblingIndex();
        }

        // 방 색 설정
        public void SetRoomColor(Color color)
        {
            _roomCell.color = color;
            _roomTypeIcon.color = color;
        }

        // 방 아이콘 설정
        public void SetRoomIcon(Sprite sprite = null)
        {
            if(sprite == null)
            {
                _roomTypeIcon.enabled = false;
                return;
            }
            _roomTypeIcon.sprite = sprite;
        }
    }
}