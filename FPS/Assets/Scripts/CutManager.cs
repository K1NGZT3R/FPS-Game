using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutManager : MonoBehaviour
{
    public GameObject ufo;

    public Animator anim;

    public void Start()
    {
        anim = ufo.GetComponent<Animator>();
        anim.enabled = false;
    }

    public void abductCut()
    {
        
    }
}
