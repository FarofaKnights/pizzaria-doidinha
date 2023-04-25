using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour {
    public Dictionary<string, int> ingredientes = new Dictionary<string, int>();
    public float temperatura = 0;
    public int pedacos;

    public void AdicionarIngrediente(Ingrediente ingrediente) {
        if (ingredientes.ContainsKey(ingrediente.nome)) {
            ingredientes[ingrediente.nome]++;
        } else {
            ingredientes.Add(ingrediente.nome, 1);
        }
    }

    public void Esquentar(float temperatura) {
        this.temperatura += temperatura;

        // Darken the pizza material as it gets hotter
        GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.black, this.temperatura / 100);
    }
}
