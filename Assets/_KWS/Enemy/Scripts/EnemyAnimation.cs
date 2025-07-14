using UnityEngine;

namespace Game.Enemy
{
    public class EnemyAnimation : MonoBehaviour
    {
        Animator animator;
        SpriteRenderer _spriteRenderer;
        [HideInInspector] public string CurrentAnimation = "";

        // Properties;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Play(string animationName)
        {
            // 모든 트리거 리셋
            foreach (var param in animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(param.name);
                }
            }

            animator.speed = Random.Range(0.95f, 1.05f);
            animator.SetTrigger(animationName);
        }

        public void PlayAnimationOnce(string animName)
        {
            // 일시 정지 시, 애니메이션 변경 무시
            if(GameManager.Instance.IsPaused) 
                return;

            //Debug.Log($"[{Time.time}]: Animation Change: {animName}");
            if (CurrentAnimation == animName) return;

            Play(animName);
            CurrentAnimation = animName;
        }
    }
}
