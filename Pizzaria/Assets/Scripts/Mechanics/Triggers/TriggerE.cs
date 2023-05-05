using UnityEngine;
using UnityEngine.Events;

public class TriggerE : MonoBehaviour {
    public GameObject indicador;
    public UnityEvent onEEvent;
    bool playerNaArea = false;


    public string sePlayerSegurando = "";

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerNaArea = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerNaArea = false;
        }
    }

    void Update() {
        if (playerNaArea && Input.GetKeyDown(KeyCode.E) && PodeMostrar()) {
            if (onEEvent != null) {
                onEEvent.Invoke();
            }
        }

        if (indicador != null && PodeMostrar()) {
            indicador.SetActive(playerNaArea);
        } else if (indicador != null) {
            indicador.SetActive(false);
        }

        if (indicador != null && PodeMostrar() && playerNaArea) {
            indicador.transform.LookAt(Camera.main.transform);
        }
    }

    bool PodeMostrar() {
        if (sePlayerSegurando == "") return true;
        return ItemInteraction.instance.heldItem != null && ItemInteraction.instance.heldItem.name == sePlayerSegurando;
    }
}
