using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageCategory 
{
    public bool importState = false;
    public string _name;
    public List<Package> packages;
    public PackageCategory(string name, List<Package> modules){
        this._name = name;
        this.packages = modules;
    }
}

public class Package{
    public bool importState = false;
    public System.Tuple<string, string> data;
    public Package(string name, string URL){
        data = System.Tuple.Create<string, string>(name, URL);
    }
}
