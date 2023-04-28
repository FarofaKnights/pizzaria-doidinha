using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FornoTrigger : ItemTrigger {
    public Text fornoTxt;
    public GameObject pizza;

    public void SetPizza(TriggerController trigger) {
        GameObject pizza = trigger.item;
        SetPizza(pizza);
    }

    public void SetPizza(GameObject pizza) {
        this.pizza = pizza;
    }

    public void RemovePizza() {
        this.pizza = null;
    }

    void FixedUpdate() {
        if (fornoTxt != null && pizza != null)
            fornoTxt.text = pizza.GetComponent<Pizza>().temperatura + "s";
        else
            fornoTxt.text = "";
    }
}
