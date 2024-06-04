using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TroopBuilderUI : MonoBehaviour
{
    [SerializeField] private Text Troopname, MPcost,Steelcost,Oilcost,Moneycost,waittime;
    private TroopBuilderPanel target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUi(string troop, float Money, float Steel, float Oil, int Manpower,float wait,TroopBuilderPanel panel)
    {
        target = panel;
        Troopname.text = troop;
        MPcost.text = Manpower+"MP";
        Steelcost.text = Steel+" Steel";
        Oilcost.text = Oil + " Oil";
        Moneycost.text = "$" + Money / 1000000 + "M";
        waittime.text = (int)wait + " Days";
    }

    public void OnButtonPressed()
    {
        target.BuildTroop(Troopname.text);
    }
}
