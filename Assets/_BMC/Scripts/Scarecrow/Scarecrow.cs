using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace BMC
{
    /// <summary>
    /// 허수아비, 플레이어가 방을 클리어 하면 나오는 허수아비
    /// </summary>
    public class Scarecrow : MonoBehaviour, IDamagable
    {
        Animator _anim;
        ShowDamageText _showDamageText;

        void Awake()
        {
            _anim = GetComponent<Animator>();
            _showDamageText = GetComponent<ShowDamageText>();
        }

        void Start()
        {
            _anim.Play("Spawn");
        }

        public void TakeDamage(float damage, Define.EnemyName attacker = Define.EnemyName.None)
        {
            AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Spawn"))
            {
                return;
            }

            _anim.Play("Hurt");
            _showDamageText.Show(damage);
        }

        void SpawnReward()
        {
            // Hp
            PlayerManager.Instance.PlayerStatus.Health += 1f;

            // 보상 결정
            List<GameObject> artifactList = StageManager.Instance.RoomTypeRewardListDict[RoomType.ArtifactRoom];
            List<GameObject> magicList = StageManager.Instance.RoomTypeRewardListDict[RoomType.MagicRoom];

            // 보상 생성
            Vector3 dir = Vector3.down;
            
            // 아티팩트 생성 및 등급 설정
            GameObject rewardObject = artifactList[Random.Range(0, artifactList.Count)];
            GameObject spawnedArtifact = Instantiate(rewardObject, transform.position + dir - transform.right * 2f, Quaternion.identity);
            if (spawnedArtifact.TryGetComponent<Artifact_YSJ>(out Artifact_YSJ artifact))
            {
                artifact.CurrentGrade = GetGrade();
            }

            // 파츠 생성
            rewardObject = StageManager.Instance.FieldParts[Random.Range(0, StageManager.Instance.FieldParts.Length)];
            Instantiate(rewardObject, transform.position + dir + transform.right * 1.5f, Quaternion.identity);
        }

        public Artifact_YSJ.Grade GetGrade()
        {
            int roomIdx = GameFlowManager.Instance.CurrentRoom;
            Artifact_YSJ.Grade grade = Artifact_YSJ.Grade.Common;
            if(roomIdx <= 3)
            {
                grade = Artifact_YSJ.Grade.Common;
            }
            else if(roomIdx <= 7)
            {
                grade = Artifact_YSJ.Grade.Uncommon;
            }
            else if (roomIdx <= 11)
            {
                grade = Artifact_YSJ.Grade.Rare;
            }
            else
            {
                grade = Artifact_YSJ.Grade.Epic;
            }
            return grade;
        }

        // Spawn 애니메이션 이벤트
        void OnSpawnAnimationEvent()
        {
            SpawnReward();
        }
    }
}