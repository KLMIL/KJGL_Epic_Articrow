using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YSJ
{
    public class ShowDescriptionWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string Description;
        GameObject descriptionWindow;
        Canvas canvas;

        GameObject spawnedObj;

        private void Awake()
        {
            descriptionWindow = Resources.Load<GameObject>("Canvas/DescriptionWindow");
            canvas = transform.root.GetComponent<Canvas>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Input.GetKey(KeyCode.Mouse0) && spawnedObj == null)
            {
                spawnedObj = Instantiate(descriptionWindow);
                spawnedObj.transform.SetParent(transform.root, true);
                spawnedObj.transform.position = transform.position + Vector3.right * 300f;
                spawnedObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Description;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (spawnedObj)
            {
                Destroy(spawnedObj);
                spawnedObj = null;
            }
        }
    }
}