using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum ManaStatus
{
    Empty,
    Half,
    Full
}

public class ManaHeart : MonoBehaviour
{
    public Sprite fullMana, halfMana, emptyMana;
    Image _manaImage;

    private void Awake()
    {
        _manaImage = GetComponent<Image>();
    }

    private void Start()
    {
        YSJ.Managers.UI.OnManaHeartFlickerEvent += ManaLack;
    }

    private void OnDisable()
    {
        YSJ.Managers.UI.OnManaHeartFlickerEvent -= ManaLack;

        if (_manaLackCoroutine != null)
        {
            StopCoroutine(_manaLackCoroutine);
            _manaLackCoroutine = null;
        }
    }

    public void SetHeartImage(ManaStatus status)
    {
        switch (status)
        {
            case ManaStatus.Empty:
                _manaImage.sprite = emptyMana;
                break;
            case ManaStatus.Half:
                _manaImage.sprite = halfMana;
                break;
            case ManaStatus.Full:
                _manaImage.sprite = fullMana;
                break;
            default:
                break;
        }
    }

    #region [ManaLack]
    Coroutine _manaLackCoroutine = null;
    int _flickerCount = 3; //깜박임 횟수
    float _flickerTime = 0.25f; //깜박임 1회 시간

    void ManaLack()
    {
        _manaLackCoroutine = _manaLackCoroutine ?? StartCoroutine(ManaLackCoroutine());
    }

    IEnumerator ManaLackCoroutine()
    {
        for (int i = 0; i < _flickerCount; i++)
        {
            _manaImage.enabled = false;

            yield return new WaitForSeconds(_flickerTime);
            _manaImage.enabled = true;

            yield return new WaitForSeconds(_flickerTime);
        }

        _manaLackCoroutine = null;
    }
    #endregion
}