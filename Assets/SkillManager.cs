using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [SerializeField] private Task.TaskHolder taskHolder;
    private MaintenanceManager manager;

    private List<int> completedSteps = new List<int>();
    [SerializeField] private Tablet.TaskListLoader1 taskListLoader;
    private int maxSteps;

    private Task.Task task => taskHolder.GetTask("Vedlikehold");


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        manager = this.gameObject.GetComponent<MaintenanceManager>();
        Task.Subtask subtask = task.GetSubtask("Runde På Ring");
        maxSteps = subtask.StepList.Count;
        // manager.BadgeChanged.AddListener(UpdateBadges);

    }

    public void UpdateBadges(Task.Step step)
    {
        if (step.ParentSubtask.SubtaskName == "Runde På Ring" && !step.IsCompeleted())
        {
            LightningBadge();
            if (step.IsCompeleted()) { StepwiseBadge(step.getStepNumber()); }
        }
        else if (step.StepName == "Snakk med Laila" && !step.IsCompeleted())
        {
            CuriousBadge();
        }
        else if (step.StepName == "Kast mat til fisken")
        {
            VideoBadge();
        }

    }
    private void CompleteBadge(string skillName)
    {
        // Task.Skill skill = taskHolder.GetSkill(skillName);
        // if (skill.IsLocked())
        // {
        //     skill.Unlock();
        //     manager.SkillCompleted.Invoke(skill);
        //     taskListLoader.LoadSkillsPage();
        // }

    }

    public void StepwiseBadge(int stepNumber)
    {
        if (!completedSteps.Contains(stepNumber)) completedSteps.Add(stepNumber);
        if (completedSteps.Count == maxSteps)
        {
            bool isIncremental = completedSteps.Zip(completedSteps.Skip(1), (current, next) => current + 1 == next).All(x => x);
            // if (isIncremental)
            //  CompleteBadge("Strukturert");  // Må finne nytt navn på denne
        }
    }

    public void CuriousBadge()
    {
        CompleteBadge("Kommunikasjon");
    }

    public void LightningBadge()
    {
        CompleteBadge("Tilpasningsdyktig");

    }
    public void VideoBadge()
    {
        CompleteBadge("Problemløsning");
    }
}