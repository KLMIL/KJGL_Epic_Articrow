using System.Collections;
using UnityEngine;

public class ExplosionTrace : MonoBehaviour
{
    SpriteRenderer _renderer;
    
    Coroutine _disableCoroutine;
    float _existTime = 5f;

    private void OnEnable()
    {
        _renderer = _renderer ?? GetComponent<SpriteRenderer>();

        _disableCoroutine = StartCoroutine(DisableCoroutine());
    }

    private void OnDisable()
    {
        if (_disableCoroutine != null)
        {
            StopCoroutine(_disableCoroutine);
            _disableCoroutine = null;
        }
    }

    IEnumerator DisableCoroutine()
    {
        float timer = _existTime;
        float deltaAlpha = 1 / _existTime;
        while (timer > 0)
        {
            float alpha = Mathf.Clamp01(timer * deltaAlpha);
            _renderer.color = new Color(1f, 1f, 1f, alpha);
            timer -= Time.deltaTime;
            yield return null;
        }
        this.gameObject.SetActive(false);
        _disableCoroutine = null;
    }
}
