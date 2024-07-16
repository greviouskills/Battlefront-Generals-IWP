using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TroopAlertScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject alerticon;
    [SerializeField] private Transform AlertPanel;
    [SerializeField] private PlayerData player;
    [SerializeField] private List<RectTransform> Alerts = new List<RectTransform>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Scan();
    }
     private void Scan()
    {
        int alertcount = 0;
        foreach(var troop in player.OwnedTroops)
        {
            if(troop.UnderAttack)
            {
                if (Alerts.Count<alertcount + 1)
                {
                    GameObject Obj = Instantiate(alerticon, AlertPanel);
                    Alerts.Add(Obj.GetComponent<RectTransform>()); 
                }
                Alerts[alertcount].gameObject.SetActive(true);
                Vector3 screenPos = cam.WorldToScreenPoint(troop.transform.position);

                // Clamp the screen position to the screen bounds
                screenPos.x = Mathf.Clamp(screenPos.x, 5, Screen.width-5);
                screenPos.y = Mathf.Clamp(screenPos.y, 70, Screen.height-70);

                // Set the UI element's position

                Alerts[alertcount].position = screenPos;
                alertcount++;
            }
        }

        for(int i = alertcount+1; i <= Alerts.Count; i++)
        {
            Alerts[i-1].gameObject.SetActive(false);
        }
    }
}
