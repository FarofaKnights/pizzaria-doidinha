using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingPizza : MiniAction
{
    public Camera cam;
    public LayerMask mask;
    public GameObject mira, mira2;

    public Material pizzaMaterial;
    public GameObject pizza;
    Collider pizzaCollider;
    Vector3 pizzaInitialPos;
    public float radius = 0.5f, pizzaHeight = 0.1f;
    public LayerMask layerMask;

    bool selecting = false;
    LineRenderer lineRenderer;
    public GameObject cutPlane;

    // OnComecar é chamado quando o mini-game começa
    public override void OnComecar() {
        if (miniActionItem != null) {
            pizza = miniActionItem;
        }
        
        pizzaInitialPos = pizza.transform.position;
        pizzaCollider = pizza.GetComponent<Collider>();

        // Create parent object for pizza
        GameObject pizzaParent = Instantiate(pizza);
        pizzaParent.transform.parent = pizza.transform.parent;
        pizzaParent.transform.position = pizzaInitialPos;
        pizzaParent.layer = 0;
        pizzaParent.name = pizza.name;
        pizza.name = "PizzaMesh";

        //Destroy all children
        foreach (Transform child in pizzaParent.transform) {
            Destroy(child.gameObject);
        }
        
        Destroy(pizzaParent.GetComponent<MeshRenderer>());
        Destroy(pizzaParent.GetComponent<MeshFilter>());

        Destroy(pizza.GetComponent<Rigidbody>());

        pizza.transform.parent = pizzaParent.transform;
        pizza.tag = "Untagged";

        pizza = pizzaParent;

        /*
        MeshCombiner.Combine(pizza);
        foreach (Transform child in pizza.transform) {
            Destroy(child.gameObject);
        }*/
    }

    void Start() {
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        if (!isActionHappening) return;

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

        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1) {
            selecting = false;
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
            mira.SetActive(false);
            mira2.SetActive(false);
            
            Terminar();
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
        Vector3 c = pizzaInitialPos;
        Vector3 p = point;

        c.y = p.y = 0;

        Vector3 direction = (p - c);
        Vector3 closestPoint = c + direction.normalized * radius;
        closestPoint.y = pizzaInitialPos.y + pizzaHeight;
        return closestPoint;
    }

    List<GameObject>[] SepareObjectsInSide(Vector3 pointA, Vector3 pointB, List<GameObject> objects) {
        List<GameObject>[] sides = new List<GameObject>[2];
        sides[0] = new List<GameObject>();
        sides[1] = new List<GameObject>();
        
        Vector3 mid = (pointA + pointB) / 2;
        Debug.DrawLine(pointA, pointB, Color.red, 100);

        foreach (GameObject obj in objects) {
            Vector3 center = obj.GetComponent<MeshFilter>().mesh.bounds.center;
            Vector3 pos = obj.transform.position + center;

            Debug.DrawLine(mid, pos, Color.blue, 100);

            if (isLeft(pointA, pointB, pos)) {
                sides[0].Add(obj);
            } else {
                sides[1].Add(obj);
            }
        }

        return sides;
    }

    public bool isLeft(Vector3 a, Vector3 b, Vector3 c){
        float det = (b.x - a.x)*(c.z - a.z) - (b.z - a.z)*(c.x - a.x);
        Debug.Log(det);
        return det > 0;
    }

    public void CutPizza() {
        Vector3 pointA = mira.transform.position;
        Vector3 pointB = mira2.transform.position;

        Vector3 pointInPlane = (pointA + pointB) / 2;
        
        Vector3 cutPlaneNormal = Vector3.Cross((pointA-pointB),(pointA)).normalized;
        cutPlaneNormal.y = 0;
        Quaternion orientation = Quaternion.FromToRotation(Vector3.up, cutPlaneNormal);
        
        var all = Physics.OverlapBox(pointInPlane, new Vector3(100, 0.01f, 100), orientation, layerMask);
        if (all.Length > 0) {
            List<CutterReturnPlane> pizzaPieces = new List<CutterReturnPlane>();
            foreach (var hit in all) {
                MeshFilter f = hit.gameObject.GetComponent<MeshFilter>();
                if(f != null) {
                    CutterReturnPlane res = Cutter.Cut(hit.gameObject, pointInPlane, cutPlaneNormal);
                    GameObject[] pieces = res.sides;
                    if (pieces != null && pieces[0].name == "PizzaMesh") {
                        pizzaPieces.Add(res);
                    }
                }
            }

            foreach (CutterReturnPlane res in pizzaPieces) {
                GameObject[] pieces = res.sides;
                Plane cutPlane = res.plane;

                List<GameObject> childs = new List<GameObject>();

                for (int i = 0; i < 2; i++) {
                    GameObject pizzaPiece = pieces[i];

                    foreach (Transform child in pizzaPiece.transform) {
                        childs.Add(child.gameObject);
                    }
                }

                List<GameObject>[] sides = SepareObjectsInSide(pointA, pointB, childs);
                foreach (GameObject child in sides[0]) {
                    child.transform.parent = pieces[0].transform;
                }

                foreach (GameObject child in sides[1]) {
                    child.transform.parent = pieces[1].transform;
                }

                float distanceToMove = 0.01f;
                Vector3 moveDir = cutPlane.normal;
                moveDir.y = 0;
                pieces[0].transform.position += moveDir * distanceToMove;
                pieces[1].transform.position += -moveDir * distanceToMove;
            }

        }
    }
}
