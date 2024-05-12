using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
public class PlayerData : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private ClickHandler clicker;
    public string playerName;
    public string playerID;
    public Color playercolor;

    public List<CityScript> Ownedcities = new List<CityScript>();
    void Start()
    {
        playerName = Photon.Pun.PhotonNetwork.LocalPlayer.NickName;
        playerID = Photon.Pun.PhotonNetwork.LocalPlayer.UserId;

        foreach (var p in Photon.Pun.PhotonNetwork.PlayerList)
        {
            if (p.UserId == playerID)
            {
                playercolor = MultiplayerGame.GetColor(p.GetPlayerNumber());
                break;
            }
        }

        clicker.playerID = playerID;
        clicker.playercolor = playercolor;
        clicker.playerName = playerName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool FindCityInPlayer(string name)
    {
        foreach(var city in Ownedcities)
        {
            if(city.gameObject.name == name)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveCityInPlayer(string name)
    {
        for (int i = 0; i< Ownedcities.Count; i++)
        {
            if (Ownedcities[i].gameObject.name == name)
            {
                Ownedcities.RemoveAt(i);
                return;
            }
        }
    }
}
