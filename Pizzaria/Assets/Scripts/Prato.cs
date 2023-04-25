using UnityEngine;
using System;

[Serializable]
public class Prato {
    public string nome;
    public float preco;
    public int tempoDePreparo;
    public QuantIngredientes[] ingredientes;

    public Prato(string nome, float preco, int tempoDePreparo, QuantIngredientes[] ingredientes) {
        this.nome = nome;
        this.preco = preco;
        this.tempoDePreparo = tempoDePreparo;
        this.ingredientes = ingredientes;
    }
}
