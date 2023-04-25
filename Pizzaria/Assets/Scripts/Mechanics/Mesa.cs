using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoMesa { Vazia, AguardandoCliente, AguardandoAtendimento, AguardandoPedido, Comendo, Suja };

public class Mesa : MonoBehaviour {
    public Cliente cliente = null;
    public Pizza pizza = null;

    public EstadoMesa estado = EstadoMesa.Vazia;

    public GameObject triggerAtendimento, triggerPedido;

    void Start() {
        triggerAtendimento.SetActive(false);
        triggerPedido.SetActive(false);
    }

    public void PizzaChegou(TriggerController trigger) {
        GameObject pizza = trigger.item;
        PizzaChegou(pizza);
    }

    public void PizzaChegou(GameObject pizza) {
        this.pizza = pizza.GetComponent<Pizza>();

        estado = EstadoMesa.Comendo;
        GameManager.instance.ProximoPedido();

        StartCoroutine(ComerPizza());
    }

    IEnumerator ComerPizza() {
        yield return new WaitForSeconds(5f);

        // Cliente terminou de comer
        Destroy(pizza.gameObject);
        pizza = null;
        estado = EstadoMesa.Suja;

        // Cliente vai embora
        cliente.personagem.gameObject.SetActive(true); // TEMP
        cliente.personagem.SetDestino(GameManager.instance.spawnPersonagem);
        cliente = null;
    }

    public void Ocupar(Cliente cliente) {
        this.cliente = cliente;
        cliente.SetMesa(this);

        estado = EstadoMesa.AguardandoCliente;
    }

    public void ClienteChegou() {
        estado = EstadoMesa.AguardandoAtendimento;
        triggerAtendimento.SetActive(true);
    }

    public void ClienteAtendido() {
        estado = EstadoMesa.AguardandoPedido;
        triggerAtendimento.SetActive(false);

        cliente.GerarPedido();
        GameManager.instance.pedidosNaMao.Adicionar(cliente.pedido);
        cliente.personagem.gameObject.SetActive(false); // TEMP
    }

    public void HabilitarTriggerPedido() {
        triggerPedido.SetActive(true);
    }
}
