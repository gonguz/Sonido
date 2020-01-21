using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentCrackerText : MonoBehaviour
{

    public void SetText(string cracker)
    {
        this.GetComponent<UnityEngine.UI.Text>().text = "Cracker selected: " + cracker;
    }
}
