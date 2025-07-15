using System.IO;
using System.Xml;
using UnityEditor.Android;
using UnityEngine;

class SingleTopLaunchModeAndroidBuild : IPostGenerateGradleAndroidProject
{
    public int callbackOrder => 0;

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        bool success = false;
        var manifestPath = Path.Combine(path, "src/main/AndroidManifest.xml");

        var xmlDoc = new XmlDocument();
        xmlDoc.Load(manifestPath);

        var activityNodes = xmlDoc.SelectNodes("/manifest/application/activity") ?? throw new InvalidDataException("Illegal xml.");
        for (var i = 0; i < activityNodes.Count; i++)
        {
            var activityNode = activityNodes[i];
            var nameAttribute = activityNode.Attributes!["android:name"];
            if (nameAttribute == null) continue;
            // if launcher Activity is not UnityPlayerActivity, change to actual Activity.
            // TODO compat
            if (nameAttribute.Value != "com.unity3d.player.UnityPlayerActivity") continue;

            var launchAttribute = activityNode.Attributes!["android:launchMode"];
            if (launchAttribute != null)
            {
                success = true;
                launchAttribute.Value = "singleTop";
            }
            break;
        }

        xmlDoc.Save(manifestPath);

        if (success)
            Debug.Log("[SimZone] Your app's AndroidManifest.xml unityActivity is modified with launchMode (singleTop)");
        else
            Debug.Log("[SimZone] Failed to modify unityActivity launchMode!");
    }
}
