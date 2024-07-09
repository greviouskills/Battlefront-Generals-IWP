using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class StatusSystem : MonoBehaviour
{
    [SerializeField] private Text StatusBar;
    private PhotonView photonView;
    private List<string> messages = new List<string>();
    private List<string> Prioritymessages = new List<string>();
    private bool Ready = true;
    // Start is called before the first frame update
    void Start()
    {
        photonView = this.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Ready)
        {
            if(Prioritymessages.Count > 0)
            {
                StartCoroutine(display(Prioritymessages[0], 5f, true));
                Prioritymessages.RemoveAt(0);
            }
            else if(messages.Count >0)
            {
                StartCoroutine(display(messages[0], 2f, false));
                messages.RemoveAt(0);
            }
        }
    }
    public void OnUpdateBar(string text, bool priority)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateBar", RpcTarget.AllViaServer, text, priority);
        }
    }
    public IEnumerator display(string text, float wait, bool priority)
    {
        Ready = false;
        StatusBar.text = text;
        if (priority)
        {
            StatusBar.color = new Color(1, 0, 0);
        }
        yield return new WaitForSeconds(wait);
        StatusBar.text = "";
        StatusBar.color = new Color(1, 1, 1);
        Ready = true;

    }

    [PunRPC]
    public void UpdateBar(string text, bool priority)
    {
        if (priority)
        {
            Prioritymessages.Add(text);
        }
        else
        {
            messages.Add(text);
        }

    }
}
