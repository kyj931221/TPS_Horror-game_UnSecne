using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }

    private static UIManager m_instance;

    //public Text ammoText;
    //public Text scoreText;
    //public Text waveText;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;

    public GameObject gameoverUI;

    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }

    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score " + newScore;
    }

    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = " Wave : " + waves + " / " + " Enemy Left : " + count;
    }

    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameHome()
    {
        SceneManager.LoadScene("01.MainTitle");
    }
}
