using UnityEngine;
using System;

public class FilaPedidos {
    public Pedido primeiroPedido, ultimoPedido;

    public FilaPedidos() {
        primeiroPedido = null;
        ultimoPedido = null;
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
    }

    public Pedido Remover() {
        if (primeiroPedido == null) {
            return null;
        } else {
            Pedido pedido = primeiroPedido;
            primeiroPedido = primeiroPedido.proximo;
            return pedido;
        }
    }

    public Pedido VerPrimeiro() {
        return primeiroPedido;
    }
}
