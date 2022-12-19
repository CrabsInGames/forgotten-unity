using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager active;
    [SerializeField] List<GameObject> open = new List<GameObject>();
    [SerializeField] public GameObject[] menus;

    private void Awake()
    {
        active = this;
    }
    private void Start()
    {
        foreach (GameObject m in menus)
            m.SetActive(false);
    }

    public void OpenMenu(int index)
    {
        if (open.Count == 0)
            Time.timeScale = 0;

        if (index < 0 || index >= menus.Length)
        {
            Debug.Log("index " + index + " outside of range");
            return;
        }

        if (menus[index].activeSelf)
        {
            menus[index].SetActive(false);
            open.Remove(menus[index]);
            if (open.Count == 0)
                Time.timeScale = 1;
        }
        else
        {
            menus[index].SetActive(true);
            open.Add(menus[index]);
        }
    }
    public void CloseMenu()
    {
        GameObject currentMenu = open[open.Count - 1];
        currentMenu.SetActive(false);
        open.Remove(currentMenu);
        if (open.Count == 0)
            Time.timeScale = 1;
    }
    public void CloseOrOpen()
    {
        if (open.Count > 0)
            CloseMenu();
        else
            OpenMenu(0);
    }

    public void ChangeScene(int index)
    {
        //load scene by index
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
