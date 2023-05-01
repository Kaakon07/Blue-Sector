using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Fish : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int gillDamage = 3;
    [SerializeField]
    private int gillDamageGuessed = 0;
    [SerializeField]
    private int id;
    
    //The following variables are used for handeling behaviour with water
    [HideInInspector]
    public Vector3 waterBodyCenter;
    private Vector3 targetPosition;
    private Quaternion lookRotation;
    public float movementSpeed = .5f;
    public float rotationSpeed = 10;
    [HideInInspector]
    public float waterBodyXLength;
    [HideInInspector]
    public float waterBodyZLength;
    [HideInInspector]
    public float waterHeight = 0;
    private Quaternion originalRotation;

    public BNG.UIPointer uIPointer;

    public GameObject marker;

    private GameObject pointerFinger;
    private List<GameObject> liceList = new List<GameObject>();
    private List<GameObject> boneList = new List<GameObject>(); //;)

    InspectionTaskManager inspectionTaskManager;
    public LayerMask layer;

    public GameObject lastMarkedLouse;

    private int markedLice = 0;

    public int health = 10;

    private AudioSource hurtSound;
    [HideInInspector]
    public int isInWaterCount = 0;
    [HideInInspector]
    public int isGrabbedCount = 0;
    private bool kinematicBones = false;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //Skamløst kokt fra FishScript.cs:
        InvokeRepeating(nameof(PeriodicUpdates), Random.Range(0.0f, 5.0f), 1.0f);
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
        targetPosition = transform.position;
        originalRotation = transform.rotation;
        liceList = FindObjectwithTag("Louse");
        boneList = FindObjectwithTag("Bone");
        Debug.Log("number of bones: " + boneList.Count);
        hurtSound = GetComponent<AudioSource>();
        //The point from which the raycast targeting lice on fishbodie will have its origin. In this case it is RightHandPointer in XR Rig Advanced
        pointerFinger = GameObject.FindGameObjectWithTag("Pointer");
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInWaterCount > 0 && isGrabbedCount <= 0){
            //Move();
        } 
        else {
            //Stop();
        }
    }

    void PeriodicUpdates() {
        if(isInWaterCount > 0 && isGrabbedCount <= 0){
            SetMoveTarget();    
        }
    }

    private void Move() {
        if(!kinematicBones) {
            foreach( GameObject bone in boneList) {
                bone.GetComponent<Rigidbody>().isKinematic = true;
                bone.GetComponent<Rigidbody>().velocity = Vector3.zero;
                kinematicBones = true;
                //animator.enabled = true;
                //animator.SetBool("Swimming", true);
            }
        }
        //Debug.Log("Y-posisjon: " + transform.position.y);
        transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);//Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, waterHeight / 2, transform.position.z), movementSpeed * Time.deltaTime);

              /* if( Vector3.Distance(transform.position, targetPosition) > .1 ) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        */
    }

    private void Stop() {
        if(kinematicBones) {
            foreach( GameObject bone in boneList) {
                bone.GetComponent<Rigidbody>().isKinematic = false;
                kinematicBones = false;
                animator.enabled = false;
            }
        }
    }

    public void SetMoveTarget() {
        /*
        prøv å bruke posisjonen kun til hvor den skal rotere, deretter bare beveg den fremmover. Virker som den blir litt spastic av setMoveTarget i on collision og...
        */
        //Mer Skamløs koking fra FishScript.cs:
        float randX = Random.Range(waterBodyCenter.x -waterBodyXLength / 2,waterBodyCenter.x + waterBodyXLength / 2);
        float randZ = Random.Range(waterBodyCenter.z -waterBodyZLength / 2,waterBodyCenter.z + waterBodyZLength / 2);
        targetPosition = new Vector3(randX, transform.position.y, randZ);
        Debug.Log("Position: " + targetPosition);
        lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
    }

    public void OnPointerClick(PointerEventData eventData) {
        lastMarkedLouse = checkForLouse(eventData.pointerCurrentRaycast.worldPosition);
        if(lastMarkedLouse != null){
            GameObject newmarker = Instantiate(marker,lastMarkedLouse.transform.position, new Quaternion(0,0,0,0));
            newmarker.transform.parent = transform;
        }
    }

    public GameObject checkForLouse(Vector3 origin) {
        if(Physics.SphereCast(origin - (pointerFinger.transform.forward*.5f), 0.02f, pointerFinger.transform.forward, out RaycastHit hitInfo, 10f, layer)) {
            GameObject hit = hitInfo.collider.gameObject;
            foreach(GameObject louse in liceList) {
                if (hit == louse && !louse.GetComponent<Louse>().marked) {
                    louse.GetComponent<Louse>().marked = true;
                    markedLice++;
                    return louse;
                }
            }
        }
        return null;
    }

    public void checkForDamage(bool hittingWater, float velocity) {
        float damageThreshold = 2f;
        if (hittingWater) {
            damageThreshold = 4f;
        }
        else if(isGrabbedCount > 0){
            damageThreshold = 0.4f;
        }
        if(velocity > damageThreshold) {
            if(health > 0) {
                health--;
            }
            hurtSound.Play(0);
        }
    }

    private void OnCollisionEnter(Collision other) {
        SetMoveTarget();
        if(other.collider.isTrigger){
            checkForDamage(true, other.relativeVelocity.magnitude);
        }
        else {
            checkForDamage(false, other.relativeVelocity.magnitude);
        }
    }
    public void SetAsSelectedFish() {
        inspectionTaskManager.SetSelectedFish(this); 
    }

    public void SetgillDamageGuessed(int guess) {
        gillDamageGuessed = guess;
    }

    public int GetGillDamage() {
        return gillDamage;
    }

    public int GetGillDamageGuessed() {
        return gillDamageGuessed;
    }

    public int GetId() {
        return id;
    }

    public int GetMarkedLice() {
        return markedLice;
    }

    public List<GameObject> GetLiceList(){
        return liceList;
    }

    //Couple of util functions for finding children by tag
    public List<GameObject> FindObjectwithTag(string _tag) {
         List<GameObject> tempList = new List<GameObject>();
         Transform parent = transform;
         GetChildObject(parent, _tag, tempList);
         return tempList;
    }
 
     public void GetChildObject(Transform parent, string _tag, List<GameObject> list) {
         for (int i = 0; i < parent.childCount; i++)
         {
             Transform child = parent.GetChild(i);
             if (child.tag == _tag)
             {
                 list.Add(child.gameObject);
             }
             if (child.childCount > 0)
             {
                 GetChildObject(child, _tag, list);
             }
         }
     }
}
