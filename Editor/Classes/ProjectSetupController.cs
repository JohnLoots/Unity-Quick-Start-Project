using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectSetupController : ITask
{
    public FolderSetupController folderSetupController = new();
    public CameraSetupController cameraSetupController = new();
    public PreferencesSetupController preferencesSetupController = new();
    
    public bool enabled = false;


    public void Start()
    {
        folderSetupController.Start();
        cameraSetupController.Start();
        preferencesSetupController.Start();
    }    
}
