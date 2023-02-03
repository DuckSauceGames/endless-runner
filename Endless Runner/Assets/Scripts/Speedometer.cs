using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cinemachine;

public class Speedometer : MonoBehaviour {

    TMP_Text speedText;
    public Rigidbody2D monitorSpeed;
    public CinemachineVirtualCamera virtualCamera;

    float startingZoom;
    float zoomSpeedScaling = .5f;
    float targetZoom;

    float zoomTime = 1f;
    
    void Awake() {
        speedText = GetComponent<TMP_Text>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player) {
            monitorSpeed = player.GetComponent<Rigidbody2D>();
        }

        if (virtualCamera) {
            startingZoom = virtualCamera.m_Lens.OrthographicSize;
            targetZoom = startingZoom;
            Debug.Log("starting FOV " + startingZoom);
        }
    }

    void FixedUpdate() {
        // Show speedometer
        if (monitorSpeed) {
            float speed = monitorSpeed.velocity.magnitude;
            speedText.text = "Speed: " + Mathf.Round(speed);
            speedText.fontSize = Mathf.Max(18, 18 + speed);

            // Zoom camera based on speed
            if (virtualCamera) {
                targetZoom = startingZoom + zoomSpeedScaling * speed;
                virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, targetZoom, Time.fixedDeltaTime / zoomTime);
            }
        } else {
            speedText.gameObject.SetActive(false);
        }
    }
}
