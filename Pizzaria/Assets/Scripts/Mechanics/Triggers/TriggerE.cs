using UnityEngine;
using UnityEngine.Events;

public class TriggerE : MonoBehaviour {
    public GameObject indicador;
    public UnityEvent onEEvent;
    bool playerNaArea = false;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerNaArea = true;
            
            if (indicador != null)
                indicador.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerNaArea = false;

            if (indicador != null)
                indicador.SetActive(false);
        }
    }

    void Update() {
        if (playerNaArea && Input.GetKeyDown(KeyCode.E)) {
            if (onEEvent != null) {
                onEEvent.Invoke();
            }
        }

        if (indicador != null && playerNaArea) {
            indicador.transform.LookAt(Camera.main.transform);
        }
    }
}
