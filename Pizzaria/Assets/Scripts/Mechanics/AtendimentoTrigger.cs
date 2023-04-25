using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtendimentoTrigger : MonoBehaviour {
    Mesa mesa;

    void Start() {
        mesa = transform.parent.GetComponent<Mesa>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            mesa.ClienteAtendido();
        }
    }
}
