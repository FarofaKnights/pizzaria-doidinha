using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingPizza : MonoBehaviour
{
    public Camera cam;
    public LayerMask mask;
    public GameObject mira, mira2;

    public Material pizzaMaterial;
    public GameObject pizza;
    Collider pizzaCollider;
    public float radius = 0.5f;
    public LayerMask layerMask;

    bool selecting = false;
    LineRenderer lineRenderer;
    public GameObject cutPlane;

    void Start() {
        cam = Camera.main;
        pizzaCollider = pizza.GetComponent<Collider>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        // draw sphere gizmo
        if (Input.GetMouseButtonDown(0)) {
            selecting = true;

            Vector3 posPizza = GetMouseInPizza();
            if (posPizza != Vector3.zero) {
                mira.SetActive(true);
                mira.transform.position = posPizza;
            }
        }
        if (Input.GetMouseButton(0) && selecting) {
            Vector3 posPizza = GetMouseInPizza();
            if (posPizza != Vector3.zero) {
                mira2.SetActive(true);
                mira2.transform.position = posPizza;
                lineRenderer.SetPosition(0,mira.transform.position);
                lineRenderer.SetPosition(1,mira2.transform.position);
                lineRenderer.startColor = Color.gray;
                lineRenderer.endColor = Color.gray;
            }
        }
        if (Input.GetMouseButtonUp(0) && selecting) {
            selecting = false;
            CutPizza();

            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);

            mira.SetActive(false);
            mira2.SetActive(false);
        }
    }

    

    Vector3 GetMouseInPizza() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, mask)) {
            Vector3 posPizza = ClosestPointInPizza(hit.point);
            return posPizza;
        }
        return Vector3.zero;
    }

    Vector3 ClosestPointInPizza(Vector3 point) {
        Vector3 c = pizza.transform.position;
        Vector3 p = point;

        c.y = p.y = 0;

        Vector3 direction = (p - c);
        Vector3 closestPoint = c + direction.normalized * radius;
        closestPoint.y = pizza.transform.position.y + pizza.transform.localScale.y;
        return closestPoint;
    }

    // Cut cilindrical mesh with plane
    public void CutPizza() {
        Vector3 pointA = mira.transform.position;
        Vector3 pointB = mira2.transform.position;

        Vector3 pointInPlane = (pointA + pointB) / 2;
        
        Vector3 cutPlaneNormal = Vector3.Cross((pointA-pointB),(pointA)).normalized;
        cutPlaneNormal.y = 0;
        Quaternion orientation = Quaternion.FromToRotation(Vector3.up, cutPlaneNormal);
        
        var all = Physics.OverlapBox(pointInPlane, new Vector3(100, 0.01f, 100), orientation, layerMask);
        if (all.Length > 0)
        {
            foreach (var hit in all)
            {
                MeshFilter f = hit.gameObject.GetComponent<MeshFilter>();
                if(f != null){
                    Cutter.Cut(hit.gameObject, pointInPlane, cutPlaneNormal);
                }
            }
        }
    }
}
