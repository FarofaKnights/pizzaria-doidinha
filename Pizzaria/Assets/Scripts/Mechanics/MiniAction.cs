using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAction : MonoBehaviour
{
    protected bool isActionHappening = false;
    protected GameObject miniActionItem = null;

    public virtual void OnComecar(){}
    public virtual void OnTerminar(){}
    
    public void Comecar() {
        Debug.Log("Começar");
        
        CameraController.instance.SetTarget(gameObject);
        isActionHappening = true;

        OnComecar();
    }

    public void Terminar() {
        Debug.Log("Terminar");
        this.OnTerminar();
        CameraController.instance.SetToPlayer();
        isActionHappening = false;
    }

    public void Comecar(TriggerController controller) {
        miniActionItem = controller.item;
        this.Comecar();
    }
}
