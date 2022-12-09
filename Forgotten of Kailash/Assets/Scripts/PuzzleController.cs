using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleController : MonoBehaviour
{
    public int tasksTotal;
    public int tasksCorrect;

    public UnityEvent AllTrue;

    public void ChangeNumberBy(int change)
    {
        tasksCorrect += change;
        if (tasksCorrect >= tasksTotal)
            AllTrue.Invoke();
    }
    public void SkipPuzzle()
    {
        AllTrue.Invoke();
    }
}
