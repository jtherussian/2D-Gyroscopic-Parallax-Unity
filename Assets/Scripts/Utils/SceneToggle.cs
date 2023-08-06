using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToggle : MonoBehaviour
{
    private int currentSceneIndex = 0;

    public void playGame()
    {
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        currentSceneIndex = (currentSceneIndex + 1) % totalScenes;

        // If the index is equal to the total number of scenes, set it back to 0.
        if (currentSceneIndex == totalScenes)
        {
            currentSceneIndex = 0;
        }

        SceneManager.LoadScene(currentSceneIndex);
    }
}
