using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Fish : MonoBehaviour
{
    [SerializeField]
    private int gillDamage = 3;
    [SerializeField]
    private int gillDamageGuessed = 0;
    [SerializeField]
    private int id;
    [HideInInspector]
    public Vector3 waterBodyCenter;
    private Vector3 targetPosition;
    private Quaternion lookRotation;
    public float movementSpeed = .5f;
    private float originalMovementSpeed;
    public float rotationSpeed = 10f;
    private float originalRotationSpeed;
    [HideInInspector]
    public float waterBodyXLength;
    [HideInInspector]
    public float waterBodyZLength;
    private Quaternion originalRotation;
    [HideInInspector]
    public float waterHeight;
    [HideInInspector]
    public BNG.UIPointer uIPointer;
    public GameObject marker;
    private GameObject pointerFinger;
    private List<GameObject> liceList = new List<GameObject>();
    private List<GameObject> boneList = new List<GameObject>(); //;)
    InspectionTaskManager inspectionTaskManager;
    public LayerMask layer;
    [HideInInspector]
    public GameObject lastMarkedLouse;
    [HideInInspector]
    public int markedLice = 0;
    public int health = 10;
    private AudioSource hurtSound;
    [HideInInspector]
    public AudioSource markSound;
    [HideInInspector]
    public int isInWaterCount = 0;
    [HideInInspector]
    public int isGrabbedCount = 0;
    private bool kinematicBones = false;
    private Animator animator;
    private Transform fishbone;
    private bool damageInvulerability = false;
    public float damageInvulnerabilityTimer = 1f;
    public float SedationLevel = 1f;
    public TankController tank;

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
        AudioSource[] sounds = GetComponents<AudioSource>();
        hurtSound = sounds[0];
        markSound = sounds[1];
        //The point from which the raycast targeting lice on fishbodie will have its origin. In this case it is RightHandPointer in XR Rig Advanced
        pointerFinger = GameObject.FindGameObjectWithTag("Pointer");
        animator = GetComponent<Animator>();
        fishbone = boneList[0].transform;
        originalMovementSpeed = movementSpeed;
        originalRotationSpeed = rotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        followchild();
        if(isInWaterCount > 0 && isGrabbedCount <= 0){
            Move();
        } 
        else {
            Stop();
        }
        damageInvulnerabilityTimer -= Time.deltaTime;
        if(damageInvulnerabilityTimer <= 0f) {
            damageInvulerability = false;
        }
        Debug.Log("Sedation level: " + SedationLevel);
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
                GetComponent<Rigidbody>().isKinematic = true;
                //bone.GetComponent<Rigidbody>().velocity = Vector3.zero;
                kinematicBones = true;
                animator.enabled = true;
                //animator.SetBool("Swimming", true);
                targetPosition = new Vector3(transform.position.x, waterHeight - .7f, transform.position.z);
            }
        }

        if( Vector3.Distance(transform.position, targetPosition) > .1 ) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        updateSedation();
    }

    private void Stop() {
        if(kinematicBones) {
            foreach( GameObject bone in boneList) {
                bone.GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().isKinematic = false;
                kinematicBones = false;
                animator.enabled = false;
            }
        }
    }

    private void updateSedation() {
        if(SedationLevel > 0f && tank != null){
            SedationLevel -= Time.deltaTime * 0.01f;
        } else if(SedationLevel < 0f) {
            SedationLevel = 0f;
        }
        animator.speed = SedationLevel;
        movementSpeed = originalMovementSpeed * SedationLevel;
        rotationSpeed = (originalRotationSpeed * SedationLevel) / 1.5f;
    }

    private void followchild() {
        Vector3 originalPosition = fishbone.position;
        transform.position = fishbone.position;
        fishbone.position = originalPosition;
    }

    public void SetMoveTarget() {
        /*
        prøv å bruke posisjonen kun til hvor den skal rotere, deretter bare beveg den fremmover. Virker som den blir litt spastic av setMoveTarget i on collision og...
        */
        //Mer Skamløs koking fra FishScript.cs:
        float randX = Random.Range(waterBodyCenter.x -waterBodyXLength / 2,waterBodyCenter.x + waterBodyXLength / 2);
        float randZ = Random.Range(waterBodyCenter.z -waterBodyZLength / 2,waterBodyCenter.z + waterBodyZLength / 2);
        targetPosition = new Vector3(randX, transform.position.y, randZ);
        //Debug.Log("Position: " + targetPosition);
        lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
    }

    /*public void OnPointerClick(PointerEventData eventData) {
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
    }*/

    public void checkForDamage(bool hittingWater, float velocity) {
        if(!damageInvulerability){
            damageInvulerability = true;
            damageInvulnerabilityTimer = 1f;
                float damageThreshold = 2f;
            if (hittingWater) {
                damageThreshold = 3f;
            }
            else if(isGrabbedCount > 0){
                damageThreshold = 2f;
            }
            if(velocity > damageThreshold) {
                if(health > 0) {
                    health--;
                }
                hurtSound.Play(0);
            }
            Debug.Log("Taking Damage");
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
