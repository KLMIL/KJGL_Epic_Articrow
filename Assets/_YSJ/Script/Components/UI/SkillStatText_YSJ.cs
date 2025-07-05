using UnityEngine;
using BMC;

// TODO: NormalStatText_YSJ.cs와 통합하기
public class SkillStatText_YSJ : MonoBehaviour
{
    ArtifactStatusItemUI[] _artifactStatusItemUIs;

    void Awake()
    {
        _artifactStatusItemUIs = GetComponentsInChildren<ArtifactStatusItemUI>();
        gameObject.SetActive(false);
    }

    public void SetText(Artifact_YSJ equipedArtifact)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        _artifactStatusItemUIs[0].SetText(equipedArtifact.skillStatus.Default_AttackPower);
        _artifactStatusItemUIs[1].SetText(equipedArtifact.skillStatus.Default_AttackCoolTime);
        _artifactStatusItemUIs[2].SetText(equipedArtifact.skillStatus.Default_AttackLife);
        _artifactStatusItemUIs[3].SetText(equipedArtifact.skillStatus.Default_BulletSpeed);
        _artifactStatusItemUIs[4].SetText(equipedArtifact.skillStatus.Default_AttackStartDelay);
        _artifactStatusItemUIs[5].SetText(equipedArtifact.skillStatus.Default_AttackCount);
        _artifactStatusItemUIs[6].SetText(equipedArtifact.skillStatus.Default_AttackSpreadAngle);
    }

    public void Reset()
    {
        for (int i = 0; i < _artifactStatusItemUIs.Length; i++)
        {
            _artifactStatusItemUIs[i].Reset();
        }
    }
}