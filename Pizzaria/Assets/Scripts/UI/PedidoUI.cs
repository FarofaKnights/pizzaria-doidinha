using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedidoUI : MonoBehaviour {
    public bool aberto = false;

    public Text tituloTxt, mesaTxt;
    public Text molhoTxt, temperaturaTxt, fatiasTxt;

    public GameObject molhoObj, temperaturaObj, fatiasObj, ingredientesObj, ingredienteHolder;

    public GameObject ingredientePrefab;

    public void DefinirPedido(Pedido pedido) {

        Debug.Log(tituloTxt);
        tituloTxt.text = "Pedido #" + pedido.CodigoPedido.ToString("D4");
        mesaTxt.text = "Mesa " + pedido.cliente.mesa.numero;
        
        molhoTxt.text = "<b>Molho:</b> " + pedido.prato.molho;
        temperaturaTxt.text = "<b>Temperatura:</b> " + pedido.prato.temperatura;
        fatiasTxt.text = "<b>Corte:</b> " + pedido.fatias + " fatias";

        for (int i = 0; i < pedido.prato.ingredientes.Length; i++) {
            GameObject ingrediente = Instantiate(ingredientePrefab);
            ingrediente.transform.SetParent(ingredienteHolder.transform);
            ingrediente.transform.localScale = Vector3.one;
            ingrediente.transform.localPosition = Vector3.zero;

            ingrediente.GetComponent<Text>().text = pedido.prato.ingredientes[i].quant + "x " + pedido.prato.ingredientes[i].ingrediente;
        }
    }

    public void Abrir() {
        aberto = true;

        molhoObj.SetActive(true);
        temperaturaObj.SetActive(true);
        fatiasObj.SetActive(true);
        ingredientesObj.SetActive(true);
    }

    public void Fechar() {
        aberto = false;

        molhoObj.SetActive(false);
        temperaturaObj.SetActive(false);
        fatiasObj.SetActive(false);
        ingredientesObj.SetActive(false);
    }

    public void Toggle() {
        if (aberto) Fechar();
        else Abrir();
    }

    public void OnClick() {
        Toggle();
    }
}
