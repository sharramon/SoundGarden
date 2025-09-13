using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    [SerializeField] private ResourceScriptable _resourceScriptable;
    public ResourceScriptable resourceScriptable { get { return _resourceScriptable; } }
}
