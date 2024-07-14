using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
public class MinMaxSliderPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        MinMaxSliderAttribute minMax = (MinMaxSliderAttribute)attribute;

        if (property.propertyType == SerializedPropertyType.Vector2 || property.propertyType == SerializedPropertyType.Vector2Int)
        {
            EditorGUI.BeginChangeCheck();

            // Calculate rects
            Rect controlRect = EditorGUI.PrefixLabel(position, label);
            Rect sliderRect = new Rect(controlRect.x, controlRect.y, controlRect.width, EditorGUIUtility.singleLineHeight);
            Rect minFieldRect = new Rect(controlRect.x, controlRect.y + EditorGUIUtility.singleLineHeight + 2, controlRect.width / 2 - 2, EditorGUIUtility.singleLineHeight);
            Rect maxFieldRect = new Rect(controlRect.x + controlRect.width / 2 + 2, controlRect.y + EditorGUIUtility.singleLineHeight + 2, controlRect.width / 2 - 2, EditorGUIUtility.singleLineHeight);

            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                Vector2 range = property.vector2Value;
                float minValue = range.x;
                float maxValue = range.y;

                EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, minMax.min, minMax.max);
                minValue = EditorGUI.FloatField(minFieldRect, minValue);
                maxValue = EditorGUI.FloatField(maxFieldRect, maxValue);

                if (EditorGUI.EndChangeCheck())
                {
                    range.x = minValue;
                    range.y = maxValue;
                    property.vector2Value = range;
                }
            }
            else if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                Vector2Int range = property.vector2IntValue;
                float minValue = range.x;
                float maxValue = range.y;

                EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, minMax.min, minMax.max);
                minValue = EditorGUI.IntField(minFieldRect, (int)minValue);
                maxValue = EditorGUI.IntField(maxFieldRect, (int)maxValue);

                if (EditorGUI.EndChangeCheck())
                {
                    range.x = (int)minValue;
                    range.y = (int)maxValue;
                    property.vector2IntValue = range;
                }
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use MinMaxSlider with Vector2 or Vector2Int.");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight * 2 + 4;
    }
}
