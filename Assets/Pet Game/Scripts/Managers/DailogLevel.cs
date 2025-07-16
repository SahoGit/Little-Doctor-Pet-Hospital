using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DailogLevel
{

    public string name;
    public Sprite CoustumerImage;
    public Sprite Pet;

    [TextArea(3, 10)]
    public string[] sentencesLevel;

    public int[] DoctorDailogeNumber;

}
