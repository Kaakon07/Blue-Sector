using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fish : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int gillDamage = 3;
    [SerializeField]
    private int gillDamageGuessed = 0;
    [SerializeField]
    private int id;
    
    //The following variables are used for handeling behaviour with water
    private Vector3 waterBodyCenter;
    private Vector3 targetPosition;
    private Quaternion lookRotation;
    public float movementSpeed;
    public float rotationSpeed;
    private float waterBodyXLength;
    private float waterBodyZLength;
    private bool isInWater;
    private Rigidbody rigidBody;
    private Quaternion originalRotation;

    public BNG.UIPointer uIPointer;

    public GameObject marker;

    public GameObject pointerFinger;
    [SerializeField]
    private List<GameObject> liceList = new List<GameObject>();

    InspectionTaskManager inspectionTaskManager;
    public LayerMask layer;

    public GameObject lastMarkedLouse;

    // Start is called before the first frame update
    void Start()
    {
        //Skamløst kokt fra FishScript.cs:
        InvokeRepeating(nameof(PeriodicUpdates), Random.Range(0.0f, 5.0f), 1.0f);
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
        targetPosition = transform.position;
        rigidBody = GetComponent<Rigidbody>();
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(isInWater){
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            Move();
        } else {
            rigidBody.freezeRotation = false;
        }
    }

    void PeriodicUpdates() {
        if(isInWater){
            SetMoveTarget();    
        }
    }

    private void Move() {
        if( Vector3.Distance(transform.position, targetPosition) > .1 ) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void SetMoveTarget() {
        //Mer Skamløs koking fra FishScript.cs:
        //Debug.Log("Setting movement");
        float randX = Random.Range(waterBodyCenter.x -waterBodyXLength / 2,waterBodyCenter.x + waterBodyXLength / 2);
        float randZ = Random.Range(waterBodyCenter.z -waterBodyZLength / 2,waterBodyCenter.z + waterBodyZLength / 2);
        targetPosition = new Vector3(randX, transform.position.y, randZ);
        lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    public void UpdateWaterBody(float waterheight, Vector3 bodyCenter, float xLength, float zLength, bool isInWater){
        Debug.Log("Updating body");
        gameObject.GetComponent<Floating>().waterHeight = waterheight;
        waterBodyCenter = bodyCenter;
        waterBodyXLength = xLength;
        waterBodyZLength = zLength;
        this.isInWater = isInWater;
    }

    public void OnPointerClick(PointerEventData eventData) {
        //Debug.Log("Clicked!!" + eventData.pointerCurrentRaycast.worldPosition);
        lastMarkedLouse = checkForLouse(eventData.pointerCurrentRaycast.worldPosition);
        if(lastMarkedLouse != null){
            GameObject newmarker = Instantiate(marker,lastMarkedLouse.transform.position, new Quaternion(0,0,0,0));
            newmarker.transform.parent = transform;
        }
    }

    public GameObject checkForLouse(Vector3 origin) {
        Debug.Log("raycaster!");
        if(Physics.SphereCast(origin, 0.01f, pointerFinger.transform.forward, out RaycastHit hitInfo, 10f, layer)) {
            Debug.Log("inni reycast!");
            Debug.DrawRay(transform.position, pointerFinger.transform.forward * hitInfo.distance, Color.yellow);
            GameObject hit = hitInfo.collider.gameObject;
            GameObject newmarker = Instantiate(marker, hitInfo.transform.position, new Quaternion(0,0,0,0));
            Debug.Log(hitInfo.collider.tag);
            foreach(GameObject louse in liceList) {
                if (hit == louse) {
                    Debug.Log("fant en!");
                    return louse;
                }
            }
        }
        return null;
    }

    private void OnCollisionEnter(Collision other) {
        SetMoveTarget();
    }

    public void SetAsSelectedFish() {
        inspectionTaskManager.SetSelectedFish(this); 
    }

    public void SetgillDamageGuessed(int guess) {
        gillDamageGuessed = guess;
    }

    public void SetIsInWater( bool isInWater) {
        this.isInWater = isInWater;
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

}
