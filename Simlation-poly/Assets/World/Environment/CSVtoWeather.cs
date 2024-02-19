using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CSVtoWeather : MonoBehaviour
{
    //string filename = "";
    public TextAsset textAssetData;

    [System.Serializable]
    public class WeatherData
    {
        public string dateTime;
        public int temperature;
        public int humidity;
        public string weather;
    }

    [System.Serializable]
    public class WeatherList
    {
        public WeatherData[] weatherData;
    }

    public WeatherList currentWeatherList = new WeatherList();


    // Start is called before the first frame update
    void Start()
    {
       //filename = Application.dataPath + "/testWeather.csv";
        ReadCSV();
    }

    void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new String[] { ";", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 4 - 1;
        currentWeatherList.weatherData = new WeatherData[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            currentWeatherList.weatherData[i] = new WeatherData();
            currentWeatherList.weatherData[i].dateTime = data[4 * (i + 1)];
            currentWeatherList.weatherData[i].temperature = int.Parse(data[4 * (i + 1) + 1]);
            currentWeatherList.weatherData[i].humidity = int.Parse(data[4 * (i + 1) + 2]);
            currentWeatherList.weatherData[i].weather = data[4 * (i + 1) + 3];
        }

    }
}
