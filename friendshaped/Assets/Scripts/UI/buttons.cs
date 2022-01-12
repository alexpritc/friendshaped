using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttons : MonoBehaviour
{

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    public void spinCogs()
    {
        animator.SetBool("spin", true);
    }

    public void stopCogs()
    {
        animator.SetBool("spin", false);
    }
}
