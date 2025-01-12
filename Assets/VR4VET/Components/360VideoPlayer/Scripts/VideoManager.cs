﻿/* Copyright (C) 2020 IMTEL NTNU - All Rights Reserved
 * Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager videoManager;
    [SerializeField] private SkyboxHolder SkyboxHolder;

    private LayerMask oldCulingMask;
    private Camera VRCamera;
    private bool specialEventShouldTrigger;


    [HideInInspector] public VideoPlayer videoPlayer;
    [SerializeField] public bool videoShouldStopAfterPlayingOnce;
    //[SerializeField] public bool videoCantBeInterupted;
    [SerializeField] public UnityEvent onVideoEndAlways;
    [SerializeField] public UnityEvent specialEventOnVideoEnd;



    /// <summary>
    /// Unity start method
    /// </summary>
    private void Awake()
    {
        if (videoManager == null)
            videoManager = this;
        else if (videoManager != this)
            Destroy(gameObject);

        videoPlayer = GetComponent<VideoPlayer>();

        //Get the cameras original cullingMask
        VRCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        oldCulingMask = VRCamera.cullingMask;
    }


    /// <summary>
    /// Add the clip to the video manager, apply to the skybox and play the video
    /// </summary>
    /// <param name="clip"></param>
    public void ShowVideo(VideoClip clip, bool triggerSpecialEvent)
	{
        //change the video clip
        videoPlayer.clip = clip;
        specialEventShouldTrigger = triggerSpecialEvent;


        // play the video using the videoPlayer attatched to the platform
        videoPlayer.Play();

        StartCoroutine(applyVideo());

    }
    
    /// <summary>
    /// This method will wait untill the video clip is fully changed then apply it to the skybox
    /// </summary>
    /// <returns></returns>
    IEnumerator applyVideo()
    {
        while (!videoPlayer.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        // set skybox to video
        SkyboxHolder.applyVideoTextureToSkybox();

        // hide everything in sceene exept potantially the player and teleporting prefab (given not recomended setup)
        VRCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        VRCamera.cullingMask = 1 << LayerMask.NameToLayer("360Video");
        
        // if (videoCantBeInterupted && !videoPlayer.isLooping)
        // {
        //     while (videoPlayer.isPlaying)
        //     {
        //         yield return new WaitForEndOfFrame();
        //     }
        // }

        if (videoShouldStopAfterPlayingOnce && !videoPlayer.isLooping)
        {
            while (videoPlayer.isPlaying)
            {
                yield return new WaitForEndOfFrame();
            }
            StopVideo();
        }
    }


    /// <summary>
    /// Set the default skybox again and stop the video
    /// </summary>
    public void StopVideo()
    {
        videoPlayer.Stop();
        // Show everything in the scene again
        VRCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        VRCamera.cullingMask = oldCulingMask;

        SkyboxHolder.applyDefaultSkybox();

        onVideoEndAlways.Invoke();
        if (specialEventShouldTrigger)
        {
            specialEventOnVideoEnd.Invoke();
        }

    }


}
