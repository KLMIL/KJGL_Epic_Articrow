using UnityEngine;

public class ConnectFieldParts_YSJ : MonoBehaviour
{
    string PartsName;
    public GameObject ConnectedFieldParts { get; private set; }

    private void Awake()
    {
        string originalName = gameObject.name;

        // ImageParts_때고 이름저장하기
        PartsName = originalName.StartsWith("ImageParts_") ? originalName.Substring("ImageParts_".Length) : originalName;
        PartsName = PartsName.Replace("(Clone)", "");
        // 이름으로 FieldParts찾기
        ConnectedFieldParts = Resources.Load<GameObject>("NewFieldParts/FieldParts_" + PartsName);
        if (!ConnectedFieldParts) 
        {
            print("필드파츠 못찾음! : " + PartsName);
        }
    }
}
