using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("収集情報")]
    public int totalItem = 3;
    private int collectedItem = 0;

    private bool isStarted = false; // ゲーム開始済みフラグ
    private bool goalUnlock = false;

    private float startTiem;
    private float clearTime;

    private void Awake()
    {
        if (instance != null && instance == null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            StartCoroutine(WaitForFadeThenInit());
        }
    }

    private IEnumerator WaitForFadeThenInit()
    {
        // フェード完了を待つ
        if (FadeManager.instance != null)
        {
            while (!FadeManager.instance.IsFadeComplete) yield return null;
        }
        InitGame();
    }

    /// <summary>
    ///  ゲームの初期化
    /// </summary>
    private void InitGame()
    {
        // 現在のシーンをチェック
        if (SceneManager.GetActiveScene().name != "GameScene") return;

        isStarted = false;
        goalUnlock = false;

        UIManager.Instance.ShowStartUI(this);
        UIManager.Instance.UpdateItem(collectedItem,totalItem);
        UIManager.Instance.HideGoal();

        // ゲーム停止中
        Time.timeScale = 0f;
    }

    private void Update()
    {
        // ==== スペースキーでゲームスタート ====
        if (!isStarted && Input.GetKeyUp(KeyCode.Space))
        {
            StartGame();
        }
        return;
    }

    private void StartGame()
    {
        isStarted = true;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UIManager.Instance.ShowStartUI(false);

        startTiem = Time.time;
    }

    /// <summary>
    /// アイテムを拾った際に呼ばれる
    /// </summary>
    public void AddItem()
    {
        collectedItem++;
        UIManager.Instance.UpdateItem(collectedItem, totalItem);

        if(collectedItem >= totalItem)
        {
            goalUnlock = true;
            UIManager.Instance.ShowGoal("Get Back in the Car");
        }
    }

    public void TryClear()
    {
        if (goalUnlock)
        {
            Debug.Log("クリア");
            clearTime = Time.time - startTiem;
            PlayerPrefs.SetFloat("ClearTime",clearTime);
            SceneManager.LoadScene("ResultScene");
        }
        else
        {
            Debug.Log("まだ集めきってません");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    // ==== シーンリセット ====
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            StartCoroutine(WaitForFadeThenInit());
        }
        else
        {
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
