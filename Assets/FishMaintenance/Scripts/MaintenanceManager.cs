using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class MaintenanceManager : MonoBehaviour
{
    public static MaintenanceManager Instance;
    [SerializeField] private Task.TaskHolder taskHolder;
    [SerializeField] private Tablet.TaskListLoader1 taskListLoader;
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip equipmentPickup;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private GameObject toBoatArrow;
    [SerializeField] private GameObject[] firstArrows;
    [SerializeField] private GameObject pauseAnchor;
    private bool twentySeconds = false;
    private AddInstructionsToWatch watch;
    private FeedbackManager feedbackManager;
    private Task.Task task => taskHolder.GetTask("Vedlikehold");
    [HideInInspector] public int stepCount;

    public Task.Task MaintenanceTask { get => task; }

    public UnityEvent<Task.Step?> BadgeChanged { get; } = new();
    public UnityEvent<Task.Skill?> SkillCompleted { get; } = new();
    public UnityEvent<Task.Subtask?> SubtaskChanged { get; } = new();
    public UnityEvent TaskCompleted { get; } = new();
    public UnityEvent<Task.Subtask?> CurrentSubtask { get; } = new();
    // Start is called before the first frame update
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
        feedbackManager = this.gameObject.GetComponent<FeedbackManager>();
        watch = this.gameObject.GetComponent<AddInstructionsToWatch>();
        UpdateCurrentSubtask(task.GetSubtask("Hent Utstyr"));

        // Reset subtsk and step progress on each play, and skill and badge progress
        foreach (Task.Subtask sub in task.Subtasks)
        {
            foreach (Task.Step step in sub.StepList)
            {
                step.Reset();
            }
        }
        foreach (Task.Skill skill in taskHolder.skillList)
        {
            skill.Lock();
        }

    }
    public void lynetEnabled(bool passed)
    {
        twentySeconds = passed;
    }
    public void CompleteStep(Task.Step step)
    {

        Task.Subtask sub = step.ParentSubtask;
        step.CompleateRep();
        taskListLoader.SubTaskPageLoader(sub);
        taskListLoader.TaskPageLoader(task);

        // Task.Skill skill = taskHolder.GetSkill("Kommunikasjon");
        if (step.IsCompeleted())
        {
            PlayAudio(success);
            stepCount += 1;
            feedbackManager.emptyInstructions();
            if (sub.SubtaskName == "Runde På Ring" && twentySeconds)
            {
                BadgeChanged.Invoke(step);
            }
            SubtaskChanged.Invoke(sub);
            if (sub.StepList.Count < sub.GetCompletedSteps())
            {
                Task.Step nextStep = sub.StepList.Find(element => (!element.IsCompeleted()));
                nextStep.CurrentStep = true;
                step.CurrentStep = false;
                UpdateCurrentSubtask(sub);

            }


        }
        if (sub.Compleated())
        {
            string subtaskName = sub.SubtaskName;
            SubtaskChanged.Invoke(sub);
            if (task.Compleated())
            {
                TaskCompleted.Invoke();
            }

            int elementIndex = task.Subtasks.FindIndex(elem => elem.SubtaskName == subtaskName);
            if (elementIndex < task.Subtasks.Count - 1)
            {
                UpdateCurrentSubtask(task.Subtasks.ElementAt(elementIndex + 1));
            }

            if ((subtaskName == "Runde På Ring" && task.GetSubtask("Håndforing").Compleated()) || (subtaskName == "Håndforing" && task.GetSubtask("Runde På Ring").Compleated()))
            {
                NavigateToBoat();
            }

        }
    }

    public void NavigateToBoat()
    {
        toBoatArrow.SetActive(true);
        foreach (GameObject arrow in firstArrows)
        {
            arrow.SetActive(false);
        }
        foreach (GameObject arrow in arrows)
        {
            arrow.transform.Rotate(0.0f, 180.0f, 0.0f, Space.World);
        }
        pauseAnchor.SetActive(true);
    }

    public void UpdateCurrentSubtask(Task.Subtask subtask)
    {
        CurrentSubtask.Invoke(subtask);
    }


    public Task.Step GetStep(string subtaskName, string stepName)
    {

        Task.Subtask sub = task.GetSubtask(subtaskName);

        Task.Step step = sub.GetStep(stepName);
        return step;
    }



    public void PlayAudio(AudioClip audio)
    {
        //if the gameobject has audiosource
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(audio);
            return;
        }

        //otherwise create audiosource
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.PlayOneShot(audio);
    }

}
