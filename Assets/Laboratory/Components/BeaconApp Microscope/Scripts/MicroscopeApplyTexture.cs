using BNG;
using UnityEditor;
using UnityEngine;

public class MicroscopeApplyTexture : MonoBehaviour
{
    private SnapZone SnapZone;
    // Start is called before the first frame update
    void Start()
    {
        SnapZone = GetComponent<SnapZone>();
    }


    public void ApplyTexture()
    {
        MicroscopeSlide CurrentMicroscopeSlide = SnapZone.HeldItem.gameObject.GetComponent<MicroscopeSlide>();
        MicroscopeSlideWithGrid CurrentMicroscopeSlideWithGrid = SnapZone.HeldItem.gameObject.GetComponent<MicroscopeSlideWithGrid>();

        if (CurrentMicroscopeSlideWithGrid != null)
            Debug.Log("The slide has a grid");
        if (CurrentMicroscopeSlide != null)
            Debug.Log("The slide does not have a grid");

        if (CurrentMicroscopeSlide)
        {
            CurrentMicroscopeSlide.SetMicroscopeMonitorTexture();
            CurrentMicroscopeSlide.SetMicroscopeSlide();
        }
        if (CurrentMicroscopeSlideWithGrid)
        {
            CurrentMicroscopeSlideWithGrid.SetMicroscopeSlide();
            CurrentMicroscopeSlideWithGrid.SetMicroscopeGrid();
        }
            
    }
}
