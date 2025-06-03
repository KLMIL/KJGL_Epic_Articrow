using System.Collections;
using UnityEngine;

public class Trace : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(ActiveTimerCoroutine());
    }

    IEnumerator ActiveTimerCoroutine()
    {
        yield return new WaitForSeconds(0.6f);
        this.gameObject.SetActive(false);
    }
}
