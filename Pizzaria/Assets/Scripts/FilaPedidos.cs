using UnityEngine;
using System;

public class FilaPedidos {
    public Pedido primeiroPedido, ultimoPedido;
    public QuadroPedidos quadro;

    public FilaPedidos() {
        primeiroPedido = null;
        ultimoPedido = null;
        quadro = null;
    }

    public FilaPedidos(QuadroPedidos quadro) {
        primeiroPedido = null;
        ultimoPedido = null;
        this.quadro = quadro;
    }

    public void Adicionar(Pedido pedido) {
        if (primeiroPedido == null) {
            primeiroPedido = pedido;
            ultimoPedido = primeiroPedido;
        } else {
            ultimoPedido.proximo = pedido;
            ultimoPedido = pedido;
            pedido.proximo = null;
        }

        if (quadro && pedido != null)
            quadro.AdicionarPedido(pedido);
    }

    public Pedido Remover() {
        if (primeiroPedido == null) {
            return null;
        } else {
            Pedido pedido = primeiroPedido;
            primeiroPedido = primeiroPedido.proximo;

            if (quadro)
                quadro.RemoverPedido();

            return pedido;
        }
    }

    public Pedido VerPrimeiro() {
        return primeiroPedido;
    }
}
