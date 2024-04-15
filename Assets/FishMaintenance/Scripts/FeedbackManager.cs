using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    private AddInstructionsToWatch watch;
    private MaintenanceManager manager;
    private Dictionary<string, List<string>> feedback;

    void Start()
    {
        manager = this.gameObject.GetComponent<MaintenanceManager>();
        watch = this.gameObject.GetComponent<AddInstructionsToWatch>();
        feedback = new Dictionary<string, List<string>>();
        manager.SubtaskChanged.AddListener(feedbackOnTaskComplete);
        feedback.Add("Hent Utstyr", new List<string> {
            "Grab equipment.",
            "Touch equipment to bring it with you.",
            "Good job! Move on to the next cylinder.",
            "",
            ""
        });
        feedback.Add("Håndforing", new List<string> {
            "Throw food to the fish 5 times. See video from the facility.",
            "",
            "Well done, this was a challenging task!",
            "Fetch BUCKET AND SHOVEL from the boat to start this task.",
            "Use the shovel to grab food from the bucket. Throw the food into the fish cage."
        });
        feedback.Add("Legg til tau på merd", new List<string> {
            "Add the missing rope.",
            "Touch the outline with the rope you have in your right hand.",
            "",
            "Fetch ROPE from the boat to start this task.",
            ""
        });
        feedback.Add("Legg til splinter på kjetting", new List<string> {
            "Add the missing splint to the chains.",
            "Touch the outline on the ground with the splint you have in your right hand.",
            "",
            "Fetch SPLINT on the boat to start this task.",
            "Touch the outline on the ground with the splint you have in your right hand. You can do this!"
        });
        feedback.Add("Reparer tau på merd", new List<string> {
            "Replace the bad rope.",
            "Touch the bad rope with the rope you have in your right hand.",
            "",
            "Fetch ROPE from the boat to start this task.",
            "Touch the bad rope with the rope you have in your right hand. You can do this!"
        });
        feedback.Add("Runde På Ring", new List<string> {
            "",
            "",
            "Good job, the fish cage is maintained.",
            "",
            ""
        });
        feedback.Add("Pause", new List<string> {
            "",
            "",
            "Well done!",
            "",
            ""
        });
    }


    public void addFeedback(string subtaskName)
    {
        if (subtaskName == "Legg til splinter på kjetting" && manager.GetStep("Runde På Ring", "Legg til tau på merd").IsCompeleted() && manager.GetStep("Runde På Ring", "Reparer tau på merd").IsCompeleted())
        // if (subtaskName == "Legg til splinter på kjetting" || subtaskName == "Håndforing")
        {
            StartCoroutine(emergencyFeedback(subtaskName));
            return;
        }
        watch.addInstructions(feedback[subtaskName][0]);
        StartCoroutine(moreFeedback(subtaskName));
        if (subtaskName == "Håndforing")
        {
            StartCoroutine(emergencyFeedback(subtaskName));
        }
    }

    IEnumerator moreFeedback(string subtaskName)
    {
        yield return new WaitForSeconds(20f);
        if (subtaskName == "Hent Utstyr" && manager.stepCount < 2)
        {
            watch.addInstructions(feedback[subtaskName][1]);
        }
        // else if (subtaskName == "Håndforing" && manager.GetStep(subtaskName, "Kast mat til fisken").getRepNumber() < 2)
        // {
        //     watch.addInstructions(feedback[subtaskName][1]);
        // }
        else if (subtaskName == "Reparer tau på merd" && !manager.GetStep("Runde På Ring", subtaskName).IsCompeleted())
        {
            watch.addInstructions(feedback[subtaskName][1]);
        }
        else if (subtaskName == "Legg til tau på merd" && !manager.GetStep("Runde På Ring", "Reparer tau på merd").IsCompeleted())
        {
            watch.addInstructions(feedback[subtaskName][1]);
        }
        else if (subtaskName == "Legg til splinter på kjetting" && !manager.GetStep("Runde På Ring", "Reparer tau på merd").IsCompeleted() && !manager.GetStep("Runde På Ring", "Legg til tau på merd").IsCompeleted())
        {
            watch.addInstructions(feedback[subtaskName][1]);
        }
        if (subtaskName != "Håndforing" && subtaskName != "Hent Utstyr")
        {
            Task.Step badgeStep=manager.GetStep("Runde På Ring", subtaskName);
            if(badgeStep.IsCompeleted()) manager.BadgeChanged.Invoke(badgeStep);
        }
    }

    IEnumerator emergencyFeedback(string subtaskName)
    {
        yield return new WaitForSeconds(40f);
        watch.addInstructions(feedback[subtaskName][4]);
    }

    public void feedbackOnTaskComplete(Task.Subtask subtask)
    {
        if (manager.stepCount < 6 || subtask.SubtaskName == "Håndforing")
        {
            watch.addInstructions(feedback[subtask.SubtaskName][2]);
        }
    }

    public void equipmentFeedback(string subtaskName)
    {
        watch.addInstructions(feedback[subtaskName][3]);
    }

    public void StopMoreFeedback()
    {
        StopAllCoroutines();
    }

    public void emptyInstructions()
    {
        watch.emptyInstructions();
        StopAllCoroutines();
    }

    public string getText()
    {
        return watch.getText();
    }
}
