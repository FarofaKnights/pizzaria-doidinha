using UnityEngine;
using System;

public class Pedido {
    public Prato prato;
    public Cliente cliente;
    public int fatias;

    static int codAtual = 0;
    int codigoPedido;

    public int CodigoPedido {
        get { return codigoPedido; }
    }

    public Pedido proximo;

    public Pedido(Cliente cliente, Prato prato, int fatias) {
        this.cliente = cliente;
        this.prato = prato;
        this.fatias = fatias;

        this.codigoPedido = GerarCodPedido();
    }

    public static void ResetarCodPedido() {
        codAtual = 0;
    }

    public static int GerarCodPedido() {
        int val = codAtual;
        codAtual++;
        return val;
    }
}
