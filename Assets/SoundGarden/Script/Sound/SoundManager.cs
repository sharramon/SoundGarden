using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class SoundManager : MonoSingleton<SoundManager>
{
    public List<AudioSource> subscribeAudioSouces = new List<AudioSource>();
    
    // BPM (Beats Per Minute)
    public float bpm = 100.0f;
    // Number of beats in a bar
    public int beatsPerBar = 4;
    // Number of bars
    public int numberOfBars = 2;

    public int currentBeat { get; private set; }
    public int currentBar { get; private set; }
    public int totalBeats { get; private set; }
    public int currentBeatCount { get; private set; }

    private int LastBeat=-1;
    private int LastBar=-1;
    
    
    public Action onStartLoop;
    public Action onEndLoop;
    public Action onBeatUpdate;

    private float beatDuration;
    private float barDuration;
    private float totalDuration;
    private float timer;

    private bool isScheduledRecording;

    private AudioSource bgmSource;
    
    public void scheduledRecording()
    {
        isScheduledRecording = true;
    }
    
    /*
    private void Start()
    {
        StartCoroutine(AudioUpdate());
    }
    */

    void Start()
    {
        // Calculate the duration of a single beat
        beatDuration = 60.0f / bpm;
        // Calculate the duration of a single bar
        barDuration = beatDuration * beatsPerBar;
        // Calculate the total duration of the sequence
        totalDuration = barDuration * numberOfBars;
        // Calculate total beats
        totalBeats = beatsPerBar * numberOfBars;

        // Initialize the timer
        timer = 0.0f;

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // If the timer exceeds the total duration, reset it
        if (timer >= totalDuration)
        {
            timer = 0.0f;
        }

        // Calculate the current beat and bar
        currentBeat = Mathf.FloorToInt(timer / beatDuration) % beatsPerBar;
        currentBar = Mathf.FloorToInt(timer / barDuration) % numberOfBars;

        currentBeatCount = (currentBar * beatsPerBar) + currentBeat + 1;

        // Display the current beat and bar
        // Debug.Log("Current Bar: " + currentBar + " Current Beat: " + currentBeat);

        // Trigger events based on the current beat and bar
        
        if(LastBeat != currentBeat || LastBar != currentBar)
            TriggerEvents(currentBar, currentBeat);
    }
    
    void TriggerEvents(int bar, int beat)
    {
        Debug.Log($"Previous bar/beat : {LastBar}/{LastBeat}. Current bar/beat : {bar}/{beat}");

        // Implement your event logic here
        // For example:
        if (bar == 0 && beat == 0)
        {
            //Debug.Log("Start of the sequence!");
            onStartLoop?.Invoke();
            
            foreach (var audio in subscribeAudioSouces)
            {
                audio.Play();
            }

            if (isScheduledRecording)
            {
                RecordingManager.instance.StartRecording();
                isScheduledRecording = false;
            }
        }

        if (beat == 0)
        {
            //Debug.Log("Start of a new bar!");
        }

        if (bar == numberOfBars - 1 && beat == beatsPerBar - 1)
        {
            // Debug.Log("End of the sequence!");
            onEndLoop?.Invoke();
           RecordingManager.instance.StopRecording();
        }

        onBeatUpdate?.Invoke();

        LastBeat = beat;
        LastBar = bar;
    }
    
    public void AddAudioSouce(AudioSource _source)
    {
        subscribeAudioSouces.Add(_source);
    }

    public void AllAudioFade(bool _on)
    {
        float volume = 0;
        
        if (_on)
        {
            volume = 1;
        }
        else
        {
            volume = 0.2f;
        }
        
        
        foreach (var audio in subscribeAudioSouces)
        {
            audio.DOFade(volume, 0.5f);
        }
    }

    public void PlayBGM(AudioClip _bgmClip)
    {
        bgmSource.clip = _bgmClip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlayOneShot(GameObject _gameObject, AudioClip audioClip)
    {
        CheckAudioSource(_gameObject).PlayOneShot(audioClip);
    }

    public AudioSource CheckAudioSource(GameObject _gameObject, bool spatial = true)
    {
        AudioSource audioSource;

        _gameObject.TryGetComponent<AudioSource>(out audioSource);

        if (audioSource == null)
        {
            audioSource = _gameObject.AddComponent<AudioSource>();
        }

        if (spatial)
        {
            audioSource.spatialize = true;
            audioSource.spatialBlend = 1f;
            audioSource.maxDistance = 10f; //just set it to 10 for fun
        }

        if (subscribeAudioSouces.Contains(audioSource) == false)
            subscribeAudioSouces.Add(audioSource); ;

        return audioSource;
    }
}
