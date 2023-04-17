using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;

    public GameObject target;
    public float smoothing = 5f;

    bool transitioning = false;

    void Start() {
        instance = this;
    }

    void Update() {
        if (target == null) return;

        Vector3 targetCamPos = target.transform.position;
        Quaternion targetCamRot = target.transform.rotation;

        if (transitioning) {
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetCamRot, smoothing * Time.deltaTime);
        } else {
            transform.position = targetCamPos;
            transform.rotation = targetCamRot;
        }
    }

    public void SetTarget(GameObject newTarget) {
        target = newTarget;
        transitioning = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetToPlayer() {
        GameObject player = GameObject.Find("Player");
        SetTarget(player);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
