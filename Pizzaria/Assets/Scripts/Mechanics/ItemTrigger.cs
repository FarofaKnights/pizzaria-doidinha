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
        string name = controller.itemName;

        if (other.gameObject.name == name && !hasChild()) {
            other.gameObject.transform.parent = slot.transform;
            other.gameObject.transform.localPosition = Vector3.zero;
            other.gameObject.transform.localRotation = Quaternion.identity;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            isOn = true;

            controller.OnItemEnter(other.gameObject);
        }
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
