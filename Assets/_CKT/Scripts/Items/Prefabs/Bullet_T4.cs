using System.Collections;
using UnityEngine;

namespace CKT
{
    public class Bullet_T4 : Projectile
    {
        Pellet[] _pellets;
        float[] _scatterSpeeds;
        float _moveXSpeed = 4.5f;

        public override void Init(bool isCreateFromPlayer)
        {
            base.Init(isCreateFromPlayer);

            StartCoroutine(PelletEnableCoroutine());
            StartCoroutine(PelletMoveCoroutine());
        }  

        private new void OnDisable()
        {
            base.OnDisable();

            _pellets = _pellets ?? GetComponentsInChildren<Pellet>();
            for (int i = 0; i < _pellets.Length; i++)
            {
                _pellets[i].transform.localPosition = Vector3.zero;
                _pellets[i].gameObject.SetActive(false);
            }
        }

        IEnumerator PelletEnableCoroutine()
        {
            _pellets = _pellets ?? GetComponentsInChildren<Pellet>();

            _scatterSpeeds = new float[_pellets.Length];
            float _deltaSpeed = (_moveXSpeed * 2) / (_pellets.Length - 1);
            for (int i = 0; i < _pellets.Length; i++)
            {
                _scatterSpeeds[i] = _moveXSpeed - (_deltaSpeed * i);
                _pellets[i].gameObject.SetActive(true);
                _pellets[i].Init(base._isCreateFromPlayer);
            }

            yield return null;
        }

        IEnumerator PelletMoveCoroutine()
        {
            while (this.gameObject.activeSelf)
            {
                for (int i = 0; i < _pellets.Length; i++)
                {
                    _pellets[i].transform.localPosition += Vector3.right * _scatterSpeeds[i] * Time.deltaTime;
                }
                yield return null;
            }
        }
    }
}