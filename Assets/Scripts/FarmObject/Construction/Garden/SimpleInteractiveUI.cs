using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SimpleInteractiveUI : MonoBehaviour
{
    public GrowingResourceManager growingResourceManager;
    public RectTransform interactiveElement;
    private Tween interactiveElementTween;

    [Range(0, 2)]
    public float startScale;
    [Range(0, 2)]
    public float endScale;
    [Range(0, 2)]
    public float endScalePress;
    public float animationSpeed;

    private bool enableUI = false;
    private bool canInteract = false;
    private bool isLookAtUI = false;

    [Tooltip("Limit angle when player look at monitor screen. Calculating by two vector: vector from center of monitor to player camera and normal vector of monitor screen")]
    public float limitAngleToInteractBuildButton;

    private void Update()
    {
        if (canInteract && isLookAtUI && Input.GetKeyDown(KeyCode.E))
        {
            PressElementAnimation(interactiveElement, new Vector3(endScalePress, endScalePress, 1), animationSpeed);
            growingResourceManager.RefillResource();
        }   
    }

    /// <summary> Zoom in and zoom out element animation. </summary>
    public void PressElementAnimation(RectTransform rect, Vector3 endScale, float cycleTime)
    {
        interactiveElementTween = rect.DOScale(endScale, cycleTime).SetLoops(1, LoopType.Restart).SetEase(Ease.OutBack).OnComplete(() => {
            EndZoomInOutAnimation(interactiveElement, new Vector3(startScale, startScale, 1), LoopType.Yoyo, animationSpeed);
        });
    }

    /// <summary> Zoom in and zoom out element animation. </summary>
    public void StartZoomInOutAnimation(RectTransform rect, Vector3 endScale, LoopType loopType, float cycleTime)
    {
        interactiveElementTween = rect.DOScale(endScale, cycleTime).SetLoops(1, loopType).SetEase(Ease.Linear);
    }

    /// <summary> End Zoom in and zoom out element animation. </summary>
    public void EndZoomInOutAnimation(RectTransform rect, Vector3 endScale, LoopType loopType, float cycleTime)
    {
        interactiveElementTween = rect.DOScale(endScale, cycleTime).SetLoops(1, loopType).SetEase(Ease.Linear);
    }

    public bool CheckLookAtMonitorScreen(Vector3 lookAtVector, Vector3 monitorScreenVector, float limitAngle)
    {
        float dotValue = Vector3.Dot(lookAtVector, monitorScreenVector);
        float angle = Mathf.Acos(dotValue / (lookAtVector.magnitude * monitorScreenVector.magnitude)) * Mathf.Rad2Deg;

        if (dotValue > 0)
        {
            if (angle <= limitAngle) return true;
            return false;
        }
        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        canInteract = true;
        isLookAtUI = CheckLookAtMonitorScreen(Camera.main.transform.forward, interactiveElement.transform.position - Camera.main.transform.position, limitAngleToInteractBuildButton);
        if (isLookAtUI && !enableUI)
        {
            StartZoomInOutAnimation(interactiveElement, new Vector3(endScale, endScale, 1), LoopType.Yoyo, animationSpeed);
            enableUI = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canInteract = false;
        isLookAtUI = false;
        enableUI = false;
        EndZoomInOutAnimation(interactiveElement, new Vector3(startScale, startScale, 1), LoopType.Yoyo, animationSpeed);
    }
}