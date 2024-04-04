using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using static WeatherCSVReader;

public class WeatherCSVReader : MonoBehaviour
{

    [System.Serializable]
    public class WeatherData
    {
        public string dateTime;
        public float temperature;
        public float precipitation;
        public float soilMoisture;
        public float fieldCapacity;
    }
    public List<WeatherData> dataSet = new List<WeatherData>();
    
        // Start is called before the first frame update

        void Start()
        {

            //filename = Application.dataPath + "/weather.csv";
           
        }

        public List<WeatherData> WeatherDatas
        {
            get { return dataSet; }
        }
        [Button]
        public List<WeatherData>ReadCSV()
        {   
            // LOAD CSV Data from Assets/Resources with name below v_v_v
            TextAsset csvData = Resources.Load<TextAsset>("temperatur_bodenfeuchte_niederschlag");
            string[] data = csvData.text.Split(new String[] { ";", "\n" }, StringSplitOptions.None);
          
            int tableSize = data.Length / 4 - 10;
    
            dataSet = new List<WeatherData>();

            for (int i = 0; i < tableSize; i++)
            {
                /*
                WeatherData weatherData = new WeatherData();
                weatherData.dateTime = DateTime.Parse(data[4 * (i + 10)]);
                weatherData.temperature = float.Parse(data[4 * (i + 10) + 1]);
                weatherData.precipitation = float.Parse(data[4 * (i + 10) + 2]);
                weatherData.soilMoisture = float.Parse(data[4 * (i + 10) + 3]);
                */
                dataSet.Add(new WeatherData 
                    {
                    dateTime = (data[4 * (i + 10)]),
                    temperature = float.Parse(data[4 * (i + 10) + 1]),
                    precipitation = float.Parse(data[4 * (i + 10) + 2]),
                    soilMoisture = float.Parse(data[4 * (i + 10) + 3])
                    }
                );
            }
            return dataSet;
        }
}
/*

{
    //string filename = "";
    public TextAsset textAssetData;

    [System.Serializable]
    public class WeatherDataset
    {
        public DateTime dateTime;
        public float temperature;
        public float precipitation;
        public float soilMoisture;
    }
    
    [System.Serializable]
    public class WeatherList
    {
        public WeatherDataset[] weatherData;
    }

    public WeatherList currentWeatherList = new WeatherList();

    public WeatherList getList()
    {
        return currentWeatherList;
    }


    // Start is called before the first frame update
    void Start()
    {
       //filename = Application.dataPath + "/testWeather.csv";
        ReadCSV();
    }

    void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new String[] { ";", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 4 - 10;
        currentWeatherList.weatherData = new WeatherDataset[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            currentWeatherList.weatherData[i] = new WeatherDataset();
            currentWeatherList.weatherData[i].dateTime = DateTime.Parse(data[4 * (i + 10)]);
            currentWeatherList.weatherData[i].temperature = float.Parse(data[4 * (i + 10) + 1]);
            currentWeatherList.weatherData[i].precipitation = float.Parse(data[4 * (i + 10) + 2]);
            currentWeatherList.weatherData[i].soilMoisture = float.Parse(data[4 * (i + 10) + 3]);
        }

    }
}

*/