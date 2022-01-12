using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogButtonController : MonoBehaviour
{
    public Animator buttonAnimator;

    
    // Start is called before the first frame update
    void Start()
    {
        buttonAnimator = this.gameObject.GetComponent<Animator>();
    }

    public void spinCog()
    {
        buttonAnimator.SetBool("hover", true);
    }

    public void stopSpinCog()
    {
        buttonAnimator.SetBool("hover", false);
    }
}


