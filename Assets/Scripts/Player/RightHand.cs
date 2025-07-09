using UnityEngine;

namespace YSJ
{
    public class RightHand : Hand
    {
        private void OnEnable()
        {
            Managers.Input.OnLeftHandAction += TryArtifactNormalAttackTrigger;
            Managers.Input.OnRightHandAction += TryArtifactSkillAttackTrigger;
            Managers.Input.OnLeftHandActionEnd += TryArtifactNormalAttackCancle;
            Managers.Input.OnRightHandActionEnd += TryArtifactSkillAttackCancle;
        }
        private void OnDisable()
        {
            Managers.Input.OnLeftHandAction -= TryArtifactNormalAttackTrigger;
            Managers.Input.OnRightHandAction -= TryArtifactSkillAttackTrigger;
            Managers.Input.OnLeftHandActionEnd -= TryArtifactNormalAttackCancle;
            Managers.Input.OnRightHandActionEnd -= TryArtifactSkillAttackCancle;
        }

        void TryArtifactNormalAttackTrigger()
        {
            if (GameManager.Instance.IsPaused)
                return;

            Transform artifact = getArtifact();

            if (artifact && artifact.TryGetComponent<Artifact_YSJ>(out Artifact_YSJ equipedArtifact))
            {
                equipedArtifact.NormalAttackClicked();      
            }
        }

        void TryArtifactNormalAttackCancle() 
        {
            if (GameManager.Instance.IsPaused)
                return;

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
            if (GameManager.Instance.IsPaused)
                return;

            Transform artifact = getArtifact();

            if (artifact)
            {
                if (artifact.TryGetComponent<Artifact_YSJ>(out Artifact_YSJ equipedArtifact))
                {
                    equipedArtifact.SkillAttackClicked();
                }
            }
        }

        void TryArtifactSkillAttackCancle() 
        {
            if (GameManager.Instance.IsPaused)
                return;

            Transform artifact = getArtifact();
            if (artifact)
            {
                if (artifact.TryGetComponent<Artifact_YSJ>(out Artifact_YSJ equipedArtifact))
                {
                    equipedArtifact.SkillAttackCancled();
                }
            }
        }

        public Transform getArtifact() 
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

        public bool IsHoldingArtifact()
        {
            return transform.childCount > 0;
        }
    }
}