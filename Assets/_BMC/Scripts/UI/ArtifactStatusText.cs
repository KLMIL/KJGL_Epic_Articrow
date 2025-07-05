using UnityEngine;
using YSJ;

namespace BMC
{
    public class ArtifactStatusText : MonoBehaviour
    {
        ArtifactStatusItemUI[] _artifactStatusItemUIs;

        void Awake()
        {
            _artifactStatusItemUIs = GetComponentsInChildren<ArtifactStatusItemUI>();
            gameObject.SetActive(false);
        }

        public void SetText(ArtifactStatus equipedArtifactStatus)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            _artifactStatusItemUIs[0].SetText(equipedArtifactStatus.Default_AttackPower);
            _artifactStatusItemUIs[1].SetText(equipedArtifactStatus.Default_AttackCoolTime);
            _artifactStatusItemUIs[2].SetText(equipedArtifactStatus.Default_AttackLife);
            _artifactStatusItemUIs[3].SetText(equipedArtifactStatus.Default_BulletSpeed);
            _artifactStatusItemUIs[4].SetText(equipedArtifactStatus.Default_AttackStartDelay);
            _artifactStatusItemUIs[5].SetText(equipedArtifactStatus.Default_AttackCount);
            _artifactStatusItemUIs[6].SetText(equipedArtifactStatus.Default_AttackSpreadAngle);
        }

        public void Reset()
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);

            for (int i = 0; i < _artifactStatusItemUIs.Length; i++)
            {
                _artifactStatusItemUIs[i].Reset();
            }
        }
    }
}