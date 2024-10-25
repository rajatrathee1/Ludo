using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class manages the behaviors and actions of the red player's pieces, based on the playerPiece class.
public class redPlayerPiece : playerPiece
{
    // Holds a reference to the rollingDice component specific to the red player's home area.
    rollingDice redHomeRollingDice;

    // Upon instantiation, this method retrieves the rollingDice component from the parent redHome object.
    void Start()
    {
        // Finds the rollingDice component that's associated with the red home area to handle dice roll outcomes.
        redHomeRollingDice = GetComponentInParent<redHome>().rollingDice;
    }

    // This method is triggered when the user clicks on the red player piece.
    public void OnMouseDown()
    {
        // Checks if a dice roll is currently active.
        if (gameManager.gm.rollingDice != null)
        {
            // Determines if the piece is not yet in play and if the current dice roll allows it to enter the board.
            if (!isReady)
            {
                // Checks if the dice roll belongs to the red player, a six was rolled (allowing a piece to start), and the player is allowed to move.
                if (gameManager.gm.rollingDice == redHomeRollingDice && gameManager.gm.numberOfStepsToMove == 6 && gameManager.gm.canPlayerMove)
                {
                    // Updates the count of red player pieces that are active on the board.
                    gameManager.gm.redOutPlayer += 1;
                    // Sets this piece to be ready for movement and positions it at the start of its designated path.
                    makePlayerReadyToMove(pathParent.redPathPoint);
                    return; // Exits the method to avoid further actions, as the piece's state has been updated.
                }
            }
            // If the piece is ready to move (in play), checks if it's the correct turn based on the dice roll and if movement is allowed.
            if (gameManager.gm.rollingDice == redHomeRollingDice && isReady && gameManager.gm.canPlayerMove)
            {
                // Initiates the movement of this piece along its path based on the roll outcome.
                movePlayer(pathParent.redPathPoint);
            }
        }
    }
    public void moveRedPlayerPiece()
    {
        // Checks if a dice roll is currently active.
        if (gameManager.gm.rollingDice != null)
        {
            // Determines if the piece is not yet in play and if the current dice roll allows it to enter the board.
            if (!isReady)
            {
                // Checks if the dice roll belongs to the red player, a six was rolled (allowing a piece to start), and the player is allowed to move.
                if (gameManager.gm.rollingDice == redHomeRollingDice && gameManager.gm.numberOfStepsToMove == 6 && gameManager.gm.canPlayerMove)
                {
                    // Updates the count of red player pieces that are active on the board.
                    gameManager.gm.redOutPlayer += 1;
                    // Sets this piece to be ready for movement and positions it at the start of its designated path.
                    makePlayerReadyToMove(pathParent.redPathPoint);
                    return; // Exits the method to avoid further actions, as the piece's state has been updated.
                }
            }
            // If the piece is ready to move (in play), checks if it's the correct turn based on the dice roll and if movement is allowed.
            if (gameManager.gm.rollingDice == redHomeRollingDice && isReady && gameManager.gm.canPlayerMove)
            {
                // Initiates the movement of this piece along its path based on the roll outcome.
                movePlayer(pathParent.redPathPoint);
            }
        }
    }
}
