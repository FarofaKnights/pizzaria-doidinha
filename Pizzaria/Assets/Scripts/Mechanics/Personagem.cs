using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Personagem : MonoBehaviour {
    public GameObject destino;
    public float velocidade = 1f;
    public bool chegou = false;
    public Animator animator;

    public Cliente cliente;

    NavMeshAgent agent;
    public GameObject personagem;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestino(GameObject destino) {
        this.destino = destino;
        chegou = false;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
    }

    void FixedUpdate() {
        if (!chegou) {
           agent.SetDestination(destino.transform.position);

           if (!agent.pathPending) {
                if (agent.remainingDistance <= agent.stoppingDistance) {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                        if (destino.name == "SpawnPersonagem"){
                            Destroy(gameObject);
                            return;
                        }

                        agent.enabled = false;
                        chegou = true;
                        destino = null;
                        cliente.mesa.ClienteChegou();
                    }
                }
            }
        }
    }

    public void Sentar(GameObject sentadoSlot) {
        animator.SetTrigger("Sentar");

        transform.SetParent(sentadoSlot.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        personagem.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void FoiAtendido() {
        animator.SetTrigger("Parar");
    }

    public void Levantar() {
        animator.SetTrigger("Levantar");
    }
}
