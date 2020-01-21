using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mecha;
    public ParticleSystem explosion;
    public ParticleSystem rocketEffect;
    public ParticleSystem steam;
    private ParticleSystem rocketEffInst;

    public Transform positionMecha;
    public Transform endMecha;
    public GameObject mechaRoot;
    public Transform baseMecha;

    private float rocketTime;

    private bool finished = true;
    private bool canDestroy;
    private bool created;
    private bool rocketCreated;
    private bool explosionCreated;

    [FMODUnity.EventRef]
    string sonidoMecha;
    FMOD.Studio.EventInstance eventoMecha;

    string sonidoExpl;
    FMOD.Studio.EventInstance eventoExpl;

    string sonidoSubida;
    FMOD.Studio.EventInstance eventoSubida;



    private GameObject prefabInst;
    void Start()
    {
        //Poner mecha 
        sonidoMecha = "event:/Mecha";
        eventoMecha = FMODUnity.RuntimeManager.CreateInstance(sonidoMecha);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventoMecha, transform, this.GetComponent<Rigidbody>());
        eventoMecha.start();

        sonidoExpl = "event:/ExplosionPlasma";
        eventoExpl = FMODUnity.RuntimeManager.CreateInstance(sonidoExpl);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventoExpl, transform, this.GetComponent<Rigidbody>());

        sonidoSubida = "event:/Despegar";
        eventoSubida = FMODUnity.RuntimeManager.CreateInstance(sonidoSubida);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(eventoSubida, transform, this.GetComponent<Rigidbody>());

        // mecha.SetActive(true);
        prefabInst = Instantiate<GameObject>(mecha, positionMecha.transform);
        prefabInst.transform.position = positionMecha.transform.position;
        Vector3 targetDirection = this.transform.localPosition - prefabInst.transform.localPosition;
        Vector3 newDirection = Vector3.RotateTowards(prefabInst.transform.forward, targetDirection, 0.0f, 0.0f);
        positionMecha.transform.localRotation = Quaternion.LookRotation(new Vector3(0, -90, 0));
        //positionMecha.localPosition = new Vector3(0, 2, 0);
        //Debug.Log(positionMecha.localPosition);
        /*ParticleSystem.MainModule main = explosion.main;
        main.prewarm = true;*/

        finished = false;
        canDestroy = false;
        created = false;
        rocketCreated = false;
        explosionCreated = false;
    }

    private void Update()
    {
        if (!finished)
        {
            float step = 0.2f * Time.deltaTime; // calculate distance to move
            positionMecha.localPosition = Vector3.MoveTowards(positionMecha.localPosition, endMecha.localPosition, step);
            mechaRoot.transform.Translate(new Vector3(0, -0.035f * Time.deltaTime, 0f));
            //Debug.Log(Vector3.Distance(positionMecha.localPosition, endMecha.localPosition));
            if (Vector3.Distance(positionMecha.localPosition, endMecha.localPosition) < 0.001f)
            {

                //Parar la mecha
                eventoMecha.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                if (!created)
                {
                    ParticleSystem steamInstance;
                    steamInstance = Instantiate(steam, GameObject.Find("SteamPool").transform);
                    steamInstance.gameObject.transform.position = this.transform.position;
                    created = true;
                }


                StartExplosionCoroutine();
                // Swap the position of the cylinder.
            }
        }
        //Debug.Log(rocketEffInst.gameObject.transform.position);
        if (rocketEffInst != null)
        {
            rocketTime += Time.deltaTime;
            if (!explosionCreated)
            {
                if (rocketTime >= 1.05f)
                {
                    ParticleSystem explosionInstance;
                    explosionInstance = Instantiate(explosion, GameObject.Find("ExplosionPool").transform);

                    eventoExpl.setVolume(10.0f);
                    FMODUnity.RuntimeManager.PlayOneShot(sonidoExpl, transform.position);
                    //No se si  explota o no porque no se oye...

                    //Explosion


                    explosionInstance.gameObject.transform.position = this.transform.root.position;
                    ParticleSystem.MainModule main = explosion.main;
                    main.loop = false;
                    explosionCreated = true;
                    Destroy(this.transform.root.gameObject);
                }
            }
        }
        //positionMecha.Translate(new Vector3(positionMecha.localPosition.x, positionMecha.localPosition.y - Time.deltaTime * 0.2f, positionMecha.localPosition.z));
    }

    IEnumerator startExplosion()
    {
        yield return new WaitForSeconds(Random.Range(2.5f, 4));
        this.transform.root.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);

        if (!rocketCreated)
        {

            //Subida del cohete 1,5
            FMODUnity.RuntimeManager.PlayOneShot(sonidoSubida, transform.position);



            rocketEffInst = Instantiate(rocketEffect, GameObject.Find("RocketPool").transform);
            rocketEffInst.transform.position = endMecha.transform.position;
            rocketCreated = true;
            yield return new WaitForSeconds(0.3f);
            /*yield return new WaitForSeconds(Random.Range(0.5f, 2.5f));
            ParticleSystem explosionInstance;
            explosionInstance = Instantiate(explosion, GameObject.Find("ExplosionPool").transform);
            explosionInstance.gameObject.transform.position = this.transform.position;
            ParticleSystem.MainModule main = explosion.main;
            main.loop = false;
            //Debug.Log(rocketEffInst.gameObject.transform.position);
            //rocketEffect.transform.parent = this.transform.root;
            //this.transform.root.position = rocketEffect.transform.position;
            rocketCreated = true;*/
        }
        //Destroy(this.transform.root.gameObject);
        //Destroy(prefabInst);
        finished = true;
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
