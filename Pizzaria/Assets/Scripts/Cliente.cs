using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cliente {
    public Mesa mesa;
    public Pedido pedido;

    public Personagem personagem;

    public Cliente() {
        this.mesa = null;
        this.pedido = null;

        GerarPersonagem(GameManager.instance.spawnPersonagem);
    }

    public void GerarPersonagem(GameObject spawn) {
        GameObject personagem = GameObject.Instantiate(GameManager.instance.personagemPrefab);
        personagem.transform.position = spawn.transform.position;
        this.personagem = personagem.GetComponent<Personagem>();
        this.personagem.cliente = this;
    }

    public void SetMesa(Mesa mesa) {
        this.mesa = mesa;
        this.personagem.SetDestino(mesa.triggerCliente);
    }

    public void GerarPedido() {
        Prato prato = GameManager.instance.listaPratos.PratoAleatorio();
        int fatias = Random.Range(1, 5);

        this.pedido = new Pedido(this, prato, fatias);
    }
}
