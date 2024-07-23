using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerData player;
    [SerializeField] private ClickHandler clicker;
    
    [Header("Tutorials")]
    [SerializeField] private List<GameObject> TutorialPanels = new List<GameObject>();
    [SerializeField] private GameObject PrimaryPanel;
    private int pagecount = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CloseTutorial()
    {
        PrimaryPanel.SetActive(false);
    }
    private void RefreshPage()
    {
        if (TutorialPanels[pagecount - 1] != null) 
        {
            TutorialPanels[pagecount - 1].SetActive(false); 
        }
        TutorialPanels[pagecount].SetActive(true);
    }

    IEnumerator delayskipslide()
    {
        yield return new WaitForSeconds(5f);
        RefreshPage();
    }
}
