using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerdata;
    [SerializeField] private UImanager uimanager;
    // Start is called before the first frame update
    public float money = 100000000;
    public float steel,oil;
    public int manpower = 100000;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetResources()
    {
        foreach(var city in playerdata.Ownedcities)
        {
            money += city.population * city.money;
            manpower +=(int)(city.population * city.manpower);
            foreach(var resource in city.Resources)
            {
                if(resource.Resource == "Steel")
                {
                    steel += resource.Production;
                }
                else if (resource.Resource == "Oil")
                {
                    oil += resource.Production;
                }
            }
        }

        uimanager.UpdateResourceUi();
    }

    public bool attemptconsume(float dmoney, int dMP, float dsteel, float doil)
    {
        if(money>= dmoney&& manpower>=dMP && steel>=dsteel && oil>=doil ){
            money -= dmoney;
            steel -= dsteel;
            oil -= doil;
            manpower -= dMP;
            return true;
        }
        return false;
    }
}
