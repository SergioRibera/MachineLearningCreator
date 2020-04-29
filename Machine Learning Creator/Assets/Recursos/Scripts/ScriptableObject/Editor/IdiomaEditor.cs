using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using LibraryPersonal;

[CustomEditor(typeof(IdiomaScriptableObject))]
public class IdiomaEditor : Editor
{
    IdiomaScriptableObject lan;
    bool addElement;
    bool import;
    string k, cc, textImport;

    private void OnEnable()
    {
        lan = (IdiomaScriptableObject) target;
        if (string.IsNullOrEmpty(lan.LanguajeName) || string.IsNullOrWhiteSpace(lan.LanguajeName))
            lan.LanguajeName = "en";
    }
    public override void OnInspectorGUI()
    {
        if (lan)
        {
            if (GUILayout.Button("Importar de Texto"))
            {
                import = true;
            }
            if (import)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical("Box");
                textImport = GUILayout.TextField(textImport);
                if (GUILayout.Button("Importar"))
                {
                    if (!string.IsNullOrEmpty(textImport) || !string.IsNullOrWhiteSpace(textImport))
                    {
                        try
                        {
                            EditorJsonUtility.FromJsonOverwrite(textImport.Desencrypt(), (Idioma) lan);
                            textImport = "";
                            import = false;
                        }
                        catch (Exception e)
                        {
                            EditorGUILayout.HelpBox(e.ToString(), MessageType.Error);
                        }
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Textos vacios", MessageType.Error);
                    }
                }
                if (GUILayout.Button("Cancelar"))
                    import = false;
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            if (GUILayout.Button("Generar Asset Serializado"))
            {
                Debug.Log("Serializar Object : " + ((Idioma) lan).Serializar());
                ((Idioma)lan).Save<Idioma>(lan.LanguajeName.Trim(), ExtensionFile.lan);
                Debug.Log("Objecto Serializado!");
            }
            if (GUILayout.Button("Añadir Elemento"))
            {
                addElement = true;
            }
            if (GUILayout.Button("Limpiar"))
            {
                lan.Contenido.Clear();
            }
            if (addElement)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical("Box");
                DrawElementEdit();
                if (GUILayout.Button("Añadir"))
                {

                    if (!string.IsNullOrEmpty(k) || !string.IsNullOrWhiteSpace(k) && !string.IsNullOrEmpty(cc) || !string.IsNullOrWhiteSpace(cc))
                    {
                        lan.Contenido.Add(k, cc);
                        addElement = false;
                        k = "";
                        cc = "";
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("Textos vacios", MessageType.Error);
                    }
                }
                if (GUILayout.Button("Cancelar"))
                    addElement = false;
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Box");
            lan.LanguajeName = GUILayout.TextField(lan.LanguajeName);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("Box");
            for (int i = 0; i < lan.Contenido.Count; i++)
            {
                DrawItem(i);
            }
            EditorGUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.HelpBox("Target Not Identify", MessageType.Error);
        }
    }

    void DrawElementEdit()
    {
        EditorGUILayout.BeginVertical("Box");

        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("Nombre Del Elemento :  ", GUI.skin.label);
        k = GUILayout.TextField(k);
        GUILayout.Label("Valor Del Elemento :  ", GUI.skin.label);
        cc = GUILayout.TextField(cc);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();
    }
    void DrawItem(int i)
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label("Identificador : ");
        lan.Contenido[i, true] = GUILayout.TextField(lan.Contenido[i, true]);
        GUILayout.Label("Valor : ");
        lan.Contenido[i, false] = GUILayout.TextField(lan.Contenido[i, false]);
        
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Eliminar"))
        {
            lan.Contenido.Remove(i);
        }
    }
}