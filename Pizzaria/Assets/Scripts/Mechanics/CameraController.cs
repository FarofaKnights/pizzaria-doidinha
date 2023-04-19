using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;

    public GameObject target;
    public float smoothing = 5f;


    GameObject player;
    bool transitioning = false;

    void Start() {
        instance = this;

        player = GameObject.Find("Player/Look");
        
        if (target == player) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update() {
        if (target == null) return;

        Vector3 targetCamPos = target.transform.position;
        Quaternion targetCamRot = target.transform.rotation;

        if (transitioning) {
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetCamRot, smoothing * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetCamPos) < 0.1f) {
                transitioning = false;
            }
        } else {
            transform.position = targetCamPos;
            transform.rotation = targetCamRot;
        }
    }

    public void SetTarget(GameObject newTarget) {
        target = newTarget;
        transitioning = true;
        Cursor.lockState = CursorLockMode.None;
        ItemInteraction.instance.canPickUp = false;
    }

    public void SetToPlayer() {
        SetTarget(player);
        Cursor.lockState = CursorLockMode.Locked;
        ItemInteraction.instance.canPickUp = true;
    }
}
