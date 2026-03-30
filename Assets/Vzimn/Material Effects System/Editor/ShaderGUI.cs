using UnityEngine;
using UnityEditor;

public class ShaderGUI : UnityEditor.ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        // Example: draw ONE property manually
        MaterialProperty alpha_clip = FindProperty("_Alpha_cliping_treshold", properties);

        EditorGUILayout.LabelField("Alpha clip", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(alpha_clip, "Strength");
    }
}