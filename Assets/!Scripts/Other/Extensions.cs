using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Extentions.Addressable
{
    public static class Addressable
    {
        /// <summary>
        /// Easily accessable addressabless
        /// </summary>
        public static readonly string[] addressables = new string[]
        {
            "SharedNetworkPlayer",
            "SharedLocalPlayer",
        };

        /// <summary>
        /// Easiy accessable lables
        /// </summary>
        public static readonly string[] labels = new string[]
        {
        };

        /// <summary>
        /// Load a addressables asset by its addressable name
        /// </summary>
        /// <typeparam name="T">The type to load</typeparam>
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

