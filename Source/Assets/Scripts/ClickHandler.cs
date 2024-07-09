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
    private CitySelectionManager selector;
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
    [SerializeField] private Image Dragger;
    [SerializeField] private CameraMovement cam;
    private bool dragselecting;
    public Vector3 clickstart, clickend, mousestart, mouseend; 
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

 
        // Check if the left mouse button is pressed
        if (selector.selected)
        {
            if (IsMouseOnUI() || uimanager.tutorialopen)
            {
                return;
            }
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0) && SelectingWaypoint)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    SelectWaypoint(hit);
                }

            }
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    CtrlSelect(hit.transform.gameObject);
                }
            }
            else if (Input.GetKey(KeyCode.LeftAlt))
            {
                DragSelecting();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (SelectingWaypoint)
                    {
                        SelectWaypoint(hit);
                    }
                    else
                    {
                        ClickSelect(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if (dragselecting)
                {
                    DragSelecting();
                }
            }


            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ClearSelection();
        }

        if (SelectingWaypoint)
        {
            linedrawer.SetPosition(0, new Vector3(Selected[0].transform.position.x, 8f, Selected[0].transform.position.z));
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
        foreach(var obj in Selected)
        {
            if (obj.CompareTag("Troop"))
            {
                var troop = obj.GetComponent<TroopScript>();
                troop.select(false);
            }
            else if (obj.CompareTag("City"))
            {
                var city = obj.GetComponent<CityScript>();
                city.select(false);
            }
        }
        Selected.Clear();
        waypoints.Clear();
        linedrawer.positionCount = 0;
    }

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
        if (uimanager.UIopen)
        {
            // Get the width of the screen
            float screenWidth = Screen.width;

            // Get the x-coordinate of the mouse pointer
            float mouseX = Input.mousePosition.x;

            // Check if the x-coordinate of the mouse pointer is less than or equal to 1/4 of the screen width
            return mouseX <= screenWidth / 4;
        }
        return false;
    }
    
    private void ClickSelect(GameObject obj)
    {
        ClearSelection();
        if (obj.CompareTag("City"))
        {

            CityScript city = obj.GetComponent<CityScript>();
            selectedOwner = city.owner.ownerID;
            Selected.Add(obj);
            city.select(true);
            if (selectedOwner == playerID)
            {
                uimanager.UpdateCityUI(Selected.Count, city, true, CanSpy);
            }
            else
            {
                uimanager.UpdateCityUI(Selected.Count, city, false, CanSpy);
            }
        }
        else if (obj.CompareTag("Troop"))
        {
            TroopScript troop = obj.transform.GetComponent<TroopScript>();
            selectedOwner = troop.owner.ownerID;
            if (selectedOwner == playerID)
            {
                uimanager.UpdateTroopUI(troop, Selected.Count);
                Selected.Add(obj);
                troop.select(true);
                SelectingWaypoint = true;
                linedrawer.positionCount++;
                linedrawer.SetPosition(0, new Vector3(obj.transform.position.x, 4.1f, obj.transform.position.z));
            }
        }
        
    }
    private void CtrlSelect(GameObject obj)
    {

        if (obj.CompareTag(Selected[0].tag))
        {
            
            if (Selected[0].CompareTag("Troop"))
            {
                var troop = obj.GetComponent<TroopScript>();
                if (selectedOwner == playerID && troop.owner.ownerID == playerID && !troop.Selected)
                {
                    troop.select(true);
                    Selected.Add(obj);
                    uimanager.UpdateTroopUI(troop, Selected.Count);
                    //troop.owner.selected = true;
                }
            }
        }
    }

    private void SelectWaypoint(RaycastHit hit)
    {
        waypoints.Add(new Vector3(hit.point.x, 0, hit.point.z));
        linedrawer.positionCount += 1;
        linedrawer.SetPosition(waypoints.Count, new Vector3(waypoints[waypoints.Count - 1].x, 4.1f, waypoints[waypoints.Count - 1].z));
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            SendTroopWP();
            ClearSelection();
        }
    }

    private void DragSelectComplete()
    {
        float minX = Mathf.Min(clickstart.x, clickend.x);
        float maxX = Mathf.Max(clickstart.x, clickend.x);
        float minY = Mathf.Min(clickstart.z, clickend.z);
        float maxY = Mathf.Max(clickstart.z, clickend.z);

        foreach (var troop in Sync.troops)
        {
            if(troop.owner.ownerID == playerID)
            {
                Vector3 trsf = troop.transform.position;
                Debug.Log(trsf + " in constraints of "+minX+" , "+ maxX + " , " + minY + " , " + maxY);
                if (trsf.x >= minX && trsf.x <= maxX && trsf.z >= minY && trsf.z <= maxY)
                {
                    if(linedrawer.positionCount < 1)
                    {
                        linedrawer.positionCount++;
                        linedrawer.SetPosition(0, troop.transform.position);
                    }
                    troop.select(true);
                    Selected.Add(troop.gameObject);

                    SelectingWaypoint = true;
                    uimanager.UpdateTroopUI(troop, Selected.Count);
                }
            }
        }

    }

    private void DragSelecting()
    {
        if (Input.GetMouseButton(0))
        {
            if (!dragselecting)
            {
                ClearSelection();
                Dragger.gameObject.SetActive(true);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    clickstart = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    dragselecting = true;
                    cam.canmove = false;
                    mousestart = Input.mousePosition;
                    Dragger.rectTransform.position = Input.mousePosition;
                }
            }
            float x = 0, y = 0, z = 0;
            x = Input.mousePosition.x - mousestart.x;
            y = Input.mousePosition.y - mousestart.y;
            z = Input.mousePosition.z - mousestart.z;
            Vector3 scale = new Vector3(x, y, z);
            Dragger.rectTransform.localScale = scale;

        }
        else if(dragselecting)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                clickend = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                DragSelectComplete();
            }
            Dragger.gameObject.SetActive(false);
            dragselecting = false;
            cam.canmove = true;
        }
    }

}
