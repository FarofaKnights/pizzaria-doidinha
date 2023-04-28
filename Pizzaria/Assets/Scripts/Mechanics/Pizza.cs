using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoPizza { FaltaMolho, FaltaIngrediente, FaltaCozinhar, FaltaCortar, Cortando, FaltaEntregar, Entregue };


public class Pizza : MonoBehaviour {
    public Dictionary<string, int> ingredientes = new Dictionary<string, int>();

    public string[] nomeMolhos;
    public GameObject[] prefabMolhos;

    EstadoPizza _estado = EstadoPizza.FaltaMolho;
    public float temperatura = 0;
    public int pedacos;
    public string molho = "";

    public Freezer freezer;

    public EstadoPizza estado {
        get {
            return _estado;
        }
        set {
            _estado = value;
            GameManager.instance.MostrarTrigger(this);
        }
    }

    public void AdicionarIngrediente(string ingrediente) {
        Debug.Log("+" + ingrediente);
        if (ingredientes.ContainsKey(ingrediente)) {
            Debug.Log("já havia");
            ingredientes[ingrediente]++;
        } else {
            ingredientes.Add(ingrediente, 1);
        }
    }

    public void RemoverIngrediente(string ingrediente) {
        Debug.Log("-" + ingrediente);
        if (ingredientes.ContainsKey(ingrediente))
        {
            ingredientes[ingrediente]--;
            if (ingredientes[ingrediente] <= 0) {
                ingredientes.Remove(ingrediente);
            }
        }
    }

    public void Esquentar(float temperatura) {
        this.temperatura += temperatura;

        // Darken the pizza material as it gets hotter
        GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.black, this.temperatura / 200);
    }

    public void AtualizarFatias() {
        pedacos = transform.childCount;
    }

    // Método que compara pizza atual com o prato em questão, retornando a porcentagem de similaridade
    public float CompararPrato(Pedido pedido) {
        if (estado != EstadoPizza.Entregue && estado != EstadoPizza.FaltaEntregar) {
            return 0;
        }

        Prato prato = pedido.prato;
        float similaridade = 0;

        // Confere o molho
        if (molho == prato.molho) {
            similaridade += 20f;
        }

        // Confere as fatias
        similaridade += 20f * Mathf.Min(pedacos, pedido.fatias) / Mathf.Max(pedacos, pedido.fatias);

        // Confere a temperatura
        similaridade += 20f * Mathf.Min(temperatura, prato.temperatura) / Mathf.Max(temperatura, prato.temperatura);

        // Confere os ingredientes
        int quantExtra = 0;

        foreach (QuantIngredientes ingrediente in prato.ingredientes) {
            Debug.Log(ingrediente.ingrediente);
            Debug.Log(ingredientes.ContainsKey(ingrediente.ingrediente));
            if (ingredientes.ContainsKey(ingrediente.ingrediente)) {
                quantExtra += Mathf.Abs(ingrediente.quant - ingredientes[ingrediente.ingrediente]);
            } else {
                quantExtra += ingrediente.quant;
            }
        }

        foreach (KeyValuePair<string, int> ingrediente in ingredientes) {
            if (!prato.TemIngrediente(ingrediente.Key)) {
                quantExtra += ingrediente.Value;
            }
        }

        similaridade += 40f / (1+quantExtra);

        Debug.Log("Similaridade: " + similaridade);
        Debug.Log("Extras: " + quantExtra);
        return similaridade;
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

    public void PizzaPegada() {
        GameManager.instance.MostrarTrigger(this);
    }

    public void PizzaSoltada() {
        GameManager.instance.EsconderTriggers();
    }

    public void SpawnNova() {
        freezer.Spawnar();
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
