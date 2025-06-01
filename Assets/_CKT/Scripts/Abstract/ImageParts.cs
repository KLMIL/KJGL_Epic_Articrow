using UnityEngine;

public abstract class ImageParts : MonoBehaviour
{
    GameObject _fieldParts;

    protected virtual void Init(string name)
    {
        _fieldParts = Resources.Load<GameObject>(name);
    }

    //슬롯에서 필드로 버리기
    public virtual void ThrowAway()
    {
        //필드 파츠 생성
        GameObject item = Instantiate(_fieldParts);
        item.transform.SetParent(null);

        // TODO: 더미 플레이어로 변경함 확인바람
        //Vector3 playerPos = FindAnyObjectByType<PlayerController>().transform.position;
        Vector3 playerPos = FindAnyObjectByType<BMC.DummyPlayerController>().transform.position;
        item.transform.position = playerPos + Vector3.down;
        
        Destroy(this.gameObject);
    }
}
