using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttons : MonoBehaviour
{

    private Animator animator;
    public AudioSource clickNoise;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    public void spinCogs()
    {
        playClickSound();
        animator.SetBool("spin", true);
    }

    public void stopCogs()
    {
        pauseClickSound();
        animator.SetBool("spin", false);
    }

    public void playClickSound()
    {
        clickNoise.Play();
    }
    
    public void pauseClickSound()
    {
        clickNoise.Pause();
    }
}
