using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshRenderer), typeof(Animator))]
public class ObjectFillController : MonoBehaviour
{
    [SerializeField] private float incrementor = 0.25f;
    private Animator animator;
    private MeshRenderer meshRenderer;

    private float progress = 0;
    private int bounceId;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
        bounceId = Animator.StringToHash("Bounce");
    }

    public void UpdateProgress(bool pressed, Action OnFIllComplete = null)
    {
        if (progress >= 1) return;
        if (pressed)
        {
            animator.SetBool(bounceId, true);
            StartCoroutine(UpdateProgress(OnFIllComplete));
        }
        else
        {
            animator.SetBool(bounceId, false);
            StopAllCoroutines();
        }
    }

    private IEnumerator UpdateProgress(Action OnFIllComplete = null)
    {
        while(progress < 1)
        {
            meshRenderer.material.SetFloat("_FillPercent", progress);
            progress += Time.deltaTime * incrementor; 
            yield return null;
        }
        OnFIllComplete?.Invoke();
        
    }

    public void SetProgress(float progress)
    {
        this.progress = progress;
        meshRenderer.material.SetFloat("_FillPercent", progress);
    }
}
