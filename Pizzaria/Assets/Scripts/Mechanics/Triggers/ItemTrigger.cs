using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemTrigger : MonoBehaviour {

    TriggerController controller;
    GameObject slot;

    bool isOn = false;

    public void Comecar(GameObject slot) {
        controller = transform.parent.GetComponent<TriggerController>();
        this.slot = slot;
    }

    void OnTriggerEnter(Collider other) {
        TryToPlace(other.gameObject);
    }

    public bool TryToPlace(GameObject item) {
        string name = controller.itemName;

        if (item.name == name && !hasChild()) {
            PlaceItem(item);
            return true;
        }

        return false;
    }

    public void PlaceItem(GameObject item) {
        item.transform.SetParent(slot.transform, false);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.GetComponent<Rigidbody>().isKinematic = true;

        isOn = true;

        controller.OnItemEnter(item);
    }

    void FixedUpdate() {
        if (isOn && !hasChild()) {
            isOn = false;
            controller.OnItemExit();
        }
    }

    public bool hasChild() {
        return slot.transform.childCount > 0;
    }
}
