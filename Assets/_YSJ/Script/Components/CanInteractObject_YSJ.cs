using UnityEngine;
using UnityEngine.SceneManagement;
using YSJ;

public class CanInteractObject_YSJ : MonoBehaviour
{
    float interactDistance = 1f;
    Transform hand;

    private void Start()
    {
        Managers.Input.OnInteractAction += TryInteract;
        hand = transform.GetComponentInChildren<RightHand>().transform; 
    }

    // 주변에 먹을 것 좀 있나 탐색
    void TryInteract()
    {
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
        if (interactItem.TryGetComponent<Parts_YSJ>(out Parts_YSJ parts))
        {
            if (Managers.UI.InventoryCanvas.inventory.TryAddItem(parts.ImageParts))
            {
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

            // 빈손이면
            if (hand.childCount == 0)
            {
                // 상호작용 비활성화
                interactItem.enabled = false;

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
            }

            artifact.UpdateEnhance();
        }
    }

    void Equip(Artifact_YSJ artifact)
    {
        artifact.currentHand = GetComponentInChildren<PlayerHand>();
        artifact.UpdateEnhance();
        artifact.isCanAttack = true;

        Transform artifactTransform = artifact.transform;
        artifactTransform.GetComponent<InteractObject_YSJ>().enabled = false;
        artifactTransform.GetComponent<Collider2D>().enabled = false;

        artifactTransform.SetParent(hand.transform, false);
        artifactTransform.localPosition = Vector3.zero;
        artifactTransform.localRotation = Quaternion.identity;

        Managers.UI.InventoryCanvas.ArtifactWindow.RemoveAllSlotUI();
        Managers.UI.InventoryCanvas.ArtifactWindow.SendInfoToUI(artifact);
    }

    void Unequip() 
    {
        Transform currentArtifacttransform = hand.GetChild(0);
        Artifact_YSJ currentArtifact = currentArtifacttransform.GetComponent<Artifact_YSJ>();

        currentArtifact.currentHand.CanHandling = true;

        if (currentArtifact.normalAttackCoroutine != null)
        {
            currentArtifact.StopCoroutine(currentArtifact.normalAttackCoroutine);
            currentArtifact.normalAttackCoroutine = null;
        }
        if (currentArtifact.skillAttackCoroutine != null)
        {
            currentArtifact.StopCoroutine(currentArtifact.skillAttackCoroutine);
            currentArtifact.skillAttackCoroutine = null;
        }

        currentArtifacttransform.GetComponent<InteractObject_YSJ>().enabled = true;
        currentArtifacttransform.GetComponent<Collider2D>().enabled = true;
        currentArtifacttransform.SetParent(null);
        currentArtifact.transform.rotation = Quaternion.identity;
        SceneManager.MoveGameObjectToScene(currentArtifacttransform.gameObject, SceneManager.GetActiveScene());
    }
}