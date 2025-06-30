using UnityEngine;

public class ArtifactWindow_YSJ : MonoBehaviour
{
    Transform ArtifactIMG;
    Transform ArtifactSlotWindow;

    public GameObject ArtifactSlotUIPrefab;

    void Awake()
    {
        GetComponentInParent<InventoryCanvas_YSJ>().ArtifactWindow = this;

        ArtifactIMG = transform.GetChild(0);
        ArtifactSlotWindow = transform.GetChild(1);
    }

    public void RemoveAllSlotUI() 
    {
        for (int i = ArtifactSlotWindow.childCount - 1; i >= 0; i--)
        {
            Destroy(ArtifactSlotWindow.GetChild(i).gameObject);
        }
    }

    public void CreateSlotUI(Artifact_YSJ equipedArtifact)
    {
        for (int i = 0; i < equipedArtifact.SlotTransform.childCount; i++) 
        {
            GameObject SpawnedSlot = Instantiate(ArtifactSlotUIPrefab, ArtifactSlotWindow);
            SpawnedSlot.name = "EmptyArtifactSlot";

            ArtifactSlotUI_YSJ ArtifactSlot = SpawnedSlot.GetComponent<ArtifactSlotUI_YSJ>();
            ArtifactSlot.CurrentArtifact = equipedArtifact;
            ArtifactSlot.SlotIndex = i;
        }
    }
}
