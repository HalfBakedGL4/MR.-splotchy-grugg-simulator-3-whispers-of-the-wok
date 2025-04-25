using UnityEditor;
using UnityEngine;
using Extentions.Addressable;
using Verpha.HierarchyDesigner;

public class S_EditorMenu : MonoBehaviour
{
    static HierarchyDesignerFolder networking;
    static HierarchyDesignerFolder mr;
    static HierarchyDesignerFolder world;

    [MenuItem("GameElements/Add All")]
    static void AllMyScene()
    {
        NetworkMyScene();
        MRMyScene();
        GameMyScene();
    }
    [MenuItem("GameElements/Add Networking")]
    static async void NetworkMyScene()
    {
        if (networking == null)
            networking = await Addressable.LoadAsset<HierarchyDesignerFolder>(AddressableAsset.Networking);

        Instantiate(networking);
    }
    [MenuItem("GameElements/Add MR")]
    static async void MRMyScene()
    {
        if (mr == null)
            mr = await Addressable.LoadAsset<HierarchyDesignerFolder>(AddressableAsset.MR);

        Instantiate(mr);
    }
    [MenuItem("GameElements/Add Game")]
    static async void GameMyScene()
    {
        if (world == null)
            world = await Addressable.LoadAsset<HierarchyDesignerFolder>(AddressableAsset.World);

        Instantiate(world);
    }
}
