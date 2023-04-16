using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneSelector : MonoBehaviour
{
    private bool reachedTrigger = false;

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            reachedTrigger = true;

            GetComponent<TextMeshProUGUI>().text = "Press E\nTo the next area";

        }
    }

    void Update()
    {
        if (reachedTrigger && Input.GetKeyDown(KeyCode.E))
        {
            LoadNextScene();
        }
    }
}
