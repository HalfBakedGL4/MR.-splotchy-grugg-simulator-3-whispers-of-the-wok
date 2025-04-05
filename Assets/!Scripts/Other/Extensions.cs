using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Extentions
{

    namespace Addressable
    {
        public enum AddressableAsset
        {
            SharedNetworkPlayer = 0,
            SharedLocalPlayer = 1,
            RecipeBook = 2,
            BurntFood = 3,
        }
        /// <summary>
        /// a extension class made for addressables
        /// </summary>
        public static class Addressable
        {
            /// <summary>
            /// Easily accessable addressable names
            /// </summary>
            public static readonly Dictionary<AddressableAsset, string> paths = new Dictionary<AddressableAsset, string>
            {
                { AddressableAsset.SharedNetworkPlayer, "SharedNetworkPlayer" },
                { AddressableAsset.SharedLocalPlayer, "SharedLocalPlayer" },
                { AddressableAsset.RecipeBook,  "RecipeBook" },
                { AddressableAsset.BurntFood,  "BurntFood" }
            };

            /// <summary>
            /// Easiy accessable addressable lables
            /// </summary>
            public static readonly string[] labels = new string[]
            {
            };

            #region Load Asset

            /// <summary>
            /// Load a addressables asset by its addressable name
            /// </summary>
            /// <param name="addressable">The addressable asset</param>
            /// <returns>Addressables gameobject</returns>
            public static async Task<GameObject> LoadAsset(AddressableAsset addressable)
            {
                return await LoadAsset<GameObject>(paths[AddressableAsset.SharedNetworkPlayer]);
            }
            /// <summary>
            /// Load a addressables asset by its addressable name
            /// </summary>
            /// <param name="addressable">The addressable asset</param>
            /// <returns>Addressables gameobject</returns>
            public static async Task<GameObject> LoadAsset(string addressable)
            {
                return await LoadAsset<GameObject>(addressable);
            }

            /// <summary>
            /// Load a addressables asset by its addressable name
            /// </summary>
            /// <typeparam name="T">The type to find</typeparam>
            /// <param name="addressable">The addressable asset</param>
            /// <returns>Addressables asset</returns>
            public static async Task<T> LoadAsset<T>(AddressableAsset addressable)
            {
                return await LoadAsset<T>(paths[AddressableAsset.SharedNetworkPlayer]);
            }

            /// <summary>
            /// Load a addressables asset by its addressable name
            /// </summary>
            /// <typeparam name="T">The type to find</typeparam>
            /// <param name="addressable">The addressable asset</param>
            /// <param name="getType">if it should return a script from a GameObject rather than return the gameobject</param>
            /// <returns>Addressables asset</returns>
            public static async Task<T> LoadAsset<T>(AddressableAsset addressable, bool getType) where T : Object
            {
                return await LoadAsset<T>(paths[AddressableAsset.SharedNetworkPlayer], getType);
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
            #endregion

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
        public enum AuthorityResult
        {
            NotNeeded,
            Failure,
            Success,
        }

        /// <summary>
        /// Class extension for shared topology
        /// </summary>
        public static class Shared
        {
            /// <summary>
            /// Use to gain state authoirty
            /// </summary>
            /// <param name="networkObject">The networkobject to gain authority over</param>
            public static async Task GainStateAuthority(NetworkObject networkObject)
            {
                Debug.Log("[StateAuthority] attempting to get authority over " + networkObject.gameObject.name);

                AuthorityResult result = await AttemptStateAuthority(networkObject);

                Debug.Log("[StateAuthority] State Authority check = " + result);
            }
            public async static Task<AuthorityResult> AttemptStateAuthority(NetworkObject networkObject, int maxTries = 1000)
            {
                if(networkObject.HasStateAuthority) 
                    return AuthorityResult.NotNeeded;

                networkObject.RequestStateAuthority();

                int i = 0;
                while (!networkObject.HasStateAuthority)
                {
                    if (i > maxTries || !Application.isPlaying)
                    {
                        Debug.Log("[StateAuthority] StateAuthority timeout or application ended");
                        return AuthorityResult.Failure;
                    }

                    await Task.Delay(10);

                    i++;
                    Debug.Log("[StateAuthority] Attempt " + i);
                }

                Debug.Log("[StateAuthority] Gained state authority after " + (10 * i / 1000) + " seconds (" + (10 * i) + " milliseconds)");
                return AuthorityResult.Success;
            }
        }
    }
}

