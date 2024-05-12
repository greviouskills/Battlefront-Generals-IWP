using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
public class ClickHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Player")]
    [SerializeField] private PlayerData data;
    public string playerName;
    public string playerID;
    public Color playercolor;
    [Header("UI")]
    [SerializeField]
    private UImanager uimanager;
    
    [Header("Selected Object")]
    [SerializeField] private List<GameObject> Selected = new List<GameObject>();
    [SerializeField] private List<Vector3> waypoints = new List<Vector3>();

    private string selectedOwner;
    public bool CanSpy = false;

    public bool SelectingWaypoint = false;
    [Header("Connections")]
    public TroopSync Sync;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

 
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0) && data.Ownedcities.Count>0)
        {

            
            // Create a ray from the camera through the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits any object
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.point.x+" , "+ hit.point.y + " , " + hit.point.z+" , " + hit.transform.gameObject.name);
                if(Selected.Count <= 0)
                {
                    if (hit.transform.CompareTag("City"))
                    {
                        
                        CityScript city = hit.transform.GetComponent<CityScript>();
                        selectedOwner = city.owner.ownerID;
                        Selected.Add(hit.transform.gameObject);
                        if (selectedOwner == playerID)
                        {
                            uimanager.UpdateCityUI(Selected.Count,city,true,CanSpy);
                        }
                        else
                        {
                            uimanager.UpdateCityUI(Selected.Count, city, false, CanSpy);
                        }
                    }
                    else if (hit.transform.CompareTag("Troop"))
                    {

                        TroopScript troop = hit.transform.GetComponent<TroopScript>();
                        selectedOwner = troop.owner.ownerID;
                        Selected.Add(hit.transform.gameObject);
                        uimanager.UpdateTroopUI(troop, Selected.Count);
                        if (selectedOwner == playerID)
                        {
                            uimanager.UpdateTroopUI(troop, Selected.Count);
                            //troop.owner.selected = true;
                            SelectingWaypoint = true;
                        }
                    }
                }
                else if(SelectingWaypoint)
                {
                    if (IsMouseOnUI())
                    {
                        return;
                    }
                    else if (hit.transform.CompareTag("City"))
                    {
                        waypoints.Add(new Vector3(hit.point.x, 0, hit.point.z));
                        if (!Input.GetKey(KeyCode.LeftShift))
                        {
                            SendTroopWP();
                            SelectingWaypoint = false ;
                            ClearSelection();
                        }
                    }
                    else if (hit.transform.CompareTag("Troop"))
                    {
                        if (Input.GetKey(KeyCode.LeftControl))
                        {
                            TroopScript troop = hit.transform.GetComponent<TroopScript>();
                            selectedOwner = troop.owner.ownerID;
                            Debug.Log("test");
                            if (selectedOwner == playerID)
                            {
                                Selected.Add(hit.transform.gameObject);
                                uimanager.UpdateTroopUI(troop, Selected.Count);
                                //troop.owner.selected = true;
                            }
                        }
                        else if (Input.GetKey(KeyCode.LeftShift))
                        {
                            waypoints.Add(new Vector3(hit.point.x, 0, hit.point.z));
                        }
                        else
                        {
                            waypoints.Add(new Vector3(hit.point.x, 0, hit.point.z));
                            SendTroopWP();
                            ClearSelection();
                        }
                        
                    }
                    else if (hit.transform.CompareTag("Map"))
                    {
                        waypoints.Add(new Vector3(hit.point.x, 0, hit.point.z));
                        if (!Input.GetKey(KeyCode.LeftShift))
                        {
                            SendTroopWP();
                            ClearSelection();
                        }
                    }
                }
                else
                {
                    if (IsMouseOnUI())
                    {
                        return;
                    }
                    else if (hit.transform.CompareTag("Troop"))
                    {
                        ClearSelection();
                        Selected.Add(hit.transform.gameObject);
                        TroopScript troop = hit.transform.GetComponent<TroopScript>();
                        selectedOwner = troop.owner.ownerID;
                        uimanager.UpdateTroopUI(troop, Selected.Count);
                    }
                    else if (hit.transform.CompareTag("City"))
                    {
                        CityScript city = hit.transform.GetComponent<CityScript>();
                        selectedOwner = city.owner.ownerID;
                        if (Input.GetKey(KeyCode.LeftControl) &&
                            city.owner.ownerID == playerID &&
                            selectedOwner == playerID
                            )
                        {
                            Selected.Add(hit.transform.gameObject);
                            uimanager.UpdateCityUI(Selected.Count, city, true, CanSpy);
                        }
                        else
                        {
                            ClearSelection();
                            Selected.Add(hit.transform.gameObject);
                            uimanager.UpdateCityUI(Selected.Count, city, false, CanSpy);
                        }
                    }
                    else
                    {

                        ClearSelection();
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ClearSelection();
        }

        //debug purposes
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Sync.SendSpawnTroops("Infantry Company", playerName, playerID, data.Ownedcities[0].transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Sync.SendSpawnTroops("RPG Company", playerName, playerID, data.Ownedcities[0].transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Sync.SendSpawnTroops("Tank Battalion", playerName, playerID, data.Ownedcities[0].transform.position);
        }

    }

    private void ClearSelection()
    {
        SelectingWaypoint = false;
        uimanager.ClearUI();
        selectedOwner = "";
        Selected.Clear();
        waypoints.Clear();
    }

    //private void UpdateTroopUI(string name, string owner, float HP, float fightingcap, int selectedcount,bool showstat )
    //{
    //    CityPanel.SetActive(false);
    //    TroopPanel.SetActive(true);
    //    if (selectedcount > 1)
    //    {
    //        TroopName.text = "Multiple";
    //        TroopOwner.text = owner;
    //        TroopFightingCapacity.text = "Fighting Capacity: Multiple";
    //        TroopHP.text = "Health: Multiple";
    //    }
    //    else
    //    {
    //        TroopName.text = name;
    //        TroopOwner.text = "Owned By: "+owner;
    //        if (showstat)
    //        {
    //            TroopFightingCapacity.text = "Fighting Capacity: " + fightingcap;
    //            TroopHP.text = "Health: " + HP;
    //        }
    //        else 
    //        {
    //            TroopFightingCapacity.text = "Fighting Capacity: ?";
    //            TroopHP.text = "Health: ?";
    //        }
    //    }

    //}

    //private void UpdateCityUI(string name, string owner, float population,  int selectedcount,CityScript city,bool isowner)
    //{
    //    CityPanel.SetActive(true);
    //    TroopPanel.SetActive(false);
    //    CityPanel.GetComponent<CityViewScript>().SetUi(city,CanSpy, isowner);
    //    if (selectedcount > 1)
    //    {
    //        Cityname.text = "Multiple";
    //        Owner.text = "Owned By: " + owner;
    //        Population.text = "Health: Multiple";
    //    }
    //    else
    //    {
    //        Cityname.text = name;
    //        Owner.text = "Owned By: " + owner;
    //        Population.text = "Poulation: "+population;
    //    }

    //}

    private void SendTroopWP()
    {
        List<string> troops = new List<string>();
        foreach(var obj in Selected)
        {
            troops.Add(obj.GetComponent<Ownership>().ID);
        }
        Vector3[] wps = waypoints.ToArray();
        string[] ids = troops.ToArray();
        Sync.SendTroopWaypoints(ids,wps);
    }
    private bool IsMouseOnUI()
    {
        // Get the width of the screen
        float screenWidth = Screen.width;

        // Get the x-coordinate of the mouse pointer
        float mouseX = Input.mousePosition.x;

        // Check if the x-coordinate of the mouse pointer is less than or equal to 1/4 of the screen width
        return mouseX <= screenWidth / 4;
    }


}