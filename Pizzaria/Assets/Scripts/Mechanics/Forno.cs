using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forno : MonoBehaviour {
    List<Pizza> pizzas = new List<Pizza>();
    public float velocidadeEsquentar = 1f;

    public void AddPizza(TriggerController trigger) {
        GameObject pizza = trigger.item;
        AddPizza(pizza);
    }

    public void AddPizza(GameObject pizza) {
        Pizza pizzaScript = pizza.GetComponent<Pizza>();
        if (pizzaScript.estado != EstadoPizza.FaltaCozinhar && pizzaScript.estado != EstadoPizza.FaltaCortar) return;

        pizzas.Add(pizzaScript);

        if (pizzaScript.estado == EstadoPizza.FaltaCozinhar) {
            pizzaScript.estado = EstadoPizza.FaltaCortar;
        }
    }

    public void RemovePizza(TriggerController trigger) {
        GameObject pizza = trigger.item;
        RemovePizza(pizza);
    }

    public void RemovePizza(GameObject pizza) {
        Pizza pizzaScript = pizza.GetComponent<Pizza>();
        pizzas.Remove(pizzaScript);
    }

    // Update is called once per frame
    void FixedUpdate() {
        foreach (Pizza pizza in pizzas) {
            pizza.Esquentar(velocidadeEsquentar * Time.fixedDeltaTime);
        }
    }
}
