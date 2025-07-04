using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactWindow_YSJ : MonoBehaviour
{
    public Image ArtifactIMG;
    public Transform ArtifactSlotWindow;

    TextMeshProUGUI normalStatText;
    TextMeshProUGUI skillStatText;
    TextMeshProUGUI otherStatText;

    public GameObject ArtifactSlotUIPrefab;

    void Awake()
    {
        GetComponentInParent<InventoryCanvas_YSJ>().ArtifactWindow = this;

        normalStatText = GetComponentInChildren<NormalStatText_YSJ>().GetComponent<TextMeshProUGUI>();
        skillStatText = GetComponentInChildren<SkillStatText_YSJ>().GetComponent<TextMeshProUGUI>();
        otherStatText = GetComponentInChildren<OtherStatText_YSJ>().GetComponent<TextMeshProUGUI>();
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
        normalStatText.text =
        "기본공격" + "\n" +
        "데미지 : " + equipedArtifact.Default_NormalAttackPower + "\n" +
        "쿨타임 : " + equipedArtifact.Default_NormalAttackCoolTime + "\n" +
        "지속시간 : " + equipedArtifact.Default_NormalAttackLife + "\n" +
        "날아가는 속도 : " + equipedArtifact.Default_NormalBulletSpeed + "\n" +
        "선딜레이 : " + equipedArtifact.Default_NormalAttackStartDelay + "\n" +
        "발사 수 : " + equipedArtifact.Default_NormalAttackCount + "\n" +
        "탄퍼짐 각도 : " + equipedArtifact.Default_NormalAttackSpreadAngle + "\n"
        ;
    }

    void ResetArtifactInfomationText() 
    {
        normalStatText.text = "";
        skillStatText.text = "";
        otherStatText.text = "";
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
