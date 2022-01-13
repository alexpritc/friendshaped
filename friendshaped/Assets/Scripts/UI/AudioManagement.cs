using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    public GameObject[] audios;
    public GameObject trainAudio;
    public GameObject LadyAudio;
    public GameObject StewardAudio;
    public GameObject InspectorAudio;
    public GameObject ConductorAudio;
    public GameObject StoctorAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        audios = new GameObject[] { trainAudio, LadyAudio, StewardAudio, InspectorAudio, ConductorAudio, StoctorAudio };
    }

    public void changeAudio(GameObject newAudio)
    {
        // give it an audio gameobject - enable that, disable the rest
        foreach(GameObject gameObj in audios)
        {
            if(gameObj == newAudio)
            {
                gameObj.SetActive(true);
            }
            else
            {
                gameObj.SetActive(false);
            }
        }
    }
}
