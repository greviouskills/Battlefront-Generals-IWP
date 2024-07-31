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
    public GameObject PrimaryPanel;
    private int pagecount = 0;
    public bool Active;
    private bool running;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Active)
        {
            TutorialProgress();
        }
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
        if (pagecount < TutorialPanels.Count)
        {
            TutorialPanels[pagecount].SetActive(true);
        }
    }
    void TutorialProgress()
    {
        switch (pagecount)
        {
            case 0:
                StartCoroutine(delayskipslide(3f));
                break;
            case 1:
                if (clicker.Selected.Count > 0)
                {
                    if (clicker.Selected[0].CompareTag("City"))
                    {

                        if (clicker.selectedOwner == player.playerID)
                        {
                            StartCoroutine(delayskipslide(1f));
                        }
                    }


                }
                break;
            case 2:
                if(player.OwnedTroops.Count >= 1)
                {
                    pagecount++;
                    RefreshPage();
                }
                break;
            case 3:
                if (clicker.Selected.Count > 0)
                {
                    if (clicker.selectedOwner == player.playerID)
                    {
                        pagecount++;
                        RefreshPage();
                    }
                }
                break;
            case 4:
                if (player.Ownedcities.Count > 1)
                {
                    StartCoroutine(delayskipslide(1f));
                }
                break;
            case 5:
                StartCoroutine(delayskipslide(5f));
                break;
            case 6:
                if(player.Ownedcities[0].Constructed.Count > 0)
                {
                    StartCoroutine(delayskipslide(1f));
                }
                break;
            case 7:
                StartCoroutine(delayskipslide(5f));
                break;
            case 8:
                StartCoroutine(delayskipslide(10f));
                break;
            case 9:
                StartCoroutine(delayskipslide(5f));
                break;



        }
    }

    IEnumerator delayskipslide(float time)
    {
        if (!running)
        {
            running = true;
            yield return new WaitForSeconds(time);
            pagecount++;
            RefreshPage();
            if (pagecount + 1 == TutorialPanels.Count)
            {
                CloseTutorial();
                Active = false;
            }
            running = false;
        }
        
      
    }
}
