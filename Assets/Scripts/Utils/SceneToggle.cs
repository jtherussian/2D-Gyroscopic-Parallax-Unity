using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToggle : MonoBehaviour
{
    private int startingSceneIndex = 0;
    private int currentSceneIndex = 0;
    private int totalScenes;

    private void Start()
    {
        totalScenes = SceneManager.sceneCountInBuildSettings;
        startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentSceneIndex = startingSceneIndex;
    }

    public void playGame()
    {
        currentSceneIndex = (currentSceneIndex + 1) % totalScenes;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
