using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.XR.Management;

public class EmulatorOffsetEnabler : MonoBehaviour
{
    private bool _hmdExists;
    [SerializeField] private Transform Player;
    private PlayerTeleport _teleportScript;
    // Start is called before the first frame update
    void Start()
    {
        if(!Player)
        {
            try
            {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            catch (UnassignedReferenceException)
            {
                Debug.LogError("Assign the player under the tooltip or with a Player tag");
                throw;
            }
        }

        // Get the PlayerController
        Player = Player.Find("PlayerController");

        _teleportScript = GetComponent<PlayerTeleport>();
        _hmdExists = XRGeneralSettings.Instance.Manager.activeLoader != null;
        if(!_hmdExists)
        {
            _teleportScript.TeleportYOffset = Player.GetComponent<BNGPlayerController>().ElevateCameraHeight;
        }
    }
}
