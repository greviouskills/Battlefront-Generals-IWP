using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class TroopBuilderPanel : MonoBehaviour
{
    [SerializeField] private TroopBuilderUI prefab;
    [SerializeField] private CityViewBarScript statusbar;
    [SerializeField] private Transform BuilderPanel, StatusPanel;
    [SerializeField] private Slider troopprogress;
    [SerializeField] private GameObject progress;
    [SerializeField] private List<Spawnables> spawnables = new List<Spawnables>();
    [SerializeField] private ResourceManager resourceManager;
    public CityScript city;
    // Start is called before the first frame update

    [Serializable]
    public class Spawnables
    {
        public string TroopName;
        public float Money;
        public float Oil;
        public float Steel;
        public int Manpower;
        public float waittime;
        public string Prequisite;
    }
    void Start()
    {
    }

    // Update is called once per frame
    
    void FixedUpdate()
    {
        troopprogress.value = city.troopprogress;
        if (city.trainingqueue.Count == 0)
        {
            progress.SetActive(false);
        }
        else
        {
            progress.SetActive(true);
        }
    }

    public void UpdateUi()
    {
        foreach (Transform child in BuilderPanel)
        {
            // Destroy the child GameObject
            Destroy(child.gameObject);
        }
        foreach (Transform child in StatusPanel)
        {
            // Destroy the child GameObject
            Destroy(child.gameObject);
        }
        foreach(var troop in city.trainingqueue)
        {
            GameObject obj = Instantiate(statusbar.gameObject,StatusPanel);
            obj.GetComponent<CityViewBarScript>().SetType1(troop.Troop, "Training");
        }

        foreach(var troop in spawnables)
        {
            if(troop.Prequisite != "-")
            {
                if (city.HasBuilding(troop.Prequisite))
                {
                    GameObject obj = Instantiate(prefab.gameObject,BuilderPanel);
                    obj.GetComponent<TroopBuilderUI>().SetUi(troop.TroopName,troop.Money,troop.Steel,troop.Oil,troop.Manpower,troop.waittime,this);
                }
            }
            else
            {
                GameObject obj = Instantiate(prefab.gameObject, BuilderPanel);
                obj.GetComponent<TroopBuilderUI>().SetUi(troop.TroopName, troop.Money, troop.Steel, troop.Oil, troop.Manpower, troop.waittime, this);
            }
        }
    }

    public void BuildTroop(string troopname)
    {
        foreach(var troop in spawnables)
        {
            if(troop.TroopName == troopname)
            {
                if (resourceManager.attemptconsume(troop.Money,troop.Manpower,troop.Steel,troop.Oil))
                {
                    city.traintroop(troopname, troop.waittime);
                    UpdateUi();
                }
            }
        }
      
    }


}
