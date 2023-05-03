using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    private InspectionTaskManager inspectionTaskManager;
    public bool isGoal = false;
    private BoxCollider collider = new BoxCollider();
    // Start is called before the first frame update
    void Start()
    {
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(isGoal){
            inspectionTaskManager.ProgressInspection(other.gameObject);
        }
        if(other.tag == "Bone"){
            other.gameObject.GetComponent<Bone>().UpdateWaterBody(transform.position.y + transform.localScale.y, transform.position, transform.localScale.x, transform.localScale.z, true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(isGoal){
            inspectionTaskManager.RegressInspection(other.gameObject);
        }
        if(other.tag == "Bone"){
            other.gameObject.GetComponent<Bone>().UpdateWaterBody(0f, new Vector3(0f,0f,0f), 0f, 0f, false);
        }
    }
}
