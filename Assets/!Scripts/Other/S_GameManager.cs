using System;
using Fusion;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Threading.Tasks;

public enum GameState
{
    Prelude = 0,
    Ongoing = 1,
    Ending =  2
}

public class S_GameManager : NetworkBehaviour
{
    public static S_GameManager instance;

    [Networked, SerializeField] public GameState GameState { get; set; } 
    [Networked, SerializeField] public float GameTime { get; set; } 

    public List<S_Food> currentFood { get; set; } = new List<S_Food>();
    public const int maxFood = 10;
    
    public static event Action OnFoodListFull;

    bool isLocal => Object && Object.HasStateAuthority;

    private async void Start()
    {
        instance = this;

        await Task.Delay(10000);

        TryUpdateGameState(GameState.Ongoing);
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (!isLocal) return;

        switch (GameState)
        {
            case GameState.Prelude:
                {
                    break;
                }
            case GameState.Ongoing:
                {
                    GameTime += Time.fixedDeltaTime;
                    break;
                }
            case GameState.Ending:
                {
                    break;
                }
        }
    }

    #region Food
    /// <summary>
    /// used to spawn food
    /// </summary>
    public static S_Food SpawnFood(S_Food food, Vector3 position, Quaternion rotation)
    {
        if (instance.currentFood.Count >= maxFood)
        {
            if (instance != null && instance.HasStateAuthority)
            {
                instance.RPC_FullFoodSpawn();
            }
            return null;
        }

        if (food == null)
        {
            Debug.LogError("Oh No, No food");
            return null;
        }

        if (instance == null)
        {
            Debug.LogError("Oh No, No game manager in scene");
            return null;
        }

        if (instance.Runner == null)
        {
            Debug.LogError("Oh No, No runner");
            return null;
        }

        S_Food instantiatedFood = instance.Runner.Spawn(food.GetComponent<NetworkObject>(), position, rotation).GetComponent<S_Food>();

        if (instantiatedFood == null) return null;
        instance.currentFood.Add(instantiatedFood);

        return instantiatedFood;
    }

    /// <summary>
    /// used to destroy food
    /// </summary>
    public static void DespawnFood(S_Food food)
    {
        S_Food toDestroy = food.GetComponent<S_Food>();
        if (toDestroy == null) return;

        instance.currentFood.Remove(toDestroy);
        instance.Runner.Despawn(toDestroy.GetComponent<NetworkObject>());
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_FullFoodSpawn()
    {
        Debug.Log("Food list is full and needs to trash some food");
        OnFoodListFull?.Invoke();
    }
    #endregion

    public static void TryUpdateGameState(GameState state)
    {
        Debug.Log("[GameManager] updating game state: " + state);
        instance.RPC_UpdateGameState(state);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_UpdateGameState(GameState state)
    {
        if (GameState == state) return;

        GameState = state;
        Debug.Log("[GameManager] successfully updated game state: " + state);

        switch (GameState)
        {
            case GameState.Prelude:
                {
                    GameTime = 0;
                    break;
                }
            case GameState.Ongoing:
                {
                    break;
                }
            case GameState.Ending:
                {
                    GameTime = 0;
                    break;
                }
        }
    }
}
