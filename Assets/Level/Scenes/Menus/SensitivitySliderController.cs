using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySliderController : MonoBehaviour
{
    public Slider mySlider;

    void Start()
    {
        // Set initial slider value
        mySlider.value = 0;
    }

    public void OnSliderValueChanged()
    {
        // Log the slider's current value
        Debug.Log("Mouse Sensitivity Slider Value: " + mySlider.value);
    }
}
