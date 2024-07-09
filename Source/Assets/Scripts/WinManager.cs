using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinManager : MonoBehaviour
{
    public bool gameend;
    public bool solo;
    [SerializeField] private int Countdowntime;
    [Header("UI")]
    [SerializeField] private Text textbox;
    [SerializeField] private Text Enemycounter,Citycounter ;
    [SerializeField] private GameObject Victory, Defeat;
    [Header("Connections")]
    [SerializeField] private PlayerData player;
    [SerializeField] private CityManager citymgr;
    [SerializeField] private CitySelectionManager selector;
    [SerializeField] private CentralSystem central;
    [SerializeField] private StatusSystem status;
    [SerializeField] private TroopSync troops;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CountDown()
    {
        for (int i = 0; i <= Countdowntime;i++)
        {
            yield return new WaitForSeconds(1);
            textbox.text = "Game Starts in " + (Countdowntime - i);
            if(i == Countdowntime - 3)
            {
                if(player.Ownedcities.Count < 1)
                {
                    selector.RandomSelect();
                }
            }

        }
        Vector3 Data = citymgr.GetCitiesCount();
        if (Data.y < 1)
        {
            solo = true;
        }
        central.StartSystem();
        StartCoroutine(Winchecker());
    }

    public IEnumerator Winchecker()
    {
        yield return new WaitForSeconds(3);
        Vector3 Data = citymgr.GetCitiesCount();
        Vector3 troopdata = troops.GetEffectiveTroopsCount();

        Citycounter.text = ""+Data.x;
        Enemycounter.text = "" + Data.y;
        if (Data.x < 1 && troopdata.x < 1)
        {
            Defeat.SetActive(true);
            gameend = true;
            status.OnUpdateBar(player.playerName + " has been annihilated. all "+player.playerName +"= troops have surrendered.", true);
            troops.PurgePlayerTroops(player.playerID);
        }
        if (Data.y < 1 && !solo && troopdata.y < 1)
        {
            Victory.SetActive(true);
            gameend = true;
            status.OnUpdateBar(player.playerName + " has emerged victorious", true);
        }
        StartCoroutine(Winchecker());
    }
}
