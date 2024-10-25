// This script serves as the central configuration point for defining the pathways and rules of a board game within Unity. 
// It differentiates between common paths used by all players and individual paths for each player, accommodating games 
// with complex board designs. The class also manages scaling and positioning for path points to ensure visual clarity 
// and gameplay fairness, identifies safe zones where player pieces are protected from being captured, and establishes base 
// points representing the starting or finishing positions for the pieces. This design allows for a flexible game board 
// setup, enabling the creation of games with varying levels of complexity and strategic depth.

// Required namespaces for collections and Unity functionality
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for the path object parent, inheriting from pathPoint
public class pathObjectParent : pathPoint
{
    // Array of common path points shared by all players
    public pathPoint[] commonPathPoint;
    // Array of path points specific to the red player
    public pathPoint[] redPathPoint;
    // Array of path points specific to the green player
    public pathPoint[] greenPathPoint;
    // Array of path points specific to the yellow player
    public pathPoint[] yellowPathPoint;
    // Array of path points specific to the blue player
    public pathPoint[] bluePathPoint;

    [Header("Scale and positioning Difference")]
    // Array for scaling differences between the path points
    public float[] scales;
    // Array for positional differences between the path points
    public float[] positionDifference;
    // Array of base points for player paths
    public pathPoint[] basePoint;
    // List of safe points on the board where players cannot be captured
    public List<pathPoint> safePoint;

    // Start is called before the first frame update
  

    void Start()
    {
        // Initialization code can be added here if needed
    }
    void OnMouseDown()
    {
        if (playerPieceList.Count > 0)
        {
            foreach (playerPiece piece in playerPieceList)
            {

                if (piece.name.Contains("blue"))
                {
                    bluePlayerPiece bluePiece = piece as bluePlayerPiece;
                    if (bluePiece != null && gameManager.gm.currentPlayerColor == "blue")
                    {
                        bluePiece.moveBluePlayerPiece();
                    }

                }
                else if (piece.name.Contains("red"))
                {
                    redPlayerPiece redPiece = piece as redPlayerPiece;
                    if (redPiece != null && gameManager.gm.currentPlayerColor == "red")
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
    // Update is called once per frame
    void Update()
    {
        // Code to update each frame can be added here if needed
    }
  
}
