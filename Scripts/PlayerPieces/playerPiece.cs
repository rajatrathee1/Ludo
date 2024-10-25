using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPiece : MonoBehaviour
{
    public bool moveNow;
    public bool isReady;
    public int numberOfStepsToMove;
    public int numberOfStepsAlreadyMoved;
    public pathObjectParent pathParent;
    Coroutine playerMovement;

    public pathPoint previousPathPoint;
    public pathPoint currentPathPoint;

    public bool isPlayerComplete = false;
    private bool isMoving = false; // Flag to track if a piece is currently moving.
    private void Awake()
    {
        pathParent = FindObjectOfType<pathObjectParent>();
    }
    void Start()
    {

    }
    public void makePlayerReadyToMove(pathPoint[] pathParent_)
    {
        isReady = true;
        transform.position = pathParent_[0].transform.position;
        numberOfStepsAlreadyMoved = 1;
        gameManager.gm.numberOfStepsToMove = 0;
        previousPathPoint = pathParent_[0];
        currentPathPoint = pathParent_[0];
      
        currentPathPoint.addPlayerPiece(this);
        gameManager.gm.addPathPoint(currentPathPoint);
        gameManager.gm.rollingDiceTransfer();
    }
    public void movePlayer(pathPoint[] pathParent_)
    {
        if (!isMoving) // Only start a new move if a move is not already in progress.
        {
            isMoving = true; // Set the flag to indicate that a move is in progress.
            playerMovement = StartCoroutine(MoveStep_enm(pathParent_));
        }
    }
    IEnumerator MoveStep_enm(pathPoint[] pathParent_)
    {
        yield return new WaitForSeconds(0.25f);
        numberOfStepsToMove = gameManager.gm.numberOfStepsToMove;
        for (int i = numberOfStepsAlreadyMoved; i < (numberOfStepsAlreadyMoved + numberOfStepsToMove); i++)
        {
            if (isPathPointAvailableToMove(numberOfStepsToMove, numberOfStepsAlreadyMoved, pathParent_))
            {
                transform.position = pathParent_[i].transform.position;
                if (gameManager.gm.sound) { gameManager.gm.ads.Play(); }
                yield return new WaitForSeconds(0.35f);
            }

        }
        if (isPathPointAvailableToMove(numberOfStepsToMove, numberOfStepsAlreadyMoved, pathParent_))
        {
            numberOfStepsAlreadyMoved += numberOfStepsToMove;
            // gameManager.gm.numberOfStepsToMove = 0;

            gameManager.gm.removePathPoint(previousPathPoint);
            previousPathPoint.removePlayerPiece(this);
            currentPathPoint = pathParent_[numberOfStepsAlreadyMoved - 1];
            bool transfer = currentPathPoint.addPlayerPiece(this);
            currentPathPoint.rescaleAndRepositionAllPlayerPiece();
            gameManager.gm.addPathPoint(currentPathPoint);

            previousPathPoint = currentPathPoint;

            if (transfer && gameManager.gm.numberOfStepsToMove != 6)
            {
                gameManager.gm.transferDice = true;
            }
            gameManager.gm.numberOfStepsToMove = 0;
        }
        gameManager.gm.canPlayerMove = true;
        gameManager.gm.rollingDiceTransfer();


        if (playerMovement != null)
        {
            StopCoroutine("MoveStep_enm");
        }
        isMoving = false; // Clear the move-in-progress flag.
    }

    public bool isPathPointAvailableToMove(int numberOfStepsToMove, int numberOfStepsAlreadyMoved, pathPoint[] pathParent_)
    {
        if (numberOfStepsToMove == 0)
        {
            return false;
        }
        if (numberOfStepsAlreadyMoved == 57)
        {
            return false;
        }


        int leftNumberOfPath = pathParent_.Length - numberOfStepsAlreadyMoved;
        if (leftNumberOfPath >= numberOfStepsToMove)
        {
            return true;
        }
       

        else
        {
            return false;
        }
    }
    public bool isplayerCompleted()
    {
        
        if (previousPathPoint != null && previousPathPoint.name == "commonPathPointCenter")
        {
            Debug.Log("previousPathPoint.name == " + previousPathPoint.name);
            isPlayerComplete = true;
            return true;
        }
        return false;
    }
   


}
