using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    [SerializeField] private GameObject troop,boat;
    //[SerializeField] private Renderer model;
    [SerializeField] private TroopMovementScript troopmover;
    [SerializeField] private float WaterSpeed = 1.5f;
    public TerrainRecorder terrain;
    private float defaultspeed;
    public bool changecolor;
    [SerializeField] private Renderer model;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(map.width + ", " + map.height);
        if(troopmover != null)
        {
            defaultspeed = troopmover.movespeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }       

    public void CheckWater()
    {
        if (changecolor)
        {
            model.material.color = terrain.GetColor(new Vector2(transform.position.x, transform.position.z));
        }
        else
        {
            if (terrain.CheckForWater(new Vector2(transform.position.x, transform.position.z)))
            {
                boat.SetActive(true);
                troop.SetActive(false);
                troopmover.movespeed = WaterSpeed;
            }
            else
            {
                troop.SetActive(true);
                boat.SetActive(false);
                troopmover.movespeed = defaultspeed;
            }
        }

    }
}
