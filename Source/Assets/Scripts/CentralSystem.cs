using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class CentralSystem : MonoBehaviour
{
    [SerializeField] private PlayerData playerdata;
    [SerializeField]private TroopSync troopsync;
    [SerializeField] private CityManager citysync;
    [SerializeField] private ResourceManager resourcemanager;
    private int Days = 0;
    [SerializeField] private Text text;
    //[SerializeField] private UImanager uimanager;
    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator UpdateLoop()
    {
        while (true)
        {
            Debug.Log("sentsync");
            yield return new WaitForSeconds(0.5f);
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("AttackSync", RpcTarget.AllViaServer);
            }
            yield return new WaitForSeconds(1f);
            resourcemanager.GetResources();
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("AttackSync", RpcTarget.AllViaServer);
            }
            yield return new WaitForSeconds(0.25f);
            citysync.CityTick();
            yield return new WaitForSeconds(0.25f);

            if (PhotonNetwork.IsMasterClient)
            {
                troopsync.SendSyncData();
            }
        }
    }

    IEnumerator Updateresource()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            resourcemanager.GetResources();
            Days++;
            text.text = "Day " + Days;
        }
    }

    public void StartSystem()
    {
        if (PhotonNetwork.IsMasterClient)
        {
           
        }
        StartCoroutine(UpdateLoop());
        StartCoroutine(Updateresource());
    }
}
