using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapOnDrop : MonoBehaviour
{

    public Transform targetPosition;
    private BNG.Grabbable grabbable;
    private BNG.ReturnToSnapZone snapReturn;
    private float snapRadius=1.5f;

    // Start is called before the first frame update
    void Start()
    {
        grabbable= GetComponent<BNG.Grabbable>();
        snapReturn= GetComponent<BNG.ReturnToSnapZone>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceTarget=Vector3.Distance(transform.position,targetPosition.position);
        if (distanceTarget<=snapRadius){
            // grabbable.DropItem(grabbable.GetPrimaryGrabber());
            // snapReturn.moveToSnapZone();
        }
    }

    private void SnapToTargetPosition(){
        transform.position=targetPosition.position;
        transform.rotation=targetPosition.rotation;
        
    }
}
