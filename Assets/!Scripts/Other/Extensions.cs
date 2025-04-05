using Fusion;
using Oculus.Interaction;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Extentions
{

    namespace Addressable
    {
        /// <summary>
        /// a extension class made for addressables
        /// </summary>
        public static class Addressable
        {
            /// <summary>
            /// Easily accessable addressable names
            /// </summary>
            public static readonly string[] names = new string[]
            {
            "SharedNetworkPlayer",
            "SharedLocalPlayer",
            "RecipeBook",
            "BurntFood"
            };

            /// <summary>
            /// Easiy accessable addressable lables
            /// </summary>
            public static readonly string[] labels = new string[]
            {
            };

            /// <summary>
            /// Load a addressables asset by its addressable name
            /// </summary>
            /// <typeparam name="T">The type to find</typeparam>
            /// <param name="addressable">The addressable name</param>
            /// <returns>Addressables asset</returns>
            public static async Task<T> LoadAsset<T>(string addressable)
            {
                AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(addressable);
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    return (T)handle.Result;
                }

                Debug.LogError(handle.Status);
                return default;
            }
            /// <summary>
            /// Load a addressables asset by its addressable name
            /// </summary>
            /// <typeparam name="T">The type to find</typeparam>
            /// <param name="addressable">The addressable name</param>
            /// <param name="getType">if it should return a script from a GameObject rather than return the gameobject</param>
            /// <returns>Addressables asset</returns>
            public static async Task<T> LoadAsset<T>(string addressable, bool getType) where T : Object
            {
                T item = await LoadAsset<T>(addressable);

                if (item is ScriptableObject)
                {
                    Debug.LogWarning(nameof(getType) + " should be false as " + typeof(T) + " is a ScriptableObject");
                    return item;
                }
                else if (item is GameObject && getType)
                {
                    return (item as GameObject).GetComponent<T>();
                }

                return item;
            }

            /// <summary>
            /// Load addressables assets by their label
            /// </summary>
            /// <typeparam name="T">The type of assets to load</typeparam>
            /// <param name="label">The label</param>
            /// <returns>List of asseets</returns>
            public static async Task<List<T>> LoadAssets<T>(string label)
            {
                AsyncOperationHandle handle = Addressables.LoadAssetsAsync<T>(label);
                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    return (List<T>)handle.Result;
                }

                Debug.LogError(handle.Status);
                return default;
            }
        }
    }

    namespace Networking
    {
        /// <summary>
        /// Class extension for shared topology
        /// </summary>
        public static class Shared
        {
            /// <summary>
            /// Use to gain state authoirty
            /// </summary>
            /// <param name="networkObject">The networkobject to gain authority over</param>
            /// <returns></returns>
            public async static Task<bool> GetStateAuthority(NetworkObject networkObject, int maxTries = 1000)
            {
                networkObject.RequestStateAuthority();

                int i = 0;
                while (!networkObject.HasStateAuthority)
                {
                    if (!Application.isPlaying) return false;
                    if (i > maxTries) return false;

                    await Task.Delay(10);

                    i++;
                    Debug.Log("[interactor] Check " + i);
                }

                Debug.Log("[interactor] Gained state authority after " + ((10 * i) / 1000) + " seconds (" + (10 * i) + " milliseconds)");
                return true;
            }
        }
    }
}

