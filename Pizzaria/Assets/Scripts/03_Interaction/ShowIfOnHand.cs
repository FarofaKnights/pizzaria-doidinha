using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIfOnHand : MonoBehaviour {
    public string nomeItem = "";
    public bool tiverEmMaos = true;

    bool _podeMostrar = false;

    void Update() {
        if (PodeMostrar() && !_podeMostrar) {
            _podeMostrar = true;
            MudaFilhos(true);
        } else if (!PodeMostrar() && _podeMostrar) {
            _podeMostrar = false;
            MudaFilhos(false);
        }
    }

    void MudaFilhos(bool ativo) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(ativo);
        }
    }

    bool PodeMostrar() {
        if (ItemInteraction.instance.heldItem == null) return !tiverEmMaos;
        bool temItem = ItemInteraction.instance.heldItem.name == nomeItem;
        return (temItem && tiverEmMaos) || (!temItem && !tiverEmMaos);
    }
}
