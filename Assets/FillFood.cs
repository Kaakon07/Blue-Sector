using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillFood : MonoBehaviour
{
public Transform merd;
private GameObject food;
    // Start is called before the first frame update
    void Start()
    {
      this.food=gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag=="Bucket"){
        food.SetActive(true);
        }
        
    }
    void Update()
    {
float radius=20f;
    Vector3 shovelPosition= new Vector3(transform.position.x,0f, transform.position.z);
    Vector3 merdPosition= new Vector3(merd.position.x, 0f, merd.position.z);
    float distanceCenter= Vector3.Distance(shovelPosition,merdPosition);
    Debug.Log(shovelPosition);
    if(distanceCenter>=radius-3f && distanceCenter<=radius+3f){
        food.SetActive(false);
       
    }
    }
}
