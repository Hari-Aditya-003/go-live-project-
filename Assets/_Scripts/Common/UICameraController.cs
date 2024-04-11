using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICameraController : MonoBehaviour
{
    [SerializeField] private CinemachineInputProvider cinemachineInputProvider;
    [SerializeField] private Button cameraScrollButton;

    private void Start()
    {

    }

    private void OnDestroy()
    {

    }

    public void EnableCameraScroll()
    {
        cinemachineInputProvider.enabled = true;
    }
    
    private void DisableCameraScroll()
    {
        cinemachineInputProvider.enabled = false;
    }
}
