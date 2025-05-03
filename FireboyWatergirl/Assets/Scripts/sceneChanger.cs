using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChanger : MonoBehaviour
{
    private void OnEnable() {
        Messenger<string>.AddListener(GameEvent.LEVEL_CHANGE, OnLevelChange);
    }

    private void OnDisable() {
        Messenger<string>.RemoveListener(GameEvent.LEVEL_CHANGE, OnLevelChange);
    }

    private void OnLevelChange(string levelName) {
        SceneManager.LoadScene(levelName);
    }
}
