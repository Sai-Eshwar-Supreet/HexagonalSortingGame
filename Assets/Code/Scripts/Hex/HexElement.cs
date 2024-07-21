using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexElement : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip destroySound;

    public int MaterialIndex { get; private set; }
    public HexStack HexStack { get; private set; }
    public void SetMaterial(Material material, int index)
    {
        meshRenderer.material = material;
        MaterialIndex = index;
    }
    public void SetParent(Transform parent) => transform.SetParent(parent);
    public void Configure(HexStack hexStack) => this.HexStack = hexStack;
    public void DisableCollider() => meshCollider.enabled = false;
    public void DestroySelf()
    {
        transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack).onComplete += () =>
        {
            SoundManager.PlayUISfx(destroySound, 1);
            Destroy(gameObject);
        };
    }

    public void LocalMove(Vector3 pos)
    {
        transform.DOLocalMove(pos, 0.1f).SetEase(Ease.InOutSine).onComplete += () => SoundManager.PlayUISfx(moveSound, 1);
        transform.rotation = Quaternion.identity;

        Vector3 direction = (pos - transform.localPosition);

        direction.y = 0;
        direction = direction.normalized;

        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);
        Quaternion rotation = Quaternion.AngleAxis(180, rotationAxis);
        transform.DORotateQuaternion(rotation * transform.rotation, 0.2f).SetEase(Ease.InOutSine);
    }
}
