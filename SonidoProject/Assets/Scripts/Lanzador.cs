using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanzador : MonoBehaviour
{
    Vector3 startPos, endPos, direction;

    float touchTimeStart, touchTimeFinish, timeInterval;

    float throwForceInXAndY = 1f;
    float throwForceInZ = 500f;

    public GameObject objectToThrow;
    public GameObject whistleCracker;
    public GameObject rocket;

    Rigidbody rb;
    GameObject fireCracker;
    public GameObject strake;
    public CurrentCrackerText currentText;


    [FMODUnity.EventRef]
    public string inputsound;
    public FMOD.Studio.EventInstance evento_esfuerzo;
    FMOD.Studio.EventDescription XXXDescription;
    FMOD.Studio.PARAMETER_DESCRIPTION ParameterDescription;
    FMOD.Studio.PARAMETER_ID ParameterId;

    [FMODUnity.ParamRef]
    public FMOD.Studio.PARAMETER_ID esfuerzoID;
    private float esfuerzo;

    [FMODUnity.EventRef]
    public string colocarSound;
    public FMOD.Studio.EventInstance eventoColocar;




    private bool strakeCreated;
    private int numberOfCrackers;

    float number;

    public enum Crackers
    {
        CRACKER, WHISTLE, ROCKET
    }

    private Crackers currentCracker;

    private void Start()
    {
        rb = objectToThrow.GetComponent<Rigidbody>();
        fireCracker = null;
        strakeCreated = false;
        currentCracker = Crackers.CRACKER;
        currentText.SetText(currentCracker.ToString());
        number = 0;


        //Inicializacion dle sonido


       
       
       
        /*XXXDescription = FMODUnity.RuntimeManager.GetEventDescription(inputsound);
        XXXDescription.getParameterDescriptionByName("Esfuerzo", out ParameterDescription);
        ParameterId = ParameterDescription.id;*/
        evento_esfuerzo = FMODUnity.RuntimeManager.CreateInstance(inputsound);
        //evento_esfuerzo.getParameterByID(esfuerzoID, out esfuerzo);
        eventoColocar = FMODUnity.RuntimeManager.CreateInstance(colocarSound);
        //evento_esfuerzo.setParameterByName("Esfuerzo", 0.9f);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(evento_esfuerzo, this.transform, this.rb);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventoColocar, this.transform, this.rb);
        //evento_esfuerzo.start();

    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            currentCracker++;
            if ((int)currentCracker > 2)
            {
                currentCracker = Crackers.CRACKER;
            }
            currentText.SetText(currentCracker.ToString());
            //Debug.Log("A");
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            currentCracker--;
            if ((int)currentCracker < 0)
            {
                currentCracker = Crackers.ROCKET;
            }
            currentText.SetText(currentCracker.ToString());
            //Debug.Log("B");
        }
        if (currentCracker == Crackers.CRACKER)//hacer esfuerzo
        {
            if (Input.GetMouseButtonDown(0))
            {
                throwForceInZ = 500;
                currentCracker = Crackers.CRACKER;
                currentText.SetText(currentCracker.ToString());
            }
            if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    endPos = hit.point;

                }
                throwForceInZ += Time.deltaTime;
                throwForceInZ += 2;
               // Debug.Log(throwForceInZ);
                direction = this.transform.position - endPos;
                //startPos = Input.GetMouseButtonDown(0).
            }
            GameObject fireCracker = null;
            if (Input.GetMouseButtonUp(1))
            {

                esfuerzo = (throwForceInZ / 1250);
                Debug.Log(esfuerzo);
                //Sonido de la explosion
                //FMODUnity.RuntimeManager.PlayOneShot(inputsound , transform.position);
                evento_esfuerzo.setParameterByName("Esfuerzo", esfuerzo);
                evento_esfuerzo.start();
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(evento_esfuerzo, this.transform, this.rb);


                touchTimeFinish = Time.time;
                timeInterval = touchTimeFinish - touchTimeStart;
                fireCracker = Instantiate(objectToThrow, this.transform.position, this.transform.rotation);
                fireCracker.GetComponent<Rigidbody>().AddForce(new Vector3(Camera.main.transform.forward.x * throwForceInZ, Camera.main.transform.forward.y * throwForceInZ, Camera.main.transform.forward.z * throwForceInZ));
                fireCracker.GetComponent<Rigidbody>().AddTorque(fireCracker.transform.right * 25);
                fireCracker.GetComponent<Rigidbody>().useGravity = true;
                fireCracker.transform.Find("Mecha").GetComponentInChildren<GenerateEffect>().StartExplosionCoroutine(1);
                throwForceInZ = 500f;                
            }

        }
        else if (currentCracker == Crackers.WHISTLE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FMODUnity.RuntimeManager.PlayOneShot(colocarSound, transform.position);
                GameObject whistleCrackerInst = Instantiate(whistleCracker, this.transform.position+(transform.forward*2), transform.rotation);
                Debug.Log("AAS");
            }
        }

        else if(currentCracker == Crackers.ROCKET)//sonido al dejarlo
        {
            if (Input.GetMouseButtonDown(0))
            {
                FMODUnity.RuntimeManager.PlayOneShot(colocarSound, transform.position);
                GameObject rocketInst = Instantiate(rocket, this.transform.position + (transform.forward * 2), transform.rotation);
                rocketInst.transform.position = new Vector3(rocketInst.transform.position.x, rocketInst.transform.position.y + 1.0f, rocket.transform.position.z);
            }
        }

        var inputValue = Input.inputString;
        switch (inputValue)
        {
            case ("0"):
                numberOfCrackers = 2;
                strakeCreated = true;
                break;
            case ("1"):
                numberOfCrackers = 4;
                strakeCreated = true;
                break;
            case ("2"):
                numberOfCrackers = 6;
                strakeCreated = true;
                break;
            case ("3"):
                numberOfCrackers = 8;
                strakeCreated = true;
                break;
            case ("4"):
                numberOfCrackers = 10;
                strakeCreated = true;
                break;
            case ("5"):
                numberOfCrackers = 12;
                strakeCreated = true;
                break;
            case ("6"):
                numberOfCrackers = 14;
                strakeCreated = true;
                break;
            case ("7"):
                numberOfCrackers = 16;
                strakeCreated = true;

                break;
            case ("8"):
                numberOfCrackers = 18;
                strakeCreated = true;

                break;
            case ("9"):
                numberOfCrackers = 20;
                strakeCreated = true;

                break;
        }

        if (strakeCreated)
        {
            FMODUnity.RuntimeManager.PlayOneShot(colocarSound, transform.position);

            GameObject strakeInst = Instantiate(strake);
            strakeInst.GetComponent<GenerateStrake>().NewStrake(numberOfCrackers);
            strakeInst.transform.position = this.transform.position + (transform.forward * 2)/*new Vector3(this.transform.position.x, 0.5f, this.transform.position.z)*/;
            strakeInst.transform.position = new Vector3(strakeInst.transform.position.x, 0.5f, strakeInst.transform.position.z);
            strakeCreated = false;
        }
    }
}
