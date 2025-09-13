using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Skybox_Ball : MonoBehaviour
{
    public GameObject go_Skybox_Ball;
    public GameObject go_SykFrame;
    public TextMeshProUGUI textTitle;
    public GameObject go_Title;
    private float rotationSpeed = 50f;

    private Vector3 rotationDirection;
    private Coroutine rotationCoroutine;
    private Vector3 targetPosition = new Vector3(0, 3f, 2.5f);


    void OnDisable() {
        Ball_StopRotation();
    }

    void Start(){
        //textTitle.alpha = 0f;
        go_Title.SetActive(false);
        go_SykFrame.SetActive(false);
        rotationDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rotationDirection.Normalize();
        rotationCoroutine = StartCoroutine(Ball_RotateObject());
    }

    IEnumerator Ball_RotateObject(){
        while (true){
            go_Skybox_Ball.transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    //볼을 잡고 놨을 때
    public void Ball_Seleted(){
        Ball_StopRotation();
        MoveBallToTargetPosition();
    }

        // 코루틴 종료 함수
        private void Ball_StopRotation(){
            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
                rotationCoroutine = null;
            }
        }

         // 볼을 목표 위치로 이동시키는 함수
        private void MoveBallToTargetPosition(){
            go_Skybox_Ball.transform.DOScale(Vector3.one * 0.1f, 2f);
            go_Skybox_Ball.transform.DOMove(targetPosition, 2f).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                Show_SkyFrame();
                Title_FadeIn();
            });
        }

        private void Show_SkyFrame(){
            go_Skybox_Ball.SetActive(false);
            go_SykFrame.transform.localScale = Vector3.one * 0.01f;
            go_SykFrame.SetActive(true);
            go_SykFrame.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutElastic);

        }

        void Title_FadeIn(){
             go_Title.SetActive(true);
            go_Title.transform.localScale = Vector3.one * 0.1f;
            go_Title.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutElastic);

           //textTitle.DOFade(1f, 2f);
        }



}
