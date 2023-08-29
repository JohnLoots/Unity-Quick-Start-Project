using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PreferencesSetupController : ITask
{
    public bool enabled = false;
    public bool scriptCompilationDuringPlayEnabled = false;
    public List<string> scriptCompilationDuringPlayOptions = new(){
        "Recompile And Continue Playing",
        "Recompile After Finish Playing",
        "Stop Play And Recompile",
    };

    public int scriptCompilationDuringPlaySelection = 0;


    // this is ugly but ill refactor it and make it better later
    public bool companyNameEnabled = false;
    public string companyName = "";

    public bool productNameEnabled = false;
    public string productName = "";


    public bool productIconEnabled = false;
    public Texture2D productIcon = null;


    public bool mouseCursorEnabled = false;
    public Texture2D mouseCursor = null;



    public void Start()
    {
        if (enabled){
            if (scriptCompilationDuringPlayEnabled){
                EditorPrefs.SetString("Script Changes While Playing", scriptCompilationDuringPlayOptions[scriptCompilationDuringPlaySelection]);
            }

            if (companyNameEnabled){
                UnityEditor.PlayerSettings.companyName = companyName;
            }

            if (productNameEnabled){
                UnityEditor.PlayerSettings.productName = productName;
            }


            if (productIconEnabled){
                //UnityEditor.PlayerSettings.SetIcons
            }

            if (mouseCursorEnabled){
                UnityEditor.PlayerSettings.defaultCursor = mouseCursor;
            }
        }
    }

}
