using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class FolderSetupController : ITask
{
    
    public List<FolderSetupSetting> folderSetupSettings = new();

    public FolderSetupController(){
        
        folderSetupSettings.Add(new FolderSetupSetting(Application.dataPath + "/Common/Scripts",             "Scripts"));
        folderSetupSettings.Add(new FolderSetupSetting(Application.dataPath + "/Common/Sprites",             "Sprites"));
        folderSetupSettings.Add(new FolderSetupSetting(Application.dataPath + "/Common/Prefabs",             "Prefabs"));
        folderSetupSettings.Add(new FolderSetupSetting(Application.dataPath + "/Common/Animations",             "Animations"));
        folderSetupSettings.Add(new FolderSetupSetting(Application.dataPath + "/Common/Scripts/Managers",             "Managers"));
        folderSetupSettings.Add(new FolderSetupSetting(Application.dataPath + "/Common/Scripts/Classes",             "Classes"));
    }

    public void Start(){
        for (int i = 0; i < folderSetupSettings.Count; i++)
        {
            if (folderSetupSettings[i].createValue){
                CreatePath(folderSetupSettings[i].data.Item1);
            }
        }
    }

    void CreatePath(string path){
        Directory.CreateDirectory(path);
    }
}
