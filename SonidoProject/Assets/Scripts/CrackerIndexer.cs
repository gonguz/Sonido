using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackerIndexer : MonoBehaviour
{
    // Start is called before the first frame update
    private int index;
    
    public void SetIndex(int i)
    {
        index = i;
    }
    public int GetIndex()
    {
        return index;
    }
}
