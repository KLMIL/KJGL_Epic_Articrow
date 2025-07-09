using UnityEngine;
using UnityEngine.UI;

public class ArtifactWindow_YSJ : MonoBehaviour
{
    public Image ArtifactIMG;
    public Transform ArtifactSlotWindow;

    BMC.ArtifactStatusText[] _artifactStatusTests;

    public GameObject ArtifactSlotUIPrefab;

    void Awake()
    {
        GetComponentInParent<InventoryCanvas_YSJ>().ArtifactWindow = this;

        _artifactStatusTests = GetComponentsInChildren<BMC.ArtifactStatusText>();
    }

    public void RemoveAllSlotUI() 
    {
        for (int i = ArtifactSlotWindow.childCount - 1; i >= 0; i--)
        {
            Destroy(ArtifactSlotWindow.GetChild(i).gameObject);
        }
    }

    void CreateSlotUI(Artifact_YSJ equipedArtifact)
    {
        for (int i = 0; i < equipedArtifact.SlotTransform.childCount; i++) 
        {
            GameObject SpawnedSlot = Instantiate(ArtifactSlotUIPrefab, ArtifactSlotWindow);
            SpawnedSlot.name = "ArtifactSlot";
            // 아티팩트 슬롯칸에 파츠가 이미 들어있었으면 복사해서 UI에도 띄워주기
            if (equipedArtifact.SlotTransform.GetChild(i).childCount != 0)
            {
                GameObject partsClone = Instantiate(equipedArtifact.SlotTransform.GetChild(i).GetChild(0).gameObject, SpawnedSlot.transform);
                partsClone.GetComponent<Image>().raycastTarget = true;
            }

            ArtifactSlotUI_YSJ ArtifactSlot = SpawnedSlot.GetComponent<ArtifactSlotUI_YSJ>();
            ArtifactSlot.CurrentArtifact = equipedArtifact;
            ArtifactSlot.SlotIndex = i;
        }
    }

    void ArtifactIMGChange(Artifact_YSJ equipedArtifact)
    {
        if (equipedArtifact.TryGetComponent<SpriteRenderer>(out SpriteRenderer artifactSpriteRenderer)) 
        {
            ArtifactIMG.sprite = artifactSpriteRenderer.sprite;
            ArtifactIMG.color = artifactSpriteRenderer.color;
        }
    }

    void ArtifactInfomationTextUpdate(Artifact_YSJ equipedArtifact)
    {
        // 기본공격 정보
        _artifactStatusTests[0].SetText(equipedArtifact, equipedArtifact.normalStatus);
        // 스킬공격 정보
        _artifactStatusTests[1].SetText(equipedArtifact, equipedArtifact.skillStatus);
    }

    void ResetArtifactInfomationText() 
    {
        _artifactStatusTests[0].Reset();
        _artifactStatusTests[1].Reset();
    }

    public void SendInfoToUI(Artifact_YSJ equipedArtifact)
    {
        ArtifactIMGChange(equipedArtifact);
        CreateSlotUI(equipedArtifact);
        ArtifactInfomationTextUpdate(equipedArtifact);
    }

    public void ResetWindow()
    {
        RemoveAllSlotUI();
        ResetArtifactInfomationText();
        ArtifactIMG.sprite = null;
        ArtifactIMG.color = new Color(1, 1, 1, 0); // 투명하게 설정
    }
}
