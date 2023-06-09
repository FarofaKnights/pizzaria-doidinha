using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour {
    public GameObject prefab;
    public GameObject tampa, invokePoint;
    bool aberto = false;

    Quaternion rotacaoDesejada;
    public float velocidadeRotacao = 0.1f;


    public void Abrir() {
        if (!aberto) {
            aberto = true;
            rotacaoDesejada = Quaternion.Euler(-90, 0, 0);
        }
    }

    public void Fechar() {
        if (aberto) {
            aberto = false;
            rotacaoDesejada = Quaternion.Euler(0, 0, 0);
        }
    }

    public void Toggle() {
        if (aberto) {
            Fechar();
        } else {
            Abrir();
        }
    }

    void FixedUpdate() {
        tampa.transform.localRotation = Quaternion.Lerp(tampa.transform.localRotation, rotacaoDesejada, velocidadeRotacao);
    }

    public void Spawnar() {
        GameObject novo = Instantiate(prefab);
        novo.name = prefab.name;
        novo.transform.position = invokePoint.transform.position;
        novo.transform.rotation = invokePoint.transform.rotation;
        novo.GetComponent<Pizza>().freezer = this;
    }
}
