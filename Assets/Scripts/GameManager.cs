using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text blockCounterText;
    [SerializeField] private Text recordText;
    [SerializeField] private float thresholdSpeed = 3f;

    [SerializeField] private Image flagImage;
    [SerializeField] private Sprite russianFlag;
    [SerializeField] private Sprite englishFlag;
    [SerializeField] private Text gameOverText;

    private int blockCount;
    private int maxBlocks;
    private int language;
    public bool isGameOver { get; private set; }

    private void Start()
    {
        language = PlayerPrefs.GetInt("language", 0);
        LoadRecord();
        UpdateUI();
        AudioManager.instance?.PlayMusic(AudioManager.instance.backgroundMusic, 0.4f);
    }

    private void Update()
    {
        if (isGameOver) return;

        if (IsTowerUnstable())
        {
            TriggerGameOver();
        }
    }

    private bool IsTowerUnstable()
    {
        foreach (var cube in FindObjectsOfType<Rigidbody>())
        {
            if (cube.velocity.magnitude > thresholdSpeed)
            {
                return true;
            }
        }
        return false;
    }

    public void IncrementBlockCount()
    {
        blockCount++;
        UpdateBlockCounter();
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        UpdateRecordIfNeeded();

        gameOverPanel.SetActive(true);
        gameOverText.text = GetLocalizedGameOverText();

        CubeStacker cubeStacker = FindObjectOfType<CubeStacker>();
        if (cubeStacker != null)
        {
            cubeStacker.DisableMarker();
        }

        var cameraMover = Camera.main.GetComponent<CameraMover>();
        if (cameraMover != null)
        {
            cameraMover.enabled = false;
        }

        StartCoroutine(PlayGameOverSoundWithDelay(1f));
    }
    private IEnumerator PlayGameOverSoundWithDelay(float delay)
    {
        AudioManager.instance?.PlayGameOverSound();
        yield return new WaitForSeconds(delay);
        AudioManager.instance?.StopMusic();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        AudioManager.instance?.PlayMusic(AudioManager.instance.backgroundMusic, 0.5f);
    }

    private void UpdateRecordIfNeeded()
    {
        if (blockCount > maxBlocks)
        {
            maxBlocks = blockCount;
            SaveRecord();

        }
        UpdateRecordText();
    }

    private void SaveRecord()
    {
        PlayerPrefs.SetInt("MaxBlocks", maxBlocks);
        PlayerPrefs.Save();
    }

    private void LoadRecord()
    {
        maxBlocks = PlayerPrefs.GetInt("MaxBlocks", 0);
    }

    private void UpdateUI()
    {
        UpdateBlockCounter();
        UpdateRecordText();
    }

    public void UpdateBlockCounter()
    {
        string blockCountText = GetLocalizedBlockCountText();
        blockCounterText.text = $"{blockCountText}: {blockCount}";
    }

    public void UpdateRecordText()
    {
        string recordTextKey = GetLocalizedRecordText();
        recordText.text = $"{recordTextKey}: {maxBlocks}";
    }
    private string GetLocalizedBlockCountText()
    {
        switch (language)
        {
            case 1:
                return "TOTAL BLOCKS";
            default:
                return "¬—≈√Œ ¡ÀŒ Œ¬";
        }
    }

    private string GetLocalizedRecordText()
    {
        switch (language)
        {
            case 1:
                return "YOUR RECORD";
            default:
                return "¬¿ÿ –≈ Œ–ƒ";
        }
    }

    private string GetLocalizedGameOverText()
    {
        switch (language)
        {
            case 1:
                return "GAME OVER";
            default:
                return "»√–¿ «¿¬≈–ÿ≈Õ¿";
        }
    }
    private void UpdateFlagIcon()
    {
        flagImage.sprite = language == 0 ? russianFlag : englishFlag;
    }

    public void ChangeLanguage()
    {
        language = (language + 1) % 2;
        PlayerPrefs.SetInt("language", language);
        PlayerPrefs.Save();

        UpdateUI();
        UpdateFlagIcon();
    }
}