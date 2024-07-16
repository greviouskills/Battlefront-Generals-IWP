using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerdata;
    [SerializeField] private UImanager uimanager;
    // Start is called before the first frame update
    public float money;
    public float steel,oil;
    public int manpower, MPrate;

    public float Mrate, Orate, Srate;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetResources()
    {
        Mrate = 0;
        MPrate = 0;
        Srate = 0;
        Orate = 0;

        foreach(var city in playerdata.Ownedcities)
        {
            Mrate += city.population * city.money;
            MPrate += (int)(city.population * city.manpower);
            Orate += city.OilProd;
            Srate += city.SteelProd;
        }
        money += Mrate;
        manpower += MPrate;
        oil += Orate;
        steel += Srate;

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
