using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{

    private Fish parent;
    private bool isInWater;
    private bool isGrabbed;
    private Rigidbody rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.root.gameObject.GetComponent<Fish>();
        rigidBody = GetComponent<Rigidbody>();
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
