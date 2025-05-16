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
    Offline,
    Intermission,
    Starting,
    Ongoing,
    Ending
}

public class S_GameManager : NetworkBehaviour
{
    public static S_GameManager instance;

    public static GameState currentGameState => instance.gameState;
    [Networked, SerializeField] public GameState gameState { get; private set; }

    [Space]
    public int startTime = 7;
    [Networked, SerializeField] public float GameTime { get; private set; }

    [Space]
    public int startDelay = 5;
    [Networked] float delay { get; set; } 

    [Networked, SerializeField] public NetworkLinkedList<S_Food> currentFood { get; private set; } = new NetworkLinkedList<S_Food>();
    public const int maxFood = 10;
    
    public static event Action OnFoodListFull;

    bool isLocal => Object && Object.HasStateAuthority;

    private void Start()
    {
        instance = this;
        gameState = GameState.Offline;
    }

    public override void Spawned()
    {
        base.Spawned();

        if (!isLocal) return;

        gameState = GameState.Intermission;

        GameTime = startTime * 60;
        delay = startDelay;

        ProgressGameState();
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (!isLocal) return;

        switch (gameState)
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
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public static S_Food RPC_SpawnFood(S_Food food, Vector3 position, Quaternion rotation)
    {
        if (currentGameState == GameState.Offline)
        {
            Debug.LogWarning("[GameManager] Do not spawn food while Offline.");
            return null;
        }
        if (currentGameState != GameState.Ongoing)
        {
            Debug.LogWarning("[GameManager] Do not spawn food while Game isn't running.");
            return null;
        }

        if (instance.currentFood.Count >= maxFood)
        {
            if (instance != null && instance.isLocal)
            {
                instance.RPC_FullFoodSpawn();
            }
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
        if(currentGameState == GameState.Offline)
        {
            Debug.LogWarning("[GameManager] do not start the game while offline.");
            return;
        }

        GameState newGameState = (GameState)((int)currentGameState + 1);

        if ((int)newGameState > (int)GameState.Ending)
            newGameState = GameState.Intermission;

        Debug.Log("[GameManager] updating game state to: " + newGameState);
        instance.RPC_UpdateGameState(newGameState);
    }

    [HorizontalLine]
    [InfoBox("The following is Master Client only: ")]
    [Space]
    public UnityEvent OnIntermission;
    public UnityEvent OnStarting;
    public UnityEvent OnOngoing;
    public UnityEvent OnEnding;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_UpdateGameState(GameState state)
    {
        if (gameState == state) return;

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

        gameState = state;
        Debug.Log("[GameManager] successfully updated game state to: " + state);
    }

    #endregion
}
