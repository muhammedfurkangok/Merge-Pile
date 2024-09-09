using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Controllers
{
    public class PlayerAnimationController : SingletonMonoBehaviour<PlayerAnimationController>
    {
        private Animator animator;
        
        public const string Idle= "Idle";
        public const string IdleMove = "IdleMove";
        public const string IdleScratch = "IdleScratch";
        public const string Hold = "Hold";
        public const string Drop = "Drop";

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayAnimation(string animationName)
        {
            animator.SetTrigger(animationName);
        }
    }
}