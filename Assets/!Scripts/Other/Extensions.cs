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
        public enum AddressableLabel
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
            static readonly Dictionary<AddressableAsset, string> paths = new Dictionary<AddressableAsset, string>
            {
                { AddressableAsset.SharedNetworkPlayer, "SharedNetworkPlayer" },
                { AddressableAsset.SharedLocalPlayer, "SharedLocalPlayer" },
                { AddressableAsset.RecipeBook,  "RecipeBook" },
                { AddressableAsset.BurntFood,  "BurntFood" }
            };

            /// <summary>
            /// Easiy accessable addressable lables
            /// </summary>
            static readonly Dictionary<AddressableLabel, string> lables = new Dictionary<AddressableLabel, string>
            {
            };

            /// <summary>
            /// return the coresponding path
            /// </summary>
            /// <param name="addressable">the addressable to load</param>
            /// <returns>path</returns>
            public static string ReturnPath(AddressableAsset addressable)
            {
                if(paths[addressable] != null)
                    return paths[addressable];

                Debug.LogError("[Addressable] path not found");
                return null;
            }

            #region Load Asset

            /// <summary>
            /// Load a addressables asset by its addressable name
            /// </summary>
            /// <param name="addressable">The addressable asset</param>
            /// <returns>Addressables gameobject</returns>
            public static async Task<GameObject> LoadAsset(AddressableAsset addressable)
            {
                return await LoadAsset<GameObject>(ReturnPath(addressable));
            }

            /// <summary>
            /// Load a addressables asset by its addressable name
            /// </summary>
            /// <typeparam name="T">The type to find</typeparam>
            /// <param name="addressable">The addressable asset</param>
            /// <returns>Addressables asset</returns>
            public static async Task<T> LoadAsset<T>(AddressableAsset addressable) where T : Object
            {
                return await LoadAsset<T>(ReturnPath(addressable));
            }

            /// <summary>
            /// Load a addressables asset by its addressable name
            /// </summary>
            /// <typeparam name="T">The type to find</typeparam>
            /// <param name="addressable">The addressable asset</param>
            /// <param name="getComponent">if it should return a script from a GameObject rather than return the gameobject</param>
            /// <returns>Addressables asset</returns>
            public static async Task<T> LoadAsset<T>(AddressableAsset addressable, bool getComponent = true) where T : MonoBehaviour
            {
                return await LoadAsset<T>(ReturnPath(addressable), getComponent);
            }

            /// <summary>
            /// Load a addressables asset by its addressable name
            /// </summary>
            /// <typeparam name="T">The type to find</typeparam>
            /// <param name="addressable">The addressable name</param>
            /// <param name="getType">if it should return a script from a GameObject rather than return the gameobject</param>
            /// <returns>Addressables asset</returns>
            static async Task<T> LoadAsset<T>(string addressable, bool getType) where T : MonoBehaviour
            {
                GameObject item = await LoadAsset<GameObject>(addressable);

                if (item == null)
                {
                    Debug.LogError("[Addressable] item is null");
                }
                if (getType)
                {
                    return item.GetComponent<T>();
                }

                return null;
            }

            /// <summary>
            /// Load a addressables asset by its addressable name
            /// </summary>
            /// <typeparam name="T">The type to find</typeparam>
            /// <param name="addressable">The addressable name</param>
            /// <returns>Addressables asset</returns>
            static async Task<T> LoadAsset<T>(string addressable) where T : Object
            {
                if (addressable == null || addressable == string.Empty)
                {
                    Debug.LogError("[Addressable] empty string");
                    return default;
                }

                Debug.Log("[Addressable] loading " + addressable);

                AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(addressable);
                await handle.Task;


                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    return (T)handle.Result;
                }

                Debug.LogError("[Addressable] " + handle.Status + " to load " + addressable);
                return default;
            }
            #endregion

            /// <summary>
            /// Load addressables assets by their label
            /// </summary>
            /// <typeparam name="T">The type of assets to load</typeparam>
            /// <param name="label">The label</param>
            /// <returns>List of asseets</returns>
            public static async Task<List<T>> LoadAssets<T>(string label) where T : Object
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

