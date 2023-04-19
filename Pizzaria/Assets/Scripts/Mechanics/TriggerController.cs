using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TriggerController : MonoBehaviour {

    public string itemName;
    GameObject slot, trigger;
    ItemTrigger itemTrigger;
    public GameObject item;

    public UnityEvent<TriggerController> onItemEnter, onItemExit;

    void Start() {
        // Find the trigger and slot objects
        foreach (Transform child in transform) {
            if (child.gameObject.name == "Trigger") {
                trigger = child.gameObject;
            } else if (child.gameObject.name == "Slot") {
                slot = child.gameObject;
            }
        }

        if (trigger == null) {
            Debug.LogError("Trigger not found");
        }

        if (slot == null) {
            Debug.LogError("Slot not found");
        }

        itemTrigger = trigger.GetComponent<ItemTrigger>();
        itemTrigger.Comecar(slot);
    }

    public void OnItemEnter(GameObject item) {
        this.item = item;
        trigger.SetActive(false);

        if (onItemEnter != null) {
            onItemEnter.Invoke(this);
        }

        Debug.Log("Item entered");
    }

    void FixedUpdate() {
        if (item != null && !itemTrigger.hasChild()) {
            OnItemExit();
        }
    }

    public void OnItemExit() {
        if (item == null) return;

        this.item = null;
        trigger.SetActive(true);

        if (onItemExit != null) {
            onItemExit.Invoke(this);
        }

        Debug.Log("Item exited");
    }

    public void Desativar() {
        trigger.SetActive(false);
    }

    public void Ativar() {
        trigger.SetActive(true);
    }
}
