using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    PlayerCharacter character;
    PlayerController controller;
    public Text randomNumber;
    public Text goldCount;
    public Text lifeCount;
    public Text time;
    public Text deadTimes;
    public Text percentage;
    public Text goldCountEndPanel;
    public Text timeEndPanel;
    public Text goldCountCompletePanel;
    public Text timeCompletePanel;
    public GameObject gameStartPanel;
    public GameObject gameEndPanel;
    public GameObject gameCompletePanel;
    public GameObject gameStopPanel;
    public GameObject frameMinimap;
    
    InputField randomNum;
    int num = -1;
    GameObject Env, Level;
    Transform AIs;
    GenerateScene genSceneScript;
    ActiveCamera activeCamera;
    AudioSource backGroundAudio;

    private void Awake()
    {
        character = FindObjectOfType<PlayerCharacter>();
        controller = FindObjectOfType<PlayerController>();
        activeCamera = GameObject.Find("Camera").GetComponent<ActiveCamera>();
        randomNum = GetComponentInChildren<InputField>();
        backGroundAudio = FindObjectOfType<AudioSource>();
    }

    public void OnClick_StartNewGame()
    {
        randomNumber.gameObject.SetActive(false);
        character.animator.enabled = true;
        Scene.gameStopped = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    public void OnClick_ContinueGame()
    {
        gameStopPanel.SetActive(false);
        GameTextOn();
        character.animator.enabled = true;
        character.SetParticleSystem(0);
        Scene.gameStopped = false;
        backGroundAudio.UnPause();
    }

    public void OnClick_RestartGame()
    {
        gameEndPanel.SetActive(false);
        gameCompletePanel.SetActive(false);
        gameStopPanel.SetActive(false);
        GameTextOn();
        character.animator.enabled = true;
        Scene.gameStopped = false;

        AIs = Level.transform.parent.GetChild(0);
        for (int i=0; i<AIs.childCount; i++)
            Destroy(AIs.GetChild(i).gameObject);
        Destroy(Level);
        //UnityEngine.Random.InitState(num);
        Level = new GameObject ("Level");
        Level.transform.parent = Env.transform;
        Level.AddComponent<GenerateScene>();

        genSceneScript = Level.GetComponent<GenerateScene>();
        genSceneScript.randomSeed = num;
        genSceneScript.genScene();
        Destroy(Level.GetComponent("GenerateScene"));

        activeCamera.gameRestart = true;
        
        character.gameObject.SetActive(true);
        character.isAlive = true;
        character.recoverBegin();
        character.particle[4].Stop();
        controller.bombParticle.Stop();
        controller.Initialize();
        backGroundAudio.Stop();
        backGroundAudio.Play();
        Scene.recoverBomb = true;
        Invoke("SetRecoverBombFalse", 0.1f);
    }

    void SetRecoverBombFalse()
    {
        Scene.recoverBomb = false;
    }

    public void OnClick_ContinueGameFromCheckpoint()
    {
        gameEndPanel.SetActive(false);
        GameTextOn();
        character.isAlive = true;
        character.recoverCheckpoint();
        controller.Initialize();
        backGroundAudio.UnPause();
        Scene.recoverBomb = true;
        Invoke("SetRecoverBombFalse", 0.1f);
        Scene.coinRestart = true;
        Invoke("SetCoinRestartFalse", 0.1f);
        activeCamera.gameRestart = true;
    }

    void SetCoinRestartFalse()
    {
        Scene.coinRestart = false;
    }

    public void OnClick_GetInputNumber()
    {
        try {
            num = int.Parse(randomNum.text);
        }
        catch (Exception e) {
            return;
        }

        gameStartPanel.SetActive(false);
        GameTextOn();
        randomNumber.gameObject.SetActive(true);

        //UnityEngine.Random.InitState(num);
        Env = GameObject.Find("Env");
        Level = new GameObject ("Level");
        Level.transform.parent = Env.transform;
        Level.AddComponent<GenerateScene>();

        genSceneScript = Level.GetComponent<GenerateScene>();
        genSceneScript.randomSeed = num;
        genSceneScript.genScene();
        Destroy(Level.GetComponent("GenerateScene"));

        character.gameObject.SetActive(true);
        character.isAlive = true;
        character.particle[4].Stop();
        controller.bombParticle.Stop();
        backGroundAudio.Play();

        // Necessary because the same seed(num) generates different results between the same time & after
        //OnClick_RestartGame();
    }

    public void GameStart()
    {
        gameStartPanel.SetActive(true);
        GameTextOff();
    }

    public void GameEnd()
    {
        gameEndPanel.SetActive(true);
        GameTextOff();
        backGroundAudio.Pause();
    }

    public void GameComplete()
    {
        gameCompletePanel.SetActive(true);
        GameTextOff();
        backGroundAudio.Stop();
    }

    public void GameStop()
    {
        gameStopPanel.SetActive(true);
        GameTextOff();
        backGroundAudio.Pause();
    }

    public void GameTextOn()
    {
        goldCount.gameObject.SetActive(true);
        lifeCount.gameObject.SetActive(true);
        time.gameObject.SetActive(true);
        frameMinimap.SetActive(true);
        //randomNumber.gameObject.SetActive(true);
    }

    public void GameTextOff()
    {
        goldCount.gameObject.SetActive(false);
        lifeCount.gameObject.SetActive(false);
        time.gameObject.SetActive(false);
        frameMinimap.SetActive(false);
        //randomNumber.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!character) return;

        int val = Mathf.RoundToInt(character.percentage*100);
        goldCount.text = "Coin Count: "+character.coinCount.ToString();
        lifeCount.text = "Life Count: "+character.diamondCount.ToString();
        time.text = "Time: "+character.gameTime.ToString("#0.00")+"s";
        randomNumber.text = "Random Seed: "+num.ToString();

        deadTimes.text = "Attempt# "+character.deadTimes.ToString();
        percentage.text = "Complete "+val.ToString()+"%";
        //goldCountEndPanel.text = "Coin Count: "+character.coinCountSaved.ToString();
        //timeEndPanel.text = "Time: "+character.gameTimeSaved.ToString("#0.00")+"s";
        goldCountEndPanel.text = goldCount.text;
        timeEndPanel.text = time.text;

        int totalWealth = character.coinCount + character.diamondCount*3;
        goldCountCompletePanel.text = "Coin Count: "+totalWealth.ToString();;
        timeCompletePanel.text = time.text;
    }
}