using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Task {
public class TaskListController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject standardTask; 
    // public GameObject completedTask; 
    [SerializeField] public List<Task> tasks; 
    void Start()
    {
        
       foreach (Task task in tasks){
        GameObject taskEntry= Instantiate(standardTask) as GameObject; 
        TaskItemController controller= taskEntry.GetComponent<TaskItemController>();
        controller.TaskName=task.TaskName;
        controller.Description=task.Description;
       }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
}