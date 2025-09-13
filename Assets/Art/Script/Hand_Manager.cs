using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hand_Manager : MonoBehaviour
{
    public OVRSkeleton ovrSkeleton_LeftHand; // OVRSkeleton 스크립트를 참조
    public Camera mainCamera; // 카메라를 참조
    public GameObject uiElement; // UI 요소를 참조


    private OVRSkeleton.BoneId wristBoneId = OVRSkeleton.BoneId.Hand_WristRoot; //참조한 손목 본 정보
    public float minAngle = 0.0f; // 최소 허용 각도
    public float maxAngle = 80.0f; // 최대 허용 각도

    public Transform wristTransform; // 손목의 Transform 정보를 저장하는 변수

    public float f_UiScaleTime = 0.5f; //도트윈 적용 텀

    void Start(){
        Invoke("GetWrist", 2f); 
        StartCoroutine(CheckWristAfterDelay(2f));
        
        if (uiElement != null){
            uiElement.SetActive(false); // 시작할 때 UI 요소를 비활성화
        }

    }

    //현재 게임 시작 이벤트가 없어 넣은 임시 함수
    void GetWrist(){ 
        if (ovrSkeleton_LeftHand == null){
            Debug.LogError("OVRSkeleton 참조안됨");
            Invoke("GetWrist", 2f); 
            return;
        }

        wristTransform = GetWristTransform(ovrSkeleton_LeftHand, wristBoneId);
        if (wristTransform == null){
            Debug.LogError("Wrist Transform 찾을수없음");
            Invoke("GetWrist", 2f); 
        }

        else
        {
            // 손목 Transform을 찾은 경우 UI 요소를 손목의 자식으로 설정
            if (uiElement != null){
                uiElement.transform.SetParent(wristTransform, false);
                //uiElement.transform.localPosition = new Vector3(0, 0.05f, 0);
                //uiElement.transform.localRotation = Quaternion.Euler(0, 0, 0);
                uiElement.transform.localPosition = new Vector3(-0.05f, 0.05f, 0);
                uiElement.transform.localRotation = Quaternion.Euler(90, -90, 0);;
            }
        }
    }

        //손목 트랜스폼 참는 함수
        Transform GetWristTransform(OVRSkeleton skeleton, OVRSkeleton.BoneId boneId){
            foreach (OVRBone bone in skeleton.Bones){
                if (bone.Id == boneId){
                    return bone.Transform;
                }
            }
            return null;
        }


    //손목 상태 체크
    IEnumerator CheckWristAfterDelay(float delay){
        yield return new WaitForSeconds(delay);

        while (true){
            if (wristTransform == null || mainCamera == null){
                yield return null;
                continue;
            }

            Vector3 wristToCameraDir = (mainCamera.transform.position - wristTransform.position).normalized;
            Vector3 palmDirection = wristTransform.up; // 손바닥 방향을 wristTransform의 up 방향으로 설정

            float angle = Vector3.Angle(palmDirection, wristToCameraDir);
            if (angle > minAngle && angle < maxAngle){
                ShowUI();
            }
            else{
                HideUI();
            }

            yield return new WaitForSeconds(0.1f); // 0.1초마다 체크
        }
    }


    //UI 보이기
    void ShowUI(){
//        Debug.Log("Wrist is looking at the camera");
        if (uiElement != null && !uiElement.activeSelf){
            uiElement.SetActive(true);
            //uiElement.transform.localScale = Vector3.one * 0.0001f;
            //uiElement.transform.DOScale(Vector3.one * 0.001f, f_UiScaleTime); // 크기를 1로 변경 (0.5초 동안)
        }
    }

    //UI 숨기기
    void HideUI(){
        if (uiElement != null && uiElement.activeSelf)
            {uiElement.SetActive(false); // 크기 변경이 완료되면 비활성화
        }
    }
}
