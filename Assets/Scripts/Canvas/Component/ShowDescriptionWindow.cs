using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YSJ
{
    public class ShowDescriptionWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string Description;
        GameObject _descriptionWindow;
        Canvas _canvas;

        GameObject _spawnedObj;

        private void Awake()
        {
            _descriptionWindow = Resources.Load<GameObject>("Canvas/DescriptionWindow");
            _canvas = transform.root.GetComponent<Canvas>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Input.GetKey(KeyCode.Mouse0) && _spawnedObj == null)
            {
                _spawnedObj = Instantiate(_descriptionWindow);
                _spawnedObj.transform.SetParent(transform.root, true);
                _spawnedObj.transform.position = transform.position + Vector3.right * 300f;
                _spawnedObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Description;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_spawnedObj)
            {
                Destroy(_spawnedObj);
                _spawnedObj = null;
            }
        }
    }
}