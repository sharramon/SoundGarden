using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using Unity.Mathematics;
using DG.Tweening;
using Meta.WitAi.Events;

public class Seed : GrabFreeTransformer
{
    public GameObject bubble;
    public int currentInstrument = 0;

    public List<GameObject> seedList = new List<GameObject>();

    [Header("UI")]
    public SeedUI seedUI;

    [Header("Particles")]
    public SeedParticles seedParticles;

    [Header("Grab")]
    public Rigidbody rigidBody;
    public HandGrabInteractable interactable;
    public Collider seedCollider;
    public Action onGrab;
    public Action onRelease;
    public bool isGrabbed = false;

    public string seedTag = "Seed";

    [Header("Swipe")]
    public Swipeable swipeable;
    public Color selectColor;
    public Color baseColor;
    public Action onSwipeLeft;
    public Action onSwipeRight;

    public bool moveFlag = false;
    public float OriginRot_Y;
    public float nextValue = 10;
    private int currentIndex = 0;

    [Header("GameObjects")]
    public GameObject seedModel;
    public GameObject flowerModel;
    
    private TransformerUtils.ScaleConstraints FlowerConstraints;

    private FollowOffset followOffset;
    private DG.Tweening.Tween bubbleScaleUpTween;
    private DG.Tweening.Tween bubbleScaleDownTween;

    //mic
    public Action onSetToMic;
    public Action onMicArrived;
    private Coroutine moveCoroutine;

    //room collide
    public Action<GameObject> onGrowFlower;

    [Header("Room Coolide")]
    public bool isGrowable = false;
    
    private void OnEnable()
    {
        rigidBody.isKinematic = true;
        if (seedCollider != null)
            seedCollider.gameObject.tag = "Untagged";
        bubble.transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 1);
    }

    private void OnDisable()
    {
        ResetSeed();
    }

    private void Start()
    {
        this.gameObject.TryGetComponent<FollowOffset>(out followOffset);

        transform.localScale = Vector3.zero;
        flowerModel.transform.localScale = Vector3.zero;

        //flower
        FlowerConstraints = new TransformerUtils.ScaleConstraints();
        FlowerConstraints.XAxis.ConstrainAxis = false;
        FlowerConstraints.YAxis.ConstrainAxis = false;
        FlowerConstraints.ZAxis.ConstrainAxis = false;

        //bubble
        bubble.transform.localScale = Vector3.zero;
    }

    public void GrowFlower(Vector3 growPoint, Vector3 normal)
    {
        onGrowFlower?.Invoke(this.gameObject);

        this.transform.DOMove(growPoint, 0.5f);

        seedModel.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            //flowerModel.transform.DOScale(Vector3.one * 0.3f, 0.5f).SetEase(Ease.Linear);
            //InjectOptionalScaleConstraints(FlowerConstraints);

            //start flower grow
            this.transform.up = normal;
            GameObject flowerObject = Instantiate(InstrumentsManager.instance._InstrumentsData.Insts[currentInstrument].flowerModel);
            flowerObject.transform.SetParent(this.transform);
            flowerObject.transform.localPosition = Vector3.zero;
            flowerObject.transform.localEulerAngles = Vector3.zero;
            flowerObject.transform.localScale = Vector3.one;
        });
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGrab()
    {
        isGrabbed = true;
        seedUI.SetSwipeUI(false); //really bad way of doing this lol
        onGrab?.Invoke();
    }

    public void OnRelease()
    {
        isGrabbed = false;
        onRelease?.Invoke();
    }

    public void ResetSeed()
    {
        onSetToMic = null;
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        bubble.gameObject.SetActive(false);
        SetSeedInstrument(0);
        currentInstrument = 0;
        isGrowable = false;
        seedParticles.TurnAllOff();
    }

    public void GrowBubble()
    {
        if (bubbleScaleDownTween != null)
        {
            bubbleScaleDownTween.Kill();
            bubbleScaleDownTween = null;
        }

        bubble.SetActive(true);

        //여기 부터
        bubbleScaleUpTween = bubble.transform.DOScale(Vector3.one * 0.15f, 1).OnComplete(() =>
        {
            MakeGrabbable(true);
            if (seedCollider != null)
                seedCollider.gameObject.tag = seedTag;

            bubbleScaleUpTween = null;
        });
    }

    public void DestroySeed()
    {
        if(seedCollider != null)
            seedCollider.gameObject.tag = "Untagged";

        if (bubbleScaleUpTween != null)
        {
            bubbleScaleUpTween.Kill();
            bubbleScaleUpTween = null;
        }

        bubbleScaleDownTween = this.transform.DOScale(Vector3.zero, .5f).OnComplete(() =>
        {
            GameManager.instance.seedObjectPool.Return(this);
        });

    }

    public void MakeGrabbable(bool isGrabbable)
    {
        //rigidBody.isKinematic = isGrabbable;
        interactable.enabled = isGrabbable!;
    }

    public void SetSeedInstrument(int index)
    {
        SetBubbleInstrument(index);
        SetInstrumentSeed(index);
        PlayIntro(index);
    }

    //setting instrument visual for seed
    public void SetBubbleInstrument(int _index)
    {
        Color color = InstrumentsManager.instance._InstrumentsData.Insts[_index].seedColor;
        currentInstrument = _index;
        SetBubbleColor(color);
    }

    public void SetBubbleColor(Color color)
    {
        bubble.gameObject.GetComponent<Renderer>().material.color = color;
    }

    public void PlayIntro(int index)
    {
        SoundManager.Instance.PlayOneShot(this.gameObject, InstrumentsManager.instance._InstrumentsData.Insts[index].introSound);
    }

    public void SetInstrumentSeed(int index)
    {
        foreach(GameObject seed in seedList)
        {
            seed.SetActive(false);
        }

        seedList[index].SetActive(true);
    }

    //Follow logic
    public void StartFollowOffset(Transform _followTransform, Vector3 _offSet, float _maxFollowSpeed)
    {
        if (followOffset == null)
            GetFollowOffSet();

        followOffset.SetFollowStats(_followTransform, _offSet, _maxFollowSpeed, true);
        followOffset.StartFollow();
    }
    private void GetFollowOffSet()
    {
        this.gameObject.TryGetComponent<FollowOffset>(out followOffset);

        if (followOffset == null)
            followOffset = this.gameObject.AddComponent<FollowOffset>();
    }
    public void StopFollow(float stopTime = 0.5f)
    {
        if (followOffset == null)
        {
            Debug.Log($"{this.gameObject} seed doesn't have a follow offset");
            return;
        }

        followOffset.StopFollow(stopTime);
    }

    //mic logic
    public void SetMic(Transform targetTransform)
    {
        Debug.Log("Set mic activated");

        // Change state
        interactable.enabled = false;
        onSetToMic?.Invoke();

        // Use DOTween to scale the object
        this.transform.DOScale(Vector3.one * 0.4f, 0.5f);

        // Start coroutine to move to the target transform
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToTransform(targetTransform, 0.6f));
    }

    private IEnumerator MoveToTransform(Transform targetTransform, float duration)
    {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetTransform.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        transform.position = targetTransform.position; // Ensure the final position is exactly the target position

        StartFollowOffset(targetTransform, Vector3.zero, 10f);
        onMicArrived?.Invoke();
    }

    //swipeable
    public void SetSwipeable(bool isSwipeable)
    {
        swipeable.isSwipeable = isSwipeable;

        if(isSwipeable)
        {
            swipeable.onSwipeRight += OnSwipeRight;
            swipeable.onSwipeLeft += OnSwipeLeft;
            swipeable.onEnter += OnSwipeSelect;
            swipeable.onExit += OnSwipeExit;
        }
        else
        {
            swipeable.onSwipeRight -= OnSwipeRight;
            swipeable.onSwipeLeft -= OnSwipeLeft;
            swipeable.onEnter -= OnSwipeSelect;
            swipeable.onExit -= OnSwipeExit;
        }
    }

    private void OnSwipeSelect()
    {
        bubble.gameObject.GetComponent<Renderer>().material.color = selectColor;
    }

    private void OnSwipeLeft()
    {
        onSwipeLeft?.Invoke();
    }

    private void OnSwipeRight()
    {
        //send to web
        //pop bubble
        onSwipeRight?.Invoke();
    }

    private void OnSwipeExit()
    {
        bubble.gameObject.GetComponent<Renderer>().material.color = baseColor;
    }

    //Collide with room
    public void CollideWithRoom()
    {

    }
}
