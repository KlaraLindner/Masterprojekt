using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BarkBeetle", menuName = "Addressables/Disease/BarkBeetle", order = 1)]
public class BarkBeetleSO : ScriptableObject
{
  [MinMaxSlider(-20,50)]
  public Vector2 humidityRage ;
  [MinMaxSlider(-20,50)]
  public Vector2 temperatureRange;
  [MinMaxSlider(1,12)]
  public Vector2 seasonRange;

 
}
