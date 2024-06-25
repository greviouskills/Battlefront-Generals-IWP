using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    [SerializeField] private Renderer model;
    [SerializeField] private TroopMovementScript troopmover;
    [SerializeField] private float WaterSpeed;
    public TerrainRecorder terrain;
    private float defaultspeed;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(map.width + ", " + map.height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }       

    public void CheckWater()
    {
        model.material.color = terrain.GetColor(new Vector2(transform.position.x, transform.position.z));
        //if(terrain.CheckForWater(new Vector2(transform.position.x, transform.position.z)))
        //{

        //}
        
    }
}
