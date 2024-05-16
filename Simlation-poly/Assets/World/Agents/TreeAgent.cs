using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using World.Agents.Animals;
using World.Agents.Modifier;
using World.Agents.Modifier.Diseases;
using World.Environment;
using World.Structure;

namespace World.Agents
{
    public abstract class TreeAgent: FloraAgent
    {
        public SphereCollider diseaseCollider = null;
        public float diseaseColliderRangeMin = 1;
        public float diseaseColliderRangeMax = 25;
        private float tickCounter = 0;
        private bool checkCollider = false;
        private float diseaseProgress =0;
        /// <summary>
        /// Handling of the disease progression
        /// </summary>
        private void Update()
        {   //Integrity check
            //If the collider should not be checked, because there is no disease attached,
            //the disease's list is for some reason null or has no entries, return
            if (!checkCollider|| diseases==null||diseases.Count==0)
                return;
             //If the diseas reached it maximum spread range, stop check of the disease
             //Needs to be adjusted, if the disease can be cured 
            if (tickCounter >= diseaseColliderRangeMax)
            {
                checkCollider = !checkCollider;
                return;
            }
            //If the conditions for speading are not met, return
            if (!CheckDiesaesSpreadCondition())
                return;
            //Set the new size of the spread collider based on the disease's progress
            diseaseCollider.radius = Mathf.Lerp(diseaseColliderRangeMin, diseaseColliderRangeMax, diseaseProgress);
            //Tickcounter adds up by the average time
            tickCounter += Time.deltaTime/2;
            //the progress is determined by the size of the radius, the larger the area can be, the longer it takes to fully spread
            diseaseProgress = tickCounter / (diseaseColliderRangeMax);
            //Inform other methods subscribed to the disease event of it's progress
            OnDisease(this, diseaseProgress, new GenEventArgs<Disease>(diseases[0]));
            }


        protected TreeAgent(Ground ground) : base(ground) { }
        /// <summary>
        /// Checks if the disease can spread
        /// </summary>
        /// <returns>Can the disease spread?</returns>
        public bool CheckDiesaesSpreadCondition()
        {   //If there is a resistance from drought set currently the ground's moisture
            //It is less then the required moisture for the disease
            //eg. the bark beetle can infect trees during heat stress, because the tree has not enough water to liquefy the resin to keep it off
            //Also it needs to be warm enough for the beetle to spread
            return dryResistance != 0 && (dryResistance < diseases[0].HumidityRange.y) && (ClimateHandler.Instance.temperature >= diseases[0].TemperatureRange.x);
        }
        public void CutTree()
        {
            OnDamage(this, new GenEventArgs<int>(-health*10));
        }
        public void OnTriggerEnter(Collider other)
        {   //If this tree has no diseases, return
            if (diseases == null)
                return;
            if (other.TryGetComponent(out Rigidbody rigidbody)&&rigidbody.TryGetComponent(out TreeAgent otherTree))
            {   
                otherTree.TryAddDisease(diseases[0]);
            }
        }

        public override void TryAddDisease(Disease d)
        { 
            base.TryAddDisease(d);
            CheckColliderSize();
        }
        public void CheckColliderSize()
        {  
            checkCollider = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f,0.5f,0f,0.1f);
            if (diseases != null && diseases.Any(x=>x.GetType() == typeof(BarkBeetle)))
            {
                Gizmos.DrawSphere(transform.position, diseaseCollider.radius);
            }
        }
    }
    
 
}