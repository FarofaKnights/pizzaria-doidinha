using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoMesa { Vazia, AguardandoCliente, AguardandoAtendimento, AguardandoPedido, Comendo, Suja };

public class Mesa : MonoBehaviour {
    public int numero;

    public Cliente cliente = null;
    public Pizza pizza = null;

    public EstadoMesa estado = EstadoMesa.Vazia;

    public GameObject triggerAtendimento, triggerPedido, triggerCliente;
    public GameObject sentadoSlot;

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

        float semelhanca = this.pizza.CompararPrato(cliente.pedido);
        if (semelhanca < 50f) {
            float dinheiro = cliente.pedido.prato.preco;
            GameManager.instance.SubDinheiro(dinheiro);
            IrEmbora();
        } else {
            float dinheiro = cliente.pedido.prato.preco; // * semelhanca / 100f;
            GameManager.instance.AddDinheiro(dinheiro);
            StartCoroutine(ComerPizza());
        }
    }

    IEnumerator ComerPizza() {
        yield return new WaitForSeconds(5f);

        IrEmbora();
    }

    public void IrEmbora() {
        pizza.SpawnNova();
        Destroy(pizza.gameObject);
        pizza = null;
        estado = EstadoMesa.Vazia;
        triggerPedido.GetComponent<TriggerController>().Ativar();

        // Cliente vai embora
        cliente.personagem.Levantar();
        cliente.personagem.SetDestino(GameManager.instance.spawnPersonagem);
        cliente = null;
     
        GameManager.instance.PedidoConcluido();
    }

    public void Ocupar(Cliente cliente) {
        this.cliente = cliente;
        cliente.SetMesa(this);

        estado = EstadoMesa.AguardandoCliente;
    }

    public void ClienteChegou() {
        estado = EstadoMesa.AguardandoAtendimento;
        triggerAtendimento.SetActive(true);

        cliente.personagem.Sentar(sentadoSlot);
    }

    public void ClienteAtendido() {
        estado = EstadoMesa.AguardandoPedido;
        triggerAtendimento.SetActive(false);

        cliente.personagem.FoiAtendido();
        cliente.GerarPedido();
        GameManager.instance.BotarNaMao(cliente.pedido);
    }

    public void HabilitarTriggerPedido() {
        triggerPedido.SetActive(true);
    }

    public void SerAtendido() {
        triggerAtendimento.SetActive(false);
        ClienteAtendido();
    }
}
