using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Serialization;
using Utility;
using World.Agents;
using World.Agents.Modifier;

public class ApperanceAgents : MonoBehaviour
{
    [Range(0.1f,1)]
    public float threshHealthy;

    public GameObject currentGameObject;
    public GameObject healthy;
    public GameObject sick;
    public GameObject dead;

    public enum State{healthy, sick , dead}

    public State state = State.healthy;
   public Agent agent;
   //Private Variables
   private Renderer currentRenderer;
   private MaterialPropertyBlock propertyBlock;


   public void OnEnable()
   {
       if (!agent)
       {
           if (!TryGetComponent(out agent))
               return;
       }

       agent.onDiseaseStatusChanged += ChangeApperance;
   }
   public void Start()
   {
       currentGameObject = healthy;
       currentRenderer = currentGameObject.GetComponent<Renderer>();
       propertyBlock = new MaterialPropertyBlock();
   }
   public void OnDisable()
   {    if(state!=State.dead)
       agent.onDiseaseStatusChanged -= ChangeApperance;
   }

   void ChangeApperance(object s, GenEventArgs<Disease>d)
   {
       SetState(d.Value.progress);
       ChangeObject();
      // ChangeMaterial(d.Value.progress);
   }

   void SetState(float progress)
   {
       
       if (state == State.dead)
           return;
       
       if (state != State.healthy && progress< threshHealthy)
       {
           state = State.healthy;
           return;
       }

       if (state != State.sick && progress >= threshHealthy)
       {
           state = State.sick;
           return;
       }
       if (state != State.dead&&progress>0.9)
       {
           state = State.dead;
           agent.onDiseaseStatusChanged -= ChangeApperance;
       }

   }
/// <summary>
/// Changes the object's apperance based on the minimum requeirement for the state
/// </summary>
/// <param name="d"></param>
   void ChangeObject()
   {   
       GameObject newStateObject = (state) switch
       {
           State.healthy => healthy,
           State.sick => sick,
           State.dead => dead
       };
       
       if (newStateObject == currentGameObject)
           return;
           currentGameObject.SetActive(false);
           newStateObject.SetActive(true);
           currentGameObject = newStateObject;
           currentRenderer = currentGameObject.GetComponent<Renderer>();
   }

   void ChangeMaterial(float progress)
   {
       currentRenderer.GetPropertyBlock(propertyBlock);
      float currentAlphaClip= propertyBlock.GetFloat("_AlphaClipThreshold");
      propertyBlock.SetFloat("_AlphaClipThreshold", Mathf.Lerp(0.1f, 0.95f, progress));
     
       currentRenderer.SetPropertyBlock(propertyBlock);
     
      
   }
}
