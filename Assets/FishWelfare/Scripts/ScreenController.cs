using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using TMPro;

public class ScreenController : MonoBehaviour
{

    private InspectionTaskManager inspectionTaskManager;
    private TMP_Text screen;
    private List<Fish> inspectedFish;
    public RowUi rowUi; 

    // Start is called before the first frame update
    void Start()
    {
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
    }

    public void DrawScreen(Fish fish) {
        RowUi row = Instantiate(rowUi, transform).GetComponent<RowUi>();
        row.fish.text = fish.GetId().ToString();
        row.gillDamage.text = fish.GetGillDamageGuessed().ToString() + "/" + fish.GetGillDamage().ToString();
        //row.handling.text = fish.health.ToString();
        row.lice.text =fish.markedLice.ToString() + "/" + fish.GetLiceList().Count;
        row.score.text = "1";
        fish.scoreBoardEntry = row;
    }

    public void RemoveItem(int id) {
        inspectedFish = GameObject.FindObjectOfType<InspectionTaskManager>().GetInspectedFish();
        foreach(RowUi item in GetComponentsInChildren<RowUi>()){
            if(id.ToString() == item.fish.text){
                Destroy(item.gameObject);
            }
        }
    }
}
