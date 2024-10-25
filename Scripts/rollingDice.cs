using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script manages the dice rolling logic, including visual and numerical representation of the roll.
public class rollingDice : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprites; // Array of dice face sprites.
    [SerializeField] SpriteRenderer numberSpriteHolder; // Displays the result of the dice roll.
    [SerializeField] SpriteRenderer rollingDiceAnimation; // Displays dice rolling animation.
    [SerializeField] int numberGot; // Stores the number obtained from the dice roll.

    Coroutine generateRandomNumberDice; // Reference to the coroutine for rolling the dice.
    int outPlayers_; // Counts the number of player pieces that have been moved out.
    int completePlayers_;
    List<playerPiece> playerPieces; // Holds references to all player pieces in the game.
    pathPoint[] currentPathPoint; // The path points that the current player can move on.
    public playerPiece currentPlayerPiece; // Reference to the current player piece that's moving.
    public Dice diceSound; // Sound effect for rolling dice.
   
    void Start()
    {
        // Initialization logic can go here.
    }

    // OnMouseDown is called when the user clicks the dice.
    public void OnMouseDown()
    {
        generateRandomNumberDice = StartCoroutine(rollDice()); // Starts the dice rolling coroutine.
    }

    // MouseRole might be an alternative or additional method to trigger dice rolling, possibly for UI buttons.
    public void MouseRole()
    {
        generateRandomNumberDice = StartCoroutine(rollDice()); // Starts the dice rolling coroutine.
    }

    // Coroutine that handles the dice roll logic.
    // Coroutine that handles the dice roll logic.
    IEnumerator rollDice()
    {
        Debug.Log("Coroutine start: rollDice");
        yield return new WaitForEndOfFrame();

        if (gameManager.gm.canDiceRoll) // Checks if it's currently possible to roll the dice.
        {
            Debug.Log("Dice roll is allowed.");
            gameManager.gm.canDiceRoll = false;
            diceSound.playSound(); // Plays the dice rolling sound.
            Debug.Log("Dice sound played.");
            numberSpriteHolder.gameObject.SetActive(false); // Hides the number sprite.
            rollingDiceAnimation.gameObject.SetActive(true); // Shows the rolling animation.
            Debug.Log("Dice animation shown.");
            yield return new WaitForSeconds(0.8f); // Wait for the animation to play.

            int maxnum = 6;
            if (gameManager.gm.totalSix == 2)
            {
                maxnum = 5;
                gameManager.gm.totalSix = 0;
                Debug.Log("Adjusted max number after two sixes rolled.");
            }
            numberGot = Random.Range(0, maxnum); // Generates a random number based on the dice logic.
            Debug.Log($"Number got before adjustment: {numberGot}");

            if (numberGot == 5)
            {
                gameManager.gm.totalSix += 1;
                Debug.Log("Rolled a six, incrementing totalSix.");
            }
            numberSpriteHolder.sprite = numberSprites[numberGot]; // Updates the sprite to show the rolled number.
            numberGot++; // Adjusts the number to match the 1-6 range instead of 0-5.
            Debug.Log($"Number got after adjustment: {numberGot}");
            gameManager.gm.numberOfStepsToMove = numberGot; // Sets the number of steps the player can move.
            Debug.Log($"Number of steps to move: {gameManager.gm.numberOfStepsToMove}");
            gameManager.gm.rollingDice = this; // Sets this dice as the current one being rolled.
            numberSpriteHolder.gameObject.SetActive(true); // Shows the number sprite.
            rollingDiceAnimation.gameObject.SetActive(false); // Hides the rolling animation.

            outPlayers(); // Prepares the player pieces based on the roll.
            Debug.Log("Checked out players.");

            if (playerCanMove()) // Checks if the player can move any piece based on the roll.
            {
                Debug.Log("Player can move.");
                if (outPlayers_ == 0 && completePlayers_ == 0)
                {
                    Debug.Log("No players out, moving first piece.");
                    readyToMove(0); // Prepares a piece to move out if none are out yet.
                }
                else
                {
                    if (gameManager.gm.totalPlayersCanPlay == 1 && gameManager.gm.numberOfStepsToMove == 6)
                    {
                        Debug.Log("Single player mode and rolled a six.");
                        if (outPlayers_ < 4)
                        {
                            bool robotmovedOut = false;
                            robotmovedOut = robotOut();
                            Debug.Log($"Robot moved out: {robotmovedOut}");
                            if (!robotmovedOut)
                            {
                                Debug.Log("Moving current player piece.");
                                currentPlayerPiece.movePlayer(currentPathPoint);
                            }
                        }
                        if (outPlayers_ == 4)
                        {
                            Debug.Log("All players out, moving current piece.");
                            currentPlayerPiece.movePlayer(currentPathPoint);
                        }
                    }
                    else
                    {
                        Debug.Log("Moving current player piece.");
                        currentPlayerPiece.movePlayer(currentPathPoint); // Moves the current player piece along its path.
                    }
                }
            }
            else
            {
                Debug.Log("Player cannot move based on roll.");
                if (gameManager.gm.numberOfStepsToMove != 6 && outPlayers_ == 0)
                {
                    yield return new WaitForSeconds(0.5f);
                    gameManager.gm.transferDice = true; // Transfers the turn if no move is possible.
                    Debug.Log("Transferring dice, no moves possible.");
                    gameManager.gm.rollingDiceTransfer(); // Transfers the dice to the next player.
                }
                else if (outPlayers_ > 0)
                {  // int outToNotReadyDiff = 0;
                    int countNotReadyOrCannotMove = 0;
                    for (int i = 0; i < playerPieces.Count; i++)
                    {
                        // Increment count if a piece is not ready or cannot move based on the dice roll
                        if (!playerPieces[i].isReady || !playerPieces[i].isPathPointAvailableToMove(gameManager.gm.numberOfStepsToMove, playerPieces[i].numberOfStepsAlreadyMoved, currentPathPoint))
                        {
                            countNotReadyOrCannotMove += 1;
                        }
                    }
                    Debug.Log("Count of players not ready to move " + countNotReadyOrCannotMove);
                    
                   // outToNotReadyDiff = outPlayers_ - countNotReadyOrCannotMove;
                    // If all pieces are either not ready or cannot move, then return false
                    if (countNotReadyOrCannotMove == playerPieces.Count && gameManager.gm.numberOfStepsToMove != 6)
                    {
                        Debug.Log("playerPieces.Count " + playerPieces.Count + "gameManager.gm.numberOfStepsToMove =" + gameManager.gm.numberOfStepsToMove);
                        Debug.Log("all pieces are either not ready or cannot move but steps not equal to six hence rolling dice transfer");

                        yield return new WaitForSeconds(0.5f);
                        gameManager.gm.transferDice = true; // Transfers the turn if no move is possible.
                        gameManager.gm.rollingDiceTransfer(); // Transfers the dice to the next player.
                        countNotReadyOrCannotMove = 0;
                    }
                    /*
                   else if (countNotReadyOrCannotMove != playerPieces.Count && outToNotReadyDiff == 1  )
                    {
                        for (int i = 0; i < playerPieces.Count; i++)
                        {
                            // Increment count if a piece is not ready or cannot move based on the dice roll
                            if (playerPieces[i].isReady && playerPieces[i].isPathPointAvailableToMove(gameManager.gm.numberOfStepsToMove, playerPieces[i].numberOfStepsAlreadyMoved, currentPathPoint))
                            {
                                Debug.Log("Moving current player piece.");
                                currentPlayerPiece.movePlayer(currentPathPoint);
                            }
                        }

                    }
                    */
                   
                    else if (countNotReadyOrCannotMove == playerPieces.Count && gameManager.gm.numberOfStepsToMove == 6)
                    {
                        if(outPlayers_ == 4)
                        {
                            Debug.Log("playerPieces.Count " + playerPieces.Count + "gameManager.gm.numberOfStepsToMove =" + gameManager.gm.numberOfStepsToMove);
                            Debug.Log("all pieces are either not ready or cannot move hence roll dice for the same player");
                            gameManager.gm.canDiceRoll = true;
                            gameManager.gm.transferDice = false;
                            gameManager.gm.rollingDiceTransfer();
                            // rollDiceAgainForTheSamePlayer();
                            Debug.Log(" rollDiceAgainForTheSamePlayer initiated");
                            countNotReadyOrCannotMove = 0;
                        }
                        else if (outPlayers_ < 4)
                        {
                            bool robotmovedOut = false;
                            robotmovedOut = robotOut();
                            Debug.Log($"Robot moved out: {robotmovedOut}");
                            if (!robotmovedOut)
                            {
                                Debug.Log("Moving current player piece.");
                                currentPlayerPiece.movePlayer(currentPathPoint);
                            }
                        }

                    }

                }
            }

            if (generateRandomNumberDice != null)
            {
                StopCoroutine(rollDice()); // Stops the rolling coroutine if it's still running.
            }
        }
    }

    // Determines which player's pieces are currently in play and sets up their potential movement paths.

    public void outPlayers()
    {
        
            if (gameManager.gm.rollingDice == gameManager.gm.rollingDiceList[0])
            // Logic to select the correct set of player pieces and their paths based on the current dice.
            {
                playerPieces = gameManager.gm.bluePlayerPieces;
                currentPathPoint = playerPieces[0].pathParent.bluePathPoint;
                outPlayers_ = gameManager.gm.blueOutPlayer;
                completePlayers_ = gameManager.gm.blueCompletePlayer;
               
             }
            else if (gameManager.gm.rollingDice == gameManager.gm.rollingDiceList[1])
            {
                playerPieces = gameManager.gm.redPlayerPieces;
                currentPathPoint = playerPieces[0].pathParent.redPathPoint;
                outPlayers_ = gameManager.gm.redOutPlayer;
                completePlayers_ = gameManager.gm.redCompletePlayer;
               
            }
            else if (gameManager.gm.rollingDice == gameManager.gm.rollingDiceList[2])
            {
                playerPieces = gameManager.gm.greenPlayerPieces;
                currentPathPoint = playerPieces[0].pathParent.greenPathPoint;
                outPlayers_ = gameManager.gm.greenOutPlayer;
                completePlayers_ = gameManager.gm.greenCompletePlayer;
                
            }
            else if (gameManager.gm.rollingDice == gameManager.gm.rollingDiceList[3])
            {
                playerPieces = gameManager.gm.yellowPlayerPieces;
                currentPathPoint = playerPieces[0].pathParent.yellowPathPoint;
                outPlayers_ = gameManager.gm.yellowOutPlayer;
                completePlayers_ = gameManager.gm.yellowCompletePlayer;
               
            }
        
    }
    public bool playerCanMove()
    // Checks if the player can move any of their pieces based on the current game state and dice
    {

        if (gameManager.gm.totalPlayersCanPlay == 1)
        {

            if (gameManager.gm.rollingDice == gameManager.gm.rollingDiceList[2])
            {

                if (outPlayers_ > 0)
                {

                    playerPiece pieceToRemoveOpponent = null;

                    foreach (var piece in playerPieces)
                    {

                        if (piece.isReady)
                        {
                            int stepsAlreadyMovedOnCommonPath = piece.numberOfStepsAlreadyMoved; // Assuming this counts steps on the common path

                            int targetIndex = stepsAlreadyMovedOnCommonPath + gameManager.gm.numberOfStepsToMove - 1;


                            if (targetIndex < 52)
                            {
                                pathPoint targetPathPoint = currentPathPoint[targetIndex];


                                if (targetPathPoint.ifplayercanRemoveOpponent(piece))
                                {

                                    pieceToRemoveOpponent = piece;
                                    break; // Found a piece that can remove an opponent
                                }
                            }
                        }
                    }

                    if (pieceToRemoveOpponent != null)
                    {
                        // Priority is given to removing an opponent.
                        currentPlayerPiece = pieceToRemoveOpponent;

                        return true;
                    }
                    else if (pieceToRemoveOpponent == null)
                    {

                        for (int i = 0; i < playerPieces.Count; i++)
                        {
                            if (playerPieces[i].isReady)
                            {
                                if (playerPieces[i].isPathPointAvailableToMove(gameManager.gm.numberOfStepsToMove, playerPieces[i].numberOfStepsAlreadyMoved, currentPathPoint))
                                {
                                    currentPlayerPiece = playerPieces[i];

                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (outPlayers_ == 1 && gameManager.gm.numberOfStepsToMove != 6)
        {
            for (int i = 0; i < playerPieces.Count; i++)
            {
                if (playerPieces[i].isReady)
                {
                    if (playerPieces[i].isPathPointAvailableToMove(gameManager.gm.numberOfStepsToMove, playerPieces[i].numberOfStepsAlreadyMoved, currentPathPoint))
                    {
                        currentPlayerPiece = playerPieces[i];
                        return true;
                    }
                }
            }
        }
        else if (outPlayers_ == 0 && completePlayers_ < 4 && gameManager.gm.numberOfStepsToMove == 6)
        {
            return true;
        }
        else if (outPlayers_ > 0 && gameManager.gm.numberOfStepsToMove != 6)
        {
            int countNotReadyOrCannotMove1 = 0; // Use a more descriptive variable name
            for (int i = 0; i < playerPieces.Count; i++)
            {
                // Increment count if a piece is not ready or cannot move based on the dice roll
                if (!playerPieces[i].isReady || !playerPieces[i].isPathPointAvailableToMove(gameManager.gm.numberOfStepsToMove, playerPieces[i].numberOfStepsAlreadyMoved, currentPathPoint))
                {
                    countNotReadyOrCannotMove1 += 1;
                }
            }
            // If all pieces are either not ready or cannot move, then return false
            if (countNotReadyOrCannotMove1 == playerPieces.Count)
            {
                return false;
            }

        }

        return false;
    }
    void readyToMove(int pos)
    {
        if (gameManager.gm.rollingDice == gameManager.gm.rollingDiceList[0])
        {
            gameManager.gm.blueOutPlayer += 1;
        }
        else if (gameManager.gm.rollingDice == gameManager.gm.rollingDiceList[1])
        {
            gameManager.gm.redOutPlayer += 1;
        }
        else if (gameManager.gm.rollingDice == gameManager.gm.rollingDiceList[2])
        {
            gameManager.gm.greenOutPlayer += 1;
        }
        else if (gameManager.gm.rollingDice == gameManager.gm.rollingDiceList[3])
        {
            gameManager.gm.yellowOutPlayer += 1;
        }
        playerPieces[pos].makePlayerReadyToMove(currentPathPoint);
    }
    bool robotOut()
    {
        // Iterate over all player pieces.
        for (int i = 0; i < playerPieces.Count; i++)
        {
            Debug.Log(" Robot Out Initiated : ");
            // Check if the current piece is not ready to move and has not started the game (currentPathPoint is null).
            if (!playerPieces[i].isReady && playerPieces[i].isPathPointAvailableToMove(gameManager.gm.numberOfStepsToMove, playerPieces[i].numberOfStepsAlreadyMoved, currentPathPoint))
            {
                Debug.Log(" Player Piece Number of Steps Already Moved : "+ playerPieces[i].numberOfStepsAlreadyMoved);
                // Make the piece ready to move by calling the 'readyToMove' function and passing the index of the piece.
                readyToMove(i);
                // Return true indicating that a robot piece was successfully moved out.
                return true;
            }
        }
        // Return false if no robot pieces were ready to be moved out.
        return false;
    }



    IEnumerator  rollDiceAgainForTheSamePlayer()
    {
        yield return new WaitForSeconds(0.8f);
        gameManager.gm.canDiceRoll = true;
        gameManager.gm.transferDice = false;
        MouseRole();
       // OnMouseDown(); // This simulates the player clicking the dice for another roll.

    }

}
