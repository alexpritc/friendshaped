using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LoopManager : MonoBehaviour {
    private void Start() {
        GameManager.Instance.onLoopComplete += OnLoopComplete;
    }
    
    private void OnLoopComplete() {
        Debug.Log("restarting loop...");
    }
}
