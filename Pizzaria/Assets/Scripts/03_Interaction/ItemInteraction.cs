using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour {
    public static ItemInteraction instance;

    public bool canPickUp = true;

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
            if (heldItem != null) TryToPlace();
            else TryToPickUp();
        }
    }

    GameObject GetClosestItem(string tag) {
        float closestDistance = float.MaxValue;
        GameObject closestItem = null;

        // Procura por itens no raio de pickUpRange, e pega o mais próximo caso o item possua a mesma tag que o parâmetro
        Collider[] hitColliders = Physics.OverlapSphere(holdPosition.position, pickUpRange);
        foreach (Collider hitCollider in hitColliders) {
            if (hitCollider.CompareTag(tag)) {
                float distance = Vector3.Distance(holdPosition.position, hitCollider.transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestItem = hitCollider.gameObject;
                }
            }
        }

        return closestItem;
    }

    void TryToPickUp() {
        if (heldItem != null) return;

        GameObject closestItem = GetClosestItem("Pickupable");

        if (closestItem != null) {
            PickUpItem(closestItem);
        }
    }

    void PickUpItem(GameObject item) {
        heldItem = item;
        heldItemRigidbody = heldItem.GetComponent<Rigidbody>();

        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        if (item.GetComponent<PickableCustomPos>() != null) {
            PickableCustomPos customPos = item.GetComponent<PickableCustomPos>();
            position = customPos.position;
            rotation = Quaternion.Euler(customPos.rotation);
        }

        heldItem.GetComponent<Collider>().enabled = false;
        heldItemRigidbody.isKinematic = true;
        heldItemRigidbody.useGravity = false;
        heldItem.transform.SetParent(holdPosition, false);
        heldItem.transform.localPosition = position;
        heldItem.transform.localRotation = rotation;

        if (heldItem.GetComponent<PickupListener>() != null) {
            heldItem.GetComponent<PickupListener>().OnPickup();
        }
    }

    void TryToPlace() {
        if (heldItem == null) return;

        GameObject closestItem = GetClosestItem("Placeable");

        if (closestItem != null) {
            PlaceItem(closestItem);
        }
    }

    void PlaceItem(GameObject place) {
        ItemTrigger trigger = place.GetComponent<ItemTrigger>();
        if (trigger == null) return;

        bool wasPlaced = trigger.TryToPlace(heldItem);

        if (wasPlaced) {
            heldItem.GetComponent<Collider>().enabled = true;

            if (heldItem.GetComponent<PickupListener>() != null) {
                heldItem.GetComponent<PickupListener>().OnRelease();
            }

            heldItem = null;
        }
    }
}