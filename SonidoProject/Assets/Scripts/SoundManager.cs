using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMOD;


public class SoundManager : MonoBehaviour
{
    public static SoundManager sm;


    FMOD.Studio.System system;
    FMOD.System lowLevelSyst;
    FMOD.Studio.Bank masterBank;

    string path;

    private void Awake()
    {
        sm = this;
        FMOD.Studio.System.create(out sm.system);

        //
        sm.lowLevelSyst.setSoftwareFormat(0, SPEAKERMODE._5POINT1, 0);
        sm.system.initialize(1024, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, System.IntPtr.Zero);
        path = Application.dataPath + "/Sounds";
    }

    public void loadSound(string name,out FMOD.Sound sound)
    {

        sm.lowLevelSyst.createSound(path + name, FMOD.MODE._3D | FMOD.MODE.LOOP_NORMAL, out sound);

    }



}
