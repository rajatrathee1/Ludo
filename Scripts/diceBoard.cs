using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diceBoard : MonoBehaviour
{
    // References to the UI panels. The mainPanel is typically the home or start screen, 
    // and the GamePanel is the primary interface or dashboard for the game.
    public GameObject blueCrown;
    public GameObject redCrown;
    public GameObject yellowCrown;
    public GameObject greenCrown;
    public GameObject blueTurn;
    public GameObject redTurn;
    public GameObject yellowTurn;
    public GameObject greenTurn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void crownBlueWinner(){ blueCrown.SetActive(true);}
    public void crownRedWinner() {  redCrown.SetActive(true);}
    public void crownYellowWinner() {  yellowCrown.SetActive(true);}
    public void crownGreenWinner() {  greenCrown.SetActive(true);}
    //Functions to handle glowing backgrounds based on turn
    //Functions to activate glowing backgrunds
    public void BlueTurnActive() { blueTurn.SetActive(true); }
    public void RedTurnActive() { redTurn.SetActive(true); }
    public void YellowTurnActive() { yellowTurn.SetActive(true); }
    public void GreenTurnActive() { greenTurn.SetActive(true); }

    // Functions to deactivate glowing backGrounds
    public void BlueTurnInActive() { blueTurn.SetActive(false); }
    public void RedTurnInActive() { redTurn.SetActive(false); }
    public void YellowTurnInActive() { yellowTurn.SetActive(false); }
    public void GreenTurnInActive() { greenTurn.SetActive(false); }
}
