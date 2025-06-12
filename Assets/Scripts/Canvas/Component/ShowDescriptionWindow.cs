using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YSJ
{
    public class ShowDescriptionWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string Description;

        Image _img_Panel;
        TextMeshProUGUI _tmp_Description;

        private void Start()
        {
            GameObject description = Instantiate(Resources.Load<GameObject>("Prefabs/Description"));
            description.transform.SetParent(this.transform);

            RectTransform rect = description.GetComponent<RectTransform>();
            rect.position = this.transform.GetComponent<RectTransform>().position;

            _img_Panel = description.GetComponentInChildren<Image>();
            _tmp_Description = description.GetComponentInChildren<TextMeshProUGUI>();

            SetDescription(false, "");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetDescription(true, Description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetDescription(false, "");
        }

        void SetDescription(bool value, string description)
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
        }
    }
}