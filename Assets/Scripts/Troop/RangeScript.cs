using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TroopScript troop;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Troop"))
        {
            TroopScript temp = other.gameObject.GetComponent<TroopScript>();
            if(temp.Type == troop.TargetType && temp.owner.ownerID != troop.owner.ownerID)
            {
                troop.combatants.Add(temp);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Troop"))
        {
            TroopScript temp = other.gameObject.GetComponent<TroopScript>();
           for(int i = 0; i < troop.combatants.Count;i++)
            {
                if(troop.combatants[i].owner.ID == temp.owner.ID && temp.owner.ownerID != troop.owner.ownerID)
                {
                    troop.combatants.RemoveAt(i);
                }
            }
        }
    }
}
