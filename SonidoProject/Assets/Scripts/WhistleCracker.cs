using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhistleCracker : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mecha;
    public ParticleSystem explosion;
    public ParticleSystem steam;

    public Transform positionMecha;
    public Transform endMecha;
    public GameObject mechaRoot;
    public Transform baseMecha;
    public Transform[] sparksBase;
    public GameObject[] sparks;

    private bool finished = true;
    private bool canDestroy;
    private bool created;


    [FMODUnity.EventRef]
    string sonidoMecha;
    FMOD.Studio.EventInstance eventoMecha;

    string silbido;
    FMOD.Studio.EventInstance EventoSilbido;

    string sonidoExpl;
    FMOD.Studio.EventInstance eventoExplosion;


    private GameObject prefabInst;
    void Start()
    {
        //Sonido de la mecha
        sonidoMecha = "event:/Mecha";
        silbido = "event:/Whistle";
        sonidoExpl= "event:/BigExplosionEvent";
        eventoMecha = FMODUnity.RuntimeManager.CreateInstance(sonidoMecha);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventoMecha, transform, this.GetComponent<Rigidbody>());
        eventoMecha.start();

        EventoSilbido = FMODUnity.RuntimeManager.CreateInstance(silbido);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(EventoSilbido, transform, this.GetComponent<Rigidbody>());


        eventoExplosion = FMODUnity.RuntimeManager.CreateInstance(sonidoExpl);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventoExplosion, transform, this.GetComponent<Rigidbody>());



        // mecha.SetActive(true);
        prefabInst = Instantiate<GameObject>(mecha, positionMecha.transform);
        prefabInst.transform.position = this.transform.position;
        Vector3 targetDirection = this.transform.localPosition - prefabInst.transform.localPosition;
        Vector3 newDirection = Vector3.RotateTowards(prefabInst.transform.forward, targetDirection, 0.0f, 0.0f);
        positionMecha.transform.localRotation = Quaternion.LookRotation(transform.up);
        positionMecha.localPosition = new Vector3(0, 2, 0);
        //Debug.Log(positionMecha.localPosition);
        ParticleSystem.MainModule main = explosion.main;
        main.prewarm = true;
      
        finished = false;
        canDestroy = false;
        created = false;

        sparks = new GameObject[4];
        //isCreated = false;*/
    }

    private void Update()
    {
        if (!finished)
        {
            float step = 0.2f * Time.deltaTime; // calculate distance to move
            positionMecha.localPosition = Vector3.MoveTowards(positionMecha.localPosition, baseMecha.localPosition, step);
            mechaRoot.transform.Translate(new Vector3(0, -0.035f * Time.deltaTime, 0f));
            //Debug.Log(Vector3.Distance(positionMecha.localPosition, endMecha.localPosition));
            if (Vector3.Distance(positionMecha.localPosition, baseMecha.localPosition) < 0.001f)
            {

                //Parar la mecha
                //Al parar la mecha la para muchisimas veces por lo que no podemos utilizarla mas veces
                eventoMecha.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
               
                Debug.Log("Para mecha");

                if (!created)
                {
                    ParticleSystem steamInstance;
                    steamInstance = Instantiate(steam, GameObject.Find("SteamPool").transform);
                    steamInstance.gameObject.transform.position = this.transform.position;

                    //El silbido
                    for (int i = 0; i < 4; i++)
                    {
                        //Cuatro mechas

                        GameObject sparksInst = Instantiate(mecha);
                        sparksInst.gameObject.transform.position = sparksBase[i].position;
                        sparksInst.transform.localRotation = Quaternion.LookRotation(transform.up);
                        sparks[i] = sparksInst;
                        FMODUnity.RuntimeManager.PlayOneShot(silbido, sparks[i].transform.position);
                    }
                    created = true;
                }
                StartExplosionCoroutine();
                // Swap the position of the cylinder.
            }
        }
        //positionMecha.Translate(new Vector3(positionMecha.localPosition.x, positionMecha.localPosition.y - Time.deltaTime * 0.2f, positionMecha.localPosition.z));
    }

    IEnumerator startExplosion()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.transform.root.gameObject);
        for(int i = 0; i < 4; i++)
        {

            //Parar todas las mechas
           

            Destroy(sparks[i]);
        }
        finished = false;
        Destroy(prefabInst);
        finished = true;
        ParticleSystem explosionInstance;
        
      //  EventoSilbido.start();
        eventoExplosion.start();
        //Sonido de la explosion

        explosionInstance = Instantiate(explosion, GameObject.Find("ExplosionPool").transform);
        explosionInstance.gameObject.transform.position = this.transform.position;
        ParticleSystem.MainModule main = explosion.main;
        main.loop = false;
        ParticleSystem steamInst = Instantiate(steam, GameObject.Find("SteamPool").transform);
        steamInst.transform.position = this.transform.position;
        ParticleSystem.MainModule mainSt = steamInst.main;
        mainSt.startSize = Random.Range(4, 8);
        mainSt.prewarm = true;
    }

    public void StartExplosionCoroutine()
    {



        StartCoroutine(startExplosion());
    }

    public bool CanDestroy()
    {
        return canDestroy;
    }
}
