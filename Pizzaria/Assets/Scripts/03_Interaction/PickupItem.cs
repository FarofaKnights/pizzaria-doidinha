using UnityEngine;
public class PickupItem : MonoBehaviour
{
      public static bool canPickUp = true;
      
      private Transform pickUpPoint, playerTransform;
      [SerializeField] private float pickUptD;
      [SerializeField] private float throwForce;
      [SerializeField] private bool readyToThrow;
      [SerializeField] private bool itemIsPickedUp;
      public float multi = 350;
      public float pickUpDistance = 2;
      Rigidbody rb;

      bool willRelease = false;
      
      void Start()
      {
            rb = GetComponent<Rigidbody>();
            playerTransform = GameObject.Find("/Player").transform;
            pickUpPoint = GameObject.Find("/Player/Vertical/PickUpPoint").transform;
      }

      // Update is called once per frame
      void Update()
      {
            if (PickupItem.canPickUp == false) return;

            if (Input.GetButton("Fire1") && readyToThrow == true)
            {
                  throwForce += multi * Time.deltaTime;
            }
            if (Input.GetButtonDown("Fire1") && itemIsPickedUp == true) readyToThrow = true;

            pickUptD = Vector3.Distance(playerTransform.position,transform.position);
            if(pickUptD < pickUpDistance)
            {
                  if (Input.GetButtonDown("Fire1") && itemIsPickedUp == false && pickUpPoint.childCount < 1)
                  {
                        PickObject();
                  }
            }
            if(itemIsPickedUp) this.transform.position = pickUpPoint.position;
            
            if (Input.GetButtonUp("Fire1") && readyToThrow == true)
            {
                  Arremecar();

                  this.transform.parent = null;
                  GetComponent<Rigidbody>().useGravity = true;
                  GetComponent<Collider>().enabled = true;

                  readyToThrow = false;
                  itemIsPickedUp = false;
                  throwForce = 0;
            }
            
      }

      public void PickObject() {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false;
            this.transform.position = GameObject.Find("/Player/Vertical/PickUpPoint").transform.position;
            this.transform.parent = GameObject.Find("/Player/Vertical/PickUpPoint").transform;
            this.transform.localPosition = Vector3.zero;

            itemIsPickedUp = true;
            readyToThrow = false; 
            throwForce = 0;
      }

      public virtual void Arremecar() {
            rb.AddForce(pickUpPoint.transform.forward * throwForce);
      }
}