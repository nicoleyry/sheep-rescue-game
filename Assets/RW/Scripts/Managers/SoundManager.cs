using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance;

	[Header("Audio Clips")]
	public AudioClip shootClip;
	public AudioClip sheepHitClip;
	public AudioClip sheepDroppedClip;

	[Header("UI References")]
    public Image muteButtonImage;
    public Sprite muteIcon;
    public Sprite unmuteIcon;

	private Vector3 cameraPosition;

    private static bool isMuted = false;

	void Awake()
	{
		Instance = this;
		cameraPosition = Camera.main.transform.position;

		if (isMuted) {
			// Game is muted
			AudioListener.volume = 0;
			if (muteButtonImage != null) {
				muteButtonImage.sprite = muteIcon;
			}
		} else {
			// Game is not muted
			AudioListener.volume = 1;
			if (muteButtonImage != null) {
				muteButtonImage.sprite = unmuteIcon;
			}
		}
	}
	
    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted) {
            AudioListener.volume = 0;
            if (muteButtonImage != null) {
                muteButtonImage.sprite = muteIcon;
            }
        } else {
            AudioListener.volume = 1;
            if (muteButtonImage != null) {
                muteButtonImage.sprite = unmuteIcon;
            }
        }
    }

	private void PlaySound(AudioClip clip)
	{
		AudioSource.PlayClipAtPoint(clip, cameraPosition);
	}

	public void PlayShootClip()
	{
		PlaySound(shootClip);
	}

	public void PlaySheepHitClip()
	{
		PlaySound(sheepHitClip);
	}

	public void PlaySheepDroppedClip()
	{
		PlaySound(sheepDroppedClip);
	}
}
