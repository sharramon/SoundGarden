using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceScriptable", menuName = "Sound Garden/Resource scriptable", order = 0)]
public class ResourceScriptable : ScriptableObject
{
    [SerializeField] private Seed _seed;
    [HideInInspector] public Seed seed { get { return _seed; } }
    [SerializeField] private MusicTimeline _musicTimeline;
    [HideInInspector] public MusicTimeline musicTimeline { get { return _musicTimeline; } }


}
