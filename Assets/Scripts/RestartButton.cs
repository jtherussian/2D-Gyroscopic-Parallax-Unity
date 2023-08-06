using UnityEngine;

public class RestartButton : MonoBehaviour
{
    private void OnGUI()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float buttonWidth = screenWidth * 0.2f;
        float buttonHeight = screenHeight * 0.1f;

        float buttonX = 0;
        float buttonY = 0;

        // Create a GUI button at the top-left corner
        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "Restart"))
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        // Get the current scene's name
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}