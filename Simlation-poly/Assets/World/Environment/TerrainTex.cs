using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using World.Environment;


public  class TerrainTex: MonoBehaviour
{

     [SerializeField] private Gradient moistureColorScheme;
     [SerializeField] private ClimateHandler _climateHandler;
    [SerializeField]  Color[] moistureColors;
    [SerializeField] private GroundProperties _groundProperties;
    private float[] fieldCapacaty;
    [SerializeField] private Color currentMoistureColor;
    [SerializeField] private bool showWeatherData = false;
    public Gradient MoistureColorScheme
    {
        get => moistureColorScheme;
    }
    public static TerrainTex Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        TimeHandler.Instance.TimeHourElapsed += HumidityToColor;
    }

    private void HumidityToColor(object sender, HourElapsedEventArgs e)
    {   
        float fieldCapa = _climateHandler.fieldCapacity;
        Color[] moistureColors = {moistureColorScheme.Evaluate(fieldCapa/100)};
        currentMoistureColor = moistureColors[0];
        _groundProperties.SetMoistureColor(moistureColors);
    }
    
    
    [Button]    
    private void AllHumidityToColor()
    {   
       fieldCapacaty = _climateHandler.InitializeWeatherOnTime();
       float fieldCapacatyLength= fieldCapacaty.Length;
       moistureColors = new Color[Mathf.CeilToInt(fieldCapacatyLength )];
       
       if (fieldCapacatyLength == 0)
           return;
       for (int i = 0; i < fieldCapacatyLength; i++)
       {  
         
          moistureColors[i]= moistureColorScheme.Evaluate((float)fieldCapacaty[i]/100);

       }
       _groundProperties.SetMoistureColor(moistureColors);
       
    }

    [Button]
    void ShowWheatherData()
    {
        showWeatherData = !showWeatherData;
    }
    private void OnDrawGizmos()
    {
        if (!showWeatherData)
            return;
        int counter =0;
        for (int i = 0; i < moistureColors.Length; i+=200)
        {   
            float temp = _climateHandler.WeatherDataset[i].temperature;
            Gizmos.color = moistureColors[i];
            Gizmos.DrawSphere(transform.position+new Vector3(counter,temp*3f,0), 0.5f);
            if(fieldCapacaty.Length!=0)
            //Handles.Label(transform.position+new Vector3(counter,temp*3f+3f,0), fieldCapacaty[i].ToString("F2"));
            counter++;
        }
    }
}