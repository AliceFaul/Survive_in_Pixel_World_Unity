using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState state;
    [SerializeField] private CinemachineCamera cam;

    [Header("Score UI")]
    [SerializeField] private TMP_Text scoreText;
    private int currentScore = 0;

    [Space]

    [Header("Enemy and Boss Score")]
    [SerializeField] private int enemyScore = 50;
    [SerializeField] private int bossScore = 200;

    [Space]

    [Header("Pause And Option UI")]
    public GameObject pausePanel;
    public GameObject optionPanel;
    [SerializeField] private Slider musicSlider, sfxSlider;
    [SerializeField] private Toggle musicToggle, sfxToggle;
    private float currentMusicVolume, currentSFXVolume;
    private float muteVolume = 0f;

    [Space]

    [Header("Game Over and Win UI")]
    public GameObject gameOverPanel;
    public GameObject winPanel;
    [SerializeField] private TMP_Text scoreGameOver;
    [SerializeField] private TMP_Text scoreWin;

    [Space]

    [Header("Level System")]
    public GameObject lvRequirePanel;
    [SerializeField] private TMP_Text lvRequireText;
    [SerializeField] private TMP_Text clickHereText;
    [SerializeField] private Image transitionPanel;
    [SerializeField] private float transitionTime = 3f;
    [SerializeField] private string spawnPoint;
    [SerializeField] private GameObject playerPrefabs;

    [Space]

    [Header("Level 1 Requirement")]
    private int currentAmount;
    [SerializeField] private int maxAmount;

    [Space]

    [Header("Level 2 Requirement")]
    private int lv2CurrentAmount;
    [SerializeField] private int lv2MaxAmount;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindWithTag("Player");
        Transform spawnPos = GameObject.Find(spawnPoint).transform;
        if (player == null)
        {
            player = Instantiate(playerPrefabs, spawnPos.position, Quaternion.identity);
            if (cam != null)
            {
                cam.Follow = player.transform;
                cam.LookAt = player.transform;
            }
            else
            {
                cam = FindAnyObjectByType<CinemachineCamera>();
                cam.Follow = player.transform;
                cam.LookAt = player.transform;
            }
            Debug.Log("Spawn Player");
        }
        else
        {
            Debug.Log("Player already in map");
        }
    }

    private void Start()
    {
        //Game state
        state = GameState.InProgress;

        //Level 1
        currentAmount = 0;
        lvRequireText.text = $"Collect 25 coin in map: \n{currentAmount} / {maxAmount}.";

        //Level 2
        lv2CurrentAmount = 0;

        //Score text
        currentScore = 0;
        scoreText.text = $"SCORE: {currentScore}";

        //UI enable and disable
        pausePanel.SetActive(false);
        optionPanel.SetActive(false);
        lvRequirePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);

        //Audio System
        musicSlider.value = 1f;
        sfxSlider.value = 1f;

        currentMusicVolume = musicSlider.value;
        currentSFXVolume = sfxSlider.value;
    }

    #region Game Over and Win UI

    public void GameOver()
    {
        AudioManager.Instance.PlaySFX("Lose");
        scoreGameOver.text = $"SCORE: {currentScore}";
        gameOverPanel.SetActive(!gameOverPanel.activeSelf);
        lv2CurrentAmount = 0;
        Time.timeScale = 0;
    }

    public void Replay()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        state = GameState.Failed;
        gameOverPanel.SetActive(!gameOverPanel.activeSelf);
        Time.timeScale = 1;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        Time.timeScale = 1;
        Destroy(this.gameObject);
    }

    public void Win()
    {
        AudioManager.Instance.PlaySFX("Win");
        scoreWin.text = $"SCORE: {currentScore}";
        winPanel.SetActive(!winPanel.activeSelf);
        Time.timeScale = 0;
    }

    #endregion

    #region Pause Controller

    public void Pause()
    {
        AudioManager.Instance.PlaySFX("Select");
        pausePanel.SetActive(!pausePanel.activeSelf);
        Time.timeScale = pausePanel.activeSelf ? 0 : 1;
    }

    public void Resume()
    {
        AudioManager.Instance.PlaySFX("Select");
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Option()
    {
        AudioManager.Instance.PlaySFX("Select");
        pausePanel.SetActive(!pausePanel.activeSelf);
        optionPanel.SetActive(!optionPanel.activeSelf);
    }

    #endregion

    #region Level Controller

    public void ActiveLvRequire()
    {
        AudioManager.Instance.PlaySFX("Select");
        lvRequirePanel.SetActive(!lvRequirePanel.activeSelf);
        clickHereText.gameObject.SetActive(false);
    }

    public void Level1Progress(int amount)
    {
        if (state == GameState.InProgress)
        {
            currentAmount += amount;
            lvRequireText.text = $"Collect 25 coin in map: \n{currentAmount} / {maxAmount}.";

            if(currentAmount >= maxAmount)
            {
                state = GameState.Completed;
                lvRequireText.text = "Completed! \nFind a teleport and move to next level.";
                Debug.Log("Level 1 Completed");
            }
        }
    }

    public void Level2Progress(int amount)
    {
        if (state == GameState.InProgress)
        {
            lv2CurrentAmount += amount;
            lvRequireText.text = $"Defeat 20 enemy in map: \n{lv2CurrentAmount} / {lv2MaxAmount}.";

            if (lv2CurrentAmount >= lv2MaxAmount)
            {
                state = GameState.Completed;
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach(var enemy in enemies)
                {
                    Destroy(enemy.gameObject);
                }
                lvRequireText.text = "Completed! \nYou have survived!";
                AudioManager.Instance.PlaySFX("LevelUp");
                Invoke(nameof(Win), 5f);
                Debug.Log("Level 2 Completed");
            }
        }
    }

    public void Transition()
    {
        StartCoroutine(TransitionEffect());
    }

    IEnumerator TransitionEffect()
    {
        if(transitionPanel != null)
        {
            Animator transiAnim = transitionPanel.GetComponent<Animator>();
            if(transiAnim != null)
            {
                transiAnim.SetTrigger("Start");
                yield return new WaitForSeconds(transitionTime);
                SceneManager.LoadSceneAsync("Level2");
                transiAnim.SetTrigger("End");
                transiAnim.SetBool("Completed", true);
                lvRequireText.text = $"Defeat 20 enemy in map: \n{lv2CurrentAmount} / {lv2MaxAmount}.";
                Debug.Log("Transition Completed");
            }
        }
        else
        {
            Debug.Log("Not have Transition Panel");
        }
    }

    #endregion

    #region Score Controller

    public void AddScore(int amount)
    {
        currentScore += amount;
        scoreText.text = $"SCORE: {currentScore}";
    }

    public void AddEnemyScore()
    {
        currentScore += enemyScore;
        scoreText.text = $"SCORE: {currentScore}";
    }

    public void AddBossScore()
    {
        currentScore += bossScore;
        scoreText.text = $"SCORE: {currentScore}";
    }

    #endregion

    #region Audio System

    public void ToggleMusic()
    {
        AudioManager.Instance.PlaySFX("Select");
        AudioManager.Instance.ToggleMusic();
        musicSlider.value = musicToggle.isOn ? muteVolume : currentMusicVolume;
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.PlaySFX("Select");
        Invoke(nameof(AudioManager.Instance.ToggleSFX), 1f);
        sfxSlider.value = sfxToggle.isOn ? muteVolume : currentSFXVolume;
    }

    public void SetMusicVolume()
    {
        AudioManager.Instance.SetMusicVolume(musicSlider.value);

        if (musicSlider.value <= 0)
        {
            musicToggle.isOn = true;
        }
        else
        {
            musicToggle.isOn = false;
        }

        if (musicSlider.value <= 0)
        {
            return;
        }
        else
        {
            currentMusicVolume = musicSlider.value;
        }
    }

    public void SetSFXVolume()
    {
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);

        if (sfxSlider.value <= 0)
        {
            sfxToggle.isOn = true;
        }
        else
        {
            sfxToggle.isOn = false;
        }

        if (sfxSlider.value <= 0)
        {
            return;
        }
        else
        {
            currentSFXVolume = sfxSlider.value;
        }
    }

    #endregion
}

public enum GameState { Completed, InProgress, Failed }
