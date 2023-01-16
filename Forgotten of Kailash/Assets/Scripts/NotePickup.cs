using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePickup : MonoBehaviour
{
    public GameObject journalPage;
    public AudioSource audio;

    private void Start()
    {
        if (!audio)
            audio = GetComponent<AudioSource>();
    }

    public void PickUp()
    {
        journalPage.SetActive(true);
        audio.Play();
        Destroy(gameObject, audio.clip.length);
    }
}
