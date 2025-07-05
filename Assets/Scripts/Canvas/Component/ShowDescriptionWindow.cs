using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

namespace YSJ
{
    /// <summary>
    /// UI 슬롯에서의 아이템 설명을 보여주는 클래스
    /// </summary>
    public class ShowDescriptionWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Localization
        static readonly string ItemDescriptionTable = "ItemDescriptionTable";
        [SerializeField] string _localizationKey;
        #endregion

        RectTransform _descriptionPanel;
        Inventory_YSJ _inventory;   // 인벤토리

        void Awake()
        {
            _inventory = transform.GetComponentInParent<Inventory_YSJ>();
            _localizationKey = GetComponent<ImagePartsRoot_YSJ>().partsName;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // 0. Window 준비
            _descriptionPanel = Managers.Pool.Get<RectTransform>(Define.PoolID.Description);

            // 1. Window 배치
            // 1-1) 인벤토리 다음에 위치하게 해서 슬롯을 가리지 않게 함
            int hierarchyIdx = _inventory.transform.GetSiblingIndex();
            _descriptionPanel.transform.SetParent(_inventory.transform.parent);
            _descriptionPanel.transform.SetSiblingIndex(hierarchyIdx + 1); // 인벤토리 뒤에 위치

            // 1-2) 슬롯의 RectTransform을 가져와서 위치 설정
            _descriptionPanel.position = this.transform.parent.GetComponent<RectTransform>().position;

            // 2. Window 문구 업데이트
            UpdateDescription();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Managers.Pool.Return(Define.PoolID.Description, _descriptionPanel.gameObject);
        }

        // 설명 업데이트
        public void UpdateDescription()
        {
            TextMeshProUGUI description = _descriptionPanel.GetComponentInChildren<TextMeshProUGUI>();
            string localeDescription = LocalizationSettings.StringDatabase.GetLocalizedString(ItemDescriptionTable, _localizationKey, LocalizationSettings.SelectedLocale);
            description.text = localeDescription;
        }
    }
}