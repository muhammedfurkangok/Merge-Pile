using UnityEngine;

namespace Runtime.Controllers
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayAnimation(string animationName)
        {
            animator.Play(animationName);
        }
    }
}