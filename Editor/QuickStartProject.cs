using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class QuickStartProject : UnityEditor.EditorWindow, ITask
{
    private static double versionNumber = 1.1;
    private static string toolName = "Unity QuickStart Project";
    static int windowWidth = 680;
    static int windowHeight = 680;
    private PackageController packageController = new();
    private ProjectSetupController projectSettingsController = new();

    Vector2 slideView = Vector2.zero;
    
    [MenuItem("Tools/Quick Start Project")]
    static void Initilize()
    {
        //Window
        QuickStartProject window = (QuickStartProject)GetWindow(typeof(QuickStartProject), true, string.Format("{0} v{1}", toolName, versionNumber), true);
        window.position = new Rect(0 + (windowWidth/2) , Screen.height - (windowHeight / 2), windowWidth, windowHeight);
        window.minSize = new Vector2(windowWidth, windowHeight);
        window.maxSize = new Vector2(windowWidth, windowHeight);
        window.Show();      
    }

    [System.Obsolete]
    void OnGUI()
    { 
        this.Repaint();

            //HEADING
            EditorGUILayout.Space(5);   
            EditorGUILayout.LabelField(toolName, EditorStyles.boldLabel);
            EditorGUILayout.Space(5);   
            slideView = GUILayout.BeginScrollView(slideView, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(false));
            /////////////////////////////////
            


            
            
            //PACKAGES
            packageController.enabled = EditorGUILayout.BeginToggleGroup("Packages", packageController.enabled);
            if (packageController.enabled){

                GUILayout.BeginHorizontal(GUILayout.MinWidth (position.width));
                foreach (PackageCategory catergory in packageController.packageCategories)
                {
                    
                    if (catergory != packageController.packageCategories[0]){
                        EditorGUILayout.Space(5);   
                    }
                    
                    GUILayout.BeginVertical(GUILayout.MaxWidth (200));
                        catergory.importState = EditorGUILayout.BeginToggleGroup(catergory._name, catergory.importState);
                        
                        if (catergory.importState){
                            
                            System.Action action;
                            
                            foreach (Package package in catergory.packages)
                            {
                                action = (catergory._name.Contains("Custom Packages")) ? (() => blitInputLable(package.data.Item1, ref package.importState, 150)) : (() => blitToggleLable(package.data.Item1, ref package.importState));
                                action.Invoke();
                            }

                            // BUTTONS
                            if (catergory._name.Contains("Custom Packages")){
                                GUILayout.BeginHorizontal(GUILayout.MaxWidth (5));
                                    blitButton("+", addCustomPackage, EditorStyles.miniButton, GUILayout.MinWidth (21));
                                    blitButton("-", removeCustomPackage, EditorStyles.miniButton, GUILayout.MinWidth (21));
                                GUILayout.EndHorizontal();
                            }
                        }
                        EditorGUILayout.EndToggleGroup();
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndToggleGroup();
            /////////////////////////////////
            
            
            
            
            //PROJECT SETUP
            EditorGUILayout.Space(5);   

            projectSettingsController.enabled = EditorGUILayout.BeginToggleGroup("Project Setup", projectSettingsController.enabled);
            if (projectSettingsController.enabled){
                
                EditorGUILayout.Space(5);   
                EditorGUILayout.LabelField("Folder Setup", EditorStyles.boldLabel);

                for (int i = 0; i < projectSettingsController.folderSetupController.folderSetupSettings.Count; i++)
                {
                    var key = projectSettingsController.folderSetupController.folderSetupSettings[i].data;
                    projectSettingsController.folderSetupController.folderSetupSettings[i].createValue = EditorGUILayout.ToggleLeft(string.Format(" Create {0} folder ( {1} )", key.Item2 , key.Item1), projectSettingsController.folderSetupController.folderSetupSettings[i].createValue);
                }
                
                EditorGUILayout.Space(5);   
                EditorGUILayout.LabelField("Project Setup", EditorStyles.boldLabel);

                
                GUILayout.BeginHorizontal(GUILayout.MinWidth (position.width));
                    GUILayout.BeginVertical(GUILayout.MaxWidth (200));

                        projectSettingsController.cameraSetupController.setup2DCamera = EditorGUILayout.BeginToggleGroup("Setup Camera2D", projectSettingsController.cameraSetupController.setup2DCamera);

                        if (projectSettingsController.cameraSetupController.setup2DCamera){
                            blitToggleLable("Create New Cinemachine Camera ( Automatically selects cinemachine package )", ref projectSettingsController.cameraSetupController.createCinemachineCamera);
                            
                            blitToggleLable("Setup Post Processing", ref projectSettingsController.cameraSetupController.setupPostProcessing);

                            blitToggleLable("2D Transparancy Sort Mode", ref projectSettingsController.cameraSetupController._2DTransparancySortMode );
                            if (projectSettingsController.cameraSetupController._2DTransparancySortMode){
                                projectSettingsController.cameraSetupController._2DTransparancySortModeOptionsSelection = EditorGUILayout.Popup("Transparancy Sorting Mode",projectSettingsController.cameraSetupController._2DTransparancySortModeOptionsSelection, projectSettingsController.cameraSetupController._2DTransparancySortModeOptions.ConvertAll(x => x.ToString()).ToArray());
                                projectSettingsController.cameraSetupController._2DTranspancencySortModeCustomAxis = EditorGUILayout.Vector3Field("Custom Axis", projectSettingsController.cameraSetupController._2DTranspancencySortModeCustomAxis);
                            }
                        //projectSettingsController.cameraSetupController._2DTransparancySortModeOptions.ToArray()
                        }

                        EditorGUILayout.EndToggleGroup();


                        projectSettingsController.preferencesSetupController.enabled = EditorGUILayout.BeginToggleGroup("General/Preferences", projectSettingsController.preferencesSetupController.enabled);

                        if (projectSettingsController.preferencesSetupController.enabled){
                            
                            GUILayout.BeginHorizontal(GUILayout.MinWidth (200));
                            blitToggleLable("Script changes while playing", ref projectSettingsController.preferencesSetupController.scriptCompilationDuringPlayEnabled);
                            if(projectSettingsController.preferencesSetupController.scriptCompilationDuringPlayEnabled){
                                projectSettingsController.preferencesSetupController.scriptCompilationDuringPlaySelection = EditorGUILayout.Popup("", projectSettingsController.preferencesSetupController.scriptCompilationDuringPlaySelection, projectSettingsController.preferencesSetupController.scriptCompilationDuringPlayOptions.ToArray());
                            }
                            GUILayout.EndHorizontal();

                            
                            GUILayout.BeginHorizontal(GUILayout.MinWidth (200));
                            blitToggleLable("Company name", ref projectSettingsController.preferencesSetupController.companyNameEnabled);
                            if(projectSettingsController.preferencesSetupController.companyNameEnabled){
                                projectSettingsController.preferencesSetupController.companyName = EditorGUILayout.TextArea(projectSettingsController.preferencesSetupController.companyName, GUILayout.MinWidth (200));
                            }
                            GUILayout.EndHorizontal();
                            
                            GUILayout.BeginHorizontal(GUILayout.MinWidth (200));
                            blitToggleLable("Product Name", ref projectSettingsController.preferencesSetupController.productNameEnabled);
                            if(projectSettingsController.preferencesSetupController.productNameEnabled){
                                projectSettingsController.preferencesSetupController.productName = EditorGUILayout.TextArea(projectSettingsController.preferencesSetupController.productName, GUILayout.MinWidth (200));
                            }
                            GUILayout.EndHorizontal();
                            
                            GUILayout.BeginHorizontal(GUILayout.MinWidth (200));
                            blitToggleLable("Icon", ref projectSettingsController.preferencesSetupController.productIconEnabled);
                            if(projectSettingsController.preferencesSetupController.productIconEnabled){
                                projectSettingsController.preferencesSetupController.productIcon = (Texture2D)EditorGUILayout.ObjectField(projectSettingsController.preferencesSetupController.productIcon, typeof(Texture2D), GUILayout.MinWidth (200));
                            }
                            GUILayout.EndHorizontal();
                            
                            GUILayout.BeginHorizontal(GUILayout.MinWidth (200));
                            blitToggleLable("Mousecursor", ref projectSettingsController.preferencesSetupController.mouseCursorEnabled);
                            if(projectSettingsController.preferencesSetupController.mouseCursorEnabled){
                                projectSettingsController.preferencesSetupController.mouseCursor = (Texture2D)EditorGUILayout.ObjectField(projectSettingsController.preferencesSetupController.mouseCursor, typeof(Texture2D), GUILayout.MinWidth(200));
                            }
                            GUILayout.EndHorizontal();
                        }

                        EditorGUILayout.EndToggleGroup();

                    GUILayout.EndVertical();
                
                GUILayout.EndHorizontal();

            }
            EditorGUILayout.EndToggleGroup();
            EditorGUILayout.Space(10);   
            //////////////////////////////




            // BOTTOM BUTTONS
            GUILayout.BeginHorizontal();
            
                EditorGUILayout.Space(5);   
                blitButton("Close Window!", this.Close, EditorStyles.miniButton , GUILayout.MinWidth (175) );

                EditorGUILayout.Space(5);   
                blitButton("Clear", this.Close, EditorStyles.miniButton , GUILayout.MinWidth (175) );
                
                EditorGUILayout.Space(5);   
                blitButton("Start", this.Start, EditorStyles.miniButton , GUILayout.MinWidth (175) );
                EditorGUILayout.Space(5);   
                
            GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }

    void addCustomPackage(){
        var packages = packageController.packageCategories.Find(x => x._name == "Custom Packages").packages;
        packages.Add(new Package("PackageUrl", "Package"));
    }

    void removeCustomPackage(){
        var packages = packageController.packageCategories.Find(x => x._name == "Custom Packages").packages;
        packages.RemoveAt(packages.Count-1);
    }


    public void Start(){
        packageController.Start();
        projectSettingsController.Start();
    }


    void blitToggleLable(string text, ref bool toggle){
        GUILayout.BeginHorizontal(GUILayout.MaxWidth (20));
        toggle = EditorGUILayout.Toggle(toggle, EditorStyles.toggle);
        EditorGUILayout.LabelField(text, GUILayout.MinWidth (200));
        GUILayout.EndHorizontal();
    }

    void blitInputLable(string text, ref bool toggle, int width=20){
        GUILayout.BeginHorizontal(GUILayout.MaxWidth (width));
        toggle = EditorGUILayout.Toggle(toggle, EditorStyles.toggle);
        EditorGUILayout.TextArea(text, GUILayout.MinWidth (width));
        GUILayout.EndHorizontal();
    }

    void blitButton(string text, System.Action eventAction, GUIStyle style, GUILayoutOption option1 = null, GUILayoutOption option2 = null){
        if(GUILayout.Button(text, style, option1)){ 
            eventAction.Invoke(); 
        }
    }
}