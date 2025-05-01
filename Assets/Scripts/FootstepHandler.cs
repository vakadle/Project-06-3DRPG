using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepHandler : MonoBehaviour
{
    [Tooltip("The AudioClip to play for each footstep.")]
    public AudioClip footstepClip;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Called by an Animation Event on your walk/run animation.
    /// Plays the footstep sound without spamming the console.
    /// </summary>
    public void OnFootstep()
    {
        if (footstepClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(footstepClip);
        }
    }
}