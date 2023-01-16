using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public static MenuManager active;
    List<GameObject> open = new List<GameObject>();
    [SerializeField] public GameObject[] menus;
    [SerializeField] GameObject[] hiddenButtons;
    bool buttonVisibility;

    [Header("Pause functionality")]
    [SerializeField] bool pausesGame;
    [SerializeField] GameObject pauseUI;

    [Header("Sounds")]
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource hoverSound;
    [SerializeField] AudioSource clickSound;

    private void Awake()
    {
        active = this;
    }
    private void Start()
    {
        foreach (GameObject m in menus)
            m.SetActive(false);

        CheckPause();

        buttonVisibility = false;
        foreach (GameObject button in hiddenButtons)
            button.SetActive(buttonVisibility);
    }

    void CheckPause()
    {
        if (!pausesGame)
        {
            Time.timeScale = 1;
            return;
        }

        bool isPaused = (open.Count != 0);
        int scale = 1;
        if (isPaused)
            scale = 0;

        Time.timeScale = scale;
        pauseUI.SetActive(isPaused);

        Debug.Log("timeScale is " + Time.timeScale);
    }

    public void OpenMenu(int index)
    {
        if (index < 0 || index >= menus.Length)
        {
            Debug.Log("index " + index + " outside of range");
            return;
        }

        if (menus[index].activeSelf)
        {
            menus[index].SetActive(false);
            open.Remove(menus[index]);
        }
        else
        {
            menus[index].SetActive(true);
            open.Add(menus[index]);
        }

        CheckPause();
    }
    public void CloseMenu()
    {
        GameObject currentMenu = open[open.Count - 1];
        currentMenu.SetActive(false);
        open.Remove(currentMenu);

        CheckPause();
    }
    public void CloseOrOpen()
    {
        if (open.Count > 0)
            CloseMenu();
        else
            OpenMenu(0);
    }
    public void ShowHideButtons()
    {
        if (open.Count <= 0)
            return;

        buttonVisibility = !buttonVisibility;
        foreach (GameObject button in hiddenButtons)
            button.SetActive(buttonVisibility);
    }

    public void ChangeScene(int index)
    {
        Debug.Log("loading scene " + index);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
        //load scene by index
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ButtonHover()
    {
        hoverSound.Play();
    }
    public void ButtonClick()
    {
        clickSound.Play();
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFXVolume", volume);
    }
}
