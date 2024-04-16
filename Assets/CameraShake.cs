using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance { get; private set; }
    public float intensity = 10f;
    public float time = .1f;
    private float shakeTimer;
    private CinemachineFreeLook freeLookCamera;
    CinemachineBasicMultiChannelPerlin perlin;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        freeLookCamera = GetComponent<CinemachineFreeLook>();

    }

    public void ShakeCamera()
    {
        perlin = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0) 
        {
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            perlin = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            perlin.m_AmplitudeGain = 0;
        }

    }
}
