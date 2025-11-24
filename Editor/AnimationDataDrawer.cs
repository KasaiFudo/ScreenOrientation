using UnityEditor;
using UnityEngine;

namespace KasaiFudo.ScreenOrientation.Editor
{
    [CustomPropertyDrawer(typeof(AnimationData))]
    public class AnimationDataDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var isAnimated = property.FindPropertyRelative(nameof(AnimationData.IsAnimated));
        var overrideDelay = property.FindPropertyRelative(nameof(AnimationData.OverrideDelayToAnimate));

        bool isAnimEnabled = isAnimated?.boolValue ?? true;
        bool showDelay = isAnimEnabled && (overrideDelay?.boolValue ?? false);

        int lines = 1; // label
        lines += 1;    // IsAnimated
        if (isAnimEnabled)
        {
            lines += 1; // OverrideDelay
            if (showDelay) lines += 1; // DelayToAnimate
            lines += 1; // Duration
            lines += 1; // Curve
        }

        return EditorGUIUtility.singleLineHeight * lines + EditorGUIUtility.standardVerticalSpacing * (lines - 1);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var isAnimated = property.FindPropertyRelative(nameof(AnimationData.IsAnimated));
        var overrideDelay = property.FindPropertyRelative(nameof(AnimationData.OverrideDelayToAnimate));
        var delay = property.FindPropertyRelative(nameof(AnimationData.DelayToAnimate));
        var duration = property.FindPropertyRelative(nameof(AnimationData.TransitionDuration));
        var curve = property.FindPropertyRelative(nameof(AnimationData.TransitionCurve));

        float height = EditorGUIUtility.singleLineHeight;
        float space = EditorGUIUtility.standardVerticalSpacing;
        var rect = new Rect(position.x, position.y, position.width, height);

        // Заголовок
        EditorGUI.LabelField(rect, label);
        rect.y += height + space;

        // Переключатель анимации
        EditorGUI.PropertyField(rect, isAnimated, new GUIContent("Is Animated"));
        rect.y += height + space;

        bool isAnimEnabled = isAnimated.boolValue;
        if (!isAnimEnabled)
            return; // скрываем всё остальное

        // --- Блок Override Delay ---
        float toggleW = 18f;
        var toggleRect = new Rect(rect.x, rect.y, toggleW, height);
        var labelRect = new Rect(rect.x + toggleW + 2, rect.y, rect.width - toggleW - 2, height);
        overrideDelay.boolValue = EditorGUI.Toggle(toggleRect, overrideDelay.boolValue);
        EditorGUI.LabelField(labelRect, "Override Delay (use custom value)");
        rect.y += height + space;

        // --- Delay To Animate ---
        using (new EditorGUI.DisabledScope(!overrideDelay.boolValue))
        {
            if (overrideDelay.boolValue)
            {
                EditorGUI.PropertyField(rect, delay, new GUIContent("Delay To Animate"));
                rect.y += height + space;
            }
        }

        // --- Duration ---
        EditorGUI.PropertyField(rect, duration, new GUIContent("Transition Duration"));
        rect.y += height + space;

        // --- Curve ---
        EditorGUI.PropertyField(rect, curve, new GUIContent("Transition Curve"));
    }
    }
}