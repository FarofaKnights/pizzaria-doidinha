using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientHolder: MonoBehaviour {
    public Ingrediente ingrediente;
    public int quantidade;

    public GameObject Use() {
        if (quantidade > 0 || quantidade < 0) {
            quantidade--;
            GameObject n = Instantiate(ingrediente.prefab);
            n.name = ingrediente.nome;
            return n;
        }
        return null;
    }

    public void Add() {
        quantidade++;
    }
}