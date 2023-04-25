using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesa : MonoBehaviour
{
    public Cliente cliente = null;
    public Pizza pizza;

    public void PizzaChegou(TriggerController trigger) {
        GameObject pizza = trigger.item;
        PizzaChegou(pizza);
    }


    public void PizzaChegou(GameObject pizza) {
        this.pizza = pizza.GetComponent<Pizza>();
    }
}
