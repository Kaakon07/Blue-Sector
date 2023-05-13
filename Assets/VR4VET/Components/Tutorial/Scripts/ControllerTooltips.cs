using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class ControllerTooltips : MonoBehaviour
{
    public Transform Player;
    public Transform ActiveControllers;
    private Transform ActiveL;
    private Transform ActiveR;
    public Transform OVRMod;
    public Transform IndexMod;
    public Transform _destinationL;
    public Transform _destinationR;
    [SerializeField]
    private List<Transform> _oldModelR;
    private List<Transform> _oldModelL;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Player = Player.Find("PlayerController/CameraRig/TrackingSpace");
        Debug.Log("player: " + Player);
        //_destinationL = Player.Find("LeftHandAnchor/LeftControllerAnchor/LeftController/ModelsLeft");
        //_destinationR = Player.Find("RightHandAnchor/RightControllerAnchor/RightController/ModelsRight");
        OVRMod = transform.Find("OculusModels");
        IndexMod = transform.Find("SteamVRModels");

        // Disable other hand models and save them for later use
        for(int i = 0; i < _destinationL.childCount; i++) {
            if (_destinationL.GetChild(i).gameObject.activeSelf) {
                _oldModelL.Add(_destinationL.GetChild(i));
                _destinationL.GetChild(i).gameObject.SetActive(false);
            }
        }
        for(int i = 0; i < _destinationR.childCount; i++) {
            if (_destinationR.GetChild(i).gameObject.activeSelf) {
                _oldModelR.Add(_destinationR.GetChild(i));
                _destinationR.GetChild(i).gameObject.SetActive(false);
                Debug.Log(_destinationR.GetChild(i).gameObject.activeSelf);
            }
        }
        Debug.Log(InputBridge.Instance.IsValveIndexController);
        if(InputBridge.Instance.IsValveIndexController)
        {
            // Load Index model
            ActiveL = IndexMod.Find("LeftTTController");
            ActiveR = IndexMod.Find("RightTTController");
            ActiveControllers = IndexMod;
        }
        else
        {
            // Load OVR model
            ActiveL = OVRMod.Find("LeftTTController");
            ActiveR = OVRMod.Find("RightTTController");
            ActiveControllers = OVRMod;
        }
        ActiveL.parent = _destinationL;
        ActiveL.localPosition = new Vector3(0, 0, 0);
        ActiveL.SetAsFirstSibling();
        ActiveR.parent = _destinationR;
        ActiveR.localPosition = new Vector3(0, 0, 0);
        ActiveR.SetAsFirstSibling();
    }

    public void disable() {
        _destinationL.Find("LeftTTController").parent = transform.Find("ActiveControllers");
        _destinationR.Find("RightTTController").parent = transform.Find("ActiveControllers");
        _oldModelL.ForEach((Model) => {
            Model.gameObject.SetActive(true);
        });
        _oldModelR.ForEach((Model) => {
            Model.gameObject.SetActive(true);
        });
    }
    // Update is called once per frame
    void Update()
    {
    }
}
