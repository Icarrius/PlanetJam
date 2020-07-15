using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundHandler : MonoBehaviour
{
    public static SoundHandler singleton;
    public AudioSource currentSource;

    [SerializeField]
    private AudioSource uiClickSound, coinSound, menuMusic, gameMusic, gameOverSound, levelSuccessSound, planetAttachSound;

    private bool soundOn = true;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        if(soundOn)
            menuMusic.Play();

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    public void PlayUiSound()
    {
        if(soundOn)
            uiClickSound.Play();
    }

    public void PlayCoinSound()
    {
        if (soundOn)
            coinSound.Play();
    }

    public void PlayGameOverSound()
    {
        if (soundOn)
            gameOverSound.Play();
    }

    public void PlayLevelSuccess()
    {
        if (soundOn)
            levelSuccessSound.Play();
    }

    public void PlayPlanetAttach()
    {
        if (soundOn)
            planetAttachSound.Play();
    }

    private void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        if (!soundOn) return;

        if (scene.buildIndex == 0)
        {
            menuMusic.Play();
            currentSource = menuMusic;
            gameMusic.Stop();
        }
        else
        {
            menuMusic.Stop();
            if (!gameMusic.isPlaying)
            {
                gameMusic.Play();
                currentSource = gameMusic;
            }
        }
    }

    public void TurnSound()
    {
        soundOn = !soundOn;
        if (soundOn)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                menuMusic.Play();
                currentSource = menuMusic;
                gameMusic.Stop();
            }
            else
            {
                menuMusic.Stop();
                gameMusic.Play();
                currentSource = gameMusic;
            }
        }
        else
        {
            menuMusic.Stop();
            gameMusic.Stop();
        }
    }
}
