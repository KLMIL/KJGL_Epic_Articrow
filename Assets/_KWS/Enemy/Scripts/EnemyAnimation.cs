using UnityEngine;

namespace Game.Enemy
{
    public class EnemyAnimation : MonoBehaviour
    {
        Animator animator;
        [HideInInspector] public string CurrentAnimation = "";

        private void Awake()
        {
            animator = GetComponent<Animator>();
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
            //Debug.Log($"[{Time.time}]: Animation Change: {animName}");
            if (CurrentAnimation == animName) return;

            Play(animName);
            CurrentAnimation = animName;
        }
    }
}
