using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UImanager : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private ResourceManager resourcemanager;
    [Header("CityView")]
    [SerializeField] private GameObject CityPanel,TroopBuilder;
    [SerializeField] private Text Population, Cityname, Owner;
    [Header("TroopView")]
    [SerializeField] private GameObject TroopPanel;
    [SerializeField] private Text TroopName, TroopOwner, HP, Attack, Integrity, Targets;
    [Header("ResourceView")]
    [SerializeField] private Text Money;
    [SerializeField] private Text Steel, Manpower, Oil;
    [Header("Tutorial Screen")]
    [SerializeField] private List<GameObject> TutorialPanels =  new List<GameObject>();
    private int pagecount = 0;
    public bool tutorialopen = false;
    public bool UIopen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearUI()
    {
   
        CityPanel.SetActive(false);
        TroopPanel.SetActive(false);
        TroopBuilder.SetActive(false);
        UIopen = false;
    }

    public void UpdateTroopUI(TroopScript troop,int selectedcount)
    {
        UIopen = true;
        CityPanel.SetActive(false);
        TroopPanel.SetActive(true);
        if (selectedcount <= 1)
        {
            TroopName.text = "Multiple ("+selectedcount+")";
            TroopOwner.text = troop.owner.ownername;
            HP.text = "HP: "+troop.Health;
            Attack.text = "Atk: " + troop.Attack;
            Integrity.text = "Integrity: " + troop.FightingCapacity * 100 + "%";
            Targets.text = "Targets: " + troop.TargetType;
            //TroopFightingCapacity.text = "Fighting Capacity: Multiple";
            //TroopHP.text = "Health: Multiple";
        }
        else
        {
            Debug.Log("Selected"+selectedcount);

            TroopName.text = troop.TroopName;
            TroopOwner.text = "Owned By: " + troop.owner.ownername;
            HP.text = "HP: " + "Multiple";
            Attack.text = "Atk: " + "Multiple";
            Integrity.text = "Integrity: " + "Multiple";
            Targets.text = "Targets: " + "Multiple";
            //if (showstat)
            //{
            //    //TroopFightingCapacity.text = "Fighting Capacity: " + troop.fightingcap;
            //    //TroopHP.text = "Health: " + HP;
            //}
            //else
            //{
            //    TroopFightingCapacity.text = "Fighting Capacity: ?";
            //    TroopHP.text = "Health: ?";
            //}
        }

    }

    public void UpdateCityUI(int selectedcount, CityScript city, bool isowner, bool canspy)
    {
        UIopen = true;
        CityPanel.SetActive(true);
        TroopPanel.SetActive(false);
        CityPanel.GetComponent<CityViewScript>().SetUi(city, canspy, isowner);
        if (selectedcount > 1)
        {
            Cityname.text = "Multiple";
            Owner.text = "Owned By: " + city.owner.ownername;
            Population.text = "Population: Multiple";
        }
        else
        {
            Cityname.text = city.gameObject.name;
            Owner.text = "Owned By: " + city.owner.ownername;
            Population.text = "Poulation: " + city.population;
        }

    }

    public void UpdateResourceUi()
    {
        
        Money.text = "$"+ (Mathf.RoundToInt(resourcemanager.money / 100) / 10000f).ToString("F2") + " M";
        Steel.text = "Steel: " + resourcemanager.steel;
        Oil.text = "Oil: " + resourcemanager.oil;
        Manpower.text = "Manpower: " + resourcemanager.manpower;

    }

    public void OpenTutorial()
    {
        tutorialopen = true;
        TutorialPanels[pagecount].SetActive(true);
    }
    public void CloseTutorial()
    {
        foreach(var Page in TutorialPanels)
        {
            Page.SetActive(false);
        }
        tutorialopen = false;
    }

    public void Nextpage()
    {
        TutorialPanels[pagecount].SetActive(false);
        pagecount++;
        TutorialPanels[pagecount].SetActive(true);
    }
    public void Prevpage()
    {
        TutorialPanels[pagecount].SetActive(false);
        pagecount--;
        TutorialPanels[pagecount].SetActive(true);
    }

}
