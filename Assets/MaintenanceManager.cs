using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintenanceManager : MonoBehaviour
{
    public static MaintenanceManager Instance;
    [SerializeField] private Task.TaskHolder taskHolder;
    [SerializeField] private Tablet.TaskListLoader1 taskListLoader;
    [SerializeField] private AudioClip success;

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
        foreach (Task.Task task in taskHolder.taskList)
        {
            foreach (Task.Subtask sub in task.Subtasks)
            {
                foreach (Task.Step step in sub.StepList)
                {
                    step.Reset();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CompleteStep(string subtaskName, string stepName)
    {
        Task.Task task = taskHolder.GetTask("Vedlikehold");
        Task.Subtask sub = task.GetSubtask(subtaskName);

        Task.Step step = sub.GetStep(stepName);
        step.CompleateRep();
        taskListLoader.SubTaskPageLoader(sub);
        taskListLoader.TaskPageLoader(task);
        taskListLoader.LoadSkillsPage();


    }

    public void PlaySuccess()
    {
        PlayAudio(success);
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