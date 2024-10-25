using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The UIManager class manages user interface interactions and transitions between different UI panels in the game.
// It provides methods to start different game configurations based on the number of players selected.
// These configurations are directly linked to the game's logic managed by the gameManager class,
// setting up the game for 1 to 4 players and adjusting the visibility of player homes and UI panels accordingly.
public class UIManager : MonoBehaviour
{
    // Reference to the main menu panel UI GameObject.
    public GameObject mainPanel;
    // Reference to the game panel UI GameObject where the game is displayed.
    public GameObject gamePanel;

    // Sets up the game for 2 players.
    public void Game1()
    {
        gameManager.gm.totalPlayersCanPlay = 2; // Sets the number of players that can play to 2 in the gameManager.
        mainPanel.SetActive(false); // Hides the main menu panel.
        gamePanel.SetActive(true); // Shows the game panel.
        gameManager.gm.playerHomes[1].SetActive(false); // Disables the second player's home, assuming player indexing starts at 0.
        gameManager.gm.playerHomes[3].SetActive(false); // Disables the fourth player's home.
    }

    // Sets up the game for 3 players.
    public void Game2()
    {
        gameManager.gm.totalPlayersCanPlay = 3; // Sets the number of players that can play to 3 in the gameManager.
        mainPanel.SetActive(false); // Hides the main menu panel.
        gamePanel.SetActive(true); // Shows the game panel.
        gameManager.gm.playerHomes[3].SetActive(false); // Disables the fourth player's home.
    }

    // Sets up the game for 4 players.
    public void Game3()
    {
        gameManager.gm.totalPlayersCanPlay = 4; // Sets the number of players that can play to 4 in the gameManager.
        mainPanel.SetActive(false); // Hides the main menu panel.
        gamePanel.SetActive(true); // Shows the game panel.
        // All player homes are active by default for 4 players, so no homes are disabled here.
    }

    // Sets up the game for a single player.
    public void Game4()
    {
        gameManager.gm.totalPlayersCanPlay = 1; // Sets the number of players that can play to 1 in the gameManager.
        mainPanel.SetActive(false); // Hides the main menu panel.
        gamePanel.SetActive(true); // Shows the game panel.
        gameManager.gm.playerHomes[1].SetActive(false); // Disables the second player's home, assuming player indexing starts at 0.
        gameManager.gm.playerHomes[3].SetActive(false); // Disables the fourth player's home.
    }
}
