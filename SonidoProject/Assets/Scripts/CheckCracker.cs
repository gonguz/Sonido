using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckCracker : MonoBehaviour
{

    public ParticleSystem splash;

    [FMODUnity.EventRef]
    public string inputsound;
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.GetComponent<Collider>().tag == "Petardo")
        {
            collision.gameObject.transform.Find("Mecha").GetComponentInChildren<GenerateEffect>().CancelExplosion();
            collision.gameObject.transform.Find("Mecha").GetComponentInChildren<GenerateEffect>().DestroyMecha();
            ParticleSystem pS = Instantiate(splash);
            pS.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y-0.5f, collision.transform.position.z);
            FMODUnity.RuntimeManager.PlayOneShot(inputsound, transform.position);
        }
    }
}
