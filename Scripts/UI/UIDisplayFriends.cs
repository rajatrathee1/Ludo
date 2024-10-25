using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class UIDisplayFriends : MonoBehaviour
{
    [SerializeField] private Transform friendContainer;
    [SerializeField] private UIFriend uiFriendPrefab;
   private void Awake()
   {
   // PhotonFriendController.OnDisplayFriends += HandleDisplayFriends;
   }
   private void OnDestroy ()
   {
  //  PhotonFriendController.OnDisplayFriends -= HandleDisplayFriends;
   }
    private void ClearFriendContainer() {
    foreach (Transform child in friendContainer) {
        Destroy(child.gameObject);  // Ensure you're destroying the gameObject, not just the Transform component.
    }
}
  private void HandleDisplayFriends(List<FriendInfo> friends) {
    Debug.Log("Clearing friends list UI");
    ClearFriendContainer();
    Debug.Log($"Adding {friends.Count} friends to UI");
    foreach (FriendInfo friend in friends) {
        UIFriend uifriend = Instantiate(uiFriendPrefab, friendContainer);
        uifriend.Initialize(friend);
        Debug.Log($"Added friend: {friend.UserId}");
    }
}

}
