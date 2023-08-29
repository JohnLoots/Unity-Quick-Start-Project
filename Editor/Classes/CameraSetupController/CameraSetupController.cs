using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Cinemachine.PostFX;
using UnityEngine.Rendering;

public class CameraSetupController : ITask
{
    public bool setup2DCamera = false, createCinemachineCamera = false, setupPostProcessing = false;
    private CinemachineVirtualCamera cmCam = null;

    public bool _2DTransparancySortMode = false;
    public List<TransparencySortMode> _2DTransparancySortModeOptions = new(){
        TransparencySortMode.Default,
        TransparencySortMode.Perspective,
        TransparencySortMode.Orthographic,
        TransparencySortMode.CustomAxis,
    };
    public int _2DTransparancySortModeOptionsSelection = 0;
    public Vector3 _2DTranspancencySortModeCustomAxis = new Vector3(0, 0, 0);

    public void Start()
    {
        if (setup2DCamera){
            OnStart();
        }
    }

    void OnStart(){
        if (createCinemachineCamera){
            CreateCMCam();
        }

        if (setupPostProcessing){
            SetupPostProcessing();
        }
            
        if (_2DTransparancySortMode){
            Setup2DTransparancySortMode();
        }
    }


    void CreateCMCam(){
        if (cmCam == null){
            var c = new GameObject();
            cmCam = (CinemachineVirtualCamera) c.AddComponent(typeof(CinemachineVirtualCamera));
            cmCam.name = "CMCam";
        }
    }

    void SetupPostProcessing(){
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = setupPostProcessing;
        if (!(GetCinemachineVirtualCam() == null)){
            cmCam = GetCinemachineVirtualCam();
            cmCam.gameObject.AddComponent<CinemachineVolumeSettings>();
            var volume = cmCam.GetComponent<CinemachineVolumeSettings>();
            cmCam.AddExtension(volume);
        }

        else{
            var volume = (Volume)new GameObject().AddComponent(typeof(Volume));
            volume.name = "Global Volume";
        }
    }

    void Setup2DTransparancySortMode(){
        GraphicsSettings.transparencySortMode = _2DTransparancySortModeOptions[_2DTransparancySortModeOptionsSelection];
        GraphicsSettings.transparencySortAxis = _2DTranspancencySortModeCustomAxis;

//        ((UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset).GetRenderer(0)
        Camera.main.transparencySortMode = _2DTransparancySortModeOptions[_2DTransparancySortModeOptionsSelection];
        Camera.main.transparencySortAxis = _2DTranspancencySortModeCustomAxis;
    }
    
    CinemachineVirtualCamera GetCinemachineVirtualCam(){
        CinemachineVirtualCamera returnCam = (GameObject.FindObjectsOfType(typeof(CinemachineVirtualCamera)).Length > 0 ) ? (CinemachineVirtualCamera)GameObject.FindObjectOfType(typeof(CinemachineVirtualCamera)) : null ;
        return returnCam;
    }
}
