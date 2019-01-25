using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }
}
