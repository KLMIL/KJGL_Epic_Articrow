using UnityEngine;
using UnityEngine.UI;

public class FieldParts_YSJ : MonoBehaviour
{
    public GameObject ImageParts;

    private void Awake()
    {
        GameObject[] goArray = Resources.LoadAll<GameObject>("NewImageParts");
        int random = Random.Range(0, goArray.Length);
        ImageParts = goArray[random];

        GetComponent<SpriteRenderer>().sprite = ImageParts.GetComponent<Image>().sprite;
    }
}
