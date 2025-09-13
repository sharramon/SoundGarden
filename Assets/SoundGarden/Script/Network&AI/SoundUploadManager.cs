using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System;
using DG.Tweening;

public class SoundUploadManager : Singleton<SoundUploadManager>
{
    CheckResult_Request_Protocol _Protocol;

    // Hand 
    private string filePath;

    // Send Data
    private int selectedVoiceModelId = 1312985; //sax

    // seed
    private GameObject _Seed;
    private Seed seedLogic;
    Sequence seedSequence;

    #region Record Button

    public void SetVoiceModelID(int _id)
    {
        selectedVoiceModelId = _id;
    }

    #endregion

    #region Send Voice

    public void SetFilePath(string _path)
    {
        filePath = _path;
    }

    public void SendToAI(Seed seed)
    {
        Send2AIserver(filePath, seed);
    }

    void Send2AIserver(string wavFilePath, Seed seed)
    {
        Audio_Request_Protocol audio_Request_Protocol = new Audio_Request_Protocol();
        audio_Request_Protocol.FilePath = wavFilePath;
        audio_Request_Protocol.AuthToken = NetworkManager.Instance.networkSetting.authToken;
        audio_Request_Protocol.VoiceModelId = InstrumentsManager.instance._InstrumentsData.Insts[seed.currentInstrument].InstID; //TODO 악기 선택

        seedLogic = seed; 

        NetworkManager.Instance.Send(audio_Request_Protocol, (Audio_Response_Protocol audio_Response_Protocol) =>
        {
            Debug.Log(audio_Response_Protocol.id);

            _Protocol = new CheckResult_Request_Protocol();
            _Protocol.id = audio_Response_Protocol.id;

            Checking();
        });
    }


    public void Checking()
    {
        NetworkManager.Instance.Send(_Protocol, (CheckResult_Response_Protocol _Response) =>
        {
            Debug.Log($"Chechking response url is {_Response.outputFileUrl}");
            if (_Response.outputFileUrl != null)
            {
                Debug.Log(_Response.outputFileUrl);
                StartCoroutine(DownloadFile(
                    _Response.outputFileUrl, 
                    StringTool.ConnectString(Application.persistentDataPath, NetworkManager.Instance.networkSetting.SavePath,
                                                                                $"/data_{DateTime.Today.ToString("MM_dd_HH_MM")}", ".WAV")));
            }
            else
            {
                Invoke("Checking", 2f);
            }
        });
    }

    public IEnumerator DownloadFile(string url, string _savePath)
    {
        Debug.Log(_savePath);

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            try
            {
                System.IO.File.WriteAllBytes(_savePath, request.downloadHandler.data);
                Debug.Log($"File downloaded successfully / {_savePath}");
            }
            catch (DirectoryNotFoundException e)
            {
                // 폴더가 없는 경우 생성
                string directoryPath = Path.GetDirectoryName(_savePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                System.IO.File.WriteAllBytes(_savePath, request.downloadHandler.data);
            }

            // 저장된 파일을 AudioClip으로 로드하고 재생
            StartCoroutine(PlayAudioFromFile(_savePath));
        }
    }

    private IEnumerator PlayAudioFromFile(string filePath)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);

                AudioSource source = seedLogic.gameObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.spatialize = true;
                source.Play();

                SoundManager.Instance.AddAudioSouce(source);

                //SeedLogic.GrowFlower();
            }
        }
    }

    #endregion

}
