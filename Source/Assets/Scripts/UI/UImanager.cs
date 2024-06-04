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
    [SerializeField] private Text TroopName, TroopOwner;
    [Header("ResourceView")]
    [SerializeField] private Text Money;
    [SerializeField] private Text Steel, Manpower, Oil;
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
    }

    public void UpdateTroopUI(TroopScript troop,int selectedcount)
    {
        CityPanel.SetActive(false);
        TroopPanel.SetActive(true);
        if (selectedcount > 1)
        {
            TroopName.text = "Multiple";
            TroopOwner.text = troop.owner.ownername;
            //TroopFightingCapacity.text = "Fighting Capacity: Multiple";
            //TroopHP.text = "Health: Multiple";
        }
        else
        {
            TroopName.text = troop.TroopName;
            TroopOwner.text = "Owned By: " + troop.owner.ownername;
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
        Money.text = "$"+resourcemanager.money/1000000 +"M";
        Steel.text = "Steel: " + resourcemanager.steel;
        Oil.text = "Oil: " + resourcemanager.oil;
        Manpower.text = "Manpower: " + resourcemanager.manpower;

    }
}
