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

    private static LevelManager instance;

    public static LevelManager Instance { get { return instance; } }

    [Header("Target Variables")]
    public Camera targetCamera;
    [HideInInspector]
    public GameObject target;
    public NPCTextureObject npcTextures;

    [Header("Money & Time")]

    //Money that the player can win in the level
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
        startMoney = levelMoney;

        SetupNPCs();
    }

    private void FixedUpdate()
    {
        MoneyInterval();
    }

    private void SetupNPCs()
    {
        List<NPC> npcs = GameObject.FindGameObjectsWithTag("NPC").Select(npc => npc.GetComponent<NPC>()).ToList();
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
        npcTarget.meshRenderer.material = npcTextures.materials[targetMaterial];
        npcTarget.isTarget = true;
        npcTarget.gameObject.tag = "Target";
        npcTarget.targetCamera = targetCamera;

        npcTarget.gameObject.layer = LayerMask.NameToLayer("Target");
        npcTarget.meshRenderer.gameObject.layer = LayerMask.NameToLayer("Target");
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
