using System;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.IO;
using System.Text;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

#region ProtocolBase
public interface IProtocol
{
    string ToJson();
    void FromJson(string jsonData);
    string RequestURL();
    UnityWebRequest CreateRequest();
}

[Serializable]
public abstract class ProtocolBase : IProtocol
{
    [JsonIgnore]
    public string AiServerURL = "https://arpeggi.io/api/kits/v1/";
    [JsonIgnore]
    public string ConversionURL = "voice-conversions";

    [JsonIgnore]
    public string AuthToken;

    [JsonIgnore]
    protected string URL;

    public string ToJson()
    {
        Debug.Log(JsonConvert.SerializeObject(this));
        return JsonConvert.SerializeObject(this);
    }

    public void FromJson(string jsonData)
    {
        JsonConvert.PopulateObject(jsonData, this);
    }

    public abstract string RequestURL();

    public abstract UnityWebRequest CreateRequest();

    public static byte[] GetFileData(string filePath)
    {
        try
        {
            Debug.Log(filePath);
            // 파일의 모든 바이트 데이터를 읽어옵니다
            return File.ReadAllBytes(filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"파일을 읽어오는 중 오류 발생: {e.Message}");
            return null;
        }
    }

    public virtual IEnumerator CheckingCor(string id)
    {
        yield return null;
    }
}
#endregion

[Serializable]
public class Audio_Request_Protocol : ProtocolBase
{
    public string FilePath { get; set; }
    public byte[] FileData { get; set; }
    public int VoiceModelId { get; set; }
    public string Url { get; set; }
    public string conversionStrength;
    public string modelVolumeMix;
    


    public override string RequestURL()
    {
        return StringTool.ConnectString(AiServerURL, ConversionURL);
    }

    public override UnityWebRequest CreateRequest()
    {
        Url = StringTool.ConnectString(AiServerURL, ConversionURL);

        WWWForm form = new WWWForm();
        form.AddBinaryData("soundFile", GetFileData(FilePath), Path.GetFileName(FilePath), "audio/wav");
        form.AddField("voiceModelId", VoiceModelId);
        form.AddField("conversionStrength", 1);
        form.AddField("modelVolumeMix", 1);

        UnityWebRequest request = UnityWebRequest.Post(Url, form);
        request.SetRequestHeader("Authorization", "Bearer " + NetworkManager.Instance.networkSetting.authToken);

        return request;
    }

}

[Serializable]
public class Audio_Response_Protocol : ProtocolBase
{
    public string id;
    public string createdAt;
    public string type;
    public string status;
    public int voiceModelId;
    public string jobStartTime;
    public string jobEndTime;
    public string outputFileUrl;
    public string lossyOutputFileUrl;
    public string recombinedAudioFileUrl;
    public Model model;


    public override string RequestURL()
    {
        return StringTool.ConnectString(AiServerURL, ConversionURL);
    }

    public override UnityWebRequest CreateRequest()
    {
        return null;
    }
}

[Serializable]
public class CheckResult_Request_Protocol : ProtocolBase
{
    public string id;

    public override UnityWebRequest CreateRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(RequestURL());
        request.SetRequestHeader("Authorization", "Bearer " + NetworkManager.Instance.networkSetting.authToken);

        return request;
    }

    public override string RequestURL()
    {
        return StringTool.ConnectString(AiServerURL, ConversionURL, "/", id);
    }
}

[Serializable]
public class CheckResult_Response_Protocol : ProtocolBase
{
    public int id;
    public DateTime createdAt;
    public string type;
    public string status;
    public int voiceModelId;
    public DateTime? jobStartTime;
    public DateTime? jobEndTime;
    public string outputFileUrl;
    public string lossyOutputFileUrl;
    public string recombinedAudioFileUrl;
    public Model model;

    public override UnityWebRequest CreateRequest()
    {
        throw new NotImplementedException();
    }

    public override string RequestURL()
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class Model
{
    public int id;
    public string title;
    public List<string> tags;
    public string twitterLink;
    public string instagramLink;
    public string tiktokLink;
    public string spotifyLink;
    public string youtubeLink;
    public string imageUrl;
    public string demoUrl;
}
