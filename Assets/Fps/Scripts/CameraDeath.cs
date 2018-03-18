using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDeath : MonoBehaviour
{
    public LayerMask originalLayerMask;
    public LayerMask deathLayerMask;
    
    [SerializeField]
    private Vector3 offset;

    private Camera mainCam;
    private bool lookBody = false;

    private Transform lastParent;
    private Vector3 lastPostion;
    private Quaternion lastRotation;
    void Awake()
    {
        mainCam = Camera.main;
        originalLayerMask = mainCam.cullingMask;
       
    }

    public void LookAtDeathBody()
    {
        
        lastParent = mainCam.transform.parent;
        lastPostion = mainCam.transform.localPosition;
        lastRotation = mainCam.transform.localRotation;

        mainCam.transform.parent = null;
        foreach (Transform child in mainCam.transform)
        {
            child.gameObject.SetActive(false);
        }
        mainCam.cullingMask = deathLayerMask;
    
        lookBody = true;
    }


    public void Reset()
    {
        lookBody = false;
        mainCam.transform.parent = lastParent;
        mainCam.transform.localPosition = lastPostion;
        mainCam.transform.localRotation = lastRotation;

        foreach (Transform child in mainCam.transform)
        {
            child.gameObject.SetActive(true);
        }
        mainCam.cullingMask = originalLayerMask;
    }

    void Update()
    {
        if (lookBody)
        {
            mainCam.transform.position = transform.position + offset;
            mainCam.transform.LookAt(transform);
        }
    }

    
}
