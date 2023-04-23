using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingrediente", menuName = "Ingrediente")]
public class Ingrediente: ScriptableObject {
    public string nome;
    public GameObject prefab;
}