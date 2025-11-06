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

    //Target Variables
    public List<NPC> npcs;
    public Camera targetCamera;
    [HideInInspector]
    public GameObject target;
    public NPCTextureObject npcTextures;

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
    [SerializeField]
    private TextMeshProUGUI winMoney; //How much money was rewarded after WINNING the level
    [SerializeField]
    private TextMeshProUGUI winTime; //How much time passed before level was completed
    [SerializeField]
    private TextMeshProUGUI loseMoney; //How much money was rewarded after LOSING the level
    [SerializeField]
    private TextMeshProUGUI loseTime; //How much time passed before the level was lost
    [SerializeField]
    private TextMeshProUGUI shopCash; //Display player cash in shop



    [Header("Money & Time")]
    //Money that is CURRENTLY in the player's posession
    public float playerMoney;

    //Money that the player can win in the level
    public float levelMoney;
    public float startMoney;

    //Duration of the level
    public float levelDuration;

    [Header("Play State")] //Used in other Gameobjects to determine if they should stay active
    public bool targetHit = false;
    public bool levelFailed;

    [Header("Scene Transition")]
    public float transTimer;
    private Scene restartScene;

    /// <summary>
    /// Make sure there is one one Game Manager Instance
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
    }

    private void Start()
    {
        startMoney = levelMoney;
        SetupNPCs();

        if (hideOutButtons != null)
        {
            hideOutButtons.SetActive(false);
        }
    }

    private void Update()
    {
        shopCash.text = "$" + playerMoney.ToString("0,000,000");
    }

    #region Button Functions
    public void CallContractsUI()
    {
        hideOutButtons.SetActive(true);

        //Due to only being accessable in the contracts menu, disable other menus
        contracts.SetActive(true);
        settings.SetActive(false);
        equipment.SetActive(false);
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

    public void GoToLevel(int levelNum)
    {
        hideOutButtons.SetActive(false);

        BlackOut();

        ResetVariables();
        npcs.Clear();

        //Turn off Contracts, Settings, and Equipment UI
        contracts.SetActive(false);
        settings.SetActive(false);
        equipment.SetActive(false);

        SceneManager.LoadScene(levelNum);
        SetupNPCs();
    }


    /// <summary>
    /// Restarts the Level
    /// </summary>
    public void RetryLevel()
    {

        //Apply Black Screen to hide level
        BlackOut();

        //Reset Level Variables
        levelMoney = startMoney;
        playerUI.gameObject.GetComponent<GameUIControl>().ResetTime();

        //Load Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        ResetVariables();
        npcs.Clear();

        //Rough Fix of the Pause Menu
        //Prevents it from bugging out
        if (pause.activeSelf == true)
        {
            pause.SetActive(false);
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Pause.isPaused = false;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        //Debug.LogAssertion("Game Quit");
    }

    private void ResetVariables()
    {
        //Turn Off UI
        lose.SetActive(false);
        win.SetActive(false);

        //Time Flows again
        levelFailed = false;
        targetHit = false;
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
    /// Pull up the fail screen
    /// </summary>
    public void Fail()
    {
        //Variable to stop time & money count
        levelFailed = true;

        //Convert Level Money if negative
        if (levelMoney <= 0) levelMoney = 0;

        //Change UI
        loseMoney.text = "$" + levelMoney.ToString("0,000,000");
        loseTime.text = playerUI.gameObject.GetComponent<GameUIControl>().timer;

        //Pull up Lose Screen
        lose.SetActive(true);

        //Show Mouse
        HiMouse();
    }

    /// <summary>
    /// Pull up the Win screen
    /// Give player the Level Cash
    /// </summary>
    public void CashOut()
    {
        //Show Mouse
        HiMouse();

        //Give money to the player
        playerMoney += levelMoney;

        //Change UI
        winMoney.text = "$" + levelMoney.ToString("0,000,000");
        winTime.text = playerUI.gameObject.GetComponent<GameUIControl>().timer;

        //Pull up win menu
        win.SetActive(true);
    }

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

    private void SetupNPCs()
    {
        npcs = GameObject.FindGameObjectsWithTag("NPC").Select(npc => npc.GetComponent<NPC>()).ToList();
        int targetMaterial = Random.Range(0, npcTextures.materials.Count);

        foreach (NPC npc in npcs)
        {
            int assignedMaterial;
            do
            {
                assignedMaterial = Random.Range(0, npcTextures.materials.Count);
            } while (targetMaterial == assignedMaterial);

            npc.meshRenderer.material = npcTextures.materials[assignedMaterial];
        }

        int targetNPC = Random.Range(0, npcs.Count);
        NPC npcTarget = npcs[targetNPC];

        //Assigning Target Variables
        npcTarget.meshRenderer.material = npcTextures.materials[targetMaterial];
        npcTarget.isTarget = true;
        npcTarget.gameObject.tag = "Target";
        npcTarget.targetCamera = targetCamera;

        npcTarget.gameObject.layer = LayerMask.NameToLayer("Target");
        npcTarget.meshRenderer.gameObject.layer = LayerMask.NameToLayer("Target");
    }
    #endregion
}
