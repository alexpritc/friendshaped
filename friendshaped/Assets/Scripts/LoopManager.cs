using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LoopManager : MonoBehaviour {

    private int actionsTaken = 0;
    private int actionsLimit = 3;
    
    private void Start() {
        GameManager.Instance.onLoopComplete += OnLoopComplete;
        GameManager.Instance.onAllActionsComplete += OnAllActionsComplete;
        GameManager.Instance.onActionCompleted += OnActionCompleted;
    }
    
    private void OnLoopComplete() {
        Debug.Log("restarting loop...");
    }
    
    private void OnAllActionsComplete() {
        Debug.Log("all actions completed");
        OnLoopComplete();
    }

    private void OnActionCompleted() {
        actionsTaken++;
        Debug.Log(string.Format("completed action #{0}", actionsTaken));
        if (actionsTaken >= actionsLimit) {
            OnAllActionsComplete();
        }
    }

    private void OnDestroy() {
        GameManager.Instance.onLoopComplete -= OnLoopComplete;
        GameManager.Instance.onAllActionsComplete -= OnAllActionsComplete;
        GameManager.Instance.onActionCompleted -= OnActionCompleted;
    }
}
