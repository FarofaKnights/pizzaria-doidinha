using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngridientsPizza : MiniAction
{
    public GameObject pizza;
    Collider pizzaCollider;

    public LayerMask ingredientHolderLayer, pizzaLayer, ingredientPutLayer, ingredientLayer;
    public GameObject ingredientPutPlane;

    public GameObject triggerEEntrar;

    GameObject inHand = null;
    IngredientHolder inHandHolder = null;
    Vector3 oldPos;

    public override bool OnComecar() {
        if (miniActionItem == null) return false;

        pizza = miniActionItem;

        if (pizza.GetComponent<Pizza>().estado != EstadoPizza.FaltaIngrediente && pizza.GetComponent<Pizza>().estado != EstadoPizza.FaltaCozinhar) {
            triggerEEntrar.SetActive(true);
            return false;
        }

        triggerEEntrar.SetActive(false);
        pizzaCollider = pizza.GetComponent<Collider>();
        ingredientPutPlane.SetActive(true);

        return true;
    }

    public override bool OnTerminar() {
        ingredientPutPlane.SetActive(false);
        triggerEEntrar.SetActive(true);

        return true;
    }

    public void TirouPizza() {
        triggerEEntrar.SetActive(false);
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
            } else if (Physics.Raycast(ray, out hit, 100, ingredientLayer)) {
                GameObject ingredient = hit.collider.gameObject;
                inHandHolder = null;
                PutInHand(ingredient);
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
                if (inHandHolder != null) {
                    inHandHolder.Add();
                    pizza.GetComponent<Pizza>().RemoverIngrediente(inHand.name);
                    Destroy(inHand);
                } else inHand.transform.position = oldPos;

                inHand = null;
            }
        }

        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1) {
            if (inHand != null) {
                if (inHandHolder != null) {
                    inHandHolder.Add();
                    pizza.GetComponent<Pizza>().RemoverIngrediente(inHand.name);
                    Destroy(inHand);
                } else inHand.transform.position = oldPos;
                inHand = null;
            }
            
            Terminar();
        }
    }

    public void PutInHand(GameObject ingrediente) {
        if (ingrediente == null) return;
        
        if (inHandHolder == null) oldPos = ingrediente.transform.position;
        
        inHand = ingrediente;
    }

    public void PutInPizza(Vector3 posPizza) {
        if (inHand == null) return;

        if (pizza.GetComponent<Pizza>().estado == EstadoPizza.FaltaIngrediente) {
            pizza.GetComponent<Pizza>().estado = EstadoPizza.FaltaCozinhar;
        }
        
        pizza.GetComponent<Pizza>().AdicionarIngrediente(inHand.name);
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
