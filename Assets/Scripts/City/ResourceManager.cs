using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerdata;
    [SerializeField] private UImanager uimanager;
    // Start is called before the first frame update
    public float money,steel,oil;
    public int manpower;
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
}
