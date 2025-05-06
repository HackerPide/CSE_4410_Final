using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndZone : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private bool player1Reached = false;
    private bool player2Reached = false;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player1"))
            player1Reached = true;

        if (other.CompareTag("Player2"))
            player2Reached = true;

        if (player1Reached && player2Reached)
        {
            hasTriggered = true;
            StartCoroutine(PlayCompletionSoundAndLoadScene());
        }
    }

    private IEnumerator PlayCompletionSoundAndLoadScene()
    {
        audioManager.PlaySFX(audioManager.victory);
        yield return new WaitForSeconds(audioManager.victory.length);

        Messenger<string>.Broadcast(GameEvent.LEVEL_CHANGE, "HomeScreen");
    }
}


/*using System.Collections;
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
}*/
