using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public string InGameName;

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(InGameName);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
