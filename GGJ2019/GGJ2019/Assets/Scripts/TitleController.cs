using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {

    public AudioClip uiButtonSound;

    public void PlaySound()
    {
        AudioManager.PlaySound(uiButtonSound);
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public string InGameName, TitleName;

    public void PlayGame()
    {
        SceneManager.LoadScene(InGameName);
    }
        
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(TitleName);
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
