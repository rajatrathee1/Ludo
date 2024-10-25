using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This class manages the behaviors and actions of the red player's pieces, based on the playerPiece class.
public class bluePlayerPiece : playerPiece
{
    // Holds a reference to the rollingDice component specific to the red player's home area.
    rollingDice blueHomeRollingDice;
    // Upon instantiation, this method retrieves the rollingDice component from the parent redHome object.
    void Start()
    {
        // Finds the rollingDice component that's associated with the blue home area to handle dice roll outcomes.
        blueHomeRollingDice = GetComponentInParent<blueHome>().rollingDice;
    }
    // This method is triggered when the user clicks on the blue player piece.
    public void OnMouseDown()
    {
        // Checks if a dice roll is currently active.
        if (gameManager.gm.rollingDice != null)
        {
            // Determines if the piece is not yet in play and if the current dice roll allows it to enter the board.

            if (!isReady)
            {
                // Checks if the dice roll belongs to the blue player, a six was rolled (allowing a piece to start), and the player is allowed to move.
                if (gameManager.gm.rollingDice == blueHomeRollingDice && gameManager.gm.numberOfStepsToMove == 6 && gameManager.gm.canPlayerMove)
                {
                    // Updates the count of blue player pieces that are active on the board.
                    gameManager.gm.blueOutPlayer += 1;
                    // Sets this piece to be ready for movement and positions it at the start of its designated path.
                    makePlayerReadyToMove(pathParent.bluePathPoint);
                    gameManager.gm.numberOfStepsToMove = 0;
                    return;// Exits the method to avoid further actions, as the piece's state has been updated.
                }

            }
            // If the piece is ready to move (in play), checks if it's the correct turn based on the dice roll and if movement is allowed.
            if (gameManager.gm.rollingDice == blueHomeRollingDice && isReady && gameManager.gm.canPlayerMove)
            {
                 // Initiates the movement of this piece along its path based on the roll outcome.
                gameManager.gm.canPlayerMove = false;
                movePlayer(pathParent.bluePathPoint);
            }
        }
    }
    public void moveBluePlayerPiece()
    {
        // Checks if a dice roll is currently active.
        if (gameManager.gm.rollingDice != null)
        {
            // Determines if the piece is not yet in play and if the current dice roll allows it to enter the board.

            if (!isReady)
            {
                // Checks if the dice roll belongs to the blue player, a six was rolled (allowing a piece to start), and the player is allowed to move.
                if (gameManager.gm.rollingDice == blueHomeRollingDice && gameManager.gm.numberOfStepsToMove == 6 && gameManager.gm.canPlayerMove)
                {
                    // Updates the count of blue player pieces that are active on the board.
                    gameManager.gm.blueOutPlayer += 1;
                    // Sets this piece to be ready for movement and positions it at the start of its designated path.
                    makePlayerReadyToMove(pathParent.bluePathPoint);
                    gameManager.gm.numberOfStepsToMove = 0;
                    return;// Exits the method to avoid further actions, as the piece's state has been updated.
                }

            }
            // If the piece is ready to move (in play), checks if it's the correct turn based on the dice roll and if movement is allowed.
            if (gameManager.gm.rollingDice == blueHomeRollingDice && isReady && gameManager.gm.canPlayerMove)
            {
                // Initiates the movement of this piece along its path based on the roll outcome.
                gameManager.gm.canPlayerMove = false;
                movePlayer(pathParent.bluePathPoint);
            }
        }
    }

}



