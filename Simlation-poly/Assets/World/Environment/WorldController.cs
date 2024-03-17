using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NaughtyAttributes;
using Player;
using UnityEditor;
using UnityEngine;
using Utility;
using World.Agents;
using World.Player;
using World.Structure;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace World.Environment
{
    public class WorldController : MonoBehaviour, ILog
    {
        public PlayerHandler player;
        
        [Header("Simulation Settings")]
        public Vector2Int size;

        public AnimationCurve highMultiplier;
        
        public static WorldController Instance;
        
        public Dictionary<Vector2, Ground> Grounds;

        public List<Agent> removeList;
        
        public GameObject water;
        public GameObject plants;
        public GameObject border;
        public GameObject rubbish;
        public GameObject[] plantSpawner;
        public GameObject[] otherSpawner;
        
        [Header("Utility")]
        public bool showMesh = false;
        public bool showGraph = false;
        public bool spawnPlants = false;
        public string LN() => "Time handler";
        
        public TimeHandler timeHandler;
        public ClimateHandler climateHandler;
        private readonly Stopwatch timer = new();
        

        [Button("Generate")]
        private void Generate()
        {
            //#if UNITY_EDITOR
            timer.Start();
            //#endif
            //spawn plants
            if (spawnPlants)
            {
                //#if UNITY_EDITOR
                timer.Start();
                //#endif
                foreach (var obj in plantSpawner)
                {
                    var spawn = obj.GetComponent<Spawner>();
                    if (spawn is null)
                    {
                        throw new Spawner.NoSpawnerException();
                    }
                    spawn.Spawn(this);
                }
                //#if UNITY_EDITOR
                timer.Stop();
                Debug.Log("SpawnPlants: " + timer.ElapsedTicks);
                timer.Reset();
                //#endif
            }
            
            //spawn others
            foreach (var obj in otherSpawner)
            {
                //#if UNITY_EDITOR
                timer.Start();
                //#endif
                var spawn = obj.GetComponent<Spawner>();
                if (spawn is null)
                {
                    throw new Spawner.NoSpawnerException();
                }
                spawn.Spawn(this);
                //#if UNITY_EDITOR
                timer.Stop();
                Debug.Log("SpawnObjects: " + timer.ElapsedTicks);
                timer.Reset();
                //#endif
            }
            //init UI 
            player.UpdateStatisticsValue();
            
//#if UNITY_EDITOR
            UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
//#endif
//            GetComponent<NavMeshSurface>().BuildNavMesh();
        }
        
        private void Start()
        {
            //loads the time handler
            timeHandler = GetComponent<TimeHandler>();
            timeHandler.TimeHourElapsed += HandleAgents;
            
            //TODO do this with coroutine!
            /*
            timeHandler.TimeChangedToMidnight += delegate(object sender, EventArgs args)
            {
                //#if UNITY_EDITOR
                timer.Start();
                //#endif
                for (var i = 0; i < size.x; i++)
                {
                    for (var j = 0; j < size.y; j++)
                    {
                        var vec = new Vector2(i * pointScale, j * pointScale);
                        var selGround = Grounds[vec];
                        CalcWaterArea(selGround ,0);
                    }
                }
                //#if UNITY_EDITOR
                timer.Stop();
                comp.WriteData("CalcWater", timer.ElapsedTicks.ToString());
                timer.Reset();
                //#endif
            };*/
            
            //loads the climate handler
            climateHandler = GetComponentInChildren<ClimateHandler>();
            
            Generate();
        }
        
        private void Awake()
        {
            if(!Instance)
            {
                Instance = this;
            }
            else
            {
                ILog.LE(LN, "World instance already set!");
            }
        }

        /// <summary>
        /// Handle all agents who are active per elapsed hour.
        /// </summary>
        /// <param name="sender">TimeHandler caller</param>
        /// <param name="e">Event data</param>
        /// <complexity>O(3p+1) => O(3p), Θ(p+1) => Θ(p), Ω(p)</complexity>
        public void HandleAgents(object sender, EventArgs e)
        {
            //#if UNITY_EDITOR
            timer.Start();
            //#endif
            StartCoroutine(IteratePlants()); //O(p) (all plants)
            foreach (var rAgent in removeList) //O(p), Θ(p/2), Ω(1)
            {
                rAgent.OnAfterDeath(this, EventArgs.Empty); //O(1)
            }
            removeList.Clear();//O(p), Θ(p/2), Ω(1)
            //#if UNITY_EDITOR
            timer.Stop();
            Debug.Log("HandleAgents: " + timer.ElapsedTicks);
            timer.Reset();
            //#endif
        }

        /// <summary>
        /// Iterate over all plants and do one plant per update.
        /// </summary>
        /// <complexity>O((n+7)*(k+7)*(p+7)), Θ((n+6)*(k+6)*(p+6)), Ω(n)</complexity>
        /// <returns>Null yield because there is no waiting time.</returns>
        public IEnumerator IteratePlants()
        {
            foreach (Transform plantTypes in plants.transform)
            {
                //trees, bushes, grass
                foreach (Transform plant in plantTypes.transform) //O(n+7), Θ(n+6), Ω(n)
                {
                    if (plant.gameObject.activeSelf) //O(7), Θ(6), Ω(6)
                    {
                        var agent = plant.GetComponent<FloraAgent>();  //O(3) because 3 components
                        if(agent)
                        {
                            plant.GetComponent<FloraAgent>().OnHandle(this); //O(3) because 3 components
                        }
                        else
                        {
                            plant.GetComponentInParent<FloraAgent>()?.OnHandle(this); //O(4) because 3 components in parent
                        }
                        yield return null;
                    }
                }
            }
        }

        public void Spawn(GameObject obj)
        {
            var center = obj.transform.position;
            var x = Random.Range(center.x - 3, center.x + 3);
            var z = Random.Range(center.z - 3, center.z + 3);
            
            Physics.Raycast(new Ray(new Vector3(x, 50, z), new Vector3(x, -50, z)), 
                out var hit, 200, LayerMask.GetMask("World", "Water"));
            
            if (hit.point == Vector3.zero || hit.transform.gameObject.layer == 4 || hit.point.y > 10)
            {
                return;
            }
            Instantiate(obj, hit.point, obj.transform.rotation, obj.transform.parent);
        }
        
        public void Spawn(GameObject obj, Vector3 pos)
        {
            Instantiate(obj, pos, obj.transform.rotation, transform);
        }
        
        public void SpawnPlant(GameObject obj, Vector3 pos)
        {
            RegisterFloraAgent(Instantiate(obj, pos, new Quaternion(0f, Random.Range(0f, 360f), 0f, 0f), plants.transform).GetComponent<FloraAgent>());
        }

        /// <summary>
        /// Register new plant agent to the handler system
        /// </summary>
        /// <param name="agent">agent to add</param>
        public void RegisterFloraAgent(FloraAgent agent)
        {
            var pos = agent.transform.position;
            //round variables to full numbers
            //TODO works only with a pointScale of 1!
            var vec = new Vector2(
                MathF.Round(pos.x, MidpointRounding.AwayFromZero),
                MathF.Round(pos.z, MidpointRounding.AwayFromZero));
           
            //agent.ground = Grounds[vec]; //connects the agent with the ground value
            
            //add modifier to world numbers
            player.o2Production += agent.o2Modifier;
            player.co2Consumption += agent.co2Modifier;
            player.waterConsumption += agent.waterConsumption;
            player.AddTree();
        }

        /// <summary>
        /// Removes an agent from the handler system
        /// </summary>
        public void RemoveAgent(Agent agent)
        {
            //TODO find a better way to deregister tree for the player statistics
            if (agent.GetType() == typeof(TreeAgent))
            {
                var tree = (TreeAgent)agent;
                player.RemoveTree(tree);
                player.co2Consumption -= tree.co2Modifier;
                player.o2Production -= tree.o2Modifier;
                player.waterConsumption -= tree.waterConsumption;
                player.UpdateStatisticsValue();
            }
            removeList.Add(agent);
        }

        public enum ActiveColor
        {
            Texture,
            Arid,
            Type,
            Height
        }
    }
}