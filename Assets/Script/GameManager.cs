using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private bool isStarted = false; // ゲーム開始済みフラグ
    
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

        // プレイヤー操作禁止
        FindObjectOfType<PlayerControll>().enabled = false;

        UIManager.Instance.ShowStartUI(this);

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

        // プレイヤー操作可能にする
        FindObjectOfType<PlayerControll>().enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UIManager.Instance.ShowStartUI(false);

        startTiem = Time.time;
    }

    public void TryClear()
    {
        Debug.Log("クリア");
        clearTime = Time.time - startTiem;
        PlayerPrefs.SetFloat("ClearTime", clearTime);
        FadeManager.instance.FadeToScene("ResultScene");
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
