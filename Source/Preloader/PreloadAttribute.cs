using System;
using JetBrains.Annotations;

namespace BossChallengeMod.Preload;

[AttributeUsage(AttributeTargets.Field)]
[MeansImplicitUse]
[PublicAPI]
public class PreloadAttribute(string scene, string path) : Attribute {
    public string Scene = scene;
    public string Path = path;
}