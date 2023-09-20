using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Plant))]
public class PlaneEditor : Editor
{
    private Plant _plant;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawSettingEditor(_plant.shapeSetting, _plant.OnShapeSettingUpdate);
        DrawSettingEditor(_plant.colorSetting, _plant.OnColorSettingsUpdate);
    }

    void DrawSettingEditor(Object settings, System.Action OnSettingsUpdate)
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            EditorGUILayout.InspectorTitlebar(true, settings);
            Editor editor = CreateEditor(settings);
            editor.OnInspectorGUI();
            if (check.changed)
            {
                OnSettingsUpdate?.Invoke();
            }
        }
    }

    public void OnEnable()
    {
        _plant = (Plant)target;
    }
}