using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    public GameObject talkUI;
    //public GameObject Button;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerControl")
        {
           talkUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       talkUI.SetActive(false);
    }
    /*
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter)&& gameObject.tag== "PlayerControl")
        {
            talkUI.SetActive(true);
        }
    }
    */
}
