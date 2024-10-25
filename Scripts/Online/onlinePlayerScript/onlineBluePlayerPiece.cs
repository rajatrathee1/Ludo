using Photon.Pun;
using UnityEngine;
using System.Collections;

public class onlineBluePlayerPiece : MonoBehaviourPun
{
    public onlinePathObjectParent onlinePathParent;
    public int currentPathIndex = 0;
    public float moveSpeed = 2f;

    void Start()
    {
        if (photonView.IsMine)
        {
            // Enable input or movement
        }
    }

    public void MovePlayer(int steps)
    {
        if (photonView.IsMine)
        {
            StartCoroutine(MoveSteps(steps));
        }
    }

    IEnumerator MoveSteps(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            currentPathIndex++;
            if (currentPathIndex < onlinePathParent.onlineCommonPath.Length)
            {
                Vector3 nextPos = onlinePathParent.onlineCommonPath[currentPathIndex].transform.position;
                while (Vector3.Distance(transform.position, nextPos) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);
                    yield return null;
                }
            }
            else
            {
                // Reached the end of the path
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
