using UnityEngine;
using System;

[Serializable]
public class QuantIngredientes {
    public string ingrediente;
    public int quant;

    public QuantIngredientes(string ingrediente, int quant) {
        this.ingrediente = ingrediente;
        this.quant = quant;
    }
}
