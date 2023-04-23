using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngridientsPizza : MiniAction
{
    public GameObject pizza;
    Collider pizzaCollider;

    public LayerMask ingredientHolderLayer, pizzaLayer, ingredientPutLayer;
    GameObject inHand = null;
    IngredientHolder inHandHolder = null;

    public override void OnComecar() {
        pizza = miniActionItem;
        pizzaCollider = pizza.GetComponent<Collider>();
    }

    void Update() {
        if (!isActionHappening) return;

        if (Input.GetMouseButtonDown(0) && inHand == null) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, ingredientHolderLayer)) {
                GameObject holder = hit.collider.gameObject;
                inHandHolder = holder.GetComponent<IngredientHolder>();
                PutInHand(inHandHolder.Use());
            }
        }

        if (inHand != null) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, ingredientPutLayer)) {
                inHand.transform.position = hit.point;
            }
        }

        if (Input.GetMouseButtonUp(0) && inHand != null) {
            Vector3 posPizza = GetMouseInPizza();
            if (posPizza != Vector3.zero) {
                PutInPizza(posPizza);
            } else {
                inHandHolder.Add();
                Destroy(inHand);
                inHand = null;
            }
        }

        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1) {
            if (inHand != null) {
                inHandHolder.Add();
                Destroy(inHand);
                inHand = null;
            }
            
            Terminar();
        }
    }

    public void PutInHand(GameObject ingrediente) {
        if (ingrediente == null) return;
        
        inHand = ingrediente;
    }

    public void PutInPizza(Vector3 posPizza) {
        if (inHand == null) return;
        
        inHand.transform.position = posPizza;
        inHand.transform.parent = pizza.transform;
        inHand = null;
    }

    Vector3 GetMouseInPizza() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, pizzaLayer)) {
            return CheckIfInPizza(hit.point);;
        }
        return Vector3.zero;
    }

    Vector3 CheckIfInPizza(Vector3 point) {
        if (pizzaCollider.bounds.Contains(point)) {
            return point;
        }
        return Vector3.zero;
    }
}
