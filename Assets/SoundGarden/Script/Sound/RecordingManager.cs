using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using DG.Tweening;
using UnityEngine.Networking;
using TMPro;
using Unity.Mathematics;



public class RecordingManager : Singleton<RecordingManager>
{
    private AudioClip recording;
    private bool isRecording = false;
    
    public string savePath;
    
    public int sampleRate = 44100;
    
    
    // 녹음을 시작하는 함수
    public void StartRecording()
    {
        if (Microphone.devices.Length > 0)
        {
            isRecording = true;
            float recordingTime = (60f / SoundManager.Instance.bpm) * SoundManager.Instance.beatsPerBar * SoundManager.Instance.numberOfBars;
            recording = Microphone.Start(null, true, Mathf.CeilToInt(recordingTime), sampleRate);
        }
        else
        {
        }
    }

    // 녹음을 종료하는 함수
    public void StopRecording()
    {
        if (isRecording == false)
            return;

        isRecording = false;
        Microphone.End(null);

        // 녹음된 오디오 데이터를 처리하고 파일로 저장
        SoundUploadManager.instance.SetFilePath(SaveRecordedAudio(recording));
    }

    // 녹음된 오디오 데이터를 WAV 파일로 저장하는 함수
    private string SaveRecordedAudio(AudioClip clip)
    {
        if (clip == null)
        {
            return null;
        }

        string filePath = Path.Combine(Application.persistentDataPath, "recordedAudio.wav");
        SaveWavFile(filePath, clip);
        
        Debug.Log($"path : {filePath}");
        return filePath;
    }

    // AudioClip을 WAV 파일로 저장하는 함수
    private void SaveWavFile(string filePath, AudioClip clip)
    {
        var samples = new float[clip.samples];
        clip.GetData(samples, 0);
        byte[] wavFile = WavTool.ConvertToWav(samples, clip.channels, clip.frequency);
        File.WriteAllBytes(filePath, wavFile);
    }

}
