using TMPro;
using UnityEngine;
using YSJ;

public class ShowInteractableWindow_YSJ : MonoBehaviour
{
    [SerializeField] Vector2 _fPosition;
    SpriteRenderer _sprite;
    TextMeshPro _tmp;

    void Awake()
    {
        GameObject window = Resources.Load<GameObject>("F");
        window = Instantiate(window, transform);
        window.transform.localPosition = _fPosition;

        _sprite = window.GetComponentInChildren<SpriteRenderer>();
        _tmp = window.GetComponentInChildren<TextMeshPro>();

        _sprite.enabled = false;
        _tmp.enabled = false;
    }

    public void ShowWindow()
    {
        _sprite.enabled = true;
        _tmp.enabled = true;
    }
    public void HideWindow()
    {
        _sprite.enabled = false;
        _tmp.enabled = false;
    }
}
