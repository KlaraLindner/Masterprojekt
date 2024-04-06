using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using World.Environment;


public class TerrainTex: MonoBehaviour
{

     [SerializeField] private Gradient moistureColorScheme;
     [SerializeField] private ClimateHandler _climateHandler;
    [SerializeField] private Terrain terrain;
    [SerializeField]  Color[] moistureColors;
    [SerializeField] private GroundProperties _groundProperties;
    private float[] fieldCapacaty;
   
   /// <summary>
   /// 
   /// </summary>
   /// <returns></returns>
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        TimeHandler.Instance.TimeHourElapsed += HumidityToColor;
    }

    private void HumidityToColor(object sender, HourElapsedEventArgs e)
    {   
        float fieldCapa = _climateHandler.fieldCapacity;
        Color[] moistureColors = {moistureColorScheme.Evaluate(fieldCapa/100)};
        _groundProperties.SetMoistureColor(moistureColors);
    }
    
    
    [Button]    
    private void AllHumidityToColor()
    {   
       float sizeInX =terrain.terrainData.size.x;
       fieldCapacaty = _climateHandler.InitializeWeatherOnTime();
       float fieldCapacatyLength= fieldCapacaty.Length;
       moistureColors = new Color[Mathf.CeilToInt(fieldCapacatyLength )];
       float cellSizeRatio = sizeInX / fieldCapacatyLength;
       if (fieldCapacatyLength == 0)
           return;
       for (int i = 0; i < fieldCapacatyLength; i++)
       {  
         
          moistureColors[i]= moistureColorScheme.Evaluate((float)fieldCapacaty[i]/100);

       }
       _groundProperties.SetMoistureColor(moistureColors);
       
    }
    
    /*private void OnDrawGizmos()
    {
        int counter =0;
        for (int i = 0; i < moistureColors.Length; i+=200)
        {   
            float temp = _climateHandler.WeatherDataset[i].temperature;
            Gizmos.color = moistureColors[i];
            Gizmos.DrawSphere(transform.position+new Vector3(counter*1f,temp*3f,0), 0.5f);
            if(fieldCapacaty.Length!=0)
            Handles.Label(transform.position+new Vector3(counter*1f,temp*3f+3f,0), fieldCapacaty[i].ToString("F2"));
            counter++;
        }
    }*/
}