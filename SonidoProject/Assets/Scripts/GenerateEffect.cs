using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mecha;
    public ParticleSystem explosion;
    public ParticleSystem strakeExplosion;
    public ParticleSystem steam;
    public ParticleSystem steamDust;

    public Transform positionMecha;
    public Transform endMecha;
    public GameObject mechaRoot;
    public GameObject strakePool;

    private bool finished;
    private bool canDestroy;

    private bool cancelExplosion;
    //private bool isCreated;

    //Sonidos
    [FMODUnity.EventRef]
    string sonidoMecha;
    FMOD.Studio.EventInstance eventoMecha;

    string sonidoExpl;
    FMOD.Studio.EventInstance eventoExpl;

    bool EnAgua;


    private GameObject prefabInst;
    void Start() //Sonido de la mecha
    {
        EnAgua = false;
        sonidoMecha = "event:/Mecha";
        sonidoExpl = "event:/Explosion1";
        //Inicializacion del sonido 

        eventoMecha = FMODUnity.RuntimeManager.CreateInstance(sonidoMecha);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventoMecha, transform, this.GetComponent<Rigidbody>());
        eventoMecha.start();

        eventoExpl = FMODUnity.RuntimeManager.CreateInstance(sonidoExpl);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventoExpl, transform, this.GetComponent<Rigidbody>());



        mecha.SetActive(true);
        prefabInst = Instantiate<GameObject>(mecha, positionMecha.transform);
        prefabInst.transform.position = this.transform.position;
        Vector3 targetDirection = this.transform.localPosition - prefabInst.transform.localPosition;
        Vector3 newDirection = Vector3.RotateTowards(prefabInst.transform.forward, targetDirection, 0.0f, 0.0f);
        positionMecha.transform.localRotation = Quaternion.LookRotation(transform.up);
        positionMecha.localPosition = new Vector3(0, 2, 0);
        //Debug.Log(positionMecha.localPosition);
        ParticleSystem.MainModule main = explosion.main;
        main.prewarm = true;

        finished = true;
        canDestroy = false;
        cancelExplosion = false;
        //isCreated = false;
    }

    private void Update()
    {
        if (!cancelExplosion)
        {
            //Debug.Log("ENTRO");
            if (!finished)
            {
                //Debug.Log("222");
                float step = 0.2f * Time.deltaTime; // calculate distance to move
                positionMecha.localPosition = Vector3.MoveTowards(positionMecha.localPosition, endMecha.localPosition, step);
                mechaRoot.transform.Translate(new Vector3(0, -0.035f * Time.deltaTime, 0f));
                //Debug.Log(Vector3.Distance(positionMecha.localPosition, endMecha.localPosition));
                if (Vector3.Distance(positionMecha.localPosition, endMecha.localPosition) < 0.001f)
                {

                    //Parar sonido de la mecha
                    eventoMecha.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    Debug.Log("Para mecha");
                    //Sonido de la explosion abajo                  



                    // Swap the position of the cylinder.
                    Destroy(prefabInst);
                    finished = true;
                    ParticleSystem explosionInstance;
                    if (this.transform.root.gameObject.tag != "Strake")
                    {
                        explosionInstance = Instantiate(explosion, GameObject.Find("ExplosionPool").transform);
                    }
                    else
                    {
                        explosionInstance = Instantiate(strakeExplosion, GameObject.Find("ExplosionPool").transform);
                    }


                    //SOnido de explosion(This.tranform.position)
                    FMODUnity.RuntimeManager.PlayOneShot(sonidoExpl, transform.position);



                    explosionInstance.gameObject.transform.position = this.transform.position;
                    ParticleSystem.MainModule main = explosion.main;
                    main.loop = false;
                    ParticleSystem steamInstance = Instantiate(steam, GameObject.Find("SteamPool").transform);
                    steamInstance.transform.position = this.transform.position;
                    ParticleSystem.MainModule mainSt = steamInstance.main;
                    mainSt.startSize = Random.Range(4, 8);
                    mainSt.prewarm = true;
                    if (this.transform.root.gameObject.tag != "Strake")
                    {
                        Destroy(this.transform.root.gameObject);
                    }
                    else
                    {
                        canDestroy = true;
                    }
                }
            }
            //cancelExplosion = true;
        }
        //positionMecha.Translate(new Vector3(positionMecha.localPosition.x, positionMecha.localPosition.y - Time.deltaTime * 0.2f, positionMecha.localPosition.z));
    }
    public void CancelExplosion()
    {
        cancelExplosion = true;
        eventoMecha.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Debug.Log("Canceled");
    }
    IEnumerator startExplosion(int i)
    {
        yield return new WaitForSeconds(Random.Range(0.5f,0.9f)*i);
        finished = false;
    }

    public void StartExplosionCoroutine(int i)
    {
        StartCoroutine(startExplosion(i));
    }

    public bool CanDestroy()
    {
        return canDestroy;
    }

    public void DestroyMecha()
    {
        Destroy(prefabInst);
    }

    public void SetSteam()
    {
        steam = steamDust;
    }

}
