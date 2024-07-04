using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CitySelectionManager : MonoBehaviour
{
    [SerializeField] private List<CitySelectionUI> Uis = new List<CitySelectionUI>();
    [SerializeField]private GameObject Uiprefab,UiPanel;
    [SerializeField] private Transform UIparent;
    [SerializeField] private CityManager mgr;
    [SerializeField] private CameraMovement cam;
    // Start is called before the first frame update
    private PhotonView photonView;
    void Start()
    {
        photonView = this.GetComponentInParent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateUis(List<string> cities)
    {
        foreach(string city in cities)
        {
            GameObject ui = Instantiate(Uiprefab, UIparent);
            CitySelectionUI temp = ui.GetComponent<CitySelectionUI>();
            Uis.Add(temp);
            temp.Setup(city, this);
        }
    }

    public void SelectedCity(string cityname)
    {

    }
    public void removecity(string cityname)
    {
        photonView.RPC("RemoveCityUi", RpcTarget.AllViaServer,cityname);
        cam.ViewCity(cityname);
        UiPanel.SetActive(false);
        mgr.SendCityOwnerChange(cityname);

    }
    public void removecityUI(string cityname)
    {
        for(int i = 0; i < Uis.Count;i++)
        {
            if (Uis[i].targetcity == cityname)
            {
                Uis[i].selfdestruct();
                Uis.RemoveAt(i);
                break;
            }
        }
    }
}
