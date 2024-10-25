using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class defines behavior for yellow player pieces, extending the generic playerPiece class to include logic specific to the yellow team.
public class yellowPlayerPiece : playerPiece
{
    // Holds a reference to the dice rolling mechanism specific to the yellow player's home area.
    rollingDice yellowHomeRollingDice;

    // Upon initialization, this fetches the associated rollingDice component from the yellow home area.
    void Start()
    {
        // Acquires the rollingDice component that's specific to the yellow home, ensuring this piece reacts correctly to dice rolls.
        yellowHomeRollingDice = GetComponentInParent<yellowHome>().rollingDice;
    }

    // Responds to mouse clicks on this game object, triggering potential movement or other actions.
    public void OnMouseDown()
    {
        // Checks if a dice roll has been made, implying the game is in a state where piece movement might be possible.
        if (gameManager.gm.rollingDice != null)
        {
            // Conditions to check before making the piece ready to move: it's not already on the board, it's the yellow dice's turn, a 6 was rolled, and movement is allowed.
            if (!isReady)
            {
                if (gameManager.gm.rollingDice == yellowHomeRollingDice && gameManager.gm.numberOfStepsToMove == 6 && gameManager.gm.canPlayerMove)
                {
                    // Updates the game state to reflect an additional yellow player piece is moving out of home.
                    gameManager.gm.yellowOutPlayer += 1;
                    // Sets this piece as ready to move, placing it at the start of its path on the board.
                    makePlayerReadyToMove(pathParent.yellowPathPoint);
                    // Ends execution for this click event after handling the piece's readiness.
                    return;
                }
            }
            // If the piece is ready to move (on the board), it's the correct turn, and movement is permitted, initiate movement along its designated path.
            if (gameManager.gm.rollingDice == yellowHomeRollingDice && isReady && gameManager.gm.canPlayerMove)
            {
                // Executes the movement logic defined in playerPiece, moving this piece along the yellow path.
                movePlayer(pathParent.yellowPathPoint);
            }
        }
    }
    public void moveYellowPlayerPiece()
    {
        // Checks if a dice roll has been made, implying the game is in a state where piece movement might be possible.
        if (gameManager.gm.rollingDice != null)
        {
            // Conditions to check before making the piece ready to move: it's not already on the board, it's the yellow dice's turn, a 6 was rolled, and movement is allowed.
            if (!isReady)
            {
                if (gameManager.gm.rollingDice == yellowHomeRollingDice && gameManager.gm.numberOfStepsToMove == 6 && gameManager.gm.canPlayerMove)
                {
                    // Updates the game state to reflect an additional yellow player piece is moving out of home.
                    gameManager.gm.yellowOutPlayer += 1;
                    // Sets this piece as ready to move, placing it at the start of its path on the board.
                    makePlayerReadyToMove(pathParent.yellowPathPoint);
                    // Ends execution for this click event after handling the piece's readiness.
                    return;
                }
            }
            // If the piece is ready to move (on the board), it's the correct turn, and movement is permitted, initiate movement along its designated path.
            if (gameManager.gm.rollingDice == yellowHomeRollingDice && isReady && gameManager.gm.canPlayerMove)
            {
                // Executes the movement logic defined in playerPiece, moving this piece along the yellow path.
                movePlayer(pathParent.yellowPathPoint);
            }
        }
    }
}
