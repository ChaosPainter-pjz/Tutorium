using System.IO;
using Basic.CSV2Table.Scripts;
using UnityEditor;
using UnityEngine;

namespace Basic.CSV2Table.Editor
{
    public class CSV2Table : EditorWindow
    {
        private TextAsset csv = null;
        private string[][] arr = null;
        private MonoScript script = null;
        private bool foldout = true;

        [MenuItem("Window/CSV to Table")]
        public static void ShowWindow()
        {
            GetWindow(typeof(CSV2Table));
        }

        private void OnGUI()
        {
            // CSV
            var newCsv = EditorGUILayout.ObjectField("CSV", csv, typeof(TextAsset), false) as TextAsset;
            if (newCsv != csv)
            {
                csv = newCsv;
                if (csv != null)
                    arr = CsvParser2.Parse(csv.text);
                else
                    arr = null;
            }

            // Script
            script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;

            // buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh") && csv != null)
                arr = CsvParser2.Parse(csv.text);
            if (GUILayout.Button("Generate Code"))
            {
                var path = "";
                if (script != null)
                    path = AssetDatabase.GetAssetPath(script);
                else
                    path = EditorUtility.SaveFilePanel("Save Script", "Assets", csv.name + "Table.cs", "cs");
                if (!string.IsNullOrEmpty(path))
                    script = CreateScript(csv, path);
            }

            EditorGUILayout.EndHorizontal();

            // columns
            if (arr != null)
            {
                foldout = EditorGUILayout.Foldout(foldout, "Columns");
                if (foldout)
                {
                    EditorGUI.indentLevel++;
                    if (csv != null && arr == null)
                        arr = CsvParser2.Parse(csv.text);
                    if (arr != null)
                        for (var i = 0; i < arr[0].Length; i++)
                            EditorGUILayout.LabelField(arr[0][i]);
                    EditorGUI.indentLevel--;
                }
            }
        }

        public static MonoScript CreateScript(TextAsset csv, string path)
        {
            if (csv == null || string.IsNullOrEmpty(csv.text))
                return null;

            var className = Path.GetFileNameWithoutExtension(path);
            var code = TableCodeGen.Generate(csv.text, className);

            File.WriteAllText(path, code);
            Debug.Log("Table script generated: " + path);

            AssetDatabase.Refresh();

            // absolute path to relative
            if (path.StartsWith(Application.dataPath)) path = "Assets" + path.Substring(Application.dataPath.Length);

            return AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript)) as MonoScript;
        }
    }
}