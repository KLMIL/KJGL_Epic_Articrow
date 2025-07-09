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

        public void SetText(Artifact_YSJ equipedArtifact, ArtifactStatus equipedArtifactStatus)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            for (int  i = 0; i < _artifactStatusItemUIs.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        _artifactStatusItemUIs[i].SetText(equipedArtifactStatus.Default_AttackPower);
                        break;
                    case 1:
                        _artifactStatusItemUIs[i].SetText(equipedArtifactStatus.Default_AttackCoolTime);
                        break;
                    case 2:
                        _artifactStatusItemUIs[i].SetText(equipedArtifactStatus.Default_BulletLife * equipedArtifactStatus.Default_BulletSpeed);
                        break;
                    case 3:
                        _artifactStatusItemUIs[i].SetText(equipedArtifactStatus.Default_AttackStartDelay);
                        break;
                    case 4:
                        _artifactStatusItemUIs[i].SetText(equipedArtifactStatus.Default_AttackCount);
                        break;
                    case 5:
                        _artifactStatusItemUIs[i].SetText(equipedArtifactStatus.Default_AttackSpreadCount);
                        break;
                    case 6:
                        _artifactStatusItemUIs[i].SetText(equipedArtifactStatus.Default_AttackSpreadAngle);
                        break;
                    case 7:
                        _artifactStatusItemUIs[i].SetText(0.5f * equipedArtifact.ManaDecreaseAmount);
                        break;
                }
            }
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