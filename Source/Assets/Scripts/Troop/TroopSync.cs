using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TroopSync : MonoBehaviour
{
    [SerializeField] private PlayerData playerdata;
    private PhotonView photonView;
    public List<TroopScript> troops = new List<TroopScript>();
    public List<TroopScript> prefabs = new List<TroopScript>();
    private List<TroopScript> toremove = new List<TroopScript>();
    [SerializeField] private Transform troopparent;
    //[SerializeField] private Renderer mat;
    [SerializeField] private TerrainRecorder terrain;
    [SerializeField] private StatusSystem status;

    private int IDcounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        //map = mat.material.GetTexture("_MainTex") as Texture2D;
        photonView = this.GetComponentInParent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateTroopWaypoints(string[] troopIDs, Vector3[] waypoints, string cityname)
    {
        foreach(var troop in troops)
        {
            foreach(var ID in troopIDs)
            {
                if(troop.owner.ID == ID)
                {
                    troop.movement.Waypoints.Clear();
                    List<Vector3> temp = new List<Vector3>(waypoints);
                    troop.movement.Waypoints = temp;
                    troop.movement.EndTarget = cityname; 
                    break;
                }
            }
        }
    }
    public void SendTroopWaypoints(string[] troopIDs, Vector3[] waypoints, string cityname)
    {
        photonView.RPC("TroopMove", RpcTarget.AllViaServer, troopIDs, waypoints, cityname);
    }

    public void SendSpawnTroops(string troop,string ownername,string ownerID,Vector3 location)
    {
        photonView.RPC("CreateTroop", RpcTarget.AllViaServer, troop,ownername,ownerID,location,new Vector3(playerdata.playercolor.r, playerdata.playercolor.g, playerdata.playercolor.b));
    }
    public void SpawnTroops(string troop, string ownername, string ownerID, Vector3 location, Vector3 color)
    {
        IDcounter++;
        foreach(var prefab in prefabs)
        {
            if(prefab.TroopName == troop)
            {
                var Modloc = new Vector3(location.x + 8+ Random.Range(0.0f, 1.0f), 0, location.z + Random.Range(-1.0f, 1.0f));
                GameObject Spawn = Instantiate(prefab.gameObject, Modloc,new Quaternion (0f, 0f, 0f, 0f), troopparent);
                TroopScript ts = Spawn.GetComponent<TroopScript>();
                ts.owner.ownerID = ownerID;
                ts.owner.ownername = ownername;
                ts.syncer = this;
                ts.owner.ID = troop + ownername + IDcounter;
                ts.SetColor(new Color(color.x, color.y, color.z));
                WaterMovement wm = Spawn.GetComponent<WaterMovement>();
                wm.terrain = terrain;
                var trooper = Spawn.GetComponent<TroopScript>();
                troops.Add(trooper);
                if (ownerID == playerdata.playerID)
                {
                    playerdata.OwnedTroops.Add(trooper);
                }

            }
        }
    }

    public bool RemoveTroop(string troopID)
    {
        for(int i = 0; i < troops.Count; i++)
        {
            if(troops[i].owner.ID == troopID)
            {
                toremove.Add(troops[i]);
                playerdata.RemoveTroopInPlayer(troopID);
                status.OnUpdateBar(troops[i].owner.ownername+"'s " + troops[i].TroopName + " has been wiped out!",false);
                return true;
            }
        }
        return false;
    }

    public void removefromtroops()
    {
        for (int x = 0; x < toremove.Count; x++)
        {
            for (int i = 0; i < troops.Count; i++)
            {
                if (troops[i].owner.ID == toremove[x].owner.ID)
                {
                    troops.RemoveAt(i);
                    toremove.RemoveAt(x);
                    x--;
                    break;
                }
            }
        }
    }
    public void SyncAll(string[] troopIDs, Vector3[] locations, float[] health)
    {
        for(int i = 0; i < troopIDs.Length;i++)
        {
            foreach(var troop in troops)
            {
                if (troop != null)
                {
                    if (troop.owner.ID == troopIDs[i])
                    {
                        troop.transform.position = locations[i];
                        troop.Health = health[i];
                    }
                }
                else
                {
                    Debug.LogError("Sync Broken");
                }
            }
        }
    }

    public void SendSyncData()
    {
        List<string> IDs = new List<string>();
        List<Vector3> locations = new List<Vector3>();
        List<float> health = new List<float>();

        foreach(var troop in troops)
        {
            IDs.Add(troop.owner.ID);
            locations.Add(troop.transform.position);
            health.Add(troop.Health);
        }
        photonView.RPC("SyncTroops", RpcTarget.AllViaServer, IDs.ToArray(),locations.ToArray(),health.ToArray());
    }

    public void SyncAttack()
    {
        foreach(var troop in troops)
        {
            troop.AttackCombatants();
        }
        foreach (var troop in troops)
        {
            troop.UpdateStats();
        }
    }

    public void CameraCheck(Vector3 pos, float Range)
    {
        if (pos.y <= 30)
        {
            foreach (var troop in troops)
            {
                if (troop.gameObject.transform.position.x <= pos.x + Range &&
                    troop.gameObject.transform.position.x >= pos.x - Range &&
                    troop.gameObject.transform.position.z <= pos.z + Range &&
                    troop.gameObject.transform.position.z >= pos.z - Range)
                {
                    troop.SetBarActive(true);
                }
                else
                {
                    troop.SetBarActive(false);
                }
            }
        }
    }
    public Vector3 GetEffectiveTroopsCount()// X= playercount, Y = enemycount, Z = Totaltroopalltype;
    {
        int y = 0;
        foreach (var troop in troops)
        {
            if (troop.Type == "Ground" && troop.TroopType != "SPAA")
            {
                if (troop.owner.ownerID != playerdata.playerID)
                {
                    y++;
                }
            }
        }

        return new Vector3(playerdata.OwnedTroops.Count, y, troops.Count);
    }

    public void PurgePlayerTroops(string playerID)
    {
        for (int i = 0; i < troops.Count; i++)
        {
            if (troops[i].owner.ownerID == playerID)
            {
                toremove.Add(troops[i]);
            }
        }
        photonView.RPC("DefeatSync", RpcTarget.AllViaServer);
    }
}
