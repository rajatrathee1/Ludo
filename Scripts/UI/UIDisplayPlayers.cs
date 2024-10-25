using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class UIDisplayPlayers : MonoBehaviour
{
    [SerializeField] private Transform playerContainer; // The container holding player items
    [SerializeField] private UIPlayer uiPlayerPrefab;   // Prefab for displaying individual players

    // Method to clear the current player list from the UI
    private void ClearPlayerContainer()
    {
        foreach (Transform child in playerContainer)
        {
            Destroy(child.gameObject);  // Destroy each player GameObject in the container
        }
    }

    // Method to handle the display of players in the room
    public void HandleDisplayPlayers(List<Player> players)
    {
        Debug.Log("Clearing players list UI");
        ClearPlayerContainer();
        Debug.Log($"Adding {players.Count} players to UI");

        // Loop through each player and instantiate a UI item for them
        foreach (Player player in players)
        {
            UIPlayer uiPlayer = Instantiate(uiPlayerPrefab, playerContainer);
            uiPlayer.Initialize(player);  // Initialize each player UI item with player data
            Debug.Log($"Added player: {player.NickName}");
        }
    }
}
