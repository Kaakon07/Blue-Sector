using System;
using UnityEngine;
using UnityEngine.Events;

public class FloatingNumPad : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool isScaling, scalingUp = false;
    private Canvas canvas;
    private Vector3 openPosition, openScale;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        openPosition = rectTransform.localPosition;
        openScale = rectTransform.localScale;
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        // scale down and hide behind note sheet
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // keyboard input for debugging and testing purposes
        if (Input.GetKeyDown(KeyCode.O))
            EnableAndScaleUp();
        if (Input.GetKeyDown(KeyCode.P))
            DisableAndScaleDown();
        if (Input.GetKeyDown(KeyCode.U))
            ToggleOnOff();

        // perform the opening/closing of canvas
        if (isScaling)
        {
            if (scalingUp)
            {
                if ((float)Math.Truncate(rectTransform.localScale.x * 1000) / 1000 == openScale.x - 0.001) // check first 3 decimals to prevent excessive precision/time spent scaling. subtraction prevents infinite interpolation
                    isScaling = false;

                float newScale = Mathf.Lerp(rectTransform.localScale.x, openScale.x, 6f * Time.deltaTime);
                float newPositionX = Mathf.Lerp(rectTransform.localPosition.x, openPosition.x, 6f * Time.deltaTime);
                float newPositionZ = Mathf.Lerp(rectTransform.localPosition.z, openPosition.z, 6f * Time.deltaTime);
                rectTransform.localScale = Vector3.one * newScale;
                rectTransform.localPosition = new Vector3(newPositionX, openPosition.y, newPositionZ);
            }
            if (!scalingUp)
            {
                if (rectTransform.localScale.x <= openScale.x / 6 /*(float)Math.Truncate(rectTransform.localScale.x * 100) / 100 == 0*/)
                {
                    isScaling = false;
                    canvas.enabled = false;
                }

                float newScale = Mathf.Lerp(rectTransform.localScale.x, 0, 6f * Time.deltaTime);
                float newPositionX = Mathf.Lerp(rectTransform.localPosition.x, 0, 6f * Time.deltaTime);
                float newPositionZ = Mathf.Lerp(rectTransform.localPosition.z, 0, 6f * Time.deltaTime);
                rectTransform.localScale = Vector3.one * newScale;
                rectTransform.localPosition = new Vector3(newPositionX, openPosition.y, newPositionZ);
            }
        }
    }

    public void ToggleOnOff()
    {
        if (scalingUp)
            DisableAndScaleDown();
        else
            EnableAndScaleUp();
    }

    public void EnableAndScaleUp()
    {
        if (canvas.enabled)
            return;

        canvas.enabled = true;
        scalingUp = true;
        isScaling = true;
    }

    public void DisableAndScaleDown()
    {
        if (!canvas.enabled)
            return;

        scalingUp = false;
        isScaling = true;
    }

    public bool IsEnabled()
    {
        return canvas.enabled;
    }
}
