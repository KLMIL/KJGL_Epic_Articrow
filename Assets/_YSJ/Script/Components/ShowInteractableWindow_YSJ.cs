using UnityEngine;
using YSJ;

public class ShowInteractableWindow_YSJ : MonoBehaviour
{
    GameObject window;
    float distance = 2f;

    void Start()
    {
        window = Resources.Load<GameObject>("F");
        window = Instantiate(window, transform);
        window.SetActive(false);
        window.transform.localPosition = new Vector3(1, 1, 0);
    }

    void Update()
    {
        foreach (Collider2D col in Physics2D.OverlapCircleAll(transform.position, distance)) 
        {
            if (col.TryGetComponent<PlayerStatus>(out PlayerStatus player))
            {
                window.SetActive(true);
                break;
            }
            else 
            {
                window.SetActive(false);
            }
        }
    }
}
