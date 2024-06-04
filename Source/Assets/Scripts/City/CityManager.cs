using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CityManager : MonoBehaviour
{

    [SerializeField]private PlayerData playerdata;
    // Start is called before the first frame update
     public List<CityScript> Capitals = new List<CityScript>();
     public List<CityScript> Cities = new List<CityScript>();
    [SerializeField] private TroopSync syncer;
    [SerializeField]private Transform CityParent;
    [SerializeField] private CitySelectionManager Uimgr;
    [SerializeField] private GameObject cam;

    private PhotonView photonView;
    void Start()
    {
        photonView = this.GetComponentInParent<PhotonView>();
        for (int i = 0; i < CityParent.childCount; i++)
        {
            Transform city = CityParent.GetChild(i);
            Cities.Add(city.gameObject.GetComponent<CityScript>());
        }

        List<string> names = new List<string>();
        foreach(var city in Capitals)
        {
            names.Add(city.gameObject.name);
        }
        Uimgr.InstantiateUis(names);

        foreach(var city in Cities)
        {
            city.manager = this;
            city.troopsync = syncer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SendCityOwnerChange(string cityname)
    {
        Vector3 color = new Vector3(playerdata.playercolor.r, playerdata.playercolor.g, playerdata.playercolor.b);
        photonView.RPC("ChangeCityOwnership", RpcTarget.AllViaServer, cityname,playerdata.playerID,playerdata.playerName,color);

    }
    public void ChangeCityOwner(string Cityname, string playerID, string playerName, Vector3 color)
    {
        Color temp = new Color(color.x, color.y, color.z);
        foreach(var city in Cities)
        {
            if(city.gameObject.name == Cityname)
            {
                city.ChangeCityOwnership(playerID, playerName, temp);
                if(playerID == playerdata.playerID)
                {
                    playerdata.Ownedcities.Add(city);
                }
                else if (playerdata.FindCityInPlayer(Cityname))
                {
                    playerdata.RemoveCityInPlayer(Cityname);
                }
                break;
            }
        }
    }

    public void SendUpdateCityBuildings(string City, List<string> Buildings, List<string> Buildables)
    {
        string[] temp1 = Buildings.ToArray();
        string[] temp2 = Buildables.ToArray();
        photonView.RPC("EditCityBuildings", RpcTarget.AllViaServer, City, temp1, temp2);
    }
    public void ReceiveUpdateCityBuildings(string City, string[] Buildings, string[] Buildables)
    {
        foreach(var city in Cities)
        {
            if(city.gameObject.name == City)
            {
                List<string> temp1 = new List<string>(Buildings);
                List<string> temp2 = new List<string>(Buildables);
                city.UpdateBuildings(temp1, temp2);
                break;
            }
        }
    }
    public void CityTick()
    {
        foreach(var city in Cities)
        {
            city.CityIncrement();
        }
    }
}
