using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderSetupSetting 
{
    public bool createValue = false;

    public System.Tuple<string, string> data;

    public FolderSetupSetting(string name, string URL){
        data = System.Tuple.Create<string, string>(name, URL);
    }
}
