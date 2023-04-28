using UnityEngine;
using UnityEngine.Events;

public class PickupListener : MonoBehaviour {
    public UnityEvent onPickup;
    public UnityEvent onRelease;

    public void OnPickup() {
        if (onPickup != null) {
            onPickup.Invoke();
        }
    }

    public void OnRelease() {
        if (onRelease != null) {
            onRelease.Invoke();
        }
    }
}
