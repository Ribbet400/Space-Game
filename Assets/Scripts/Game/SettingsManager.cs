using UnityEngine;
using MLAPI;

public class SettingsManager : NetworkBehaviour
{
    public GameObject settingsHolder;
    public GameObject settingsPanel;

    public GameObject gameHolder;
    public GameObject gamePanel;

    private string state;

    public FirstPersonController controller;

    private void Start()
    {
        controller = GetComponent<FirstPersonController>();

        state = "Game";

        settingsHolder = GameObject.FindGameObjectWithTag("SettingsHolder");
        gameHolder = GameObject.FindGameObjectWithTag("GameHolder");

        settingsPanel = settingsHolder.transform.GetChild(0).gameObject;
        gamePanel = gameHolder.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (state == "Game")
                {
                    OpenSettings();
                }
                else if (state == "Settings")
                {
                    CloseSettings();
                }
            }
        }
    }

    void OpenSettings()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        controller.inGame = false; // can't move

        gamePanel.SetActive(false);
        settingsPanel.SetActive(true);
        state = "Settings";
    }

    void CloseSettings()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller.inGame = true; // can move

        settingsPanel.SetActive(false);
        gamePanel.SetActive(true);
        state = "Game";
    }

    public void ChangeXSensitivity(float x)
    {
        //if (IsLocalPlayer)
        //{
         //   gameObject.GetComponent<FirstPersonController>().sensitivityX = x;
        //}
        
        //controller.sensitivityX = x; // not working???
    }

    public void ChangeYSensitivity(float y)
    {
        //gameObject.GetComponent<FirstPersonController>().sensitivityY = y;
    }
}
