using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Serializable]
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
    public Difficulty difficulty;

    private static LevelManager instance;

    public static LevelManager Instance { get { return instance; } }

    [Header("Target Variables")]
    public Camera targetCamera;
    public TargetDummy targetDummy;
    [HideInInspector]
    public GameObject target;
    public NPCTextureObject npcTextures;

    //Money that the player can win in the level
    public float easyMoney, mediumMoney, hardMoney;
    [HideInInspector]
    public float levelMoney;
    private float startMoney;

    //Duration of the level
    public float levelDuration;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        switch (GameManager.Instance.lastDifficulty)
        {
            case Difficulty.Easy:
                difficulty = Difficulty.Easy;
                Debug.Log("Easy Money");
                levelMoney = easyMoney;
                levelDuration = 240f;
                break;
            case Difficulty.Medium:
                difficulty = Difficulty.Medium;
                Debug.Log("Medium Money");
                levelMoney = mediumMoney;
                levelDuration = 180f;
                break;
            case Difficulty.Hard:
                difficulty = Difficulty.Hard;
                Debug.Log("Hard Money");
                levelMoney = hardMoney;
                levelDuration = 150f;
                break;
            default:
                Debug.Log("Something has gone terrible wrong with the startMoney assignments in LevelManager Start");
                break;
        }

        startMoney = levelMoney;

        SpawnNPCs();
        SetupNPCs();
    }

    private void FixedUpdate()
    {
        MoneyInterval();
    }

    private void SpawnNPCs()
    {
        //Acquires all CrowdSpawners in the scene, and only assigns them to the list when spawnPoints local difficulty enum
        //is set to the difficulty setting in LevelManager, assigned in GameManager through the selection of Contracts in the hideout
        List<CrowdSpawner> spawnPoints = FindObjectsOfType<CrowdSpawner>().Where(spawnPoints => spawnPoints.difficulty == difficulty).ToList();

        //Grab the specific spawnPoint we're spawning npcs at by utilizing a random.range within the list
        //Then, set the specific spawner within the list using that targeted spawnpoint
        int targetSpawnPoint = UnityEngine.Random.Range(0, spawnPoints.Count);
        CrowdSpawner spawner = spawnPoints[targetSpawnPoint];

        //Spawn the horde
        spawner.Spawn();
    }

    private void SetupNPCs()
    {
        List<NPC> npcs = FindObjectsOfType<NPC>().ToList();
        int targetMaterial = UnityEngine.Random.Range(0, npcTextures.materials.Count);

        foreach (NPC npc in npcs)
        {
            int assignedMaterial;
            do
            {
                assignedMaterial = UnityEngine.Random.Range(0, npcTextures.materials.Count);
            } while (targetMaterial == assignedMaterial);

            npc.meshRenderer.material = npcTextures.materials[assignedMaterial];
        }

        int targetNPC = UnityEngine.Random.Range(0, npcs.Count);
        NPC npcTarget = npcs[targetNPC];

        //Assigning Target Variables
        npcTarget.meshRenderer.material = GameManager.Instance.lastTarget;
        targetDummy.meshRenderer.material = GameManager.Instance.lastTarget;
        npcTarget.isTarget = true;
        npcTarget.gameObject.tag = "Target";
    }

    /// <summary>
    /// Pull up the fail screen
    /// </summary>
    public void Fail()
    {
        var gm = GameManager.Instance;

        //Variable to stop time & money count
        gm.levelFailed = true;

        //Convert Level Money if negative
        if (levelMoney <= 0) levelMoney = 0;

        //Change UI
        gm.loseMoney.text = "$" + levelMoney.ToString("0,000,000");
        gm.loseTime.text = gm.playerUI.gameObject.GetComponent<GameUIControl>().timer;

        //Pull up Lose Screen
        gm.lose.SetActive(true);

        //Show Mouse
        gm.HiMouse();
    }

    /// <summary>
    /// Pull up the Win screen
    /// Give player the Level Cash
    /// </summary>
    public void CashOut()
    {
        var gm = GameManager.Instance;
        //Show Mouse
        gm.HiMouse();

        //Give money to the player
        gm.playerMoney += levelMoney;

        //Change UI
        gm.winMoney.text = "$" + levelMoney.ToString("0,000,000");
        gm.winTime.text = gm.playerUI.gameObject.GetComponent<GameUIControl>().timer;

        //Pull up win menu
        gm.win.SetActive(true);
    }

    /// <summary>
    /// Called in FixedUpdate; removes a specific amount of money per second (interest is amount lost per second)
    /// </summary>
    private void MoneyInterval()
    {
        //Check if there is enough money
        if (levelMoney <= 0)
        {
            Fail();
        }
        //Decrease money
        else
        {
            float interest = startMoney / levelDuration;
            levelMoney -= interest * Time.fixedDeltaTime;
        }
    }
}
