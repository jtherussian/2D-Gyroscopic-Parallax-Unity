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
        SceneManager.LoadScene(currentSceneIndex);
    }
}