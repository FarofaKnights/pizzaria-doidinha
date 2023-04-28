using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoPizza { FaltaMolho, FaltaIngrediente, FaltaCozinhar, FaltaCortar, Cortando, FaltaEntregar, Entregue };


public class Pizza : MonoBehaviour {
    public Dictionary<string, int> ingredientes = new Dictionary<string, int>();

    public string[] nomeMolhos;
    public GameObject[] prefabMolhos;

    public EstadoPizza estado = EstadoPizza.FaltaMolho;
    public float temperatura = 0;
    public int pedacos;
    public string molho = "";

    public void AdicionarIngrediente(string ingrediente) {
        if (ingredientes.ContainsKey(ingrediente)) {
            ingredientes[ingrediente]++;
        } else {
            ingredientes.Add(ingrediente, 1);
        }
    }

    public void RemoverIngrediente(string ingrediente) {
        if (ingredientes.ContainsKey(ingrediente)) {
            ingredientes[ingrediente]--;
            if (ingredientes[ingrediente] <= 0) {
                ingredientes.Remove(ingrediente);
            }
        }
    }

    public void Esquentar(float temperatura) {
        this.temperatura += temperatura;

        // Darken the pizza material as it gets hotter
        GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.black, this.temperatura / 100);
    }

    // Método que compara pizza atual com o prato em questão, retornando a porcentagem de similaridade
    public float CompararPrato(Prato prato) {
        return 100.0f;
    }

    public void AddMolho(string molho) {
        if (this.molho != "") return;

        if (estado == EstadoPizza.FaltaMolho) {
            estado = EstadoPizza.FaltaIngrediente;
        }

        for (int i = 0; i < nomeMolhos.Length; i++) {
            if (molho == nomeMolhos[i]) {
                GameObject molhoInstanciado = Instantiate(prefabMolhos[i], transform);
                molhoInstanciado.transform.parent = transform;
                molhoInstanciado.transform.localPosition = new Vector3(0, 0.001f, 0);
                molhoInstanciado.transform.localRotation = Quaternion.identity;

                this.molho = molho;
                break;
            }
        }
    }

    public Pizza CopyComponent(GameObject destination)  {
         System.Type type = this.GetType();
         var dst = destination.GetComponent(type) as Pizza;
         if (!dst) dst = destination.AddComponent(type) as Pizza;
         var fields = type.GetFields();
         foreach (var field in fields) {
             if (field.IsStatic) continue;
             field.SetValue(dst, field.GetValue(this));
         }
         var props = type.GetProperties();
         foreach (var prop in props) {
             if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
             prop.SetValue(dst, prop.GetValue(this, null), null);
         }
         return dst as Pizza;
     }
}
