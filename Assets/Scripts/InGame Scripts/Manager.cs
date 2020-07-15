using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager singleton;
    private SlingshotHandler slingHandler;
    [SerializeField] private int coinsCollected;
    [SerializeField] private GameObject pauseUI, playerPrefab, victoryScreen, tutorialText;
    private Animator coinAnimation;
    private TextMeshProUGUI coinsText, completionText;//, completionText2;
    [SerializeField] private GameObject completionTextGO;
    private Transform rightbound;
    private Transform leftbound;
    private GameObject player;
    private PlanetsGenerator generator;
    public int level;
    public Transform coinCollectPosition;
    private int progression, coins;
    [SerializeField] private float TotalPoints;
    [SerializeField] private Transform progressionCanvas;

    private bool CreditUIBackButtonToGameOver = true;

    private void Awake()
    {
        singleton = this;
        generator = gameObject.GetComponent<PlanetsGenerator>();

        // Load current level
        if (PlayerPrefs.HasKey("level"))
        {
            level = PlayerPrefs.GetInt("level");
        }
        if (PlayerPrefs.HasKey("coins"))
        {
            coins = PlayerPrefs.GetInt("coins");
        }

        // Activate generating
        generator.SetupGenerator(level);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 1;
        slingHandler = GameObject.Find("Player").GetComponent<SlingshotHandler>();
        Debug.Log("level: " + PlayerPrefs.GetInt("level").ToString());
        PauseGame(false);
        victoryScreen.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        coinsText = GameObject.Find("Coins").transform.Find("CoinsText").GetComponent<TextMeshProUGUI>();
        //completionText = GameObject.Find("progressionCanvas").transform.Find("CompletionText").GetComponent<TextMeshProUGUI>();
        //completionText2 = GameObject.Find("progressionCanvas").transform.Find("CompletionText2").GetComponent<TextMeshProUGUI>();
        UpdateUI();
        rightbound = GameObject.Find("RightBoundary").GetComponent<Transform>();
        leftbound = GameObject.Find("LeftBoundary").GetComponent<Transform>();
        float boundaryX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x;

        rightbound.position = new Vector3(boundaryX, 0f, 0f);
        leftbound.position = new Vector3(-boundaryX, 0f, 0f);
        rightbound.SetParent(Camera.main.transform);
        leftbound.SetParent(Camera.main.transform);
        coinAnimation = GameObject.Find("CoinAnimation").GetComponent<Animator>();
        tutorialText = GameObject.Find("TutorialText");
        if (level == 0)
        {
            tutorialText.SetActive(true);
        }
        else
        {
            tutorialText.SetActive(false);
        }
        GenerateMilestones(10); //generates the milestone UI numbers in the sides of the screen
        TotalPoints = ScoreHandler.singleton.startPoints;
    }

    private void Update()
    {
        UpdateUI();
    }

    public void AddCoins(int amount) //add an amount of coins to coins collected
    {
        coinsCollected += amount;
        UpdateUI();
        coinAnimation.Play("CoinCollected_Anim", -1, 0);
    }

    public void UpdateUI() //this will update all UI on screen
    {
        coinsText.text = coinsCollected.ToString() + "x";
        if(slingHandler != null)
        {
            progression = slingHandler.CheckProgession();
            //progression /= 10;
            //progression *= 10;
            //completionText.text = progression.ToString();
            //completionText2.text = progression.ToString();
        }
    }

    public void RestartGame()
    {
        PauseGame(false);
        coinsCollected = 0;
        UpdateUI();
        //completionText.gameObject.SetActive(true);
        Vector2 spawnPosition = new Vector2(0f, -10f);
        Vector3 rotation = new Vector3(0f, 0f, 90f);
        generator.GenerateCoins();
        generator.GenerateAsteroids();
        //GameObject.FindGameObjectWithTag("Deathzone").GetComponent<DeathZone>().ResetPosition();
        player = Instantiate(playerPrefab, spawnPosition, Quaternion.Euler(rotation));
        slingHandler = GameObject.Find("Player(Clone)").GetComponent<SlingshotHandler>();
        Camera.main.transform.position = new Vector2(0, player.transform.position.y);
        Camera.main.GetComponent<CameraMovement>().SetPlayerObject(player.transform);
        if (level == 0)
        {
            tutorialText.GetComponent<Tutorial>().Timer = 5;
            tutorialText.SetActive(true);
        }
        TotalPoints = ScoreHandler.singleton.startPoints;
        //Debug.Log("start points: " + TotalPoints.ToString());
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            pauseUI.SetActive(true);
            if (tutorialText)
            {
                tutorialText.SetActive(false);
            }
            //completionText.gameObject.SetActive(false);
            TotalPoints += progression;
            pauseUI.transform.Find("Text").transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level " + PlayerPrefs.GetInt("level").ToString();
            pauseUI.transform.Find("Text").transform.Find("Progression").GetComponent<TextMeshProUGUI>().text = progression.ToString() + "%";
            pauseUI.transform.Find("Text").transform.Find("TotalProgression").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(TotalPoints).ToString();
            //Time.timeScale = 0;
        }
        else
        {
            pauseUI.SetActive(false);
            //Time.timeScale = 1;
        }
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void WinHandling()
    {
        SoundHandler.singleton.PlayLevelSuccess();
        StartCoroutine(WinScreen());
    }

    public void Win()
    {
        victoryScreen.SetActive(true);

        if (tutorialText)
        {
            tutorialText.SetActive(false);
        }
        PlayerPrefs.SetInt("level", ++level);
        coins += coinsCollected;
        PlayerPrefs.SetInt("coins", coins);
        //Time.timeScale = 0;
        WinUI();
    }

    public void WinUI()
    {
        if(GameObject.Find("CompletionText"))
            GameObject.Find("CompletionText").SetActive(false);

        TotalPoints += progression; 
        GameObject.Find("WinProgression").GetComponent<TextMeshProUGUI>().text = slingHandler.CheckProgession().ToString() + "%";
        GameObject.Find("WinLevel").GetComponent<TextMeshProUGUI>().text = "Level " + (level - 1).ToString();
        GameObject.Find("TotalProgression").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(TotalPoints).ToString();
        GameObject.Find("WinNextLevel").GetComponent<TextMeshProUGUI>().text = "Touch to start level " + level.ToString();
    }

    public void LoseHandling()
    {
        ScoreHandler.singleton.startPoints = 0;
        TotalPoints = 0;
        SoundHandler.singleton.PlayGameOverSound();
        StartCoroutine(LoseScreen());
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //TotalPoints = 0;
        ScoreHandler.singleton.startPoints = TotalPoints;
        //Debug.Log("start points: " + levelStartpoints.ToString());
    }

    private IEnumerator LoseScreen()
    {
        Destroy(player);
        yield return new WaitForSecondsRealtime(.5f);
        PauseGame(true);
    }
    private IEnumerator WinScreen()
    {
        yield return new WaitForSecondsRealtime(.8f);
        player.SetActive(false);
        Win();
    }
    
    public void ShowObject(GameObject Object)
    {
        Object.SetActive(true);
    }

    public void HideObject(GameObject Object)
    {
        Object.SetActive(false);
    }

    public void CreditUI(GameObject credits)
    {
        GameObject GameOver;
        GameObject Victory;
        
        if (GameOver = GameObject.Find("UI_GameOver"))
        {
            pauseUI.SetActive(false);
            credits.SetActive(true);
            CreditUIBackButtonToGameOver = true;
        }
        else if (Victory = GameObject.Find("UI_Victory"))
        {
            victoryScreen.SetActive(false);
            credits.SetActive(true);
            CreditUIBackButtonToGameOver = false;
        }
        else if (!(GameOver = GameObject.Find("UI_GameOver")) && !(Victory = GameObject.Find("UI_Victory")))
        {
            if (CreditUIBackButtonToGameOver)
            {
                credits.SetActive(false);
                pauseUI.SetActive(true);
            }
            else
            {
                credits.SetActive(false);
                victoryScreen.SetActive(true);
            }
        }
    }

    public void TurnSound()
    {
        SoundHandler.singleton.TurnSound();
    }

    private void GenerateMilestones(int amount)
    {
        float levelLength = GameObject.Find("Finish(Clone)").transform.position.y;
        float distanceBtwnMilestones = levelLength / amount;
        for (int i = 0; i < amount; i++)
        {
            float ypos = i * distanceBtwnMilestones;
            Vector2 pos = new Vector2(-5f, ypos);
            Vector2 pos2 = new Vector2(5f, ypos);
            GameObject GO = Instantiate(completionTextGO, pos, Quaternion.identity, progressionCanvas);
            GameObject GO2 = Instantiate(completionTextGO, pos2, Quaternion.identity, progressionCanvas);
            string text = (Mathf.RoundToInt(distanceBtwnMilestones / levelLength * 100) * i).ToString();
            GO.GetComponent<TextMeshProUGUI>().text = text;
            GO.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;
            GO2.GetComponent<TextMeshProUGUI>().text = text;
            GO2.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        }
    }

    public void AddScore(float amount)
    {
        TotalPoints += amount;
    }
}
