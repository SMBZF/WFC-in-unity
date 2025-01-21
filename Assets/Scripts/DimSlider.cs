using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DimSlider : MonoBehaviour
{    
    [SerializeField]
    private Text text;
    private void Start()
    {
        GetComponent<Slider>().onValueChanged.AddListener(UpdateDim);
        GetComponent<Slider>().value = Data.dim;
        text.text = "DIM = " + Data.dim.ToString();
    }

    private void UpdateDim(float value)
    {
        text.text = "DIM = " + value.ToString();
        Data.dim = (int) value;
    }


}
