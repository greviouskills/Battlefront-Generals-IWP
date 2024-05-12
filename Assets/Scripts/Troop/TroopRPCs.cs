using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TroopRPCs : MonoBehaviour
{
    [SerializeField] private TroopSync syncer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    [PunRPC]
    public void TroopMove(string[] IDs, Vector3[] Waypoints)
    {
        syncer.UpdateTroopWaypoints(IDs, Waypoints);
    }

    [PunRPC]
    public void CreateTroop(string troop, string ownername, string ownerID, Vector3 location, Vector3 color)
    {
        syncer.SpawnTroops(troop, ownername, ownerID, location, color);
    }

    [PunRPC]
    public void SyncTroops(string[] IDs, Vector3[] locations, float[] health)
    {
        syncer.SyncAll(IDs, locations, health);
    }

    [PunRPC]
    public void AttackSync()
    {
        syncer.SyncAttack();
        syncer.removefromtroops();
    }
}
