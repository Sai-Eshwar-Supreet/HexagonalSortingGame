using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteColorChanger : MonoBehaviour
{
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.white;
    [SerializeField] private Color clickedColor = Color.white;
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color disabledColor = Color.gray;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void OnNormal() => spriteRenderer.color = normalColor;
    public void OnHover() => spriteRenderer.color = hoverColor;
    public void OnClicked() => spriteRenderer.color = clickedColor;
    public void OnSelected() => spriteRenderer.color = selectedColor;
    public void OnDisabled() => spriteRenderer.color = disabledColor;

}
