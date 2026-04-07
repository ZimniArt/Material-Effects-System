using UnityEngine;
using UnityEditor;
using System.Web;

public class ShaderGUI : UnityEditor.ShaderGUI
{
        bool textureDistortionGroup_active = true;
        bool textureDistortionGroup_visible = true;
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        #region Functions
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
            rawProperty.vectorValue = new Vector4(v3.x, v3.y, v3.z, v4.w);
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

        int Get_v4_index(string property)
        {
            var rawProperty = FindProperty(property, properties);
            Vector4 v4 = rawProperty.vectorValue;
            int index = 0;

            if (v4.x == 1) index = 0;
            else if (v4.y == 1) index = 1;
            else if (v4.z == 1) index = 2;
            else if (v4.w == 1) index = 3;
            return index;
        }
        #endregion


        //Main Texture

        EditorGUILayout.PrefixLabel("Main Texture");

        string[] spaceOptions = { "uv", "world", "local", "view" };
        v4_DropDown("_frag_uv_world_local_view", "Space", spaceOptions);
        int space = Get_v4_index("_frag_uv_world_local_view");
        if (space == 1 || space == 2)  DrawProperty_v3("_frag_plane_XYZ", "Plane");
        DrawProperty("_main_tint_color", "Color");
        DrawProperty("_Alpha_cliping_treshold", "Alpha Clipping Threshold");
        DrawProperty("_main_texture", "Texture");


        //Texture distortion

        EditorGUILayout.BeginHorizontal();
        textureDistortionGroup_active = EditorGUILayout.Toggle(textureDistortionGroup_active, GUILayout.Width(20));

        textureDistortionGroup_visible = GUILayout.Toggle(textureDistortionGroup_visible, "Texture Distortion", EditorStyles.foldoutHeader);
        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginDisabledGroup(!textureDistortionGroup_active);
        if (textureDistortionGroup_visible)
        {
            DrawProperty_v3("_Fragment_scroll", "scroll speed");
            DrawProperty("_frag_dist_noise_map", "Distortion texture");
            DrawProperty("_frag_dist_noise_amount", "Distortion amount");
            DrawProperty_v3("_frag_dist_noise_scroll", "noise scroll speed");
            DrawProperty("_frag_dist_detail_map", "Detail map");
            DrawProperty("_frag_dist_detail_amount", "detail amount");
        } 
        EditorGUI.EndDisabledGroup();
        
    }


}


