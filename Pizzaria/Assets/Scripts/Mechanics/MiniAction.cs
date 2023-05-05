using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAction : MonoBehaviour
{
    protected bool isActionHappening = false;
    protected GameObject miniActionItem = null;

    public virtual bool OnComecar(){ return true; }
    public virtual bool OnTerminar(){ return true; }

    public virtual void CameraChegou() {}
    
    public void Comecar() {
        if (!OnComecar()) return;

        CameraController.instance.SetTarget(gameObject);
        isActionHappening = true;
    }

    public void Terminar() {
        if (!OnTerminar()) return;
        CameraController.instance.SetToPlayer();
        isActionHappening = false;
    }

    public void Comecar(TriggerController controller) {
        miniActionItem = controller.item;
        this.Comecar();
    }
}
