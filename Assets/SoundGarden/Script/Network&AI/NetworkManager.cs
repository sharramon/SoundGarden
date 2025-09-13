using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityCommunity.UnitySingleton;

public class NetworkManager : MonoSingleton<NetworkManager>
{
    public NetworkSetting networkSetting;

    public Coroutine Send<TReq, TRes>(TReq requestData, Action<TRes> _callback)
        where TReq : ProtocolBase
        where TRes : ProtocolBase, new()
    {
        
       return StartCoroutine(Co_Send(requestData, _callback));
    }

    private IEnumerator Co_Send<TReq, TRes>(TReq requestData,Action<TRes> _callback)
        where TReq : ProtocolBase
        where TRes : ProtocolBase, new()
    {
        string url = requestData.RequestURL();
        string jsonData = requestData.ToJson();

        // 로그를 통해 JSON 데이터가 올바르게 직렬화되는지 확인합니다.

        UnityWebRequest req = requestData.CreateRequest();

        yield return req.SendWebRequest();

        UnityWebRequest.Result result = req.result;

        if (result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"NetworkManager :: Success :: {req.downloadHandler.text}");
            TRes responseProtocol = new TRes();
            responseProtocol.FromJson(req.downloadHandler.text);
            _callback(responseProtocol);
        }
        else if (result == UnityWebRequest.Result.ConnectionError ||
                 result == UnityWebRequest.Result.DataProcessingError ||
                 result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"NetworkManager :: Error :: {req.error}");
            Debug.LogError($"Response: {req.downloadHandler.text}");
        }
    }

    private string ToQueryString(object obj)
    {
        var properties = from p in obj.GetType().GetProperties()
                         where p.GetValue(obj, null) != null
                         select p.Name + "=" + Uri.EscapeDataString(p.GetValue(obj, null).ToString());

        return string.Join("&", properties.ToArray());
    }

    
}
