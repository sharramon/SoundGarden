using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using DG.Tweening;
using Unity.Mathematics;

public class Flower : GrabFreeTransformer
{
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 lastScale;

    private bool positionChange;
    private bool scaleChange;

    private AudioSource _source;
    
    void Start()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        lastScale = transform.localScale;
        _source = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (HasTransformChanged())
        {
            OnTransformChanged();
        }
    }
    
    private bool HasTransformChanged()
    {
        if (transform.position != lastPosition || transform.rotation != lastRotation || transform.localScale != lastScale)
        {
            if (transform.position != lastPosition)
            {
                positionChange = true;
            }

            if (transform.localScale != lastScale)
            {
                scaleChange = true;
            }
            
            lastPosition = transform.position;
            lastRotation = transform.rotation;
            lastScale = transform.localScale;
            return true;
        }
        return false;
    }

    private RaycastHit hit;
    private void OnTransformChanged()
    {
        if (positionChange)
        {
            if(Physics.Raycast(transform.position,Vector3.down,out hit,0.01f))
            {
                Debug.Log($"Hit : {hit.transform.name}");
                transform.rotation = quaternion.identity;
                transform.DOMove(hit.point,0.5f);
            }
        }

        if (scaleChange)
        {
            Debug.Log(_source.volume);
        }
        
    }
}
