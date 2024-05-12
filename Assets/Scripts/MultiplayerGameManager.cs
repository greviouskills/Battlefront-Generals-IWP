using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class MultiplayerGameManager : MonoBehaviourPunCallbacks
{
    public static MultiplayerGameManager Instance = null;
    public Text InfoText;


    public void Awake()
    {
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    }

    // Start is called before the first frame update
    public void Start()
    {
        Hashtable props = new Hashtable
            {
                {MultiplayerGame.PLAYER_LOADED_LEVEL, true}
            };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
    }

    #region PUN CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DemoAsteroids-LobbyScene");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckEndOfGame();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
        int startTimestamp;
        bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

        if (changedProps.ContainsKey(MultiplayerGame.PLAYER_LOADED_LEVEL))
        {
            if (CheckAllPlayerLoadedLevel())
            {
                if (!startTimeIsSet)
                {
                    CountdownTimer.SetStartTime();
                }
            }
            else
            {
                // not all players loaded yet. wait:
                Debug.Log("setting text waiting for players! ", this.InfoText);
                InfoText.text = "Waiting for other players...";
            }
        }

    }

    #endregion

    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerLoadedLevel;

            if (p.CustomProperties.TryGetValue(MultiplayerGame.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
            {
                if ((bool)playerLoadedLevel)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }
    private void StartGame()
    {
        Debug.Log("StartGame!");

        //Vector3 position = new Vector3(-9.8f, 0.0f, -3.2f);
        //Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        //GameObject player = PhotonNetwork.Instantiate("JohnLemon", position, rotation, 0);

        //if (player.GetComponent<PhotonView>().IsMine)
        //{
        //    virtualCam.Follow = player.transform;
        //    virtualCam.LookAt = player.transform;
        //}

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    SpawnGhosts();
        //}
        //player.GetComponentInChildren<GetCubeColor>().UpdateColor();
    }

   
    private void CheckEndOfGame()
    {
    }

    private void OnCountdownTimerIsExpired()
    {
        StartGame();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
