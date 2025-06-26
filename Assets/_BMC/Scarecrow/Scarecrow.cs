using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YSJ;
using static Define;

namespace BMC
{
    /// <summary>
    /// 허수아비, 플레이어가 방을 클리어 하면 나오는 허수아비
    /// </summary>
    public class Scarecrow : MonoBehaviour, IDamagable
    {
        Animator _anim;
        TextMeshPro _damageText;

        void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        void Start()
        {
            _anim.Play("Spawn");
        }

        public void TakeDamage(float damage)
        {
            _anim.Play("Hurt");
            ShowDamageText(damage);
        }

        // Spawn 애니메이션 이벤트
        void OnSpawnAnimationEvent()
        {
            SpawnReward();
        }

        void SpawnReward()
        {
            List<GameObject> artifactList = StageManager.Instance.RoomTypeRewardListDict[RoomType.ArtifactRoom];
            List<GameObject> magicList = StageManager.Instance.RoomTypeRewardListDict[RoomType.MagicRoom];
            GameObject rewardObject = artifactList[Random.Range(0, artifactList.Count)];
            
            Vector3 dir = Vector3.down;
            Instantiate(rewardObject, transform.position + dir - transform.right, Quaternion.identity);
            rewardObject = magicList[Random.Range(0, magicList.Count)];
            Instantiate(rewardObject, transform.position + dir + transform.right, Quaternion.identity);
        }

        // 데미지 텍스트 띄우기
        void ShowDamageText(float damage)
        {
            /*TextMeshPro damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
            damageText.transform.position = transform.position;
            damageText.text = damage.ToString();*/

            // 대미지 부여 텍스트
            if (_damageText != null && _damageText.gameObject.activeInHierarchy)
            {
                _damageText.text = (float.Parse(_damageText.text) + damage).ToString();

                Color color = _damageText.color;
                color.a = 1;
                _damageText.color = color;
            }
            else
            {
                _damageText = Managers.TestPool.Get<TextMeshPro>(Define.PoolID.DamageText);
                _damageText.text = damage.ToString("F0");
            }
            _damageText.transform.position = this.transform.position + this.transform.up;
        }
    }
}