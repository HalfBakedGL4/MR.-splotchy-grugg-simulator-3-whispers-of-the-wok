using System;
using Fusion;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;
using NaughtyAttributes;
using UnityEngine.Rendering;

public enum GameState
{
    Intermission,
    Starting,
    Ongoing,
    Ending
}

public class S_GameManager : NetworkBehaviour
{
    public static S_GameManager instance;

    public static GameState CurrentGameState => instance.GameState;
    [Networked, SerializeField] public GameState GameState { get; private set; }

    [Space]
    public int startTime = 7;
    [Networked, SerializeField] public float GameTime { get; private set; }

    [Space]
    public int startDelay = 5;
    [Networked] float delay { get; set; } 

    public List<S_Food> currentFood { get; private set; } = new List<S_Food>();
    public const int maxFood = 10;
    
    public static event Action OnFoodListFull;

    bool isLocal => Object && Object.HasStateAuthority;

    public override void Spawned()
    {
        base.Spawned();

        instance = this;


        if (!isLocal) return;

        GameTime = startTime * 60;
        delay = startDelay;

        ProgressGameState();
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (!isLocal) return;

        switch (GameState)
        {
            case GameState.Intermission:
                {
                    Intermission();
                    break;
                }
            case GameState.Starting:
                {
                    Starting();
                    break;
                }
            case GameState.Ongoing:
                {
                    Ongoing();
                    break;
                }
            case GameState.Ending:
                {
                    Ending();
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

    #region Game States
    void Intermission()
    {

    }

    void Starting()
    {
        delay -= Time.fixedDeltaTime;

        if (delay <= 0)
        {
            ProgressGameState();
        }
    }

    void Ongoing()
    {
        GameTime -= Time.fixedDeltaTime;

        if(GameTime <= 0)
        {
            ProgressGameState();
        }
    }

    void Ending()
    {
        delay -= Time.fixedDeltaTime;

        if (delay <= 0)
        {
            ProgressGameState();
        }
    }

    public static string GetGameTime(int decimals = 0)
    {
        if(decimals > 0)
        {
            float f = MathF.Pow(10, decimals);
            float value = Mathf.Round(instance.GameTime * f) / f;

            string format = "0.";

            for (int i = 0; i < decimals; i++)
            {
                format += "0";
            }

            return value.ToString(format);
        }

        return Mathf.Round(instance.GameTime).ToString();
    }

    public static void ProgressGameState()
    {
        GameState newGameState = (GameState)((int)instance.GameState + 1);

        if ((int)newGameState > (int)GameState.Ending)
            newGameState = 0;

        Debug.Log("[GameManager] updating game state to: " + newGameState);
        instance.RPC_UpdateGameState(newGameState);
    }

    public UnityEvent OnIntermission;
    public UnityEvent OnStarting;
    public UnityEvent OnOngoing;
    public UnityEvent OnEnding;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_UpdateGameState(GameState state)
    {
        if (GameState == state) return;

        switch (state)
        {
            case GameState.Intermission:
                {
                    delay = startDelay;
                    break;
                }
            case GameState.Starting:
                {
                    GameTime = startTime * 60;
                    break;
                }
            case GameState.Ongoing:
                {
                    delay = startDelay;
                    break;
                }
            case GameState.Ending:
                {
                    break;
                }
        }

        GameState = state;
        Debug.Log("[GameManager] successfully updated game state to: " + state);
    }

    #endregion
}
