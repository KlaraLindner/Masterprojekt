using System;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using World.Environment;
using World.Player;
using World.Player.Camera;

public class GroundProperties : MonoBehaviour
{   
    public Texture2D tex;
    public FreeLookUserInput _freeLookUserInput;
    private InputProvider input;
    private Bounds bounds;
  
    private Vector3 hitPos;
    private Vector3 startBounds;
    private float terrainSize = float.MinValue;
  
    public static GroundProperties Instance { get; private set; }

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
        
        if (GetComponent<Terrain>() is Terrain terrain)
        {
            terrainSize = terrain.terrainData.size.x;
            return;
        }

        if (GetComponent<Collider>() is Collider colliderBounds)
            startBounds = colliderBounds.bounds.extents;
    }
    
    public Color GetGroundInfo(Vector3 objectPos)
    { 
     
       Vector3 startcorners = Vector3.zero;
       
       // Calculate the corners of the BoxCollider in world space
       Vector3 startPoint = transform.position - startBounds;
       Vector3 startVector =   startPoint-transform.position;

       // Create a rotation quaternion
       Quaternion rotationQuaternion = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
      
       // Rotate the vector using the rotation quaternion
       Vector3 rotatedVector = (rotationQuaternion * startVector);
       Vector3 finalPoint = transform.position + rotatedVector;
       float ratioPixelCoords = tex.width / terrainSize;
       Vector3 startAtZero = objectPos-finalPoint; 
       Vector2Int pixelCoord = new Vector2Int( Mathf.FloorToInt(startAtZero.x*ratioPixelCoords), Mathf.FloorToInt(startAtZero.z*ratioPixelCoords));
       
     return  tex.GetPixel(pixelCoord.x, pixelCoord.y);
       Debug.Log("textcoord "+  pixelCoord.x+"  "+ pixelCoord.y);
    }

    private void OnValidate()
    {
        if (TryGetComponent<Terrain>(out Terrain terrain) && terrain!=null)
        {
            terrainSize = terrain.terrainData.size.x;
            return;
        }

        if (TryGetComponent<Collider>(out Collider colliderBounds) &&colliderBounds!=null)
            startBounds = colliderBounds.bounds.extents;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(hitPos,5f);
     /*   // Calculate the corners of the BoxCollider in world space
        Vector3 startPoint = transform.position - startBounds;
        Vector3 startVector =   startPoint-transform.position;

       // Create a rotation quaternion
       Quaternion rotationQuaternion = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

       // Rotate the vector using the rotation quaternion
       Vector3 rotatedVector = (rotationQuaternion * startVector);
       Vector3 finalPoint = transform.position + rotatedVector;
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position,rotatedVector);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(finalPoint, 2);*/
  
    }
    
    
    
    
    
}