using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class defines the behaviors specific to the green player's pieces in the game, handling how these pieces are moved and interact based on dice rolls.
public class greenPlayerPiece : playerPiece
{
    // Holds a reference to the rollingDice component associated with the green home area, allowing this piece to respond to dice rolls.
    rollingDice greenHomeRollingDice;

    // Upon initialization, this method retrieves the rollingDice component from the parent greenHome object, linking the piece with the dice logic.
    void Start()
    {
        // Retrieves the rollingDice component specific to the green player's home, enabling interaction with dice rolls.
        greenHomeRollingDice = GetComponentInParent<greenHome>().rollingDice;
    }

    // This method is invoked when the player clicks on this piece, initiating movement or other actions based on the game's rules.
    public void OnMouseDown()
    {
        // Checks if a dice roll is in progress or completed to determine if the piece can move.
        if (gameManager.gm.rollingDice != null)
        {
            // Evaluates if the piece is not yet active on the board (isReady flag is false).
            if (!isReady)
            {
                // Verifies that the dice roll belongs to the green player, a six was rolled, allowing pieces to enter the board, and it's currently possible to move a piece.
                if (gameManager.gm.rollingDice == greenHomeRollingDice && gameManager.gm.numberOfStepsToMove == 6 && gameManager.gm.canPlayerMove)
                {
                    // Increments the count of green players' pieces on the board, indicating one more piece is active.
                    gameManager.gm.greenOutPlayer += 1;
                    // Makes this piece ready to move by setting its initial path based on the green player's path configuration.
                    makePlayerReadyToMove(pathParent.greenPathPoint);
                    // No further action needed; exits the method early.
                    return;
                }
            }
            // Checks if the piece is active (ready) and it's the correct turn for this piece to move according to the dice roll.
            if (gameManager.gm.rollingDice == greenHomeRollingDice && isReady && gameManager.gm.canPlayerMove)
            {
                // Initiates the movement of this piece along its designated path, adhering to the rules defined for green player movements.
                movePlayer(pathParent.greenPathPoint);
            }
        }
    }

    public void moveGreenPlayerPiece()
    {
        // Checks if a dice roll is in progress or completed to determine if the piece can move.
        if (gameManager.gm.rollingDice != null)
        {
            // Evaluates if the piece is not yet active on the board (isReady flag is false).
            if (!isReady)
            {
                // Verifies that the dice roll belongs to the green player, a six was rolled, allowing pieces to enter the board, and it's currently possible to move a piece.
                if (gameManager.gm.rollingDice == greenHomeRollingDice && gameManager.gm.numberOfStepsToMove == 6 && gameManager.gm.canPlayerMove)
                {
                    // Increments the count of green players' pieces on the board, indicating one more piece is active.
                    gameManager.gm.greenOutPlayer += 1;
                    // Makes this piece ready to move by setting its initial path based on the green player's path configuration.
                    makePlayerReadyToMove(pathParent.greenPathPoint);
                    // No further action needed; exits the method early.
                    return;
                }
            }
            // Checks if the piece is active (ready) and it's the correct turn for this piece to move according to the dice roll.
            if (gameManager.gm.rollingDice == greenHomeRollingDice && isReady && gameManager.gm.canPlayerMove)
            {
                // Initiates the movement of this piece along its designated path, adhering to the rules defined for green player movements.
                movePlayer(pathParent.greenPathPoint);
            }
        }

    }
}
