using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToggle : MonoBehaviour
{
    private int currentSceneIndex = 0;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void PlayGame()
    {
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        // Increase the current scene index by 1, and loop back to 0 if it exceeds the total scenes.
        currentSceneIndex = (currentSceneIndex + 1) % totalScenes;

        // Load the next scene by index.
        SceneManager.LoadScene(currentSceneIndex);
    }
}