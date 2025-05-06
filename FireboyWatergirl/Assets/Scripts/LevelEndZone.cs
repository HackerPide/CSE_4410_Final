using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndZone : MonoBehaviour
{
    private bool player1Reached = false;
    private bool player2Reached = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            player1Reached = true;
            Debug.Log("Player 1 reached end");
        }
        else if (other.CompareTag("Player2"))
        {
            player2Reached = true;
            Debug.Log("Player 2 reached end");
        }

        if (player1Reached && player2Reached)
        {
            Messenger<string>.Broadcast(GameEvent.LEVEL_CHANGE, "HomeScreen");
        }
    }
}
