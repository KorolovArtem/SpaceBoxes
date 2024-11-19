using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [SerializeField] private Image soundIcon;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    void Start()
    {
        UpdateButtonIcon();
    }

    public void ToggleMute()
    {
        AudioManager.instance.ToggleMute();
        UpdateButtonIcon();
    }

    private void UpdateButtonIcon()
    {
        if (AudioManager.instance.IsMuted())
        {
            soundIcon.sprite = soundOffSprite;
        }
        else
        {
            soundIcon.sprite = soundOnSprite;
        }
    }
}