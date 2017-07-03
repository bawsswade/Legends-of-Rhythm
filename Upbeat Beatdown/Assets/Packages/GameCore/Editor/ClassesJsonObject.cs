using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Folder
{
    public string Path { get; set; }
}

public class Property
{
    public string Access { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public bool IsProperty { get; set; }
}

public class Argument
{
    public string Name { get; set; }
    public string Type { get; set; }
}

public class Method
{
    public string Name { get; set; }
    public bool IsOverride { get; set; }
    public string ReturnType { get; set; }
    public string Access { get; set; }
    public List<Argument> Arguments { get; set; }
    public List<string> Content { get; set; } 
}

public class Class
{
    public string Name { get; set; }
    public string Extends { get; set; }
    public string Path { get; set; }
    public List<string> Using { get; set; }
    public List<Property> Properties { get; set; }
    public List<Method> Methods { get; set; }
    
}

public class ClassJsonObject
{
    public List<Folder> Folders { get; set; }
    public List<Class> Classes { get; set; }
}