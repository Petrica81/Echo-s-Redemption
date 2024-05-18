using System.Collections;
using UnityEngine;

public abstract class BaseButtons : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The sound played when a button click event occurs.")]
    private AudioClip clickSound;
    protected AudioSource audioSource;

    private void Start()
    {
        if(gameObject.GetComponent<AudioSource>())
            audioSource = gameObject.GetComponent<AudioSource>();
        else
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
    protected void ButtonSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
