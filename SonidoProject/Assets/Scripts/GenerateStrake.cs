using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStrake : MonoBehaviour
{

    public GameObject fireCracker;
    //public int nCrackers;
    private GameObject[] crackersArray;
    private int rotateValue;

    private bool strakeCreated;
    // Start is called before the first frame update


    public GameObject GetCrackerOfStrake(int i)
    {
        return crackersArray[i];
    }

    public void NewStrake(int nCrackers)
    {
        crackersArray = new GameObject[nCrackers];
        rotateValue = -90;
        /*for(int i = 0; i < nCrackers; i++)
        {
            GameObject crackerInst = Instantiate(fireCracker, this.transform);
            crackerInst.transform.position = new Vector3(this.transform.position.x +(crackerInst.GetComponent<Collider>().bounds.size.x*i/nCrackers), this.transform.position.y, this.transform.position.z+crackerInst.GetComponent<Collider>().bounds.size.y);
            crackerInst.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints.FreezeRotationX & RigidbodyConstraints.FreezeRotationY & RigidbodyConstraints.FreezeRotationZ) & RigidbodyConstraints.FreezePosition;
            crackerInst.transform.Rotate(-90, 0, 0);
            crackersArray[i] = crackerInst;
            //StartCoroutine(startExplosion(i));
            crackersArray[i].transform.Find("Mecha").GetComponentInChildren<GenerateEffect>().StartExplosionCoroutine(i+1);
        }*/
        int indI = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < nCrackers - (nCrackers / 2); j++)
            {
                GameObject crackerInst = Instantiate(fireCracker, this.transform);
                crackerInst.transform.position = new Vector3(this.transform.position.x + (crackerInst.GetComponent<Collider>().bounds.size.x * j / (nCrackers - nCrackers / 2)), this.transform.position.y, this.transform.position.z - crackerInst.GetComponent<Collider>().bounds.size.y * i * 1.65f);
                crackerInst.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints.FreezeRotationX & RigidbodyConstraints.FreezeRotationY & RigidbodyConstraints.FreezeRotationZ) & RigidbodyConstraints.FreezePosition;
                crackerInst.transform.Rotate(rotateValue, 0, 0);
                crackerInst.GetComponent<CrackerIndexer>().SetIndex(indI);
                crackersArray[indI] = crackerInst;
                //StartCoroutine(startExplosion(i));
                crackersArray[indI].transform.Find("Mecha").GetComponentInChildren<GenerateEffect>().StartExplosionCoroutine(indI);
                indI++;
            }
            rotateValue = 90;
        }
        strakeCreated = true;
    }

    private void Update()
    {
        if (strakeCreated)
        {
            for (int i = 0; i < crackersArray.Length; i++)
            {
                if (crackersArray[i] != null && crackersArray[i].transform.Find("Mecha").GetComponentInChildren<GenerateEffect>().CanDestroy())
                {
                    Debug.Log(crackersArray[i].gameObject.name);
                    Destroy(crackersArray[i].gameObject);
                }
            }
        }
    }
}
