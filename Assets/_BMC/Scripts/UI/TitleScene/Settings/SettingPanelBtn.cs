using UnityEngine;
using UnityEngine.UI;

public class SettingPanelBtn : MonoBehaviour
{
    Button _btn;
    [SerializeField] Image _image;
    [SerializeField] int _index = -1;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        _index = transform.GetSiblingIndex();
        _btn = GetComponent<Button>();
        _image = transform.GetChild(0).GetComponent<Image>();
        _btn.onClick.AddListener(OnClickBtn);
        UI_TitleEventBus.OnActivePanelBtnHighlightLine += ActiveHighlightLine;
    }

    void OnClickBtn()
    {
        UI_TitleEventBus.OnActivePanelBtnHighlightLine?.Invoke(_index);
        UI_TitleEventBus.OnActivePanelCanvas?.Invoke(_index);
    }

    void ActiveHighlightLine(int idx)
    {
        _image.enabled = false;
        _image.gameObject.SetActive(false);

        if(_index == idx)
        {
            _image.enabled = true;
            _image.gameObject.SetActive(true);
        }
    }
}