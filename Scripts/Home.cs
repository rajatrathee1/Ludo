using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class manages UI transitions in a game's home screen, controlling the visibility of main and game panels.
public class Home : MonoBehaviour
{
    // References to the UI panels. The mainPanel is typically the home or start screen, 
    // and the GamePanel is the primary interface or dashboard for the game.
    public GameObject mainPanel;
    public GameObject GamePanel;

    // OnMouseDown is a built-in Unity method that detects mouse clicks on the object this script is attached to.
    public void OnMouseDown()
    {
        // When the home object is clicked, the main panel becomes active, 
        // making it visible if it was previously hidden.
        mainPanel.SetActive(true);

        // Simultaneously, the game panel is deactivated, or hidden, 
        // which could be used to transition from a gameplay view back to the main menu.
        GamePanel.SetActive(false);
    }
}
