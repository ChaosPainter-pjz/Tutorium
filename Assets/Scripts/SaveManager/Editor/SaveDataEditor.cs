using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;

/************************************************************************************
 * 
 *							           Save Manager
 *							  
 *				               Save Manager Data Editor Window
 *			
 *			                        Script written by: 
 *			           Jonathan Carter (https://jonathan.carter.games)
 *			        
 *									Version: 1.0.2
 *						   Last Updated: 05/10/2020 (d/m/y)					
 * 
*************************************************************************************/

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Editor Window Script (*not static*) | Controls what is shown on the save data editor window tool.
    /// </summary>
    public class SaveDataEditor : EditorWindow
    {
        /// <summary>
        /// Enum containing all the supported save types by this asset
        /// </summary>
        public enum dataTypes { intValue, floatValue, shortValue, longValue, boolValue, stringValue, Vector2Value, Vector3Value, Vector4Value, ColorValue, classValue };

        // The position on the tab menu variable
        private int tabPos;

        // Variables used in the generate class window
        private bool isCreatingFile;
        private List<dataTypes> types;
        private List<string> valueNames;
        private List<bool> shouldArray;
        private List<bool> shouldList;
        private List<string> classNames;

        // Variables used in the read class window
        private bool hasReadFile;
        private List<string> readLines;
        private List<dataTypes> readTypes;
        private List<string> readValueNames;
        private List<bool> readShouldArray;
        private List<bool> readShouldList;
        private List<string> readClassNames;

        // Tools for the delecting of panels
        public Rect DeselectWindow;

        // Variable used in scroll views
        Vector2 ScrollPos;


        /// <summary>
        /// Static | Adds the button to call the editor window under Tools/SaveDataEditor
        /// </summary>
        [MenuItem("Carter Games/Save Manager/Save Data Editor", priority = 3)]
        public static void ShowWindow()
        {
            GetWindow<SaveDataEditor>("Save Data Editor");
        }

        /// <summary>
        /// Shows the GUI on the editor window.
        /// </summary>
        public void OnGUI()
        {
            DeselectWindow = new Rect(0, 0, position.width, position.height);

            ShowAssetLogo();


            // Label that shows the name of the asset
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Save Manager", EditorStyles.boldLabel, GUILayout.Width(95.5f));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            // The tab menu used to decide what is shown on the editor window.
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            tabPos = GUILayout.Toolbar(tabPos, new string[] { "Create New SaveData", "Edit Existing SaveData", "About Asset" }, GUILayout.MaxWidth(800f), GUILayout.MaxHeight(25f));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            switch (tabPos)
            {
                case 0:
                    TabOneDisplay();    // show the creation menu
                    break;
                case 1:
                    TabTwoDisplay();    // show the edit menu
                    break;
                case 2:
                    ShowAboutTab();     // show the about asset menu
                    break;
                default:
                    break;
            }

            // defines the min/max sixe of the editor window.
            SetMinMaxWindowSize();

            // Makes it so you can deselect elements in the window by adding a button the size of the window that you can't see under everything
            //make sure the following code is at the very end of OnGUI Function
            if (GUI.Button(DeselectWindow, "", GUIStyle.none))
            {
                GUI.FocusControl(null);
            }
        }

        /// <summary>
        /// Defines the min and max size for the editor window
        /// </summary>
        private void SetMinMaxWindowSize()
        {
            EditorWindow editorWindow = this;
            editorWindow.minSize = new Vector2(400f, 500f);
            editorWindow.maxSize = new Vector2(800f, 750f);
        }

        /// <summary>
        /// Shows the asset logo or and alt text if it is not in the correct folder.
        /// </summary>
        private void ShowAssetLogo()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            // Shows either the Carter Games Logo or an alternative for if the icon is deleted when you import the package
            if (Resources.Load<Texture2D>("Carter Games/Save Manager/LogoSM"))
            {
                if (GUILayout.Button(Resources.Load<Texture2D>("Carter Games/Save Manager/LogoSM"), GUIStyle.none, GUILayout.Width(50), GUILayout.Height(50)))
                {
                    GUI.FocusControl(null);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("Carter Games", EditorStyles.boldLabel, GUILayout.Width(100));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        /// <summary>
        /// Shows the Generate class display
        /// </summary>
        private void TabOneDisplay()
        {
            EditorGUILayout.HelpBox("Create a new SaveData class here.\n\nYou may just write it yourself, however if you wish " +
                "for the asset to work with the data you want to save, we advise you use the provided editor to generate it. " +
                "To begin, press the add field button, and repeat for each field you need. When done, press the Generate Class button.", MessageType.Info, true);

            GUI.backgroundColor = Color.green;
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+ Add Field", GUILayout.Width(90f)))
            {
                if (!isCreatingFile)
                {
                    isCreatingFile = true;
                    types = new List<dataTypes>();
                    shouldArray = new List<bool>();
                    shouldList = new List<bool>();
                    valueNames = new List<string>();
                    classNames = new List<string>();
                    types.Add(dataTypes.stringValue);
                    shouldArray.Add(false);
                    shouldList.Add(false);
                    valueNames.Add("");
                    classNames.Add("");
                }
                else
                {
                    types.Add(dataTypes.stringValue);
                    shouldArray.Add(false);
                    shouldList.Add(false);
                    valueNames.Add("");
                    classNames.Add("");
                }
            }
            GUI.backgroundColor = Color.white;
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            /*
             * 
             *      Stuff that displays the fields...
             * 
             */ 

            if (isCreatingFile)
            {
                if (types.Count > 0)
                {
                    ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, GUILayout.Width(position.width), GUILayout.ExpandHeight(true));

                    for (int i = 0; i < types.Count; i++)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Data Type", EditorStyles.boldLabel, GUILayout.Width(125f));
                        EditorGUILayout.LabelField("Variable Name", EditorStyles.boldLabel, GUILayout.MinWidth(100), GUILayout.MaxWidth(300));
                        if (types[i] == dataTypes.classValue)
                        {
                            EditorGUILayout.LabelField("Class Name", EditorStyles.boldLabel, GUILayout.MinWidth(100), GUILayout.MaxWidth(300));
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();

                        GUI.backgroundColor = Color.yellow;
                        types[i] = (dataTypes)EditorGUILayout.EnumPopup(types[i], GUILayout.Width(125f));
                        GUI.backgroundColor = Color.white;

                        valueNames[i] = EditorGUILayout.TextField(valueNames[i], GUILayout.MinWidth(100), GUILayout.MaxWidth(300));
                        if (types[i] == dataTypes.classValue)
                        {
                            classNames[i] = EditorGUILayout.TextField(classNames[i], GUILayout.MinWidth(100), GUILayout.MaxWidth(300));
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Should be an Array of type?", GUILayout.Width(170f));
                        shouldArray[i] = EditorGUILayout.Toggle(shouldArray[i], GUILayout.Width(38f));

                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Should be an List of type?", GUILayout.Width(170f));
                        shouldList[i] = EditorGUILayout.Toggle(shouldList[i], GUILayout.MinWidth(38f), GUILayout.MaxWidth(535));
                        GUILayout.FlexibleSpace();
                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("- Remove Field", GUILayout.MaxWidth(110f)))
                        {
                            types.Remove(types[i]);
                            shouldArray.Remove(shouldArray[i]);
                            shouldList.Remove(shouldList[i]);
                            valueNames.Remove(valueNames[i]);
                            classNames.Remove(classNames[i]);
                        }
                        GUI.backgroundColor = Color.white;

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.EndScrollView();
                }

                GUI.backgroundColor = Color.green;
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Generate New SaveData Class", GUILayout.MaxWidth(200f)))
                {
                    if (Directory.Exists(Application.dataPath + "/Scripts/"))
                    {
                        if (Directory.Exists(Application.dataPath + "/Scripts/Carter Games/"))
                        {
                            if (Directory.Exists(Application.dataPath + "/Scripts/Carter Games/Save Manager"))
                            {
                                GenerateClass();
                            }
                            else
                            {
                                AssetDatabase.CreateFolder("Assets/Scripts/Carter Games", "Save Manager");
                                GenerateClass();
                            }
                        }
                        else
                        {
                            AssetDatabase.CreateFolder("Assets/Scripts", "Carter Games");
                            AssetDatabase.CreateFolder("Assets/Scripts/Carter Games", "Save Manager");
                            GenerateClass();
                        }
                    }
                    else
                    {
                        AssetDatabase.CreateFolder("Assets", "Scripts");
                        AssetDatabase.CreateFolder("Assets/Scripts", "Carter Games");
                        AssetDatabase.CreateFolder("Assets/Scripts/Carter Games", "Save Manager");
                        GenerateClass();
                    }
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                GUI.backgroundColor = Color.white;
            }
        }

        /// <summary>
        /// Shows the Edit Class display
        /// </summary>
        private void TabTwoDisplay()
        {
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox("Edit a existing SaveData class here.\n\nIf you have a generated class and want to add or remove elements of it, you can use this " +
                "tool to do so, or just do it manually if you wish...\n\nPress the Resfresh File to get the latest version of the file and then use the editor as you would " +
                "when generating a file. Once done, remember to press the save changes button! to apply the changes to the class file.", MessageType.Info, true);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Refresh File", GUILayout.Width(110f)))
            {
                hasReadFile = false;
                ReadFile();
            }

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Save Changes", GUILayout.Width(110f)))
            {
                GenerateClass(true);
            }
            GUI.backgroundColor = Color.white;
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();


            if (hasReadFile)
            {
                ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, GUILayout.Width(position.width), GUILayout.ExpandHeight(true));

                for (int i = 0; i < readTypes.Count; i++)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Data Type", EditorStyles.boldLabel, GUILayout.Width(125f));
                    EditorGUILayout.LabelField("Variable Name", EditorStyles.boldLabel, GUILayout.MinWidth(100), GUILayout.MaxWidth(300));
                    if (readTypes[i] == dataTypes.classValue)
                    {
                        EditorGUILayout.LabelField("Class Name", EditorStyles.boldLabel, GUILayout.MinWidth(100), GUILayout.MaxWidth(300));
                    }
                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginHorizontal();
                    GUI.backgroundColor = Color.yellow;
                    readTypes[i] = (dataTypes)EditorGUILayout.EnumPopup(readTypes[i], GUILayout.Width(125f));
                    GUI.backgroundColor = Color.white;

                    readValueNames[i] = EditorGUILayout.TextField(readValueNames[i], GUILayout.MinWidth(100), GUILayout.MaxWidth(300));

                    if (readTypes[i] == dataTypes.classValue)
                    {
                        readClassNames[i] = EditorGUILayout.TextField(readClassNames[i], GUILayout.MinWidth(100), GUILayout.MaxWidth(300));
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Should be an Array of type?", GUILayout.Width(170f));
                    readShouldArray[i] = EditorGUILayout.Toggle(readShouldArray[i], GUILayout.Width(38f));

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Should be an List of type?", GUILayout.Width(170f));
                    readShouldList[i] = EditorGUILayout.Toggle(readShouldList[i], GUILayout.MinWidth(38f), GUILayout.MaxWidth(535));
                    GUILayout.FlexibleSpace();
                    GUI.backgroundColor = Color.red;

                    if (GUILayout.Button("- Remove Field", GUILayout.MaxWidth(110f)))
                    {
                        readTypes.Remove(readTypes[i]);
                        readShouldArray.Remove(readShouldArray[i]);
                        readShouldList.Remove(readShouldList[i]);
                        readValueNames.Remove(readValueNames[i]);
                        readClassNames.Remove(readClassNames[i]);
                    }

                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();

                    EditorGUILayout.Space();
                }

                GUI.backgroundColor = Color.green;
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("+ Add Field", GUILayout.Width(90f)))
                {
                    readTypes.Add(dataTypes.stringValue);
                    readValueNames.Add("");
                    readClassNames.Add("");
                    readShouldArray.Add(false);
                    readShouldList.Add(false);
                }
                GUI.backgroundColor = Color.white;
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
            }
        }

        /// <summary>
        /// Generates the SaveData.cs class based on the values inputted
        /// </summary>
        private void GenerateClass(bool isRead = false)
        {
            string copyPath = "Assets/Scripts/Carter Games/Save Manager/SaveData.cs";
            //Debug.Log("Creating Classfile: " + copyPath);

            if (!isRead)
            {
                if (!File.Exists(copyPath))
                {
                    // do not overwrite
                    using (StreamWriter outfile =
                        new StreamWriter(copyPath))
                    {
                        outfile.WriteLine("using UnityEngine;");
                        outfile.WriteLine("using System;");
                        outfile.WriteLine("using System.Collections.Generic;");
                        outfile.WriteLine("");
                        outfile.WriteLine("// *** Class Generated By SaveDataEditor ***");
                        outfile.WriteLine("namespace CarterGames.Assets.SaveManager");
                        outfile.WriteLine("{");
                        outfile.WriteLine("    [Serializable]");
                        outfile.WriteLine("    public class SaveData");
                        outfile.WriteLine("    {");

                        if (types.Count > 0)
                        {
                            for (int i = 0; i < types.Count; i++)
                            {
                                outfile.WriteLine(ConvertTypeToString(types[i], shouldArray[i], shouldList[i], valueNames[i], classNames[i], i));
                            }
                        }

                        outfile.WriteLine("    }");
                        outfile.WriteLine("}");
                    }
                    //File written
                }
                else
                {
                    File.Delete(copyPath);

                    // do not overwrite
                    using (StreamWriter outfile =
                        new StreamWriter(copyPath))
                    {
                        outfile.WriteLine("using UnityEngine;");
                        outfile.WriteLine("using System;");
                        outfile.WriteLine("using System.Collections.Generic;");
                        outfile.WriteLine("");
                        outfile.WriteLine("// *** Class Generated By SaveDataEditor ***");
                        outfile.WriteLine("namespace CarterGames.Assets.SaveManager");
                        outfile.WriteLine("{");
                        outfile.WriteLine("    [Serializable]");
                        outfile.WriteLine("    public class SaveData");
                        outfile.WriteLine("    {");

                        if (types.Count > 0)
                        {
                            for (int i = 0; i < types.Count; i++)
                            {
                                outfile.WriteLine(ConvertTypeToString(types[i], shouldArray[i], shouldList[i], valueNames[i], classNames[i], i));
                            }
                        }

                        outfile.WriteLine("    }");
                        outfile.WriteLine("}");
                    }
                    //File written
                }
            }
            else
            {
                if (!File.Exists(copyPath))
                {
                    // do not overwrite
                    using (StreamWriter outfile =
                        new StreamWriter(copyPath))
                    {
                        outfile.WriteLine("using UnityEngine;");
                        outfile.WriteLine("using System;");
                        outfile.WriteLine("using System.Collections.Generic;");
                        outfile.WriteLine("");
                        outfile.WriteLine("// *** Class Generated By SaveDataEditor ***");
                        outfile.WriteLine("namespace CarterGames.Assets.SaveManager");
                        outfile.WriteLine("{");
                        outfile.WriteLine("    [Serializable]");
                        outfile.WriteLine("    public class SaveData");
                        outfile.WriteLine("    {");

                        if (readTypes.Count > 0)
                        {
                            for (int i = 0; i < readTypes.Count; i++)
                            {
                                outfile.WriteLine(ConvertTypeToString(readTypes[i], readShouldArray[i], readShouldList[i], readValueNames[i], readClassNames[i], i));
                            }
                        }

                        outfile.WriteLine("    }");
                        outfile.WriteLine("}");
                    }
                    //File written
                }
                else
                {
                    File.Delete(copyPath);

                    // do not overwrite
                    using (StreamWriter outfile =
                        new StreamWriter(copyPath))
                    {
                        outfile.WriteLine("using UnityEngine;");
                        outfile.WriteLine("using System;");
                        outfile.WriteLine("using System.Collections.Generic;");
                        outfile.WriteLine("");
                        outfile.WriteLine("// *** Class Generated By SaveDataEditor ***");
                        outfile.WriteLine("namespace CarterGames.Assets.SaveManager");
                        outfile.WriteLine("{");
                        outfile.WriteLine("    [Serializable]");
                        outfile.WriteLine("    public class SaveData");
                        outfile.WriteLine("    {");

                        if (readTypes.Count > 0)
                        {
                            for (int i = 0; i < readTypes.Count; i++)
                            {
                                outfile.WriteLine(ConvertTypeToString(readTypes[i], readShouldArray[i], readShouldList[i], readValueNames[i], readClassNames[i], i));
                            }
                        }

                        outfile.WriteLine("    }");
                        outfile.WriteLine("}");
                    }
                    //File written
                }
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Converts the value and type to a string to be inputted into the file
        /// </summary>
        /// <param name="type">the datatype to convert</param>
        /// <param name="isArray">should the data type be an array?</param>
        /// <param name="isList">should the data type be a list?</param>
        /// <param name="name">the value name to apply</param>
        /// <param name="cName">class name value</param>
        /// <returns>string with the result</returns>
        private string ConvertTypeToString(dataTypes type, bool isArray, bool isList, string name, string cName, int pos)
        {
            string _newString = "";

            string _type = type.ToString().Replace("Value", "");

            if (name == "")
            {
                name = _type + pos;
            }


            if (type != dataTypes.classValue)
            {
                if (!_type.Equals("Vector2") && !_type.Equals("Vector3") && !_type.Equals("Vector4") && !_type.Equals("Color"))
                {
                    if (isArray)
                    {
                        _newString = "        [SerializeField] public " + _type + "[] " + name + ";";
                    }
                    else if (isList)
                    {
                        _newString = "        [SerializeField] public List<" + _type + "> " + name + ";";
                    }
                    else
                    {
                        _newString = "        [SerializeField] public " + _type + " " + name + ";";
                    }
                }
                else
                {
                    if (isArray)
                    {
                        _newString = "        [SerializeField] public " + "Save" + _type + "[] " + name + ";";
                    }
                    else if (isList)
                    {
                        _newString = "        [SerializeField] public " + "List<Save" + _type + "> " + name + ";";
                    }
                    else
                    {
                        _newString = "        [SerializeField] public " + "Save" + _type + " " + name + ";";
                    }
                }
            }
            else
            {
                if (cName != "")
                {
                    if (isArray)
                    {
                        _newString = "        [SerializeField] public " + cName + "[] " + name + ";";
                    }
                    else if (isList)
                    {
                        _newString = "        [SerializeField] public " + "List<" + cName + "> " + name + ";";
                    }
                    else
                    {
                        _newString = "        [SerializeField] public " + cName + " " + name + ";";
                    }
                }
                else
                {
                    Debug.LogError(" *** (Save Manager) *** | Unable to final a class of type " + cName + ". Please check the class exsits in your project, if it is under a namespace you will need to add the using... to the generated class yourself or declare it with the namespace in the class value.");
                }
            }
            

            return _newString;
        }

        /// <summary>
        /// Shows the about tab
        /// </summary>
        private void ShowAboutTab()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Version: 1.0.2", GUILayout.Width(90));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Released: 04/10/2020", GUILayout.Width(140));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Documentation", GUILayout.Width(100)))
            {
                Application.OpenURL("https://carter.games/savemanager");
            }

            GUI.color = Color.cyan;
            if (GUILayout.Button("Discord", GUILayout.Width(65f)))
            {
                Application.OpenURL("https://carter.games/discord");
            }
            GUI.color = Color.white;

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.HelpBox("Save Manager is a tool to help beginners save and load their games with ease. The asset allow the saving and loading of multiple data types.\n\n" +
                "should you need any help with the asset, please get in touch either via our community discord server or via email (hello@carter.games)", MessageType.Info);

            EditorGUILayout.Space();
        }

        /// <summary>
        /// Reads the SaveData.cs file and gets the data types, variable names and if they are array or not there.
        /// </summary>
        private void ReadFile()
        {
            if (File.Exists("Assets/Scripts/Carter Games/Save Manager/SaveData.cs"))
            {
                if (!hasReadFile)
                {
                    string[] _fileData = File.ReadAllLines("Assets/Scripts/Carter Games/Save Manager/SaveData.cs");
                    readLines = new List<string>();

                    for (int i = 0; i < _fileData.Length; i++)
                    {
                        if (_fileData[i].Contains("[SerializeField]"))
                        {
                            readLines.Add(_fileData[i]);
                        }
                    }

                    ConvertStringsToData();
                }
            }
        }

        /// <summary>
        /// Converts the read lines from the ReadFile() method to thier respective types so it can be used in the editor
        /// </summary>
        private void ConvertStringsToData()
        {
            readTypes = new List<dataTypes>();
            readValueNames = new List<string>();
            readShouldArray = new List<bool>();
            readShouldList = new List<bool>();
            readClassNames = new List<string>();

            for (int i = 0; i < readLines.Count; i++)
            {
                if (!IsClass(readLines[i].Split(' ')[10]))
                {
                    readTypes.Add((dataTypes)Enum.Parse(typeof(dataTypes), readLines[i].Split(' ')[10].Replace("Save", "").Replace("[]", "").Replace("List<", "").Replace(">", "") + "Value", true));
                    readClassNames.Add("");
                }
                else
                {
                    readTypes.Add(dataTypes.classValue);
                    readClassNames.Add(readLines[i].Split(' ')[10].Replace("[]", "").Replace("List<", "").Replace(">", ""));
                }

                readValueNames.Add(readLines[i].Split(' ')[11].Replace(";", ""));
                readShouldArray.Add(readLines[i].Contains("[]"));
                readShouldList.Add(readLines[i].Contains("<"));
            }

            //Debug.Log(readTypes.Count + " | " + readValueNames.Count + " | " + readShouldArray.Count + " | " + readShouldList.Count + " | " + readClassNames.Count);

            hasReadFile = true;
        }

        /// <summary>
        /// Checks to see if the dataType is a class or not, Used in the ConvertStringsToData() method
        /// </summary>
        /// <param name="checkValue">string value to check</param>
        /// <returns></returns>
        private bool IsClass(string checkValue)
        {
            if ((!checkValue.Contains("int")) &&
                (!checkValue.Contains("float")) &&
                (!checkValue.Contains("short")) &&
                (!checkValue.Contains("long")) &&
                (!checkValue.Contains("string")) &&
                (!checkValue.Contains("Vector2")) &&
                (!checkValue.Contains("Vector3")) &&
                (!checkValue.Contains("Vector4")) &&
                (!checkValue.Contains("Color")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}