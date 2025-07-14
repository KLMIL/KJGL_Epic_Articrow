using UnityEngine;
using UnityEngine.SceneManagement;
using YSJ;

public class CanInteractObject_YSJ : MonoBehaviour
{
    Transform hand;

    private void Start()
    {
        Managers.Input.OnInteractAction += TryInteract;
        hand = transform.GetComponentInChildren<RightHand>().transform; 
    }

    // 주변에 먹을 것 좀 있나 탐색
    void TryInteract()
    {
        float interactDistance = BMC.PlayerManager.Instance.PlayerStatus.InteractDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactDistance);

        InteractObject_YSJ interactObj = null;

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<InteractObject_YSJ>(out InteractObject_YSJ item) && item.enabled)
            {
                if (interactObj)
                {
                    if (Vector2.Distance(transform.position, interactObj.transform.position) > Vector2.Distance(transform.position, item.transform.position))
                    {
                        interactObj = item;
                    }
                }
                else 
                {
                    interactObj = item;
                }
            }
        }

        if (interactObj)
        {
            interact(interactObj);
        }
        else 
        {
            print("주변 상호작용 아이템 없음!");
        }
    }

    // 상호작용하기
    void interact(InteractObject_YSJ interactItem) 
    {
        // 파츠일때
        if (interactItem.TryGetComponent<FieldParts_YSJ>(out FieldParts_YSJ parts))
        {
            if (Managers.UI.InventoryCanvas.inventory.TryAddItem(parts.ConnectedImageParts))
            {
                Managers.Sound.PlaySFX(Define.SFX.Pickup);
                Destroy(interactItem.gameObject);
            }
            else
            {
                print("템창꽉참!");
            }
        }

        // 아티팩트일때
        else if (interactItem.TryGetComponent<Artifact_YSJ>(out Artifact_YSJ artifact))
        {
            if (!hand) return;

            Managers.Sound.PlaySFX(Define.SFX.Equip);

            // 상호작용 비활성화
            interactItem.enabled = false;

            // 빈손이면
            if (hand.childCount == 0)
            {
                //장착
                Equip(artifact);
            }
            // 아티팩트 들고있으면
            else 
            {
                // 원래 끼고 있던거 장착해제
                Unequip();

                // 새거 장착
                Equip(artifact);

                // 통계
                AnalyticsManager.Instance.analyticsData.countArtifactSwap++;
            }

            artifact.UpdateEnhance();
        }
    }

    void Equip(Artifact_YSJ artifact)
    {
        artifact.CurrentHand = GetComponentInChildren<PlayerHand>();
        artifact.UpdateEnhance();
        artifact.isCanAttack = true;

        Transform artifactTransform = artifact.transform;
        artifactTransform.GetComponent<InteractObject_YSJ>().enabled = false;
        artifactTransform.GetComponent<Collider2D>().enabled = false;

        artifactTransform.SetParent(hand.transform, false);
        artifactTransform.localPosition = Vector3.zero;
        artifactTransform.localRotation = Quaternion.identity;

        Managers.UI.InventoryCanvas.ArtifactWindow.RemoveAllSlotUI();
        Managers.UI.InventoryCanvas.ArtifactWindow.ArtifactWindowUpdate(artifact);
    }

    void Unequip() 
    {
        Transform currentArtifacttransform = hand.GetChild(0);
        Artifact_YSJ currentArtifact = currentArtifacttransform.GetComponent<Artifact_YSJ>();

        currentArtifact.CurrentHand.CanHandling = true;

        if (currentArtifact.normalStatus.attackCoroutine != null)
        {
            currentArtifact.StopCoroutine(currentArtifact.normalStatus.attackCoroutine);
            currentArtifact.normalStatus.attackCoroutine = null;
        }
        if (currentArtifact.skillStatus.attackCoroutine != null)
        {
            currentArtifact.StopCoroutine(currentArtifact.skillStatus.attackCoroutine);
            currentArtifact.skillStatus.attackCoroutine = null;
        }

        currentArtifacttransform.GetComponent<InteractObject_YSJ>().enabled = true;
        currentArtifacttransform.GetComponent<Collider2D>().enabled = true;
        currentArtifacttransform.SetParent(null);
        currentArtifact.transform.rotation = Quaternion.identity;

        // 파츠 인벤에 추가 시도하고 꽉차면 떨구기
        foreach(Transform slot in currentArtifact.GetComponent<Artifact_YSJ>().SlotTransform)
        {
            if (slot.childCount > 0) 
            {
                if (slot.GetChild(0).TryGetComponent<ShowDescriptionWindow>(out ShowDescriptionWindow description)) 
                {
                    description.ReturnPanel();
                }

                // 인벤에 이미지 파츠추가 시도
                if (Managers.UI.InventoryCanvas.inventory.TryAddItem(slot.GetChild(0).gameObject))
                {
                }
                // 추가 못하면 땅바닥에 스폰
                else 
                {
                    print("템창꽉참!");

                    GameObject fieldParts = slot.GetChild(0).GetComponent<ConnectFieldParts_YSJ>().ConnectedFieldParts;
                    GameObject SpawnedFeldParts = Instantiate(fieldParts, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity);
                }
                // 슬롯에 있는 애는 삭제
                Destroy(slot.GetChild(0).gameObject);
            }
        }

        // 돈 디스트로이 해제
        SceneManager.MoveGameObjectToScene(currentArtifacttransform.gameObject, SceneManager.GetActiveScene());
    }

    void OnDestroy()
    {
        Managers.Input.OnInteractAction -= TryInteract;
    }
}