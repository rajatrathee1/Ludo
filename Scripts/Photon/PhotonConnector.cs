using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Random = System.Random;

public class photonConnector : MonoBehaviourPunCallbacks
{
    public static Action GetPhotonFriends = delegate { };
    public GameObject friendsPanel;
    public GameObject onlinePanel;
    public GameObject homePanel;
    public GameObject connecting;
    public GameObject connected;
    public GameObject disconnected;
    public GameObject createTable;
    public GameObject shareCode;
    public GameObject joinTable;
    public GameObject selectPlayer;
    string tableCode;
    public TMP_Text codeShare;
    public TMP_InputField roomCode;
    public GameObject playerListItemPrefab;
    public GameObject playerListItemParent;
    private Dictionary<int, GameObject> playerListGameObject;
    public TMP_Text playerCount;
    public GameObject playButton;
    #region Unity Method
    private void Start()
    {
        //string nickname = PlayerPrefs.GetString("USERNAME");
        //ConnectToPhoton(nickname);
    }
    #endregion
    #region Private Methods
    private void ConnectToPhoton(string nickName)
    {
        Debug.Log($"Connect to Photon as {nickName}");
        PhotonNetwork.AuthValues = new AuthenticationValues(nickName);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = nickName;

    }
    public void activateMyPanel(string panelName)
    {
        friendsPanel.SetActive(panelName.Equals(friendsPanel.name));
        homePanel.SetActive(panelName.Equals(homePanel.name));
        onlinePanel.SetActive(panelName.Equals(onlinePanel.name));
    }
    public void activateOnlinePanel(string panelName)
    {
        connecting.SetActive(panelName.Equals(connecting.name));
        connected.SetActive(panelName.Equals(connected.name));
        disconnected.SetActive(panelName.Equals(disconnected.name));
    }

    public void activateMyOnlinelinePanel(string panelName)
    {
        createTable.SetActive(panelName.Equals(createTable.name));
        shareCode.SetActive(panelName.Equals(shareCode.name));
        joinTable.SetActive(panelName.Equals(joinTable.name));
    }
    public void globalEvent()
    {
        activateMyPanel(onlinePanel.name);
        PhotonNetwork.ConnectUsingSettings();
        string nickname = PlayerPrefs.GetString("USERNAME");
        PhotonNetwork.AuthValues = new AuthenticationValues(nickname);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = nickname;
    }
    public void friendsEvent()
    {
        activateMyPanel(friendsPanel.name);
    }
    public void selectPlayers()
    {
        selectPlayer.SetActive(true);
    }
    public void closePlayers()
    {
        selectPlayer.SetActive(false);
    }
    public void joinRooms()
    {
        activateMyOnlinelinePanel(joinTable.name);
    }
    public void closeRooms()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            playerListGameObject = new Dictionary<int, GameObject>();
        }
        activateMyOnlinelinePanel(createTable.name);
    }

    public void start2Players()
    {
        gameManager.gm.totalPlayersCanPlay = 2;
        generateCode();

    }
    public void start3Players()
    {
        gameManager.gm.totalPlayersCanPlay = 3;
        generateCode();
    }
    public void start4Players()
    {
        gameManager.gm.totalPlayersCanPlay = 4;
        generateCode();
    }
    public void generateCode()
    {
        Random generatore = new Random();
        tableCode = generatore.Next(0, 1000000).ToString("D6");
        codeShare.text = tableCode;

        CreatePhotonRoom(tableCode);
        activateMyOnlinelinePanel(shareCode.name);
        playerCount.text = "";
    }
    private void CreatePhotonRoom(string roomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = Byte.Parse(gameManager.gm.totalPlayersCanPlay.ToString());
        PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
    }
    #endregion
    #region  Public Methods
    #endregion
    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("You have connected to the Photon Master Server");
        activateOnlinePanel(connected.name);
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("You have connected to a Photon Lobby");
        GetPhotonFriends?.Invoke();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"You have created a Photon Room named {PhotonNetwork.CurrentRoom.Name}");

    }
    public void buttonForShare()
    {
        shareText();
    }

    public void shareText()
    {
#if UNITY_ANDROID
           AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Code Share");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Please join using this code "+tableCode+ "\nDownload App: https://play.google.com/store/apps/details?id=com.DefaultCompany.Ludo");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intentObject);
#endif
    }
    public void JoinCreatedRoom()
    {
        string code = roomCode.text;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRoom(code);  // Attempt to join the room with the entered code
            activateMyOnlinelinePanel(shareCode.name);
            codeShare.text = code;

        }
        else
        {
            Debug.LogError("Photon is not connected and ready to join a room.");
        }
    }

    public void playGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
       
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"You have joined the Photon room {PhotonNetwork.CurrentRoom.Name}");

        if (playerListGameObject == null)
        {
            playerListGameObject = new Dictionary<int, GameObject>();
        }
        else
        {
            playerListGameObject.Clear();
        }

        int childIndex = 0;
        int totalChildren = playerListItemParent.transform.childCount;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject playerListItem;

            if (childIndex < totalChildren)
            {
                // Use existing child and activate it
                playerListItem = playerListItemParent.transform.GetChild(childIndex).gameObject;
                playerListItem.SetActive(true);
            }
            else
            {
                // Instantiate new item
                playerListItem = Instantiate(playerListItemPrefab, playerListItemParent.transform);
                playerListItem.transform.localScale = Vector3.one;
            }

            // Set player name
            TMP_Text nameText = playerListItem.GetComponentInChildren<TMP_Text>();
            if (nameText != null)
            {
                nameText.text = p.NickName;
            }
            else
            {
                Debug.LogError("Name Text component not found on playerListItem prefab");
            }

            // Add to dictionary
            playerListGameObject[p.ActorNumber] = playerListItem;

            // Indicate if this is the local player
            if (p.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                // Assuming child at index 1 is the indicator (e.g., a highlight)
                if (playerListItem.transform.childCount > 1)
                {
                    playerListItem.transform.GetChild(1).gameObject.SetActive(true);
                }
            }

            childIndex++;
        }

        // Disable any extra UI elements
        for (int i = childIndex; i < totalChildren; i++)
        {
            playerListItemParent.transform.GetChild(i).gameObject.SetActive(false);
        }

        playerUpdate();
    }

    public void playerUpdate()
    {
        playerCount.text = PhotonNetwork.PlayerList.Length + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            playButton.SetActive(true);
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("You have left a Photon Room");
        playerListGameObject = new Dictionary<int, GameObject>();

        foreach (GameObject obj in playerListGameObject.Values)
        {
            Destroy(obj);
        }

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"You failed to join a Photon Room : {message}");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        foreach (Transform child in playerListItemParent.transform)
        {
            TMP_Text playerNameText = child.GetChild(0).GetComponent<TMP_Text>();

            // Check if it's the default "Rajat" prefab and destroy it
            if (playerNameText != null && (playerNameText.text == "Rajat" || playerNameText.text == "SamplePlayerName"))
            {
                Destroy(child.gameObject);
            }
        }
        Debug.Log($"Another Player has joined the room {newPlayer.UserId}");
        GameObject playerListItem = Instantiate(playerListItemPrefab);
        Debug.Log($"You have joined the Photon room {PhotonNetwork.CurrentRoom.Name} as {newPlayer.NickName}");
        playerListItem.transform.SetParent(playerListItemParent.transform);
        playerListItem.transform.localScale = Vector3.one;
        playerListItem.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = newPlayer.NickName;
        playerListGameObject.Add(newPlayer.ActorNumber, playerListItem);
        playerUpdate();
    }

    /*
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Another Player has joined the room {newPlayer.UserId}");

        // Instantiate the player prefab
        GameObject playerListItem = Instantiate(playerListItemPrefab, playerListItemParent.transform);
        playerListItem.transform.localScale = Vector3.one;

        // Get the UIPlayer script attached to the prefab
        UIPlayer playerScript = playerListItem.GetComponent<UIPlayer>();

        // Initialize the UIPlayer with the new player's data
        playerScript.Initialize(newPlayer);

        // Add the player to the dictionary for tracking
        playerListGameObject.Add(newPlayer.ActorNumber, playerListItem);

        // Update the player count or any other logic
        playerUpdate();
    }
    */

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player has left the room {otherPlayer.UserId}");
        Destroy(playerListGameObject[otherPlayer.ActorNumber]);
        playerListGameObject.Remove(otherPlayer.ActorNumber);
        playerUpdate();
    }

    public override void OnMasterClientSwitched(Player NewMasterClient)
    {
        Debug.Log($"New Master Client is {NewMasterClient.UserId}");
    }
    #endregion
}


