using UnityEngine;
using UnityEditor;
using System.Web;

public class ShaderGUI : UnityEditor.ShaderGUI
{ 
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        void DrawProperty(string property, string inspectorName)
        {
            materialEditor.ShaderProperty(FindProperty(property, properties), inspectorName);
        }
        void DrawProperty_v3(string property, string inspectorName)
        {
            var rawProperty = FindProperty(property, properties);
            Vector4 v4 = rawProperty.vectorValue;
            Vector3 v3 = rawProperty.vectorValue;
            v3 = EditorGUILayout.Vector3Field(inspectorName, v3);
            rawProperty.vectorValue = new Vector4(v3.x, v3.y,v3.z, v4.w);
        }
        void v4_DropDown(string property, string inspectorName, string[] options)
        {
            var rawProperty = FindProperty(property, properties);
            Vector4 v4 = rawProperty.vectorValue;

            int selected = 0;

            if (v4.x == 1) selected = 0;
            else if (v4.y == 1) selected = 1;
            else if (v4.z == 1) selected = 2;
            else if (v4.w == 1) selected = 3;

            selected = EditorGUILayout.Popup(inspectorName, selected, options);

            Vector4 new_v4 = Vector4.zero;
            if (selected == 0) new_v4.x = 1;
            else if (selected == 1) new_v4.y = 1;
            else if (selected == 2) new_v4.z = 1;
            else if (selected == 3) new_v4.w = 1;
            rawProperty.vectorValue = new_v4;
        }

        //Main Texture
        string[] spaceOptions = { "uv", "world", "local", "view" };
        v4_DropDown("_frag_uv_world_local_view", "Space", spaceOptions);

        DrawProperty_v3("_frag_plane_XYZ", "Plane");
        DrawProperty("_main_texture", "Texture");
        DrawProperty("_main_tint_color", "Color");
        DrawProperty("_Alpha_cliping_treshold", "Threshold");
        
        //Texture Distrorion
    }


}


