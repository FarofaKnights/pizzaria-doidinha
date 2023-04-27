using UnityEngine;
using System;

[Serializable]
public class Prato {
    public string nome;
    public float preco;
    public int temperatura;
    public string molho;
    public QuantIngredientes[] ingredientes;

    public Prato(string nome, float preco, string molho, int temperatura, QuantIngredientes[] ingredientes) {
        this.nome = nome;
        this.preco = preco;
        this.molho = molho;
        this.temperatura = temperatura;
        this.ingredientes = ingredientes;
    }
}
