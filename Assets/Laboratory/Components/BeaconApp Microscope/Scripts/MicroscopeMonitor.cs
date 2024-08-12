using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeMonitor : MonoBehaviour
{
    [SerializeField] private float ScrollSpeed = 0.01f;
    [SerializeField] private Texture DefaultTexture;
    private List<int> MagnificationLevels = new List<int> { 2, 4, 8, 16 };
    private RawImage RawImage;
    private Vector2 CurrentXY = new Vector2(0.5f, 0.5f);
    private int CurrentMagnificationStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        RawImage = GetComponentInChildren<RawImage>();
        RawImage.texture = DefaultTexture;

        // set initial magnification level
        SetMagnification(1.0f / MagnificationLevels[CurrentMagnificationStep]);
    }

    private void Update()
    {
        // Zooming
        if (Input.GetKeyDown(KeyCode.Keypad1))
            Magnify();
        if (Input.GetKeyDown(KeyCode.Keypad2))
            Minimize();
    }

    private void FixedUpdate()
    {
        // Scrolling
        if (Input.GetKey(KeyCode.RightArrow))
            ScrollRight();
        if (Input.GetKey(KeyCode.LeftArrow))
            ScrollLeft();
        if (Input.GetKey(KeyCode.UpArrow))
            ScrollUp();
        if (Input.GetKey(KeyCode.DownArrow))
            ScrollDown();
    }

    public void ScrollX(bool right)
    {
        if (right)
            ScrollRight();
        else
            ScrollLeft();
    }

    public void ScrollY(bool up)
    {   
        if (up)
            ScrollUp();
        else
            ScrollDown();
    }

    public void Magnify()
    {
        CurrentMagnificationStep = (CurrentMagnificationStep + 1) % MagnificationLevels.Count;
        SetMagnification(1.0f / MagnificationLevels[CurrentMagnificationStep]);
    }

    public void Minimize()
    {
        CurrentMagnificationStep = (CurrentMagnificationStep - 1) % MagnificationLevels.Count;
        
        if (CurrentMagnificationStep < 0)
            CurrentMagnificationStep = MagnificationLevels.Count - 1;

        // UV x and y must be offset to keep looking at the same point of image when zooming
        SetMagnification(1.0f / MagnificationLevels[CurrentMagnificationStep]);
    }

    private void SetMagnification(float magnification)
    {
        // UV x and y must be offset to keep looking at the same point of image when zooming
        RawImage.uvRect = new Rect(CurrentXY.x - (magnification * 0.5f), CurrentXY.y - (magnification * 0.5f), magnification, magnification);
    }

    private void ScrollRight()
    {
        if (RawImage.uvRect.x < 1 - RawImage.uvRect.width)
        {
            CurrentXY.x += ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x + ScrollSpeed, RawImage.uvRect.y, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    private void ScrollLeft()
    {
        if (RawImage.uvRect.x > 0.01f)
        {
            CurrentXY.x -= ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x - ScrollSpeed, RawImage.uvRect.y, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    private void ScrollUp()
    {
        if (RawImage.uvRect.y < 1 - RawImage.uvRect.height)
        {
            CurrentXY.y += ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x, RawImage.uvRect.y + ScrollSpeed, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    private void ScrollDown()
    {
        if (RawImage.uvRect.y > 0.01f)
        {
            CurrentXY.y -= ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x, RawImage.uvRect.y - ScrollSpeed, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    public void SetTexture(Texture texture)
    {
        RawImage.texture = texture;
    }

    public void SetDefaultTexture()
    {
        RawImage.texture = DefaultTexture;
    }
}
