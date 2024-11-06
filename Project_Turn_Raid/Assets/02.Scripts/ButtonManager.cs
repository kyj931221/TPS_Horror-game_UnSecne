using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public string Game_Start_scene_name;
    public AudioClip btClick;

    private AudioSource bt_Click_Play;

    private void Awake()
    {
        bt_Click_Play = GetComponent<AudioSource>();
    }

    public void Game_Start_SceneLoad()
    {
        bt_Click_Play.PlayOneShot(btClick);
        SceneManager.LoadScene(Game_Start_scene_name);
    }

    public void Game_Exit()
    {
        Debug.Log("GameExit");
        bt_Click_Play.PlayOneShot(btClick);
        Application.Quit();
    }
}
