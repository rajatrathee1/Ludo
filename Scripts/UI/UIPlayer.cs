using System;
using TMPro;
using Photon.Realtime;
using UnityEngine;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Player player;

    public static Action<int> OnRemovePlayer = delegate { };

    // Initialize the player's data and update the UI
    public void Initialize(Player player)
    {
        this.player = player;
        playerNameText.SetText(this.player.NickName); // Use NickName to display in the room
    }
    /*
    // Method to remove player, if needed
    public void RemovePlayer()
    {
        OnRemovePlayer?.Invoke(player.ActorNumber);
    }
    */
}
