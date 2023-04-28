using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadroPedidos : MonoBehaviour {
    public GameObject[] slots;
    int index = 0;

    public GameObject folhaPrefab;

    void Start() {
        Transform slotsHolder = transform.Find("Slots");
        slots = new GameObject[slotsHolder.childCount];

        for (int i = 0; i < slotsHolder.childCount; i++) {
            slots[i] = slotsHolder.GetChild(i).gameObject;
        }
    }

    public void AdicionarPedido(Pedido pedido) {
        if (index < slots.Length) {
            GameObject slot = slots[index];
            GameObject folha = Instantiate(folhaPrefab);
            folha.transform.SetParent(slot.transform, false);
            folha.transform.localPosition = Vector3.zero;
            folha.transform.localRotation = Quaternion.identity;

            folha.GetComponent<Folha>().pedidoUI.DefinirPedido(pedido);
            index++;
        }
    }

    public void RemoverPedido() {
        if (index > 0) {
            index--;
            GameObject slot = slots[0];
            GameObject folha = slot.transform.GetChild(0).gameObject;
            Destroy(folha);

            for (int i = 0; i < index; i++) {
                GameObject slotAtual = slots[i+1];
                GameObject slotAnterior = slots[i];

                GameObject folhaAtual = slotAtual.transform.GetChild(0).gameObject;
                folhaAtual.transform.SetParent(slotAnterior.transform, false);
            }
        }
    }
}
