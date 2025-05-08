using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndZone : MonoBehaviour
{
    AudioManager audioManager;
	string currentScene;
	string nextScene = "HomeScreen";
	
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
		
		currentScene = (SceneManager.GetActiveScene()).name;
		switch(currentScene) 
		{
		  case "level1":
			nextScene = "level2";
			break;
		  case "level2":
			nextScene = "End";
			break;
/* 		  case "level3":
			nextScene = "level4";
			break;
		  case "level4":
			nextScene = "End";
			break; */
		  default:
			nextScene = "HomeScreen";
			break;
		}

        Messenger<string>.Broadcast(GameEvent.LEVEL_CHANGE, nextScene);
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
