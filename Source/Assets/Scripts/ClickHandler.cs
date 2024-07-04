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
    [SerializeField]
    private LineRenderer linedrawer;
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
            if (IsMouseOnUI() || uimanager.tutorialopen)
            {
                return;
            }


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
                            linedrawer.positionCount++;
                            linedrawer.SetPosition(0, hit.transform.position);
                        }
                    }
                }
                else if(SelectingWaypoint)
                {
                    
                  
                    if (hit.transform.CompareTag("City"))
                    {
                        waypoints.Add(new Vector3(hit.point.x, 0, hit.point.z));
                        linedrawer.positionCount += 1;
                        linedrawer.SetPosition(waypoints.Count, new Vector3(waypoints[waypoints.Count - 1].x, 0.1f, waypoints[waypoints.Count - 1].z));
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
                            //if (Selected.Count == 1)
                            //{
                            //    linedrawer.positionCount = 0;
                            //}
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
                            linedrawer.positionCount+=1;
                           
                            linedrawer.SetPosition(waypoints.Count, new Vector3(waypoints[waypoints.Count-1].x, 0.1f, waypoints[waypoints.Count - 1].z));
                        }
                        else
                        {
                            waypoints.Add(new Vector3(hit.point.x, 0, hit.point.z));
                            linedrawer.positionCount++;
                            linedrawer.SetPosition(waypoints.Count, hit.transform.position);
                            SendTroopWP();
                            ClearSelection();
                        }
                        
                    }
                    else if (hit.transform.CompareTag("Map"))
                    {
                        waypoints.Add(new Vector3(hit.point.x, 0, hit.point.z));
                        linedrawer.positionCount += 1;
                        linedrawer.SetPosition(waypoints.Count, new Vector3(waypoints[waypoints.Count - 1].x, 0.1f, waypoints[waypoints.Count - 1].z));
                        if (!Input.GetKey(KeyCode.LeftShift))
                        {
                            SendTroopWP();
                            ClearSelection();
                        }
                    }
                }
                else
                {
                    if (hit.transform.CompareTag("Troop"))
                    {
                        ClearSelection();
                        TroopScript troop = hit.transform.GetComponent<TroopScript>();
                        selectedOwner = troop.owner.ownerID;
                        Selected.Add(hit.transform.gameObject);
                        uimanager.UpdateTroopUI(troop, Selected.Count);
                        if (selectedOwner == playerID)
                        {
                            uimanager.UpdateTroopUI(troop, Selected.Count);
                            //troop.owner.selected = true;
                            SelectingWaypoint = true;
                            linedrawer.positionCount++;
                            linedrawer.SetPosition(0, hit.transform.position);
                        }
                    }
                    else if (hit.transform.CompareTag("City"))
                    {
                        ClearSelection();
                        CityScript city = hit.transform.GetComponent<CityScript>();
                        selectedOwner = city.owner.ownerID;
                        //if (Input.GetKey(KeyCode.LeftControl) &&
                        //    city.owner.ownerID == playerID &&
                        //    selectedOwner == playerID
                        //    )
                        //{
                        //    Selected.Add(hit.transform.gameObject);
                        //    uimanager.UpdateCityUI(Selected.Count, city, true, CanSpy);
                        //}
                        //else
                        //{
                        Selected.Add(hit.transform.gameObject);
                        if (selectedOwner == playerID)
                        {
                            uimanager.UpdateCityUI(Selected.Count, city, true, CanSpy);
                        }
                        else
                        {
                            uimanager.UpdateCityUI(Selected.Count, city, false, CanSpy);
                        }
                        //}
                    }
                    else
                    {

                        ClearSelection();
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ClearSelection();
        }

        if (SelectingWaypoint)
        {
            linedrawer.SetPosition(0, Selected[0].transform.position);
        }
        //debug purposes
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Sync.SendSpawnTroops("Blackjack Squadron", playerName, playerID, data.Ownedcities[0].transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Sync.SendSpawnTroops("Felon Squadron", playerName, playerID, data.Ownedcities[0].transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Sync.SendSpawnTroops("SPAA Battalion", playerName, playerID, data.Ownedcities[0].transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Sync.SendSpawnTroops("Bear Squadron", playerName, playerID, data.Ownedcities[0].transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Sync.SendSpawnTroops("S-400 Battery", playerName, playerID, data.Ownedcities[0].transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Sync.SendSpawnTroops("SAM Battery", playerName, playerID, data.Ownedcities[0].transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Sync.SendSpawnTroops("Falcon Squadron", playerName, playerID, data.Ownedcities[0].transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Sync.SendSpawnTroops("Frogfoot Squadron", playerName, playerID, data.Ownedcities[0].transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Sync.SendSpawnTroops("Tank Battalion", playerName, playerID, data.Ownedcities[0].transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Sync.SendSpawnTroops("Infantry Company", playerName, playerID, data.Ownedcities[0].transform.position);
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            Sync.SendSpawnTroops("Debugger", playerName, playerID, data.Ownedcities[0].transform.position);
        }
    }

    private void ClearSelection()
    {
        SelectingWaypoint = false;
        uimanager.ClearUI();
        selectedOwner = "";
        Selected.Clear();
        waypoints.Clear();
        linedrawer.positionCount = 0;
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
