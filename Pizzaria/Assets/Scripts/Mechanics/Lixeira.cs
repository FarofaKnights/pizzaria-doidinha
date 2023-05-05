using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lixeira : MonoBehaviour
{
    public void DeletePizza(TriggerController trigger) {
        Pizza pizza = trigger.item.GetComponent<Pizza>();
        trigger.Ativar();
        DeletePizza(pizza);
    }

    public void DeletePizza(Pizza pizza) {
        pizza.SpawnNova();
        Destroy(pizza.gameObject);

        GameManager.instance.SubDinheiro(10f);

        
    }
}
