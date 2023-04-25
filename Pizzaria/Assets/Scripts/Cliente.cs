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
    }

    public void SetMesa(Mesa mesa) {
        this.mesa = mesa;
        this.personagem.SetDestino(mesa.gameObject);
    }

    public void GerarPedido() {
        this.pedido = new Pedido();
        this.pedido.cliente = this;
        this.pedido.prato = GameManager.instance.listaPratos.PratoAleatorio();
        this.pedido.fatias = Random.Range(1, 5);
    }
}
