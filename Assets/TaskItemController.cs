using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskItemController : MonoBehaviour
{
    public string TaskName;
    public string Description; 
    
    // Start is called before the first frame update
    void Start(){
         GameObject textObject = transform.GetChild(0).gameObject;
         textObject.GetComponent<TextMeshPro>().text = TaskName;

    }
   
}
