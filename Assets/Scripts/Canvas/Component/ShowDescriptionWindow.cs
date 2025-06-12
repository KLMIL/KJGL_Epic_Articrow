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

        private void Awake()
        {
            Image[] images = GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i].transform != this.transform)
                {
                    _img_Panel = images[i];
                }
            }

            TextMeshProUGUI[] tmps = GetComponentsInChildren<TextMeshProUGUI>();
            for (int i = 0; i < tmps.Length; i++)
            {
                if (tmps[i].transform != this.transform)
                {
                    _tmp_Description = tmps[i];
                }
            }

            _img_Panel.gameObject.SetActive(false);
            _tmp_Description.text = "";
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _img_Panel.gameObject.SetActive(true);
            _tmp_Description.text = Description;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _img_Panel.gameObject.SetActive(false);
            _tmp_Description.text = "";
        }
    }
}