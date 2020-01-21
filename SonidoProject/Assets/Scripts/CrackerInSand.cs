using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackerInSand : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.GetComponent<Collider>().tag == "Petardo" || collision.GetComponent<Collider>().tag == "Petardo")
        {
            collision.gameObject.transform.Find("Mecha").GetComponentInChildren<GenerateEffect>().SetSteam();
            Debug.Log("SAND");
        }
    }
}
