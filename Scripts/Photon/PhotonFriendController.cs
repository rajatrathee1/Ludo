using Photon.Pun;
using Photon.Realtime;
//using PlayfabFriendInfo = PlayFab.ClientModels.FriendInfo;
//using PhotonFriendInfo = Photon.Realtime.FriendInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PhotonFriendController : MonoBehaviourPunCallbacks
{
    /*
    public static Action<List <PhotonFriendInfo>> OnDisplayFriends = delegate { };
    private void Awake()
    {
        PlayFabFriendController.OnFriendListUpdated += HandleFriendsUpdated;
    }
    private void OnDestroy()
    {
        PlayFabFriendController.OnFriendListUpdated -= HandleFriendsUpdated;
    }

    private void HandleFriendsUpdated(List<PlayfabFriendInfo> friends)
    {
        if (friends.Count > 0)
        {
            if (friends.Count != 0)
            {
                string[] friendDisplayNames = friends.Select(f => f.TitleDisplayName).ToArray();
                PhotonNetwork.FindFriends(friendDisplayNames);
            }
        }
    }

    public override void OnFriendListUpdate(List<PhotonFriendInfo> friendList)
    {
        OnDisplayFriends?.Invoke(friendList);
    }
    */
}
