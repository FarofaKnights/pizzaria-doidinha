using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public ListaPratos listaPratos;

    public List<Mesa> mesas = new List<Mesa>();

    public GameObject spawnPersonagem;
    public GameObject personagemPrefab;

    public QuadroPedidos quadroPedidos;
    public FilaPedidos pedidosNaMao = new FilaPedidos();
    public FilaPedidos pedidos;

    void Start() {
        instance = this;
        
        listaPratos = new ListaPratos();
        ListaPratos.LerPratosArquivo(listaPratos);

        for (int i = 0; i < 2; i++) {
            Cliente cliente = new Cliente();

            Mesa mesa = MesaVaziaAleatoria();
            if (mesa != null) {
                mesa.Ocupar(cliente);
            }
        }


        pedidos = new FilaPedidos(quadroPedidos);
    }

    void Update() {
        
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
}


