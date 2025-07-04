using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InteractObject_YSJ))]

public class FieldParts_YSJ : MonoBehaviour
{
    string PartsName;
    public GameObject ConnectedImageParts { get; private set; }

    private void Awake()
    {
        //GameObject[] goArray = Resources.LoadAll<GameObject>("NewImageParts");
        //int random = Random.Range(0, goArray.Length);
        //ConnectedImageParts = goArray[random];

        //GetComponent<SpriteRenderer>().sprite = ConnectedImageParts.GetComponent<Image>().sprite;

        string originalName = gameObject.name;
        // FieldParts_ 때고 이름 저장하기
        PartsName = originalName.StartsWith("FieldParts_") ? originalName.Substring("FieldParts_".Length) : originalName;
        PartsName = PartsName.Replace("(Clone)", "");
        // 이름으로 ImageParts찾기
        ConnectedImageParts = Resources.Load<GameObject>("NewImageParts/ImageParts_" + PartsName);
    }
}
