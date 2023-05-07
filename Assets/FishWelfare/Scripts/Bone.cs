using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bone : MonoBehaviour, IPointerClickHandler
{

    private Fish parent;
    private bool isInWater;
    private bool isGrabbed;
    private Rigidbody rigidBody;

    private GameObject lastMarkedLouse = null;
    private GameObject pointerFinger = null;
    public GameObject marker;
    private List<GameObject> liceList = new List<GameObject>();
    public LayerMask layer;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.root.gameObject.GetComponent<Fish>();
        rigidBody = GetComponent<Rigidbody>();
        //The point from which the raycast targeting lice on fishbodie will have its origin. In this case it is RightHandPointer in XR Rig Advanced
        pointerFinger = GameObject.FindGameObjectWithTag("Pointer");
        liceList = parent.FindObjectwithTag("Louse");
        layer = parent.layer;
        marker = parent.marker;
    }

    // Update is called once per frame
    void Update()
    {
        if(isInWater && !isGrabbed){
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        } else {
            rigidBody.freezeRotation = false;
        }
    }

    public void UpdateWaterBody(float waterheight, Vector3 bodyCenter, float xLength, float zLength, bool isInWater){
        //gameObject.GetComponent<Floating>().waterHeight = waterheight;
        parent.waterBodyCenter = bodyCenter;
        parent.waterBodyXLength = xLength;
        parent.waterBodyZLength = zLength;
        parent.waterHeight = waterheight;
        SetIsInWater(isInWater);
    }

    public void OnPointerClick(PointerEventData eventData) {
        lastMarkedLouse = checkForLouse(eventData.pointerCurrentRaycast.worldPosition);
        if(lastMarkedLouse != null){
            GameObject newmarker = Instantiate(marker,lastMarkedLouse.transform.position, new Quaternion(0,0,0,0));
            newmarker.transform.parent = lastMarkedLouse.transform;
        }
    }

    public GameObject checkForLouse(Vector3 origin) {
        if(Physics.SphereCast(origin - (pointerFinger.transform.forward*.5f), 0.02f, pointerFinger.transform.forward, out RaycastHit hitInfo, 10f, layer)) {
            GameObject hit = hitInfo.collider.gameObject;
            foreach(GameObject louse in liceList) {
                if (hit == louse && !louse.GetComponent<Louse>().marked) {
                    louse.GetComponent<Louse>().marked = true;
                    parent.markedLice++;
                    return louse;
                }
            }
        }
        return null;
    }

    private void OnCollisionEnter(Collision other) {
        //parent.SetMoveTarget();
    }

    public void SetIsInWater(bool isInWater) {
        if (isInWater){
            parent.isInWaterCount++;
        } 
        else {
            parent.isInWaterCount--;
        }
    }

    public void SetIsGrabbed(bool isGrabbed) {
        if (isGrabbed) parent.isGrabbedCount++;
        else parent.isGrabbedCount--;
    }
}
