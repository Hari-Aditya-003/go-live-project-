using UnityEngine;

namespace Common
{
    public static class AnimationConstants
    {
        public const float ANIMDAMPTIME = 0.1f;
        public const float CROSSFADETIME = 0.1f;
        
        //UI Anims
        public static readonly int ShowUI = Animator.StringToHash("ShowUI");
        public static readonly int HideUI = Animator.StringToHash("HideUI");
    }
}