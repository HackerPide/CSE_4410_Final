using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public string sceneToLoad;

    public void OnMouseDown() {
        if (!string.IsNullOrEmpty(sceneToLoad)) {
            SceneManager.LoadScene(sceneToLoad);
        }
        else {
            Debug.LogWarning("Scene name is not set!");
        }
    }
}
