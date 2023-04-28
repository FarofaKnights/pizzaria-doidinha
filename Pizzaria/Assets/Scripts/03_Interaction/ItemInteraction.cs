using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour {
    public static ItemInteraction instance;

    public bool canPickUp = true;

    public float throwForce = 15.0f;
    public float dropForce = 10.0f;
    public float pickUpRange = 1.0f;
    public Transform holdPosition;

    GameObject heldItem;
    Rigidbody heldItemRigidbody;

    void Start() {
        instance = this;

        if (holdPosition == null)
            Debug.LogError("Hold position Transform is not set. Please set a Transform for the hold position.");
    }

    void Update() {
        if (!canPickUp) return;

        if (Input.GetButtonDown("Fire1")) {
            if (heldItem != null) DropItem();
            else TryToPickUp();
        }

        if (Input.GetButtonDown("Fire2") && heldItem != null) {
            ThrowItem();
        }
    }

    void TryToPickUp() {
        if (heldItem != null) return;

        float closestDistance = float.MaxValue;
        GameObject closestItem = null;

        // Procura por itens no raio de pickUpRange, e pega o mais próximo caso o item seja "Pickupable"
        Collider[] hitColliders = Physics.OverlapSphere(holdPosition.position, pickUpRange);
        foreach (Collider hitCollider in hitColliders) {
            if (hitCollider.CompareTag("Pickupable")) {
                float distance = Vector3.Distance(holdPosition.position, hitCollider.transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestItem = hitCollider.gameObject;
                }
            }
        }

        if (closestItem != null) {
            PickUpItem(closestItem);
        }
    }

    void PickUpItem(GameObject item) {
        heldItem = item;
        heldItemRigidbody = heldItem.GetComponent<Rigidbody>();

        heldItem.GetComponent<Collider>().enabled = false;
        heldItemRigidbody.isKinematic = true;
        heldItemRigidbody.useGravity = false;
        heldItem.transform.SetParent(holdPosition);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;

        if (heldItem.GetComponent<PickupListener>() != null) {
            heldItem.GetComponent<PickupListener>().OnPickup();
        }
    }

    void DropItem() {
        heldItem.GetComponent<Collider>().enabled = true;
        heldItemRigidbody.isKinematic = false;
        heldItemRigidbody.useGravity = true;
        heldItem.transform.SetParent(null);
        heldItemRigidbody.AddForce(holdPosition.forward * dropForce, ForceMode.Impulse);

        if (heldItem.GetComponent<PickupListener>() != null) {
            heldItem.GetComponent<PickupListener>().OnRelease();
        }

        heldItem = null;
    }

    void ThrowItem() {
        heldItem.GetComponent<Collider>().enabled = true;
        heldItemRigidbody.isKinematic = false;
        heldItemRigidbody.useGravity = true;
        heldItem.transform.SetParent(null);
        heldItemRigidbody.AddForce(holdPosition.forward * throwForce, ForceMode.Impulse);

        if (heldItem.GetComponent<PickupListener>() != null) {
            heldItem.GetComponent<PickupListener>().OnRelease();
        }

        heldItem = null;
    }
}