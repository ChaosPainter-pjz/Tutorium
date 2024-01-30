using Basic.CSV2Table.Scripts;
using UnityEditor;
using UnityEngine;

namespace Basic.CSV2Table.Editor
{
    public class CSVView : EditorWindow
    {
        private TextAsset csv = null;
        private string[][] arr = null;

        [MenuItem("Window/CSV View")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            GetWindow(typeof(CSVView));
        }

        private void OnGUI()
        {
            var newCsv = EditorGUILayout.ObjectField("CSV", csv, typeof(TextAsset), false) as TextAsset;
            if (newCsv != csv)
            {
                csv = newCsv;
                arr = CsvParser2.Parse(csv.text);
            }

            if (GUILayout.Button("Refresh") && csv != null)
                arr = CsvParser2.Parse(csv.text);

            if (csv == null)
                return;

            if (arr == null)
                arr = CsvParser2.Parse(csv.text);

            for (var i = 0; i < arr.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (var j = 0; j < arr[i].Length; j++) EditorGUILayout.TextField(arr[i][j]);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}