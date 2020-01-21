using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Steeps : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string inputsound;
    FMOD.Studio.EventInstance evento;

    [FMODUnity.EventRef]
    public string inputsoundOuch;
    FMOD.Studio.EventInstance eventoOuch;

    [FMODUnity.EventRef]
    public string ambienceMusic;
    FMOD.Studio.EventInstance eventoAmbience;
    float zone;

    FMOD.Studio.PARAMETER_ID waterSteeps;

    FMODUnity.StudioEventEmitter pasitos;


    float waterValue;

    bool playerismoving;

    public float walkingSpeed;






    void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventoAmbience, this.transform, this.GetComponent<Rigidbody2D>());
        evento.setParameterByID(waterSteeps, (float)waterValue);
        eventoAmbience.setParameterByName("Zone", zone);


        if (Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Vertical") <= -0.01f || Input.GetAxis("Horizontal") <= -0.01f)
        {
            //Debug.Log ("Player is moving");
            playerismoving = true;
        }
        else if (Input.GetAxis("Vertical") == 0 || Input.GetAxis("Horizontal") == 0)
        {
            //Debug.Log ("Player is not moving");
            playerismoving = false;
        }


    }


    void CallFootsteps()
    {

        if (playerismoving == true)
        {
            //Debug.Log ("Player is moving");
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //e.setParameterByName("Velocidad", 1f);
                // FMODUnity.RuntimeManager.PlayOneShot(inputsound);
                evento.start();
            }
        }

    }
    void CallFootstepsfast()
    {

        if (playerismoving == true)
        {
            //Debug.Log ("Player is moving");
            if (!Input.GetKey(KeyCode.LeftShift))
            {

                //  FMODUnity.RuntimeManager.PlayOneShot(inputsound);
                evento.start();

            }
        }

    }

    void Awake()
    {

        //e = FMODUnity.RuntimeManager.CreateInstance(inputsound);
        //        e2 = GetComponent<FMODUnity.StudioEventEmitter>();


    }
    void Start()
    {
        evento = FMODUnity.RuntimeManager.CreateInstance(inputsound); 
        evento.getParameterByName("Tipos", out waterValue);

        FMOD.Studio.EventDescription waterDescription;
        evento.getDescription(out waterDescription);
        FMOD.Studio.PARAMETER_DESCRIPTION waterparameterDescription;
        waterDescription.getParameterDescriptionByName("Tipos", out waterparameterDescription);
        waterSteeps = waterparameterDescription.id;

        eventoOuch = FMODUnity.RuntimeManager.CreateInstance(inputsoundOuch);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventoOuch, transform, this.GetComponent<Rigidbody>());


        eventoAmbience = FMODUnity.RuntimeManager.CreateInstance(ambienceMusic);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventoAmbience, this.transform, this.GetComponent<Rigidbody>());
        zone = 2;
        eventoAmbience.start();
        eventoAmbience.setVolume(0.25f);



        //e.getParameterByName("WaterSteeps",out waterValue);
        //pasitos = GetComponent<FMODUnity.StudioEventEmitter>(); 


        InvokeRepeating("CallFootsteps", 0, walkingSpeed);
        InvokeRepeating("CallFootstepsfast", 0, walkingSpeed * 2);
    }


    void OnDisable()
    {
        playerismoving = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            Debug.Log("Aguita pal cuerpo");
            waterValue = 1;

        }
        else if (other.gameObject.tag == "Grass")
        {
            Debug.Log("Pisando Hierbita");
            waterValue = 2;
            
        }
        else if(other.gameObject.tag == "Fire")
        {
            Debug.Log("ENTRO");
            FMODUnity.RuntimeManager.PlayOneShot(inputsoundOuch, transform.position);
        }
        else if(other.gameObject.tag == "Sand")
        {
            Debug.Log("Pisando tierra");
            waterValue = 3;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            Debug.Log("Fuera del aguita");
            waterValue = 0; 
        }
        else if (other.gameObject.tag == "Grass")
        {
            Debug.Log("fuera de la Hierbita");
            waterValue = 0;  
        }
        else if (other.gameObject.tag == "Fire")
        {
            //eventoOuch.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        else if (other.gameObject.tag == "Sand")
        {
            Debug.Log("Pisando tierra");
            waterValue = 0;
        }
    }
}
