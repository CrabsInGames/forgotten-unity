using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePickup : MonoBehaviour
{
    public GameObject journalPage;

    public void PickUp()
    {
        journalPage.SetActive(true);
        Destroy(gameObject);
    }
}
