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
            if (StageManager.Instance.CurrentRoom == null ||
                StageManager.Instance.CurrentRoom.RoomData.RoomType == RoomType.TutorialRoom)
            {
                return;
            }

            _anim.Play("Spawn");
        }

        public void TakeDamage(float damage)
        {
            AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Spawn"))
            {
                return;
            }

            _anim.Play("Hurt");
            _showDamageText.Show(damage);

            if(StageManager.Instance.CurrentRoom == null ||
                StageManager.Instance.CurrentRoom.RoomData.RoomType == RoomType.TutorialRoom)
            {
                TutorialManager.Instance.TutorialClear();
            }
        }

        void SpawnReward()
        {
            // Hp, Mp 회복
            PlayerManager.Instance.PlayerStatus.Health += PlayerManager.Instance.PlayerStatus.MaxHealth * 0.3f;
            PlayerManager.Instance.PlayerStatus.Mana += PlayerManager.Instance.PlayerStatus.MaxMana * 0.3f;

            // 보상 결정
            List<GameObject> artifactList = StageManager.Instance.RoomTypeRewardListDict[RoomType.ArtifactRoom];
            List<GameObject> magicList = StageManager.Instance.RoomTypeRewardListDict[RoomType.MagicRoom];
            GameObject rewardObject = artifactList[Random.Range(0, artifactList.Count)];

            // 보상 생성
            Vector3 dir = Vector3.down;
            Instantiate(rewardObject, transform.position + dir - transform.right, Quaternion.identity);
            rewardObject = StageManager.Instance.PartPrefab;
            Instantiate(rewardObject, transform.position + dir + transform.right, Quaternion.identity);
        }

        // Spawn 애니메이션 이벤트
        void OnSpawnAnimationEvent()
        {
            SpawnReward();
        }
    }
}