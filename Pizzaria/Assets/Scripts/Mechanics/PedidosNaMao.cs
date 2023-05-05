using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedidosNaMao : MonoBehaviour {
    int quant = 0;
    public GameObject folhaPrefab;

    public LayerMask noPostLayer;

    public FilaPedidos pedidos = new FilaPedidos();

    public Vector3 folhasOffset;

    public GameObject quadroTrigger;

    void Start() {
        quadroTrigger.SetActive(false);
    }

    public void Adicionar(Pedido pedido) {
        pedidos.Adicionar(pedido);

        GameObject folha = Instantiate(folhaPrefab);
        folha.transform.SetParent(transform, false);
        folha.transform.localPosition = Vector3.zero;
        folha.transform.localRotation = Quaternion.identity;
        folha.layer = (int) Mathf.Log(noPostLayer.value, 2);

        foreach (Transform child in folha.transform) {
            child.gameObject.layer = folha.layer;
        }

        folha.GetComponent<Folha>().pedidoUI.DefinirPedido(pedido);
        quant++;

        quadroTrigger.SetActive(true);

        UpdateFolhas();
    }

    void UpdateFolhas() {
        for (int i = 0; i < quant; i++) {
            int index = quant - i - 1;
            GameObject folha = transform.GetChild(index).gameObject;
            Vector3 pos = folha.transform.localPosition;
            pos.x = i * folhasOffset.x;
            pos.y = i * folhasOffset.y;
            pos.z = i * folhasOffset.z;

            folha.transform.localPosition = pos;
        }
    }

    public Pedido Remover() {
        if (quant > 0) {
            Pedido pedido = pedidos.Remover();
            quant--;
            GameObject folha = transform.GetChild(0).gameObject;
            Destroy(folha);

            if (quant == 0) {
                quadroTrigger.SetActive(false);
            }

            return pedido;
        }
        return null;
    }

    public Pedido[] RemoverTodos() {
        Pedido[] todosPedidos = new Pedido[quant];
        for (int i = 0; i < quant; i++) {
            todosPedidos[i] = pedidos.Remover();
            GameObject folha = transform.GetChild(i).gameObject;
            Destroy(folha);
        }

        quadroTrigger.SetActive(false);
        quant = 0;
        return todosPedidos;
    }
}
