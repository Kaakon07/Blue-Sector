using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Task;

public class TabletTaskManager : MonoBehaviour
{
    TaskHolder TH;
    public Gauge Gauge;
    float GaugeValue;
    // Start is called before the first frame update
    void Start()
    {
        TH = transform.GetComponent<TaskHolder>();
    }

    // Update is called once per frame
    void Update()
    {
       GaugeValue = Gauge.measuringGauge.value; 
    }

    public void CheckIfAnestheticSatisfactory()
    {
        if (GaugeValue >= 0.01f)
        {
            TH.GetTask("Health Inspection").GetSubtask("Apply Anesthetic").GetStep("Pour anesthetic into tank").CompleateRep();
        }
    }
}
