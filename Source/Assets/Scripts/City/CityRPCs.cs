using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CityRPCs : MonoBehaviour
{
    [SerializeField]private CitySelectionManager UImgr;
    [SerializeField] private CityManager citymgr;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [PunRPC]
    public void RemoveCityUi(string target)
    {
        UImgr.removecityUI(target);
    }

    [PunRPC]
    public void ChangeCityOwnership(string City, string playerID, string playerName,Vector3 color)
    {
        citymgr.ChangeCityOwner(City, playerID, playerName, color);
    }

    [PunRPC]
    public void EditCityBuildings(string City, string[] Buildings, string[] Buildables)
    {
        citymgr.ReceiveUpdateCityBuildings(City,Buildings,Buildables);
    }
}
