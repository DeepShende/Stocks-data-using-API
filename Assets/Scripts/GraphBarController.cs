using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraphBarController : MonoBehaviour
{
    //public static GraphBarController graphBarController { get; set; }
    public float BarValue;
    public string date;
    public TextMeshPro ValueText, DateText;
    public int VolumeValue;
    GraphManager _graphManager = GraphManager.graphManager;
    private void Awake()
    {

    }
    void Update()
    {
        //Do calculations according to the scale factor and scale the bar here using flag. 
    }
    public void StartScaling()
    {
        float ScalingFactorY = _graphManager.factorY;
        float ScaleValueY = BarValue / ScalingFactorY;
        float ScalingFactorZ = _graphManager.factorZ;
        float ScaleValueZ = BarValue / ScalingFactorZ;
        ValueText.text = BarValue.ToString();
        DateText.text = date;
        transform.localScale += new Vector3(0, ScaleValueY*2, ScaleValueZ*5);
    }
}
