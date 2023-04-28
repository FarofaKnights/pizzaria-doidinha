using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public ListaPratos listaPratos;

    public List<Mesa> mesas = new List<Mesa>();

    public GameObject spawnPersonagem;
    public GameObject personagemPrefab;

    public QuadroPedidos quadroPedidos;
    public FilaPedidos pedidosNaMao = new FilaPedidos();
    public FilaPedidos pedidos;
    
    public GameObject triggerIngredientes, triggerCorte;
    public GameObject[] triggerForno, triggerEntrega;

    public Freezer freezerPizza, freezerPizza2;

    public float dinheiros = 100f;
    public Text txtDinheiros;

    public int quantMaxClientes = 6, quantClientes = 0;

    void Start() {
        instance = this;
        
        listaPratos = new ListaPratos();
        ListaPratos.LerPratosArquivo(listaPratos);

        for (int i = 0; i < 2; i++) {
            GerarPedido();
        }

        pedidos = new FilaPedidos(quadroPedidos);

        for (int i = 0; i < 5; i++) {
            freezerPizza.Spawnar();
            freezerPizza2.Spawnar();
        }

        AddDinheiro(0f);
    }

    public void AddDinheiro(float valor) {
        dinheiros += valor;
        txtDinheiros.text = dinheiros.ToString("C");
    }

    public void SubDinheiro(float valor) {
        dinheiros -= valor;
        txtDinheiros.text = dinheiros.ToString("C");
    }

    public Mesa MesaVaziaAleatoria() {
        List<Mesa> mesasVazias = new List<Mesa>();
        foreach (Mesa mesa in mesas) {
            if (mesa.cliente == null) {
                mesasVazias.Add(mesa);
            }
        }
        if (mesasVazias.Count == 0) {
            return null;
        }
        int index = Random.Range(0, mesasVazias.Count);
        return mesasVazias[index];
    }

    public void PassarDaMaoParaFila() {
        Pedido pedido = pedidosNaMao.Remover();

        Debug.Log(pedido);
        
        while (pedido != null) {
            Debug.Log(pedido);
            pedidos.Adicionar(pedido);
            pedido = pedidosNaMao.Remover();
        }

        Debug.Log(pedidos);

        if (pedidos.VerPrimeiro() != null) {
            pedidos.VerPrimeiro().cliente.mesa.HabilitarTriggerPedido();
        }
    }

    public void ProximoPedido() {
        Pedido pedido = pedidos.Remover();
        if (pedidos.VerPrimeiro() != null) {
            pedidos.VerPrimeiro().cliente.mesa.HabilitarTriggerPedido();
        }
    }

    public void MostrarTrigger(Pizza pizza) {/*
        if (pizza.estado == EstadoPizza.FaltaMolho || pizza.estado == EstadoPizza.FaltaIngrediente || pizza.estado == EstadoPizza.FaltaCozinhar) {
            triggerIngredientes.SetActive(true);
        }
        if (pizza.estado == EstadoPizza.FaltaCozinhar || pizza.estado == EstadoPizza.FaltaCortar) {
            for (int i = 0; i < triggerForno.Length; i++) {
                triggerForno[i].SetActive(true);
            }
        }
        if (pizza.estado == EstadoPizza.FaltaCortar || pizza.estado == EstadoPizza.FaltaEntregar || pizza.estado == EstadoPizza.Cortando) {
            triggerCorte.SetActive(true);
        }
        if (pizza.estado == EstadoPizza.FaltaEntregar) {
            for (int i = 0; i < triggerEntrega.Length; i++) {
                triggerEntrega[i].SetActive(true);
            }
        }*/
    }

    public void EsconderTriggers() {/*
        triggerIngredientes.SetActive(false);
        for (int i = 0; i < triggerForno.Length; i++) {
            triggerForno[i].SetActive(false);
        }
        triggerCorte.SetActive(false);
        for (int i = 0; i < triggerEntrega.Length; i++) {
            triggerEntrega[i].SetActive(false);
        }*/
    }

    public void PedidoConcluido() {
        quantClientes--;

        int quant = Random.Range(0, (quantMaxClientes - quantClientes) / 2);
        for (int i = 0; i < quant; i++) {
            GerarPedido();
        }
    }

    public void GerarPedido() {
        Cliente cliente = new Cliente();

        Mesa mesa = MesaVaziaAleatoria();
        if (mesa != null) {
            mesa.Ocupar(cliente);
        }

        quantClientes++;
    }
}


