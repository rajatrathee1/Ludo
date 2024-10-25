// Import necessary Unity libraries and collections framework
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Defines a class for path points in a game, which are specific locations on the game's map where player pieces can land or pass
public class pathPoint : MonoBehaviour
{
    // Public reference to a parent object, which is typically a larger path or game board structure this point belongs to
    public pathObjectParent pathObjectParent1;
    // A list to keep track of all player pieces currently positioned at this path point
    public List<playerPiece> playerPieceList = new List<playerPiece>();
    // An array holding references to other path points; this dictates where a piece might move next but is not initialized here
    pathPoint[] pathPointToMoveOn_;
    // pubic diceBoard Object to access crowns
    public diceBoard diceBoard;
    // Called when the script instance is being loaded
    void Start()
    {
        // Initializes the pathObjectParent1 variable by finding and getting the parent component of this path point
        pathObjectParent1 = GetComponentInParent<pathObjectParent>();
    }
    /*
    void OnMouseDown()
    {
        if (playerPieceList.Count > 0)
        {
            foreach (playerPiece piece in playerPieceList)
            {

                if (piece.name.Contains("blue"))
                {
                    bluePlayerPiece bluePiece = piece as bluePlayerPiece;
                    if(bluePiece != null && gameManager.gm.currentPlayerColor == "blue")
                    {
                        bluePiece.moveBluePlayerPiece();
                    }
                       
                }
                else if (piece.name.Contains("red"))
                {
                    redPlayerPiece redPiece = piece as redPlayerPiece;
                    if(redPiece != null && gameManager.gm.currentPlayerColor == "red")
                    {
                        redPiece.moveRedPlayerPiece();
                    }
                    
                }
                else if (piece.name.Contains("green"))
                {
                    greenPlayerPiece greenPiece = piece as greenPlayerPiece;
                    if (greenPiece != null && gameManager.gm.currentPlayerColor == "green")
                    {
                        greenPiece.moveGreenPlayerPiece();
                    }
                }
               else if (piece.name.Contains("yellow"))
                {
                    yellowPlayerPiece yellowPiece = piece as yellowPlayerPiece;
                    if (yellowPiece != null && gameManager.gm.currentPlayerColor == "yellow")
                    {
                        yellowPiece.moveYellowPlayerPiece();
                    }
                }
                break; // Optional: break after finding the first eligible piece
                
            }
        }
    }
    */
    // Attempts to add a player piece to this path point, implementing game logic to determine if the addition is allowed
    public bool addPlayerPiece(playerPiece playerPiece_)
    {
        // Special case handling for a "commonPathPointCenter", a specific game logic condition
        if (this.name == "commonPathPointCenter")
        {
            addPlayer(playerPiece_); // Add the player to the path point
            complete(playerPiece_); // Mark the move as complete for this piece
            if(playerPiece_.name.Contains("blue") && gameManager.gm.blueCompletePlayer == 4)
            {
               return true;
            }
            else if (playerPiece_.name.Contains("red") && gameManager.gm.redCompletePlayer == 4)
            {
                return true;
            }
            else if (playerPiece_.name.Contains("green") && gameManager.gm.greenCompletePlayer == 4)
            {
                return true;
            }
            else if (playerPiece_.name.Contains("yellow") && gameManager.gm.yellowCompletePlayer == 4)
            {
                return true;
            }
            return false; // Indicate that the piece should not continue moving
        }

        // Checks if this path point is not marked as a safe point and already has one player piece on it
        if (!pathObjectParent1.safePoint.Contains(this))
        {
            
            // Logic to handle interaction between player pieces when not on a safe point
            if (playerPieceList.Count == 1)
            {
                // Compare the incoming piece with the one already there to determine if they belong to different players
                string prePlayerPieceName = playerPieceList[0].name;
                string currentPlayerPieceName = playerPiece_.name;
                // Remove the last 4 characters to normalize the name for comparison
                currentPlayerPieceName = currentPlayerPieceName.Substring(0, currentPlayerPieceName.Length - 4);

                // If the pieces belong to different players, initiate a conflict resolution
                if (!prePlayerPieceName.Contains(currentPlayerPieceName))
                {
                    // Mark the existing piece as not ready, effectively resetting its state
                    playerPieceList[0].isReady = false;
                    // Start a coroutine to move the existing piece back to its starting position
                    StartCoroutine(revertOnStart(playerPieceList[0]));
                    // Reset the number of steps the piece has moved
                    playerPieceList[0].numberOfStepsAlreadyMoved = 0;
                    // Remove the existing piece from this path point
                    removePlayerPiece(playerPieceList[0]);
                    // Add the new piece to the path point
                    playerPieceList.Add(playerPiece_);
                    return false; // Indicate the new piece's move is complete, given the conflict resolution
                }
            }
        }
        // For cases not covered by special conditions, simply add the player piece to this path point
        addPlayer(playerPiece_);
        return true; // Indicate that the piece has been successfully added without conflict
    }

    // Coroutine to move a conflicted player piece back to its start position, used in conflict resolution
    IEnumerator revertOnStart(playerPiece playerPiece_)
    {
        // Determine the color of the piece to adjust the game state accordingly
        if (playerPiece_.name.Contains("blue"))
        {
            gameManager.gm.blueOutPlayer -= 1; // Decrement the count of blue players out on the board
            pathPointToMoveOn_ = pathObjectParent1.bluePathPoint; // Set the movement path for the blue player piece
        }
        else if (playerPiece_.name.Contains("red"))
        {
            gameManager.gm.redOutPlayer -= 1; // Decrement the count of red players out on the board
            pathPointToMoveOn_ = pathObjectParent1.redPathPoint; // Set the movement path for the red player piece
        }
        else if (playerPiece_.name.Contains("green"))
        {
            gameManager.gm.greenOutPlayer -= 1; // Decrement the count of green players out on the board
            pathPointToMoveOn_ = pathObjectParent1.greenPathPoint; // Set the movement path for the green player piece
        }
        else if (playerPiece_.name.Contains("yellow")) // Default case for other colors, assumed to be yellow here
        {
            gameManager.gm.yellowOutPlayer -= 1; // Decrement the count of yellow players out on the board
            pathPointToMoveOn_ = pathObjectParent1.yellowPathPoint; // Set the movement path for the yellow player piece
        }

        // Iterate backwards through the path to move the piece step by step towards the start
        for (int i = playerPiece_.numberOfStepsAlreadyMoved - 1; i >= 0; i--)
        {
            // Update the position of the player piece to each preceding path point
            playerPiece_.transform.position = pathPointToMoveOn_[i].transform.position;
            // Wait for a short period before moving to the next step, creating a visual effect of movement
            yield return new WaitForSeconds(0.02f);
        }

        // Finally, set the player piece's position to its designated starting base point
        playerPiece_.transform.position = pathObjectParent1.basePoint[basePointPosition(playerPiece_.name)].transform.position;
    }

    // Utility method to find the index of a player piece's base (starting) point based on its name
    int basePointPosition(string name)
    {
        // Loop through the available base points to find a match by name
        for (int i = 0; i < pathObjectParent1.basePoint.Length; i++)
        {
            if (pathObjectParent1.basePoint[i].name == name)
            {
                return i; // Return the index of the matching base point
            }
        }
        return -1; // Return -1 if no match is found
    }

    // Adds a player piece to the list and updates the positioning and scaling of all pieces at this path point
    void addPlayer(playerPiece playerPiece_)
    {
        playerPieceList.Add(playerPiece_); // Add the new piece to the list
        rescaleAndRepositionAllPlayerPiece(); // Adjust the scale and position of all pieces to accommodate the new addition
    }

    // Removes a player piece from the list and updates the positioning and scaling of the remaining pieces
    public void removePlayerPiece(playerPiece playerPiece_)
    {
        if (playerPieceList.Contains(playerPiece_)) // Check if the piece is actually in the list
        {
            playerPieceList.Remove(playerPiece_); // Remove the piece from the list
            rescaleAndRepositionAllPlayerPiece(); // Re-adjust the scale and position of the remaining pieces
        }
    }

    // Placeholder method to handle the completion of a player piece's move; additional game logic might be added here
    void complete(playerPiece playerPiece_)
    {
        // Logic to adjust the count of player pieces based on their color and possibly trigger game state changes
        int totalCompletePlayer; // Variable to track the total number of pieces a player has completed
        if (playerPiece_.name.Contains("blue")) { gameManager.gm.blueOutPlayer -= 1; totalCompletePlayer = gameManager.gm.blueCompletePlayer += 1; }
        else if (playerPiece_.name.Contains("red")) { gameManager.gm.redOutPlayer -= 1; totalCompletePlayer = gameManager.gm.redCompletePlayer += 1; }
        else if (playerPiece_.name.Contains("green")) { gameManager.gm.greenOutPlayer -= 1; totalCompletePlayer = gameManager.gm.greenCompletePlayer += 1; }
        else { gameManager.gm.yellowOutPlayer -= 1; totalCompletePlayer = gameManager.gm.yellowCompletePlayer += 1; }
        playerPiece_.isPlayerComplete = true;

        // Conditional logic to handle cases where a player has completed all their pieces could be placed here
        if (totalCompletePlayer == 4)
        {
            Debug.Log("totalCompletePlayer = " + totalCompletePlayer + " hence playFireWorks is triggered ");
            gameManager.gm.playFireWorks();
            // gameManager.gm.fireworkAnimation.gameObject.SetActive(true);
            crownFirstPlayer(playerPiece_);
        }


    }

    // Adjusts the scale and position of all player pieces at this path point to prevent overlap and ensure each piece is visible
    public void rescaleAndRepositionAllPlayerPiece()
    {
        int playerCount = playerPieceList.Count; // Count of player pieces currently at this path point
        bool isOdd = (playerCount % 2) != 0; // Determine if the count is odd

        int extent = playerCount / 2; // Calculate the extent to which pieces should be spread out
        int counter = 0; // Counter for iterating through player pieces

        // Adjust positions based on whether there's an odd or even number of pieces
        if (isOdd)
        {
            for (int i = -extent; i <= extent; i++)
            {
                // Adjust the scale and position for each piece, spreading them evenly around the path point
                playerPieceList[counter].transform.localScale = new Vector3(pathObjectParent1.scales[playerCount - 1], pathObjectParent1.scales[playerCount - 1], 1f);
                playerPieceList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent1.positionDifference[playerCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }
        else
        {
            for (int i = -extent; i < extent; i++)
            {
                // Similar adjustment for an even number of pieces, ensuring even spacing
                playerPieceList[counter].transform.localScale = new Vector3(pathObjectParent1.scales[playerCount - 1], pathObjectParent1.scales[playerCount - 1], 1f);
                playerPieceList[counter].transform.position = new Vector3(transform.position.x + (i * pathObjectParent1.positionDifference[playerCount - 1]), transform.position.y, 0f);
                counter++;
            }
        }
        // Loop to adjust the sorting layer of each piece, ensuring pieces are layered correctly for visibility
        int spriteLayer = 0;
        for (int i = 0; i < playerPieceList.Count; i++)
        {
            playerPieceList[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = spriteLayer;
            spriteLayer++;
        }
    }
    public bool ifplayercanRemoveOpponent(playerPiece playerPiece_)
    {
       
        if (!pathObjectParent1.safePoint.Contains(this))
        {
           
            // Logic to handle interaction between player pieces when not on a safe point
            if (playerPieceList.Count == 1)
            {
                string prePlayerPieceName = playerPieceList[0].name;
               
                string currentPlayerPieceName = playerPiece_.name;
               
                // Remove the last 4 characters to normalize the name for comparison
                currentPlayerPieceName = currentPlayerPieceName.Substring(0, currentPlayerPieceName.Length - 4);

                // If the pieces belong to different players, initiate a conflict resolution
                if (!prePlayerPieceName.Contains(currentPlayerPieceName))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void crownFirstPlayer (playerPiece playerPiece_)
    {
        if (playerPiece_.name.Contains("blue") && gameManager.gm.blueCompletePlayer == 4)
        {
           if(gameManager.gm.redCompletePlayer != 4 && gameManager.gm.greenCompletePlayer != 4 && gameManager.gm.yellowCompletePlayer != 4)
            {
                diceBoard.crownBlueWinner();
            }
        }
        if (playerPiece_.name.Contains("red") && gameManager.gm.redCompletePlayer == 4)
        {
            if (gameManager.gm.blueCompletePlayer != 4 && gameManager.gm.greenCompletePlayer != 4 && gameManager.gm.yellowCompletePlayer != 4)
            {
                diceBoard.crownRedWinner();
            }
        }
        if (playerPiece_.name.Contains("green") && gameManager.gm.greenCompletePlayer == 4)
        {
            if (gameManager.gm.blueCompletePlayer != 4 && gameManager.gm.redCompletePlayer != 4 && gameManager.gm.yellowCompletePlayer != 4)
            {
                diceBoard.crownGreenWinner();
            }
        }
        if (playerPiece_.name.Contains("yellow") && gameManager.gm.yellowCompletePlayer == 4)
        {
            if (gameManager.gm.blueCompletePlayer != 4 && gameManager.gm.redCompletePlayer != 4 && gameManager.gm.greenCompletePlayer != 4)
            {
                diceBoard.crownYellowWinner();
            }
        }
    }
}
