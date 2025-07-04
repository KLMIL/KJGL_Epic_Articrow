using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YSJ
{
    /// <summary>
    /// UI 슬롯에서의 아이템 설명을 보여주는 클래스
    /// </summary>
    public class ShowDescriptionWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [TextArea(5, 10)]
        public string Description;

        RectTransform _descriptionPanel;
        //Image _img_Panel;
        //TextMeshProUGUI _tmp_Description;

        Inventory_YSJ _inventory;   // 인벤토리

        void Awake()
        {
            _inventory = transform.GetComponentInParent<Inventory_YSJ>();
        }


        private void Start()
        {
            /*GameObject description = Instantiate(Resources.Load<GameObject>("Prefabs/Description"));
            description.transform.SetParent(this.transform);

            RectTransform rect = description.GetComponent<RectTransform>();
            rect.position = this.transform.GetComponent<RectTransform>().position;

            _img_Panel = description.GetComponentInChildren<Image>();
            _tmp_Description = description.GetComponentInChildren<TextMeshProUGUI>();

            SetDescription(false, "");*/
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // 0. Window 준비
            _descriptionPanel = Managers.Pool.Get<RectTransform>(Define.PoolID.Description);

            // 1. Window 배치
            // 1-1) 인벤토리 다음에 위치해서 슬롯을 가리지 않음
            int hierarchyIdx = _inventory.transform.GetSiblingIndex();
            _descriptionPanel.transform.SetParent(_inventory.transform.parent);
            _descriptionPanel.transform.SetSiblingIndex(hierarchyIdx + 1); // 인벤토리 뒤에 위치

            // 1-2) 슬롯의 RectTransform을 가져와서 위치 설정
            _descriptionPanel.position = this.transform.parent.GetComponent<RectTransform>().position;

            // 2. Window 문구 설정
            TextMeshProUGUI tmp_Description = _descriptionPanel.GetComponentInChildren<TextMeshProUGUI>();
            tmp_Description.text = Description;

            //SetDescription(true, Description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Managers.Pool.Return(Define.PoolID.Description, _descriptionPanel.gameObject);
            //SetDescription(false, "");
        }

        /*void SetDescription(bool value, string description)
        {
            if (_img_Panel == null)
            {
                Debug.LogError("Description Panel is null");
                return;
            }

            if (value && (Description == null))
            {
                Debug.LogError("Description string is null");
                return;
            }
            
            _img_Panel.enabled = value;
            _tmp_Description.text = description;
        }*/
    }
}