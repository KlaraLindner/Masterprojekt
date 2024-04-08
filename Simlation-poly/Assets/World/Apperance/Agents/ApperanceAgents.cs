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
   /// <summary>
   /// initializes current vairables
   /// the tree is always healthy at the beginning
   /// assign the renderer for deatial changes
   /// propertyblock change the matieral individualiy not for all objects having the same material,see:
   /// https://www.ronja-tutorials.com/post/048-material-property-blocks/
   /// </summary>
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
   /// <summary>
   /// Method subscribed on the change of the disease's progress from the tree 
   /// </summary>
   /// <returns></returns>
    
   void ChangeApperance(object s, GenEventArgs<Disease>d)
   {    //Set the state of the tree based on the progress of the disease
       SetState(d.Value.progress);
       //change the appearence of the object
       ChangeObject();
       //Optional detailed change of the tree's twigs and leaves regression when sick
       
      // ChangeMaterial(d.Value.progress);
   }
    //Compare the progress from the disease with the state of the tree
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
       if (state != State.dead&&progress>0.99)
       {
           state = State.dead;
           agent.onDiseaseStatusChanged -= ChangeApperance;
       }

   }
/// <summary>
/// Changes the object's apperance based on the minimum requirement for the state
/// </summary>
/// <param name="d"></param>
   void ChangeObject()
   {   //Set the object based on the state to check if the obejct needs to be swaped
       //E.g If the tree has the same state sick already, it does not need to be changed
       GameObject compareStateObject = (state) switch
       {
           State.healthy => healthy,
           State.sick => sick,
           State.dead => dead
       };
       // so if the object from the state is the sick tree and the current object assigned is the same sick tree, 
       //return because nothing changes
       if (compareStateObject == currentGameObject)
           return;
            //If there is a change in the gamobjects required however, 
            //deactivate the object, befor enale the next one
           currentGameObject.SetActive(false);
           compareStateObject.SetActive(true);
           //Assign the new object to the current object
           currentGameObject = compareStateObject;
   //required for the change material method
   //renderer is per object, so it needs to be re-assigned
           currentRenderer = currentGameObject.GetComponent<Renderer>();
   }
    /// <summary>
    /// Optional,Additional changes on the Treeleaves
    /// </summary>
    /// <param name="progress"></param>
   void ChangeMaterial(float progress)
   {
       currentRenderer.GetPropertyBlock(propertyBlock);
      float currentAlphaClip= propertyBlock.GetFloat("_AlphaClipThreshold");
      propertyBlock.SetFloat("_AlphaClipThreshold", Mathf.Lerp(0.1f, 0.95f, progress));
     
       currentRenderer.SetPropertyBlock(propertyBlock);
   }
}
