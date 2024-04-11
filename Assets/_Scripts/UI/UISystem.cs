using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UISystem : MonoBehaviour
    {
        [Header("Main Properties")] 
        [SerializeField] private UIView startScreen;
        
        [Header("System Events")] 
        [SerializeField] private UnityEvent onSwitchedScreen;

        [Header("Fader Properties")] 
        [SerializeField] private Image fader;
        [SerializeField] private float fadeInDuration = 1f;
        [SerializeField] private float fadeOutDuration = 1f;
        
        [SerializeField] private UIView[] screens;
        [SerializeField] private UIView previousScreen;
        [SerializeField] private UIView currentScreen;
        
        public UIView PreviousScreen { get => previousScreen; private set => previousScreen = value; }
        public UIView CurrentScreen { get => currentScreen; private set => currentScreen = value; }

        void Start()
        {
            screens = GetComponentsInChildren<UIView>(true);
            if (startScreen)
            {
                SwitchScreens(startScreen);
            }
            
            if (fader)
            {
                fader.gameObject.SetActive(true);
            }
            FadeIn();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoToPreviousScreen();
            }
        }

        public void FadeIn()
        {
            fader.CrossFadeAlpha(0f, fadeInDuration, false);
        }

        public void FadeOut()
        {
            fader.CrossFadeAlpha(1f, fadeOutDuration, false);
        }

        public void SwitchScreens(UIView screen)
        {
            if (!screen) { return; }
            
            //If we are on the first screen
            if (screen == startScreen && previousScreen == null)
            {
                currentScreen = startScreen;
                currentScreen.StartScreen();
                currentScreen.gameObject.SetActive(true);
                onSwitchedScreen?.Invoke();
                return;
            }
            //Go back to previous screen
            if (currentScreen) 
            { 
                currentScreen.CloseScreen();
                previousScreen = currentScreen; 
            }
            currentScreen = screen;
            currentScreen.gameObject.SetActive(true);
            currentScreen.StartScreen();
            onSwitchedScreen?.Invoke();
        }

        public void GoToPreviousScreen()
        {
            if(previousScreen == null) {return;}
            
            if(currentScreen == startScreen) {return;}
            
            SwitchScreens(previousScreen);
            
        }

        public void LoadScene(int sceneIndex)
        {
            StartCoroutine(WaitToLoadScene(sceneIndex));
        }

        IEnumerator WaitToLoadScene(int sceneIndex)
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(sceneIndex);
        }
    }
}