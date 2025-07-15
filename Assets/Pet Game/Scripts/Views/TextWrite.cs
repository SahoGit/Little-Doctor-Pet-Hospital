using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWrite : MonoBehaviour
{
    Text txt;
    string story;

    void Awake()
    {
        txt = GetComponent<Text>();
        story = txt.text;
        txt.text = "";
        StartCoroutine("PlayText");
    }

    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            //SoundManager.PlaySound(SoundManager.NameOfSounds.typing); 
            txt.text += c;
            yield return new WaitForSeconds(0.125f);
        }
    }
}
