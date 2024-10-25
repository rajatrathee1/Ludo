using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class onlinePlayerPiece : MonoBehaviourPun
{
    public onlinePathObjectParent onlinePathParent;

    private void Awake()
    {
        onlinePathParent = FindObjectOfType<onlinePathObjectParent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
