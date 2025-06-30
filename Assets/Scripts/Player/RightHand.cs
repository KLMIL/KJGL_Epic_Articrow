using UnityEngine;

namespace YSJ
{
    public class RightHand : Hand
    {
        private void Awake()
        {
            Managers.Input.OnLeftHandAction += TryArtifactNormalAttackTrigger;
            Managers.Input.OnRightHandAction += TryArtifactSkillAttackTrigger;
            Managers.Input.OnLeftHandActionEnd += TryArtifactNormalAttackCancle;
            Managers.Input.OnRightHandActionEnd += TryArtifactSkillAttackCancle;
        }

        void TryArtifactNormalAttackTrigger()
        {
            Transform artifact = getArtifact();

            if (artifact)
            {
                if (artifact.TryGetComponent<Artifact_YSJ>(out Artifact_YSJ equipedArtifact))
                {
                    equipedArtifact.NormalAttackTriggered();
                }
            }
        }

        void TryArtifactNormalAttackCancle() 
        {
            Transform artifact = getArtifact();

            if (artifact)
            {
                if (artifact.TryGetComponent<Artifact_YSJ>(out Artifact_YSJ equipedArtifact))
                {
                    equipedArtifact.NormalAttackCancled();
                }
            }
        }

        void TryArtifactSkillAttackTrigger() 
        {
            Transform artifact = getArtifact();

            if (artifact)
            {
                if (artifact.TryGetComponent<Artifact_YSJ>(out Artifact_YSJ equipedArtifact))
                {
                    equipedArtifact.SkillAttackTriggered();
                }
            }
        }

        void TryArtifactSkillAttackCancle() 
        {
            Transform artifact = getArtifact();
            if (artifact)
            {
                if (artifact.TryGetComponent<Artifact_YSJ>(out Artifact_YSJ equipedArtifact))
                {
                    equipedArtifact.SkillAttackCancled();
                }
            }
        }

        Transform getArtifact() 
        {
            if (transform.childCount == 0)
            {
                Debug.LogWarning("오른손에 아티팩트가 없음!");
                return null;
            }
            else 
            {
                return transform.GetChild(0);
            }
        }
    }
}