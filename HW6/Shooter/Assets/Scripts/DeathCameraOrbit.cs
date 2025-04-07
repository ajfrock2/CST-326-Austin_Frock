using System;
using UnityEngine;
using Unity.Cinemachine;

public class DeathCameraOrbit : MonoBehaviour
{
    public CinemachineCamera deathCam;
    public float orbitSpeed = 20f;

    private CinemachineOrbitalFollow orbitalFollow;

    void Start()
    {
        orbitalFollow = deathCam.GetComponent<CinemachineOrbitalFollow>();
    }

    void Update()
    {
        if (orbitalFollow != null)
        {
            // Increment the horizontal axis value to rotate the camera
            orbitalFollow.HorizontalAxis.Value += orbitSpeed * Time.deltaTime;
        }
    }
}