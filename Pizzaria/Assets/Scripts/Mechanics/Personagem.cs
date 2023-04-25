using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour {
    public GameObject destino;
    public float velocidade = 1f;
    public bool chegou = false;

    public void SetDestino(GameObject destino) {
        this.destino = destino;
        chegou = false;
    }

    void FixedUpdate() {
        if (destino != null && !chegou) {
            Vector3 direcao = destino.transform.position - transform.position;
            transform.position += direcao.normalized * velocidade * Time.deltaTime;
            if (direcao.magnitude < 0.1f) {
                chegou = true;

                if (destino.GetComponent<Mesa>() != null) {
                    destino.GetComponent<Mesa>().ClienteChegou();
                } else if (destino == GameManager.instance.spawnPersonagem) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
