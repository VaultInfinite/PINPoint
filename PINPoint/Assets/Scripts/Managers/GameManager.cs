using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    private GameUIControl gameUIControl;

    [Header("UI GameObjects")]
    public GameObject pause;
    public GameObject lose;
    public GameObject win;
    public GameObject contracts;
    public GameObject equipment;
    public GameObject settings;
    public GameObject load;
    public GameObject playerUI;
    public GameObject reticle;

    public GameObject hideOutButtons;

    //Settings UI
    public GameObject settingsExitButton;
    public GameObject settingsBackButton;

    [Header("UI Text")]
    public TextMeshProUGUI winMoney; //How much money was rewarded after WINNING the level
    public TextMeshProUGUI winTime; //How much time passed before level was completed
    public TextMeshProUGUI loseMoney; //How much money was rewarded after LOSING the level
    public TextMeshProUGUI loseTime; //How much time passed before the level was lost

    //Money that is CURRENTLY in the player's posession
    public float playerMoney;

    [Header("Scene Transition")]
    public float transTimer;

    [Header("Play State")] //Used in other Gameobjects to determine if they should stay active
    public bool targetHit = false;
    public bool levelFailed;

    [NonSerialized]
    public LevelManager.Difficulty lastDifficulty = LevelManager.Difficulty.Easy;
    [NonSerialized]
    public Material lastTarget;
    public NPCTextureObject npcTextures;
    public Material[] targetMat;

    public TargetDummy[] targets;

    private bool setupRan;

    /// <summary>
    /// Make sure there is one Game Manager Instance
    /// </summary>
    private void Awake()
    {
        //Make sure this is the only Game Manager in the Scene
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        //Keep this object even when changing scenes
        DontDestroyOnLoad(gameObject);

        SetupTargets();
    }

    private void Start()
    {
        gameUIControl = FindObjectOfType<GameUIControl>();
    }

    #region Button Functions
    public void HideoutReturn()
    {
        AudioControl.Instance.StopMusic();

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //Due to only being accessable in the contracts menu, disable other menus
            contracts.SetActive(true);
            settings.SetActive(false);
            equipment.SetActive(false);
        }
        else
        {
            setupRan = false;
            SceneManager.LoadScene(1);
            hideOutButtons.SetActive(true);

            //Due to only being accessable in the contracts menu, disable other menus
            contracts.SetActive(true);
            settings.SetActive(false);
            equipment.SetActive(false);

            foreach (TargetDummy target in targets)
            {
                target.gameObject.SetActive(true);
            }
            SetupTargets();

        }

    }

    //Pulls up the settings UI
    public void CallSettingsUI()
    {
        // If Settings is called when game is Paused (during killing time)
        if (Pause.isPaused)
        {
            settings.SetActive(true);
            settingsExitButton.SetActive(false);
            settingsBackButton.SetActive(true);

        }
        // If settings is called in between killings
        else
        {
            settingsExitButton.SetActive(true);
            settingsBackButton.SetActive(false);

            contracts.SetActive(false);
            settings.SetActive(true);
            equipment.SetActive(false);
        } 
    }

    /// <summary>
    /// Returns back to the Pause menu
    /// </summary>
    public void GoBackToPause()
    {
        settings.SetActive(false);
        settingsExitButton.SetActive(false);
        settingsBackButton.SetActive(false);
    }

    //Calls the shop UI in the menu
    public void CallEquipmentUI()
    {
        //Due to only being accessable in the contracts menu, disable other menus
        contracts.SetActive(false);
        settings.SetActive(false);
        equipment.SetActive(true);

        //shopCash.text = "$" + playerMoney.ToString("0,000,000");
        //ShopManager.Instance.RefreshShop();
    }

    public void ResumeMenu()
    {
        HideMouse();
        Time.timeScale = 1;
        Pause.isPaused = false;
        pause.SetActive(false);
    }

    private void GoToLevel(LevelManager.Difficulty difficulty, Material targetMaterial)
    {
        lastDifficulty = difficulty;
        lastTarget = targetMaterial;

        hideOutButtons.SetActive(false);

        foreach (TargetDummy target in targets)
        {
            target.gameObject.SetActive(false);
        }

        BlackOut();

        AudioControl.Instance.PlayMusic();

        //Turn off Contracts, Settings, and Equipment UI
        contracts.SetActive(false);
        settings.SetActive(false);
        equipment.SetActive(false);

        SceneManager.LoadScene(2);

        pause.SetActive(false);
        lose.SetActive(false);
        win.SetActive(false);

        levelFailed = false;
        targetHit = false;

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Pause.isPaused = false;
        gameUIControl.ResetTime();
        gameUIControl.SetTargetPrice();
    }

    public void GoToEasy() => GoToLevel(LevelManager.Difficulty.Easy, targetMat[0]);

    public void GoToMedium() => GoToLevel(LevelManager.Difficulty.Medium, targetMat[1]);

    public void GoToHard() => GoToLevel(LevelManager.Difficulty.Hard, targetMat[2]);


    /// <summary>
    /// Restarts the Level
    /// </summary>
    public void RetryLevel()
    {
        GoToLevel(lastDifficulty, lastTarget);
        gameUIControl.ResetTime();
    }

    public void QuitGame()
    {
        Application.Quit();
        //Debug.LogAssertion("Game Quit");
    }
    
    /// <summary>
    /// Applies a blackout to transition to the next scene
    /// </summary>
    private void BlackOut()
    {
        load.SetActive(true);
        StartCoroutine(BlackIn());
    }


    IEnumerator BlackIn()
    {
        yield return new WaitForSeconds(transTimer);
        load.SetActive(false);
    }
    #endregion

    #region Menu Calls
    /// <summary>
    /// Unlocks the screen and shows the mouse
    /// </summary>
    public void HiMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //Hide cursor and lock mouse to screen
    private void HideMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetupTargets()
    {
        if (!setupRan)
        {
            for (int index = 0; index < targets.Length; index++)
            {
                int targetMaterial = UnityEngine.Random.Range(0, npcTextures.materials.Count);
                targets[index].meshRenderer.material = npcTextures.materials[targetMaterial];
                targetMat[index] = npcTextures.materials[targetMaterial];
            }
        }
        setupRan = true;
    }
    #endregion
}
