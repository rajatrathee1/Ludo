// The gameManager class acts as the central hub for controlling and monitoring the overall game state and logic in a Unity-based board game.
// It utilizes a singleton pattern to ensure only one instance of the game manager exists, facilitating easy access to its functions and variables from other scripts.
// Key functionalities include:
// - Managing the dice rolling process, including handling the outcomes and determining if and when players can move based on dice rolls.
// - Keeping track of all player pieces on the board, ensuring correct movement and interactions such as collisions or reaching specific board locations.
// - Managing turn-based logic, including determining which player's turn is next and handling special cases like rolling a six or transferring the dice roll to another player.
// - Tracking the number of 'out' and 'complete' players for each color, crucial for determining game progress and potentially declaring a winner.
// - Controlling the game's audio for feedback and immersion, toggling sound effects based on gameplay events or player preferences.    
using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

// The gameManager class manages the state and logic of the game
public class gameManager : MonoBehaviour
{
    // Singleton instance of gameManager
    public static gameManager gm;

    // Number of steps to move as determined by dice roll
    public int numberOfStepsToMove;

    // Reference to the rollingDice script for dice roll functionality
    public rollingDice rollingDice;

    // Boolean to check if the player is allowed to move
    public bool canPlayerMove = true;

    // List to keep track of all players currently on a path point
    List<pathPoint> playersOnPathPoint = new List<pathPoint>();

    // Boolean to check if the dice can be rolled
    public bool canDiceRoll = true;

    // Boolean to determine if dice roll should be transferred to another player
    public bool transferDice = false;

    // List containing all rollingDice instances in the game
    public List<rollingDice> rollingDiceList;

    // Count of 'out' players for each color
    public int blueOutPlayer;
    public int redOutPlayer;
    public int greenOutPlayer;
    public int yellowOutPlayer;

    // Count of 'complete' players for each color
    public int blueCompletePlayer;
    public int redCompletePlayer;
    public int greenCompletePlayer;
    public int yellowCompletePlayer;

    // Total number of players that can play in the current turn
    public int totalPlayersCanPlay;

    // List containing all player home GameObjects
    public List<GameObject> playerHomes;

    // Total number of times a six has been rolled in the current turn
    public int totalSix;

    // Lists containing all player pieces for each color
    public List<playerPiece> bluePlayerPieces;
    public List<playerPiece> redPlayerPieces;
    public List<playerPiece> greenPlayerPieces;
    public List<playerPiece> yellowPlayerPieces;

    // AudioSource component for playing sounds
    public AudioSource ads;

    // Boolean to toggle sound on or off
    public bool sound = true;

    public Firework fireworkAnimation; // Assign this via the Inspector

    // pubic diceBoard Object to access GlowingBoards
    public diceBoard diceBoard;

    //public string for current player color
    public string currentPlayerColor;
    private void Awake()
    {
        // Assign the singleton instance to this object, or destroy it if one already exists
        gm = this;

        // Get the AudioSource component attached to this GameObject
        ads = GetComponent<AudioSource>();
    }

    // Method to add a path point to the list of players on path points
    public void addPathPoint(pathPoint pathPoint_)
    {
        playersOnPathPoint.Add(pathPoint_);
    }

    // Method to remove a path point from the list of players on path points
    public void removePathPoint(pathPoint pathPoint_)
    {
        if (playersOnPathPoint.Contains(pathPoint_))
        {
            playersOnPathPoint.Remove(pathPoint_);
        }
    }

    // Method to handle the transfer of dice rolling between players
    public void rollingDiceTransfer()
    {
        // Check if it's time to transfer the dice to another player
        if (transferDice)
        {
            // Reset the count of consecutive sixes rolled to zero
            totalSix = 0;
            // Call the transferRollingDice method to handle the actual transfer
            transferRollingDice();
        }
        else
        {
            /*
                // If not transferring, check if there is only one player that can play
                if (gameManager.gm.totalPlayersCanPlay == 1)
                {
                    // If the current rollingDice is the third one in the list
                    if (rollingDice == rollingDiceList[2])
                    {
                        // Call the role method after a delay of 0.6 seconds to simulate dice roll
                        Invoke("role", 0.6f);
                    }
                }
            */

            Invoke("role2", 0.6f);
        }

        // Set canDiceRoll to true allowing the next dice roll to occur
        canDiceRoll = true;
        // Set transferDice to false as the transfer has been completed or is not needed
        transferDice = false;
    }


    // Method called to initiate a dice roll for the current player
    void role()
    {
        // Calls the MouseRole method on the third rollingDice in the list, which handles the dice roll action
        rollingDiceList[2].MouseRole();
    }
    void role2()
    {
        // Calls the MouseRole method on the third rollingDice in the list, which handles the dice roll action
        rollingDice.MouseRole();
    }
    IEnumerator InvokeRole3WithDelay(int i)
    {
        yield return new WaitForSeconds(0.6f);
        role3(i);
    }
    void role3(int rollingDiceTurn)
    {
        // Calls the MouseRole method on the third rollingDice in the list, which handles the dice roll action
        rollingDiceList[rollingDiceTurn].MouseRole();
    }


    // Method called to handle the transfer of dice rolling between players
    void transferRollingDice()
    {
        // Check if there is only one player that can play
        if (gameManager.gm.totalPlayersCanPlay == 1)
        {
            // If the current rollingDice is the first one in the list
            if (rollingDice == rollingDiceList[0] && blueCompletePlayer < 4)
            {
                // Deactivate the first rollingDice gameObject and activate the third one
                rollingDiceList[0].gameObject.SetActive(false);
                rollingDiceList[2].gameObject.SetActive(true);
                playerTurn(2);
                // Call the role method after a delay of 0.6 seconds
                StartCoroutine(InvokeRole3WithDelay(2));
            }
            else if (rollingDice == rollingDiceList[2] && greenCompletePlayer < 4)
            {
                // If it's not the first rollingDice, then deactivate the third and activate the first one
                rollingDiceList[2].gameObject.SetActive(false);
                rollingDiceList[0].gameObject.SetActive(true);
                playerTurn(0);
                // Call the role method after a delay of 0.6 seconds
                StartCoroutine(InvokeRole3WithDelay(0));
            }
            else if (greenCompletePlayer == 4 || blueCompletePlayer == 4)
            {
                // If it's not the first rollingDice, then deactivate the third and activate the first one
                rollingDiceList[2].gameObject.SetActive(false);
                rollingDiceList[0].gameObject.SetActive(false);
            }
        }
        // Check if there are two players that can play
        else if (gameManager.gm.totalPlayersCanPlay == 2)
        {
            // Similar logic as above, toggling between first and third rollingDice based on current active one
            if (rollingDice == rollingDiceList[0] && blueCompletePlayer < 4)
            {
                rollingDiceList[0].gameObject.SetActive(false);
                rollingDiceList[2].gameObject.SetActive(true);
                playerTurn(2);
                // Call the role method after a delay of 0.6 seconds
                StartCoroutine(InvokeRole3WithDelay(2));
            }
            else if (rollingDice == rollingDiceList[2] && greenCompletePlayer < 4)
            {
                rollingDiceList[2].gameObject.SetActive(false);
                rollingDiceList[0].gameObject.SetActive(true);
                playerTurn(0);
                // Call the role method after a delay of 0.6 seconds
                StartCoroutine(InvokeRole3WithDelay(0));
            }
            else if (greenCompletePlayer == 4 || blueCompletePlayer == 4)
            {
                // If it's not the first rollingDice, then deactivate the third and activate the first one
                rollingDiceList[2].gameObject.SetActive(false);
                rollingDiceList[0].gameObject.SetActive(false);
            }

        }
        // Check if there are three players that can play
        else if (gameManager.gm.totalPlayersCanPlay == 3)
        {
            int nextDice;
            // Loop through the first three dice in the list
            for (int i = 0; i < 3; i++)
            {
                // Call paseout method which likely determines if a player is passed out and returns an index
                i = paseout(i);
                // Determine the next dice to activate based on current index
                if (i == 2) { nextDice = 0; } else { nextDice = i + 1; }

                // If current rollingDice matches, deactivate it and activate the next one in sequence
                if (rollingDice == rollingDiceList[i])
                {
                    rollingDiceList[i].gameObject.SetActive(false);
                    rollingDiceList[nextDice].gameObject.SetActive(true);
                    playerTurn(nextDice);
                    // Call the role method after a delay of 0.6 seconds
                    StartCoroutine(InvokeRole3WithDelay(nextDice));
                }
            }

        }
        // The rest of the transferRollingDice method for when there are four players
        else if (gameManager.gm.totalPlayersCanPlay == 4)
        {
            int nextDice;
            // Loop through all dice in the list since there are four players
            for (int i = 0; i < rollingDiceList.Count; i++)
            {
                // Call paseout method which likely determines if a player is passed out and returns an index
                i = paseout(i);
                // Determine the next dice to activate based on current index, wrapping around to zero at end of list
                if (i == (rollingDiceList.Count - 1)) { nextDice = 0; } else { nextDice = i + 1; }

                // If current rollingDice matches, deactivate it and activate the next one in sequence
                if (rollingDice == rollingDiceList[i])
                {
                    rollingDiceList[i].gameObject.SetActive(false);
                    rollingDiceList[nextDice].gameObject.SetActive(true);
                    playerTurn(nextDice);
                    // Call the role method after a delay of 0.6 seconds
                    StartCoroutine(InvokeRole3WithDelay(nextDice));
                }
            }
        }
    }
    // Method to determine if a player is 'out' and return the next index to be used
    int paseout(int i)
    {
        // Check if the blue player is complete and return the next index
        if (i == 0) { if (blueCompletePlayer == 4) { return i + 1; } }
        // Check if the red player is complete and return the next index
        else if (i == 1) { if (redCompletePlayer == 4) { return i + 1; } }
        // Check if the green player is complete and return the next index
        else if (i == 2) { if (greenCompletePlayer == 4) { return i + 1; } }
        // Check if the yellow player is complete and return the next index
        else if (i == 3) { if (yellowCompletePlayer == 4) { return i + 1; } }
        // If no player is out, return the current index
        return i;
    }




    public bool colourPlayerFinished(playerPiece playerPiece1_)
    {
        Debug.Log("colourPlayerFinished initiated for player Piece : " + playerPiece1_.name);
        if (playerPiece1_.name.Contains("blue") && blueCompletePlayer == 4) { return true; }
        else if (playerPiece1_.name.Contains("red") && redCompletePlayer == 4) { return true; }
        else if (playerPiece1_.name.Contains("green") && greenCompletePlayer == 4) { return true; }
        else if (playerPiece1_.name.Contains("yellow") && greenCompletePlayer == 4) { return true; }
        return false;

    }
    public void playFireWorks()
    {
        if (fireworkAnimation != null)
        {
            StartCoroutine(PlayAndStopFireworks());

        }
        else
        {
            Debug.LogError("Firework animation reference not set.");
        }
    }
    IEnumerator PlayAndStopFireworks()
    {
        fireworkAnimation.PlayFireworks();

        yield return new WaitForSeconds(3f);
        fireworkAnimation.StopFireworks();
    }
    public void playerTurn(int rollingDiceTurn)
    {
        if (rollingDiceTurn == 0)
        {
            currentPlayerColor = "blue";
            diceBoard.BlueTurnActive();
            diceBoard.RedTurnInActive();
            diceBoard.YellowTurnInActive();
            diceBoard.GreenTurnInActive();
        }
        else if (rollingDiceTurn == 1)
        {
            currentPlayerColor = "red";
            diceBoard.RedTurnActive();
            diceBoard.BlueTurnInActive();
            diceBoard.YellowTurnInActive();
            diceBoard.GreenTurnInActive();
        }
        else if (rollingDiceTurn == 2)
        {
            currentPlayerColor = "green";
            diceBoard.GreenTurnActive();
            diceBoard.BlueTurnInActive();
            diceBoard.RedTurnInActive();
            diceBoard.YellowTurnInActive();
        }
        else if (rollingDiceTurn == 3)
        {
            currentPlayerColor = "yellow";
            diceBoard.YellowTurnActive();
            diceBoard.BlueTurnInActive();
            diceBoard.RedTurnInActive();
            diceBoard.GreenTurnInActive();
        }
    }
}
