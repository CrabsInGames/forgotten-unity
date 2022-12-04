using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleController : MonoBehaviour
{
    public int tasksTotal;
    public int tasksCorrect;

    public void ChangeNumberBy(int change)
    {
        tasksCorrect += change;
        if (tasksCorrect >= tasksTotal)
            AllTrue.Invoke();
        else if (tasksCorrect <= 0)
            AllFalse.Invoke();
    }

    public UnityEvent AllTrue;
    public UnityEvent AllFalse;
}
