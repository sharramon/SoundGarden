using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Meta.XR.MRUtilityKit;


public class SkyChooseState : MonoBehaviour, IState
{
    public GameObject go_Skybox_Ball;
    public GameObject go_SykFrame;
    public TextMeshProUGUI textTitle;
    public GameObject go_Title;
    public AudioClip bgm;

    private Vector3 rotationDirection;
    private float rotationSpeed = 50f;

    private Coroutine rotationCoroutine;

    private Vector3 targetPosition = new Vector3(0, 3f, 2.5f); //hard coded for now

    [SerializeField] private bool isRoomLoaded = false;

    private void Start()
    {

    }

    public void Enter()
    {
        ResetSkybox();
        GetSkyboxPos();
    }

    public void Exit()
    {

    }

    public void GetSkyboxPos()
    {
        MRUKRoom currentRoom = MRUK.Instance.GetCurrentRoom();
        MRUKAnchor ceiling = currentRoom.CeilingAnchor;
        if (ceiling != null)
        {
            ceiling.gameObject.SetActive(false);
            Vector3 ceilingCenter = ceiling.GetAnchorCenter();
            targetPosition = new Vector3(ceilingCenter.x, ceilingCenter.y, ceilingCenter.z);
        }
        else
        {
            MRUKAnchor floor = currentRoom.FloorAnchor;
            Vector3 floorCenter = floor.GetAnchorCenter();
            targetPosition = new Vector3(floorCenter.x, floorCenter.y + 3f, floorCenter.z);
        }

        go_SykFrame.transform.position = targetPosition;
        GetBallPos();
        GetTitlePos();
    }

    private void GetBallPos()
    {
        Vector3 cameraPosition = PlayerInputHandler.instance.mainCamera.transform.position;
        Vector3 pointUnderTarget = new Vector3(targetPosition.x, cameraPosition.y, targetPosition.z);
        Vector3 direction = (pointUnderTarget - cameraPosition).normalized;

        // Calculate the new position 0.3 units away from the camera towards the point under the target
        Vector3 newPosition = cameraPosition + direction * 0.3f;
        newPosition.y = 1.2f; //force it

        go_Skybox_Ball.transform.position = newPosition;

        go_Skybox_Ball.SetActive(true);
        rotationCoroutine = StartCoroutine(Ball_RotateObject());
    }

    private void GetTitlePos()
    {
        go_Title.gameObject.transform.position = targetPosition + new Vector3(0, -1f, 0);
        go_Title.gameObject.transform.LookAt(PlayerInputHandler.instance.mainCamera.transform);
    }

    private void SubscribeEvents()
    {

    }

    private void UnsubscribeEvents()
    {

    }

    private void ResetSkybox()
    {
        go_Skybox_Ball.SetActive(false);
        go_Title.SetActive(false);
        go_SykFrame.SetActive(false);

        //get a random rotation direction for spice
        rotationDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rotationDirection.Normalize();
    }

    IEnumerator Ball_RotateObject()
    {
        while (true)
        {
            go_Skybox_Ball.transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void Ball_Selected()
    {
        Ball_StopRotation();
        MoveBallToTargetPosition();
    }

    private void Ball_StopRotation()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }
    }

    private void MoveBallToTargetPosition()
    {
        go_Skybox_Ball.transform.DOScale(Vector3.one * 0.1f, 2f);
        go_Skybox_Ball.transform.DOMove(targetPosition, 2f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            Show_SkyFrame();
            Title_FadeIn();
        });
    }

    private void Show_SkyFrame()
    {
        go_SykFrame.SetActive(true);
    }

    private void Title_FadeIn()
    {
        go_Title.SetActive(true);
        go_Title.transform.localScale = Vector3.one * 0.1f;
        go_Title.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutElastic).OnComplete(() =>
        {
            AudioSource audioSource = SoundManager.Instance.CheckAudioSource(this.gameObject, false);
            audioSource.clip = bgm;
            //change here

            go_Title.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutElastic).OnComplete(() =>
            {
                go_Title.SetActive(false);
                StateManager.instance.ChangeState(AppState.InGame);
            });
        });
    }
}

