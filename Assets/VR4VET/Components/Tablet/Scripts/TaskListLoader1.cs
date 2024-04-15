/*
 * Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tablet
{
    public class TaskListLoader1 : MonoBehaviour
    {
        private List<Task.Task> _tasks = new List<Task.Task>();
        private List<Task.Skill> _skills = new List<Task.Skill>();

        private StaticPanelManager panelManager;

        //main pages

        [Header("Main Page Canvas")]
        public GameObject tasksListCanvas;

        public GameObject subtaskPageCanvas;
        public GameObject TaskPageCanvas;
        public GameObject skillsListPageCanvas;
        public GameObject skillPageCanvas;

        //parents objects to load the buttons in
        [Header("Content Spaces")]
        public GameObject taskContent;

        public GameObject TaskSubtaskContent;
        public GameObject skillContent;
        [SerializeField] private GameObject _subtaskContent;
        [SerializeField] private GameObject _skillSubtaskContent;

        [Header("Badge elements")]
        [SerializeField] private TMP_Text _badgeName;
        [SerializeField] private TMP_Text _badgeInstructions;
        [SerializeField] private Image _displayBadge;

        [Header("skill other")]
        [SerializeField] private TMP_Text _skillTab;

        [SerializeField] private TMP_Text _skillFeedback;
        [SerializeField] private TMP_Text _skillabout;
        [SerializeField] private TMP_Text _skillPoints;

        [Header("task other")]
        [SerializeField] private TMP_Text _taskNameTab;

        [SerializeField] private TMP_Text _taskFeedback;
        [SerializeField] private TMP_Text _taskAboutTab;

        [Header("subtask other")]
        [SerializeField] private TMP_Text _subtaskNameTab;

        [SerializeField] private TMP_Text _subtaskAboutTab;

        [Header("Experience Name")]
        [SerializeField] private string Exp_Name;

        [Header("UI Prefabs")]
        [SerializeField] private GameObject _skillEntryList;
        [SerializeField] private GameObject _stepListEntry;
        [SerializeField] private GameObject _subtaskListEntry;
        [SerializeField] private GameObject _taskListEntry;
        [SerializeField] private GameObject _badgeEntry;
        [SerializeField] private GameObject _skillBadgesList;

        [Header("Additional Events")]
        [SerializeField] private UnityEvent _skillPageOpen;

        [SerializeField] private UnityEvent _tasksListOpen;
        [SerializeField] private UnityEvent _taskPageOpen;
        [SerializeField] private UnityEvent _subtaskPageOpen;

        private List<GameObject> _skillsClones = new List<GameObject>();

        public static TaskListLoader1 Ins;
        private void Start()
        {
            if (Ins == null)
            {
                Ins = this;
            }
            else
            {
                Destroy(this);
            }

        }
        private void Awake()
        {
            //setting loading the scriptable objects
            Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];
            _tasks = th.taskList;
            _skills = th.skillList;

            panelManager = this.gameObject.GetComponent<StaticPanelManager>();
            //load info in the tablet
            StartCoroutine(LoadTabletInfo());
            panelManager.SetExperienceName(Exp_Name);
        }

        //since task and skill won't change in the experience we can load them from the beginning
        private IEnumerator LoadTabletInfo()
        {
            yield return new WaitForSeconds(2);
            TaskPageLoader(_tasks[0]);
            // LoadTaskPage();
            LoadSkillsPage();
        }

        public void UpdateSkillPoints()
        {
            for (int i = 0; i < _skills.Count; i++)
            {
                _skillsClones[i].GetComponentInChildren<TMP_Text>().text =
                  _skills[i].GetArchivedPoints() + "/" + _skills[i].MaxPossiblePoints;
            }
        }
        public void LoadSkillsPage()
        {

            // Reset existing gameobjects in skills page
            Transform parentTransform = skillContent.transform;
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Destroy(parentTransform.GetChild(i).gameObject);
            }

            //loads each skill on the parent object
            foreach (Task.Skill skill in _skills)
            {
                // Initiate a parent for list of badges and skill title
                GameObject skillBadgesContent = Instantiate(_skillBadgesList, Vector3.zero, Quaternion.identity);
                // Add the horizontal list to vertical content list
                skillBadgesContent.transform.SetParent(skillContent.transform);
                skillBadgesContent.transform.localPosition = Vector3.zero;
                skillBadgesContent.transform.localScale = Vector3.one;
                skillBadgesContent.transform.localRotation = Quaternion.Euler(0, 0, 0);


                // Set title to be name of this skill
                TMP_Text skillName = skillBadgesContent.transform.Find("Txt_SkillName").GetComponent<TMP_Text>();
                skillName.text = GetEnglishName(skill.Name, "skill");

                // Find Horizontal list to place badges
                GameObject BadgesList = skillBadgesContent.transform.Find("List_Badges").gameObject;

                // Start display vs badge info display
                // GameObject badgeDisplay = skillBadgesContent.transform.Find("badge_display").gameObject;
                // GameObject startDisplay = skillBadgesContent.transform.Find("Txt_start").gameObject;

                //Add connected badges to this horizontal list
                foreach (Task.Badge badge in skill.ConnectedBadges)
                {
                    GameObject badgeItem = Instantiate(_badgeEntry, Vector3.zero, Quaternion.identity);

                    badgeItem.transform.SetParent(BadgesList.transform);
                    badgeItem.transform.localPosition = Vector3.zero;
                    badgeItem.transform.localScale = Vector3.one;
                    badgeItem.transform.localRotation = Quaternion.Euler(0, 0, 0);

                    // Set Badge Icon
                    UnityEngine.UI.Image buttonIcon = badgeItem.transform.Find("icon_badge").GetComponent<UnityEngine.UI.Image>();
                    buttonIcon.sprite = badge.Icon;

                    // Find button
                    Button button = badgeItem.transform.Find("btn_badge").GetComponent<Button>();

                    // Set icon with shader and padlock if badge is locked
                    GameObject padlock = badgeItem.transform.Find("padlock").gameObject;
                    padlock.SetActive(badge.IsLocked());
                    GameObject completedBackground = badgeItem.transform.Find("CompletedBackground").gameObject;
                    completedBackground.SetActive(!badge.IsLocked());



                    // Set a listener for badge button click

                    button.onClick.AddListener(() => BadgeInfoLoader(badge));

                    // if (startDisplay.activeInHierarchy)
                    // {
                    //     startDisplay.SetActive(false);
                    //     badgeDisplay.SetActive(true);
                    // }
                    // Debug.Log("skill item added, " + item.name + " skill:" + skill.Name);
                    // _skillsClones.Add(item);

                    // // we find the button first and then its text component
                    // Button button = item.transform.Find("btn_SubTask").GetComponent<Button>();
                    // TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(true);
                    // buttonText.text = skill.Name;
                    // item.GetComponentInChildren<TMP_Text>().text =
                    //     skill.GetArchivedPoints() + "/" + skill.MaxPossiblePoints;

                    // button.onClick.AddListener(() => SkillPageLoader(skill));
                }
            }
            // refreshing after adding the new elements for the Page loader to set the pages correctly
            // skillContent.GetComponent<ContentPageChanger>().Refresh();
        }
        public void BadgeInfoLoader(Task.Badge badge)
        {
            Transform badgeInfo = skillsListPageCanvas.transform.Find("BadgeInfo").transform;
            GameObject badgeDisplay = badgeInfo.Find("badge_display").gameObject;
            if (!badgeDisplay.activeInHierarchy)
            {
                GameObject startDisplay = badgeInfo.Find("Txt_start").gameObject;
                startDisplay.SetActive(false);
                badgeDisplay.SetActive(true);
            }
            _badgeName.text = GetEnglishName(badge.Name, "badge");
            _badgeInstructions.text = badge.Instruction;
            _displayBadge.sprite = badge.Icon;
        }
        public void SkillPageLoader(Task.Skill skill)
        {
            if (_skillPageOpen != null) _skillPageOpen.Invoke();





            // hide the subtask list view
            panelManager.OnClickToAboutSkill();

            _skillTab.text = skill.Name;
            _skillabout.text = skill.Description;
            _skillFeedback.text = skill.Feedback;
            _skillPoints.text = skill.GetArchivedPoints() + "/" + skill.MaxPossiblePoints;

            //cleaning list before loading the new subtasks
            foreach (Transform child in _skillSubtaskContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }


            foreach (Task.Badge badge in skill.ConnectedBadges)
            {
                GameObject item = Instantiate(_badgeEntry, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(_skillSubtaskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }



            // foreach (Task.Subtask sub in skill.Subtasks)
            // {
            //     //task for the list
            //     GameObject item = Instantiate(_subtaskListEntry, Vector3.zero, Quaternion.identity);
            //     item.transform.SetParent(_skillSubtaskContent.transform);
            //     item.transform.localPosition = Vector3.zero;
            //     item.transform.localScale = Vector3.one;
            //     item.transform.localRotation = Quaternion.Euler(0, 0, 0);

            //     TMP_Text caption = item.transform.Find("txt_SubTaskNr").GetComponent<TMP_Text>();
            //     // GameObject points = item.transform.Find("PointText").gameObject; points for later
            //     caption.text = sub.SubtaskName;

            //     Button button = item.transform.Find("btn_SubTask").GetComponent<Button>();
            //     GameObject checkmark = item.transform.Find("img_Checkmark").gameObject;
            //     if (sub.Compleated()) checkmark.SetActive(true);
            //     button.onClick.AddListener(() => SubTaskPageLoader(sub));
            // }
            // refreshing after adding the new elements for the Page loader to set the pages correctly
        }



        public string GetEnglishName(string name, string type)
        {
            if (type == "subtask")
            {
                if (name == "Hent Utstyr")
                {
                    return "Get Equipment";
                }
                if (name == "Runde På Ring")
                {
                    return "Fish Cage Check";
                }
                if (name == "Håndforing")
                {
                    return "Hand Feeding";
                }
                if (name == "Pause")
                {
                    return "Break";
                }
            }
            else if (type == "step")
            {
                if (name == "Reparer tau på merd")
                {
                    return "Repair rope on fish cage";
                }
                if (name == "Legg til tau på merd")
                {
                    return "Add rope to fish cage";
                }
                if (name == "Legg til splinter på kjetting")
                {
                    return "Add splinters to chain";
                }
                if (name == "Kast mat til fisken")
                {
                    return "Feed the fish";
                }
                if (name == "Snakk med Laila")
                {
                    return "Talk to Laila";
                }
                // Hent bøtte og spade, Hent tau, Hent splinter
                if (name == "Hent bøtte og spade")
                {
                    return "Get bucket and shovel";
                }
                if (name == "Hent tau")
                {
                    return "Get rope";
                }
                if (name == "Hent splinter")
                {
                    return "Get splinter";
                }
            }
            else if (type == "skill")
            {
                if (name == "Tilpasningsdyktig")
                {
                    return "Adaptable";
                }
                if (name == "Kommunikasjon")
                {
                    return "Communicative";
                }
                if (name == "Problemløsning")
                {
                    return "Problem Solving";
                }
            }
            // Create english variant for badges: Lynet, Skarpsyn, Stålvilje, Stegmester
            else if (type == "badge")
            {
                if (name == "Nyskjerrigper")
                {
                    return "Curious";
                }
                if (name == "Lynet")
                {
                    return "Quick";
                }
                if (name == "Skarpsyn")
                {
                    return "Sharp Vision";
                }
                if (name == "Stålvilje")
                {
                    return "Strong willed";
                }
                if (name == "Stegmester")
                {
                    return "Step master";
                }
            }
            return "";
        }
        //gets called on Start since the list of task is always the same
        public void LoadTaskPage()
        {
            if (_tasksListOpen != null) _tasksListOpen.Invoke();

            Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];
            _tasks = th.taskList;


            //loads each task on the parent object
            // will add the task
            foreach (Task.Task task in _tasks)
            {

                // Remove line after testing and uncomment line in end of for loop 
                TaskPageLoader(task);



                //task for the list
                GameObject item = Instantiate(_taskListEntry, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(taskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                TMP_Text caption = item.transform.Find("txt_TaskNr").GetComponent<TMP_Text>();
                caption.text = task.TaskName;
                Button button = item.transform.Find("btn_Task").GetComponent<Button>();
                GameObject completedButton = item.transform.Find("btn_TaskComplete").gameObject;
                GameObject checkmark = item.transform.Find("img_Checkmark").gameObject;
                if (task.Compleated())
                {
                    checkmark.SetActive(true);
                    completedButton.SetActive(true);
                    button = item.transform.Find("btn_TaskComplete").GetComponent<Button>();
                };

                button.onClick.AddListener(() => panelManager.OnClickBackToAboutTask());
                // Commented out for testing to start on Task About page
                // button.onClick.AddListener(() => TaskPageLoader(task));
            }
            // refreshing after adding the new elements for the Page loader to set the pages correctly
            // taskContent.GetComponent<ContentPageChanger>().Refresh();
        }

        public void TaskPageLoader(Task.Task task)
        {
            //for extra events
            if (_taskPageOpen != null) _taskPageOpen.Invoke();

            // panelManager.OnClickBackToAboutTask();

            //hide previos pagee
            _taskNameTab.text = task.TaskName;
            _taskAboutTab.text = task.Description;
            _taskFeedback.text = task.Feedback;

            //cleaning list before loading the new subtasks
            foreach (Transform child in TaskSubtaskContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (Task.Subtask sub in task.Subtasks)
            {

                //task for the list
                GameObject item = Instantiate(_subtaskListEntry, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(TaskSubtaskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                TMP_Text caption = item.transform.Find("txt_SubTaskNr").GetComponent<TMP_Text>();
                // GameObject points = item.transform.Find("PointText").gameObject; points for later
                caption.text = GetEnglishName(sub.SubtaskName, "subtask");

                Button button = item.transform.Find("btn_SubTask").GetComponent<Button>();
                GameObject completedButton = item.transform.Find("btn_SubTaskComplete").gameObject;
                GameObject checkmark = item.transform.Find("img_Checkmark").gameObject;
                if (sub.Compleated())
                {
                    checkmark.SetActive(true);
                    completedButton.SetActive(true);
                    button = item.transform.Find("btn_SubTaskComplete").GetComponent<Button>();
                    caption.text = GetEnglishName(sub.SubtaskName, "subtask");
                };
                button.onClick.AddListener(() => SubTaskPageLoader(sub));
            }
        }

        public void SubTaskPageLoader(Task.Subtask subtask)
        {

            if (_subtaskPageOpen != null) _subtaskPageOpen.Invoke();

            //hide previos pagee
            // panelManager.OnClickSkillSubtasks();

            TaskPageCanvas.SetActive(false);
            subtaskPageCanvas.SetActive(true);

            _subtaskNameTab.GetComponent<TMP_Text>().text = GetEnglishName(subtask.SubtaskName, "subtask");
            _subtaskAboutTab.GetComponent<TMP_Text>().text = subtask.Description;

            //cleaning list before loading the new subtasks
            foreach (Transform child in _subtaskContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            // int stepNumber = 1;
            foreach (Task.Step step in subtask.StepList)
            {
                GameObject item = Instantiate(_stepListEntry, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(_subtaskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);


                if (step.IsCompeleted())
                {
                    GameObject completedStep = item.transform.Find("Completed").gameObject;
                    GameObject standardStep = item.transform.Find("Standard").gameObject;
                    completedStep.SetActive(true);
                    standardStep.SetActive(false);
                };
                TMP_Text caption = item.transform.Find("txt_Desc").GetComponent<TMP_Text>();
                caption.text = GetEnglishName(step.StepName, "step");

                TMP_Text reps = item.transform.Find("txt_repetitionNr").GetComponent<TMP_Text>();
                if (step.RepetionNumber > 1) reps.text = step.RepetionsCompleated + "/" + step.RepetionNumber;

                TMP_Text number = item.transform.Find("txt_SubTaskNr").GetComponent<TMP_Text>();
                number.text = step.getStepNumber() + "";
                // number.text = stepNumber + "";
                // stepNumber++;
            }
        }
    }
}