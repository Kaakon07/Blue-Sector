using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    /*[Header("Scene Controller")]
    [Tooltip("The name of the scene to change to")]
    [SerializeField] private string sceneName;
    private AudioClip doorSound;
    private Vector3 nextScenePlayerPosition;
    private Vector3 nextScenePlayerRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    /// <summary>
    /// When the player collides with the scene controller, change the scene
    /// </summary>
    /// <param name="collisionObject">The player collider</param>
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.gameObject.name == "Grabber")
        {
            //OnChangeScene.Invoke();
            ChangeScene(sceneName);
        }
    }

    /// <summary>
    /// Change the scene.
    /// </summary>
    /// <param name="scene">The name of the scene to change to</param>
    private void ChangeScene(string scene)
    {
        if (scene == "FishFeeding")
        {
            GameManager.Instance.PlaySound("door");
            SceneManager.LoadScene(scene);
            return;
        }

        // Check if the player is in the HSE room and if the requirements are fulfilled. Player should only be able to progress after the HSE room is completed.
        if (
            !GameManager.Instance.HSERoomCompleted
            && SceneManager.GetActiveScene().name == "HSERoom"
        )
        {
            GameManager.Instance.PlaySound("incorrect");
            return;
        }

        // Set the target location (exit door) the player should be placed by after the scene change
        SetPlayerTargetLocation(scene);

        // Adds a delegate to trigger when the scene is loaded
        // The delegate will move the player to the correct location
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameManager.Instance.PlaySound("door");
        SceneManager.LoadScene(scene);
    }

    /// <summary>
    /// Set the target location the player should be placed by after the scene change
    /// </summary>
    /// <param name="scene">The name of the scene to change to</param>
    private void SetPlayerTargetLocation(string scene)
    {
        // Get scene name and destination scene in the format "CurrentSceneNameToDestinationScene"
        string targetSpawnLocation = SceneManager.GetActiveScene().name + "To" + scene;
        switch (targetSpawnLocation)
        {
            case "HSERoomToBleedingStation":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    11.1490002f,
                    1.03299999f,
                    -6.76000023f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, 270f, 0f);
                break;
            case "HSERoomToQAStation":
                GameManager.Instance.NextScenePlayerPosition = new Vector3(
                    -10.191f,
                    1.06599998f,
                    -10.0489998f
                );
                GameManager.Instance.NextScenePlayerRotation = new Vector3(0f, 90f, 0f);
                break;
        }
    }

    /// <summary>
    /// Move the player to the correct location after the scene has been loaded
    /// </summary>
    /// <param name="scene"> The scene that has been loaded</param>
    /// <param name="mode"> The mode the scene has been loaded in</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // The player should be placed at the initial spawn point if the new player position is not set. This is to prevent the player from spawning at 0,0,0.
        if (GameManager.Instance.NextScenePlayerPosition == new Vector3(0, 0, 0))
        {
            Debug.LogError("Location was not set correctly. Spawning player at initial location.");
            SceneManager.sceneLoaded -= OnSceneLoaded;
            return;
        }

        // Moving the player will not work if the XR Rig prefab has been renamed!
        GameObject player = GameObject.Find("XR Rig Advanced VR4VET");
        player.transform.position = GameManager.Instance.NextScenePlayerPosition;
        player.transform.eulerAngles = GameManager.Instance.NextScenePlayerRotation;

        // Reset the new player position
        GameManager.Instance.NextScenePlayerPosition = new Vector3(0, 0, 0);
        GameManager.Instance.NextScenePlayerRotation = new Vector3(0, 0, 0);

        GameManager.Instance.IsTaskOn = false;
        GameManager.Instance.IsSecondaryTaskOn = false;

        // Remove the delegate to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }*/
}
