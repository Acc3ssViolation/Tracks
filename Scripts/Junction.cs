using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Junction : MonoBehaviour
{
    public TrackJunction junction;
    Slider slider;
    float sliderValue = 0.0f;

    public void Init(TrackJunction junction)
    {
        this.junction = junction;
        slider = GetComponentInChildren<Slider>();
        slider.wholeNumbers = true;
        slider.minValue = 0;
        slider.maxValue = junction.Sections.Count - 1;
        slider.onValueChanged.AddListener(OnUIChanged);

        GetComponent<Canvas>().worldCamera = Camera.main;

        transform.position = junction.position.Vector3() + Vector3.up * 5f;
    }

    public void OnUIChanged(float value)
    {
        if(sliderValue != value)
        {
            sliderValue = value;
            junction.Switch((int)sliderValue);
        }
    }

    public void Update()
    {
        Vector3 v = Camera.main.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(Camera.main.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}
