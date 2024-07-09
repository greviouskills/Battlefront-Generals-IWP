using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
public class PlayerData : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private ClickHandler clicker;
    [SerializeField] private FogManager fog;
    public string playerName;
    public string playerID;
    public Color playercolor;

    public List<CityScript> Ownedcities = new List<CityScript>();
    public List<TroopScript> OwnedTroops = new List<TroopScript>();
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
        StartCoroutine(scantroops());
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

    public void RemoveTroopInPlayer(string ID)
    {
        for (int i = 0; i < OwnedTroops.Count; i++)
        {
            if (OwnedTroops[i].owner.ID == ID)
            {
                OwnedTroops.RemoveAt(i);
                return;
            }
        }
    }

    public void AddCity(CityScript city)
    {
        Ownedcities.Add(city);
        Vector2 key = new Vector2(city.gameObject.transform.position.x, city.gameObject.transform.position.z);
        fog.UpdateMap(key, 3);
    }

    public IEnumerator scantroops()
    {
        yield return new WaitForSeconds(0.2f);
        foreach(var troop in OwnedTroops)
        {
            if(troop.movement.Waypoints.Count > 0)
            {
                Vector2 key = new Vector2(troop.transform.position.x, troop.transform.position.z);
                fog.UpdateMap(key, 1);
            }
        }
        StartCoroutine(scantroops());
    }
}
