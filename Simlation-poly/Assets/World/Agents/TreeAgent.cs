using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using World.Agents.Animals;
using World.Agents.Modifier;
using World.Environment;
using World.Structure;

namespace World.Agents
{
    public abstract class TreeAgent: FloraAgent
    {
        public SphereCollider diseaseCollider = null;
         public float diseaseColliderRangeMin = 5;
         public float diseaseColliderRangeMax = 25;
        private float tickCounter = 0;
        private bool checkCollider = false;
        private float diseaseProgress =0;
        private void Update()
        {
            if (!checkCollider)
                return;
            
            if (tickCounter >= diseaseColliderRangeMax)
            {
                checkCollider = !checkCollider;
                return;
            }

            diseaseCollider.radius = Mathf.Lerp(diseaseColliderRangeMin, diseaseColliderRangeMax, diseaseProgress);

            tickCounter += Time.deltaTime;
            diseaseProgress=tickCounter / diseaseColliderRangeMax;
            if(diseases!=null)
                OnDisease(this, diseaseProgress, new GenEventArgs<Disease>(diseases[0]));
        }

    
        protected TreeAgent(Ground ground) : base(ground) { }

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
                otherTree.AddDisease(diseases[0]);
            }
        }

        public override void AddDisease(Disease d)
        { 
            base.AddDisease(d);
            CheckColliderSize();
        }
        public void CheckColliderSize()
        {  
            checkCollider = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (diseases != null && diseases.Count > 0)
            {
                Gizmos.DrawSphere(transform.position, diseaseCollider.radius);
            }
        }
    }
    
 
}