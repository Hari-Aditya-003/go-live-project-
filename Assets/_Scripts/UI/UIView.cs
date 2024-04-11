using System;
using Common;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(Animator), typeof(CanvasGroup))]
    public class UIView : MonoBehaviour
    {
        [Header("Main Properties")]
        [SerializeField] private Selectable startSelectable;

        [Header("Screen Events")]
        [SerializeField] private UnityEvent onScreenStart;
        [SerializeField] private UnityEvent onScreenClose;
        
        
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (startSelectable)
            {
                EventSystem.current.SetSelectedGameObject(startSelectable.gameObject);
            }
        }

        public virtual void StartScreen()
        {
            onScreenStart?.Invoke();
            HandleAnimator(AnimationConstants.ShowUI);
            
        }

        public virtual void CloseScreen()
        {
            onScreenClose?.Invoke();

            HandleAnimator(AnimationConstants.HideUI);
        }

        private void HandleAnimator(int id)
        {
            if (animator)
            {
                animator.SetTrigger(id);
            }
        }
    }
}