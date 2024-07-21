using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AutoSetAnchors : MonoBehaviour
{
    void Start()
    {
        // Find all RectTransform components in the scene
        RectTransform[] rectTransforms = FindObjectsOfType<RectTransform>(true);

        foreach (RectTransform rt in rectTransforms)
        {
            // Check if RectTransform is not part of any layout group or content
            if (!IsInLayoutGroup(rt))
            {
                SetAnchorsToCorners(rt);
            }
        }
    }

    bool IsInLayoutGroup(RectTransform rt)
    {
        // Check if RectTransform or any of its parents have a LayoutGroup component
        Transform current = rt;
        while (current != null)
        {
            if (current.GetComponent<LayoutGroup>() != null || current.GetComponent<ContentSizeFitter>() != null)
            {
                return true;
            }
            current = current.parent;
        }
        return false;
    }

    void SetAnchorsToCorners(RectTransform rt)
    {
        // Make sure RectTransform is not null
        if (rt == null)
        {
            Debug.LogError("RectTransform is null");
            return;
        }

        // Calculate new anchor positions
        RectTransform parent = rt.parent as RectTransform;
        if (parent == null)
        {
            Debug.LogError("Parent RectTransform is null");
            return;
        }

        Vector2 newAnchorMin = new Vector2(
            rt.anchorMin.x + rt.offsetMin.x / parent.rect.width,
            rt.anchorMin.y + rt.offsetMin.y / parent.rect.height);

        Vector2 newAnchorMax = new Vector2(
            rt.anchorMax.x + rt.offsetMax.x / parent.rect.width,
            rt.anchorMax.y + rt.offsetMax.y / parent.rect.height);

        // Set new anchor values
        rt.anchorMin = newAnchorMin;
        rt.anchorMax = newAnchorMax;

        // Reset offsets to zero
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
