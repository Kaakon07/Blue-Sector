using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;


public class GameManagerTests
{
    private GameManager gameManager;
    private GameObject gameManagerObject;
    private GameObject audioManager;
    private const string TestSceneName = "GameManagerTestScene";

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject and add the GameManager component to it
        gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();

        // Load the sound effects
        var audioManagerPrefab = Resources.Load<GameObject>("Prefabs/AudioManager");
        audioManager = Object.Instantiate(audioManagerPrefab);
        gameManager.SoundEffects = Resources.LoadAll<AudioClip>("Sounds");
        gameManager.AudioManager = audioManager;

        //create an audio listener
        var listener = new GameObject("AudioListener");
        listener.AddComponent<AudioListener>();

        //configure hand objects
        gameManager.LeftHandGameObj = new GameObject();
        gameManager.RightHandGameObj = new GameObject();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(gameManagerObject);
        Object.Destroy(gameManager);
    }

    [UnityTest]
    public IEnumerator ToggleTaskOnTest()
    {
        Assert.IsFalse(gameManager.IsTaskOn);
        gameManager.ToggleTaskOn();
        Assert.IsTrue(gameManager.IsTaskOn);
        gameManager.ToggleTaskOn();
        Assert.IsFalse(gameManager.IsTaskOn);
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlaySoundTest()
    {
        gameManager.PlaySound("correct");
        yield return new WaitForSeconds(1);
        Debug.Log(audioManager.GetComponent<AudioSource>().clip);
        Debug.Log(gameManager.SoundEffects[0]);
        Assert.AreEqual(audioManager.GetComponent<AudioSource>().clip, gameManager.SoundEffects[0]);
        gameManager.PlaySound("incorrect");
        yield return new WaitForSeconds(1);
        Assert.AreEqual(audioManager.GetComponent<AudioSource>().clip, gameManager.SoundEffects[1]);
    }
};
