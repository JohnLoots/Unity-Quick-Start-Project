using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PackageController : ITask
{
    public List<PackageCategory> packageCategories = new List<PackageCategory>();
    public bool enabled = false;
    public PackageController(){
        
        packageCategories.Add(new PackageCategory("Generic", new List<Package>(){
            
            new Package("Addressables", "com.unity.addressables"),
            new Package("AI Navigation", "com.unity.ai.navigation"),
            new Package("Febucci Custom Hierarchy", "https://github.com/febucci/unitypackage-custom-hierarchy.git"),
            new Package("Visual Studio Editor", "com.unity.ide.visualstudio"),
            new Package("Gameplay and Storytelling", "com.unity.feature.gameplay-storytelling"),
            new Package("Mobile", "com.unity.feature.mobile"),
            new Package("New Input System", "com.unity.inputsystem"),
            new Package("Post Processing", "com.unity.postprocessing"),
            new Package("In App Purchasing", "com.unity.purchasing"),
            new Package("Unity Economy", "com.unity.services.economy"),
            new Package("TextMeshPro", "com.unity.textmeshpro"),
        
        }));

        packageCategories.Add(new PackageCategory("2D Basics", new List<Package>(){
            
            new Package("2D Animation", "com.unity.2d.animation"),
            new Package("2D Pixel Perfect", "com.unity.2d.pixel-perfect"),
            new Package("2D PSD Importer", "com.unity.2d.psdimporter"),
            new Package("2D Sprite", "com.unity.ide.visualstudio"),
            new Package("2D SpriteShape", "com.unity.2d.spriteshape"),
            new Package("2D Tilemap Editor", "com.unity.2d.tilemap"),
            new Package("2D Tilemap Extras", "com.unity.2d.tilemap.extras"),
            new Package("Post Processing", "com.unity.postprocessing"),
            new Package("In App Purchasing", "com.unity.purchasing"),
            new Package("Unity Economy", "com.unity.services.economy"),
            new Package("TextMeshPro", "com.unity.textmeshpro"),
            new Package("2D Navmesh Plus", "https://github.com/h8man/NavMeshPlus.git"),
        
        }));
        
        packageCategories.Add(new PackageCategory("Custom Packages", new List<Package>(){
        
        }));
    }

    public void Start(){
        List<string> s = new List<string>();

        foreach (var category in packageCategories)
        {
            foreach (var package in category.packages)
            {

                if (package.importState){
                    s.Add(package.data.Item2);
                }
            }
        }

        
        UnityEditor.PackageManager.Requests.AddAndRemoveRequest t_request = UnityEditor.PackageManager.Client.AddAndRemove(s.ToArray());
    }
}
