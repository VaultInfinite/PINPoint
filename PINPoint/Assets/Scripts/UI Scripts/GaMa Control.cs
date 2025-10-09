using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GaMaControl : MonoBehaviour
{
    private static GaMaControl instance;

    public static GaMaControl Instance { get { return instance; } }

    //Target Variables
    public List<NPC> npcs = new List<NPC>();
    public Camera targetCamera;

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

    public GameObject tutorial;

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
    public int playerMoney;

    //Money that the player can win in the level
    public int levelMoney;
    private int startMoney;

    [Header("Play State")]
    public bool targetHit = false;
    public bool levelFailed;

    [Header("Scene Transition")]
    public float transTimer;
    private Scene restartScene;
    //public int levelNum;

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
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        startMoney = levelMoney;
        StartCoroutine(TutorialDisplay());
        StartCoroutine(TargetSelect());
    }

    #region Button Functions
    public void CallContractsUI()
    {
        //Due to only being accessable in the contracts menu, disable other menus
        contracts.SetActive(true);
        settings.SetActive(false);
        equipment.SetActive(false);
    }

    //Pulls up the settings UI
    public void CallSettingsUI()
    {
        //Due to only being accessable in the contracts menu, disable other menus
        contracts.SetActive(false);
        settings.SetActive(true);
        equipment.SetActive(false);
    }

    //Calls the shop UI in the menu
    public void CallEquipmentUI()
    {
        //Due to only being accessable in the contracts menu, disable other menus
        contracts.SetActive(false);
        settings.SetActive(false);
        equipment.SetActive(true);

        shopCash.text = "$" + playerMoney.ToString("0,000,000");
        
    }

    public void GoToLevel(int levelNum)
    {
        BlackOut();

        ResetVariables();

        //Turn off Contracts, Settings, and Equipment UI
        contracts.SetActive(false);
        settings.SetActive(false);
        equipment.SetActive(false);

        SceneManager.LoadSceneAsync(levelNum);
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

    private void NPCList()
    {
        bool hasTarget = false;

        for (int index = 0; index < npcs.Count; index++)
        {
            Debug.Log("This line activated");

            float randomNumber = Random.Range(0f, 1f);

            if (randomNumber >= 0.9f && !hasTarget || index >= npcs.Count && !hasTarget)
            {
                npcs[index].isTarget = true;
                npcs[index].targetCamera = targetCamera;
                Debug.Log("target assigned at" + npcs[index].gameObject);
                npcs[index].gameObject.tag = "Target";

                hasTarget = true;
            }
            if (npcs[index].isTarget == false)
            {
                npcs[index].targetCamera = null;
            }
        }
    }

    private IEnumerator TutorialDisplay()
    {
        yield return new WaitForSeconds(18f);
        tutorial.SetActive(false);
    }

    private IEnumerator TargetSelect()
    {
        yield return new WaitForSeconds(0.5f);
        NPCList();
    }
    #endregion
}
