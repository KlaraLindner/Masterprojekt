using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace World.Agents.Animals
{
    public class SeeBehaviour : MonoBehaviour, IColliderBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            OnSense?.Invoke(other.gameObject);
        }

        public void OnTriggerStay(Collider other)
        {
            OnRadiusSense?.Invoke(other.gameObject);
        }
        
        public void OnTriggerExit(Collider other)
        {
            OnRadiusExitSense?.Invoke(other.gameObject);
        }

        public event IColliderBehaviour.TriggerHandler OnSense;
        public event IColliderBehaviour.TriggerHandler OnRadiusSense;
        public event IColliderBehaviour.TriggerHandler OnRadiusExitSense;
    }
}