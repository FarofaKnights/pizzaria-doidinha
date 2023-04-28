using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolhoPutter : MonoBehaviour {
    public string molho;
    public bool checkPizza;
    public float checkPizzaRadius = 1f;

    void FixedUpdate() {
        if (checkPizza) {
            Collider[] colliders = Physics.OverlapSphere(transform.position, checkPizzaRadius);
            foreach (Collider collider in colliders) {
                if (collider.gameObject.name == "Pizza") {
                    Pizza pizza = collider.gameObject.GetComponent<Pizza>();
                    pizza.AddMolho(molho);
                }
            }
        }
    }

    public void EnableCheckPizza() {
        checkPizza = true;
    }

    public void DisableCheckPizza() {
        checkPizza = false;
    }
}
