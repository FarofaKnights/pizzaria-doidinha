using UnityEngine;
using UnityEngine.SceneManagement;

public class MudaCena : MonoBehaviour {
    public string cena;

    public void MudarCena() {
        SceneManager.LoadScene(cena);
    }
}
