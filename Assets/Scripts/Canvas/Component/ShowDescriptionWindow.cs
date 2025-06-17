using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YSJ
{
    public class ShowDescriptionWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [TextArea(5, 10)]
        public string Description;

        RectTransform _descriptionPanel;
        //Image _img_Panel;
        //TextMeshProUGUI _tmp_Description;

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
            _descriptionPanel = Managers.TestPool.Get<RectTransform>(Define.PoolID.Description);
            _descriptionPanel.transform.SetParent(this.transform);
            _descriptionPanel.position = this.transform.GetComponent<RectTransform>().position;

            TextMeshProUGUI tmp_Description = _descriptionPanel.GetComponentInChildren<TextMeshProUGUI>();
            tmp_Description.text = Description;

            //SetDescription(true, Description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Managers.TestPool.Return(Define.PoolID.Description, _descriptionPanel.gameObject);
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