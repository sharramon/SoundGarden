using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.Serialization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Oculus.Interaction;
using UnityEngine.UI;


// �������� �Ǳ��� ��ư�� ������ ������ ��ȹ �������� �Ǳ��� �����͸� ������ ����
public class InstrumentsManager : Singleton<InstrumentsManager>
{
    [SerializeField]
    public InstrumentsData _InstrumentsData;

    //public Transform InstT;
    //public List<instButton> _buttons = new List<instButton>();

    public int GetDate(int _index)
    {
        return _InstrumentsData.Insts[_index].InstID;
    }

    
    /*

public void Start()
{
    //FindObject(InstT);
}



public void FindObject(Transform _transform)
{
Transform pokeTransform;

for (int i = 0; i < _transform.childCount; i++)
{
pokeTransform = _transform.GetChild(i);

Debug.Log(pokeTransform.name);
instButton _button = new instButton(
pokeTransform.gameObject,
pokeTransform.GetComponentInChildren<TMP_Text>(),
pokeTransform.GetComponentInChildren<PointableUnityEventWrapper>(),
pokeTransform.GetComponentInChildren<Image>()
);

        _button.SetData(_InstrumentsData.Insts[i]);
        _buttons.Add(_button);
    }
}

    public void ResetButton()
    {
        foreach (var btn in _buttons)
        {
            if (btn.buttonObject.transform.localScale != Vector3.one)
            {
                btn.buttonObject.transform.DOScale(Vector3.one, 0.5f);
                btn.buttonImage.DOColor(Color.white,0.5f);
            }
        }
    }
    */

}


/* // ���� ���
[Serializable]
public class instButton
{
    public GameObject buttonObject;
    public TMP_Text instName;
    public PointableUnityEventWrapper btnEvents;
    public Image buttonImage;

    public instButton(GameObject obj, TMP_Text tmpText, PointableUnityEventWrapper wrapper, Image btnImage)
    {
        buttonObject = obj;
        instName = tmpText;
        btnEvents = wrapper;
        buttonImage = btnImage;
        
    }

    
    
    public void SetData(Inst _data)
    {
        instName.text = _data.InstName;
        btnEvents.WhenSelect.AddListener((eventData) =>
        {
            Debug.Log("btnEvents");
            
            UploadState.instance.SetVoiceModelID(_data.InstID);
            InstrumentsManager.instance.ResetButton();
            buttonImage.DOColor(Color.red * 0.5f, 1f);
            buttonObject.transform.DOScale(Vector3.one*1.5f, 0.5f);
        });
    }
}
*/
