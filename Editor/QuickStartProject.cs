using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEditor.AnimatedValues;
using System.Linq;

public class QuickStartProject : UnityEditor.EditorWindow
{
    private static double versionNumber = 1.1;
    private static string toolName = "Unity QuickStart Project";
    private List<Pack> packs;
    static AddAndRemoveRequest Request;
    static int windowWidth = 680;
    static int windowHeight = 480;
    private bool displayPackages = false;
    private bool displayProjectSetup = false;
    private Dictionary<System.Tuple<string, string>, bool> projectSetupPaths = new();

    public QuickStartProject(){
        packs = new List<Pack>(){
            new Pack("Generic", new List<PackageModule>(){
                new PackageModule("com.unity.addressables", "Addressables"),
                new PackageModule("com.unity.ai.navigation", "AI Navigation"),
                new PackageModule("https://github.com/febucci/unitypackage-custom-hierarchy.git", "Febucci Custom Hierarchy"),
                new PackageModule("com.unity.ide.visualstudio", "Visual Studio Editor"),
                new PackageModule("com.unity.feature.gameplay-storytelling", "Gameplay and Storytelling"),
                new PackageModule("com.unity.feature.mobile", "Mobile"),
                new PackageModule("com.unity.inputsystem", "New Input System"),
                new PackageModule("com.unity.postprocessing", "Post Processing"),
                new PackageModule("com.unity.purchasing", "In App Purchasing"),
                new PackageModule("com.unity.services.economy", "Unity Economy"),

            }),

            new Pack("2D Basics", new List<PackageModule>(){
                new PackageModule("com.unity.2d.animation", "2D Animation"),
                new PackageModule("com.unity.2d.pixel-perfect", "2D Pixel Perfect"),
                new PackageModule("com.unity.2d.psdimporter", "2D PSD Importer"),
                new PackageModule("com.unity.2d.sprite", "2D Sprite"),
                new PackageModule("com.unity.2d.spriteshape", "2D SpriteShape"),
                new PackageModule("com.unity.2d.tilemap", "2D Tilemap Editor"),
                new PackageModule("com.unity.2d.tilemap.extras", "2D Tilemap Extras"),
            }),
            new Pack("Custom Package", new List<PackageModule>(){
            })
        };

        projectSetupPaths.Add(System.Tuple.Create<string, string>(Application.dataPath + "/Common/Scripts",             "Scripts"), false);
        projectSetupPaths.Add(System.Tuple.Create<string, string>(Application.dataPath + "/Common/Sprites",             "Sprites"), false);
        projectSetupPaths.Add(System.Tuple.Create<string, string>(Application.dataPath + "/Common/Prefabs",             "Prefabs"), false);
        projectSetupPaths.Add(System.Tuple.Create<string, string>(Application.dataPath + "/Common/Animations",          "Animations"), false);
        projectSetupPaths.Add(System.Tuple.Create<string, string>(Application.dataPath + "/Common/Scripts/Managers",    "Managers"), false);

    }


    [MenuItem("Tools/Quick Start Project")]
    static void Initilize()
    {
        //Window
        QuickStartProject window = (QuickStartProject)EditorWindow.GetWindow(typeof(QuickStartProject), true, string.Format("{0} v{1}", toolName, versionNumber), true);
     
        window.position = new Rect(Screen.width, Screen.height - (windowHeight / 2), windowWidth, windowHeight);
        window.Show();        
    }


    void OnGUI()
    { 

        this.Repaint();

        //PACKAGES
        EditorGUILayout.LabelField(toolName, EditorStyles.boldLabel);
        EditorGUILayout.Space(15);   

        displayPackages = EditorGUILayout.BeginToggleGroup("Packages", displayPackages);

        if (displayPackages){

            GUILayout.BeginHorizontal(GUILayout.MinWidth (position.width));
            
            foreach (var pack in packs)
            {

                GUILayout.BeginVertical(GUILayout.MaxWidth (5));

                //PACK NAME
                pack.importState.value = EditorGUILayout.ToggleLeft(pack.pack_Name, pack.importState.value);
                if (EditorGUILayout.BeginFadeGroup(pack.importState.faded)){
                
                    //CUSTOM PACKAGE AREA
                    if (pack.pack_Name.Contains("Custom Package")){

                        foreach (PackageModule packModule in pack.packageModules)
                        {

                            GUILayout.BeginHorizontal(GUILayout.MaxWidth (20));
                            
                            packModule.importState = EditorGUILayout.Toggle(packModule.importState, EditorStyles.toggle);
                            EditorGUILayout.TextArea(packModule.packageURL, GUILayout.MinWidth (200));

                            GUILayout.EndHorizontal();
                        }

                        // ADD AND REMOVE BUTTONS
                        GUILayout.BeginHorizontal(GUILayout.MaxWidth (5));

                        //ADD
                        if(GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width (21))){
                            addCustomPackage();
                        }

                        //REMOVE
                        if(GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width (21))){
                            removeCustomPackage();
                        }

                        GUILayout.EndHorizontal();
                    }

                    else{
                        foreach (PackageModule packModule in pack.packageModules)
                        {
                            packModule.importState = EditorGUILayout.ToggleLeft(packModule.package_Name, packModule.importState);
                        }
                    }
                }
                EditorGUILayout.EndFadeGroup();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.EndToggleGroup();

        }
        
        //PROJECT SETUP

        EditorGUILayout.Space(15);   

        displayProjectSetup = EditorGUILayout.BeginToggleGroup("Project Setup", displayProjectSetup);

        if (displayProjectSetup){
            foreach (var Key in projectSetupPaths.Keys)
            {
                projectSetupPaths[Key] = EditorGUILayout.ToggleLeft(string.Format(" Create {0} folder ( {1} )", Key.Item2 , Key.Item1), projectSetupPaths[Key]);
            }
        }

        EditorGUILayout.EndToggleGroup();

        /*
        if (GUILayout.Button("Close Window!")){
            this.Close();
        }

        if (GUILayout.Button("Import All")){
            importAll();
        }

        if (GUILayout.Button("Clear All")){
            this.Close();
        }
        */
    }

    void addCustomPackage(){
        var modules = packs.Find(x => x.pack_Name == "Custom Package").packageModules;
        modules.Add(new PackageModule("PackageUrl", "Package"));
    }

    void removeCustomPackage(){
        var modules = packs.Find(x => x.pack_Name == "Custom Package").packageModules;
        modules.RemoveAt(modules.Count-1);
    }





    void importAll(){
        List<string> packages = new List<string>();
        for (int i = 0; i < packs.Count; i++){
            var module = packs[i];
            for (int x = 0; x < module.packageModules.Count; x++)
            {
                if (module.packageModules[x].importState){
                    packages.Add(module.packageModules[x].packageURL);
                }
                
            }
        }
        Request = Client.AddAndRemove(packages.ToArray());
    }

     string GetRelativePath(string fullPath){
        if (fullPath != null && fullPath != ""){
            fullPath = fullPath.Replace("/", "\\");
            string[] pathSplit = fullPath.Split("\\Assets");
            string relativepath = "Assets" + pathSplit[pathSplit.Length-1];
            return relativepath;
        }
        else{
            return "";
        }
    }


}

public class Pack{
    public string pack_Name;
    public List<PackageModule> packageModules;
    public AnimBool importState = new AnimBool(false);
    public Pack(string package_Name, List<PackageModule> modules){
        this.pack_Name = package_Name;
        this.packageModules = modules;
    }
}

public class PackageModule{

    public PackageModule(string URL, string name){
        this.packageURL = URL;
        this.package_Name = name;
    }
    public bool importState = false;
    public string packageURL;
    public string package_Name;
}





    
