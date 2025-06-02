using Fusion;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

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

    public static GameState CurrentGameState
    {
        get { return !isConnected ? GameState.Offline : instance.gameState; }
    }
    [Networked] GameState gameState { get; set; }

    [Space]

    [Min(1)] public int playersRequired = 1;
    public static bool ready => instance.Ready;
    [Networked] bool Ready { get; set; }
    [SerializeField] bool waitForPlayers = true;

    [Space]

    public int startDelay = 5;

    public int startTime = 7;
    public static float currentGameTime => instance.GameTime;
    [Networked] public float GameTime { get; private set; }

    [Networked, Capacity(maxFood)] public NetworkLinkedList<S_Food> currentFood => default;
    public const int maxFood = 10;

    public static event Action OnFoodListFull;

    bool isLocal => Object && Object.HasStateAuthority;
    public static bool isConnected = false;
    public static SessionInfo sessionInfo => instance.Runner.SessionInfo;

    private void Start()
    {
        instance = this;
    }
    public override void Spawned()
    {
        base.Spawned();

        isConnected = true;

        if (!isLocal) return;

        gameState = GameState.Intermission;

        GameTime = startTime * 60;
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
        isConnected = false;
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        //string message = "";

        //foreach (var item in currentFood)
        //{
        //    message += item.GetFoodType() + "\n";
        //}

        //Debug.Log("[GameManager] Food: " + message);

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
    public static S_Food TrySpawnFood(S_Food food, Vector3 position, Quaternion rotation)
    {
        return instance.SpawnFood(food, position, rotation);
    }
    /// <summary>
    /// used to destroy food
    /// </summary>
    public static void TryDespawnFood(S_Food food, float t = 0)
    {
        instance.DespawnFood(food, t);
    }

    S_Food SpawnFood(S_Food food, Vector3 position, Quaternion rotation)
    {
        S_Food newFood = null;

        if (CurrentGameState == GameState.Offline)
        {
            Debug.LogWarning("[GameManager] Do not spawn food while Offline.");
            return null;
        }
        if (CurrentGameState != GameState.Ongoing)
        {
            Debug.LogWarning("[GameManager] Do not spawn food while Game isn't running.");
            return null;
        }

        if (currentFood.Count >= maxFood)
        {
            /*
            if (instance != null && instance.HasStateAuthority)
            {
                instance.RPC_FullFoodSpawn();
            }*/
            // Maybe we shouldn't check who does anything because it doesn't matter
            // yes, good job
            instance.RPC_FullFoodSpawn();

            return null;
        }

        newFood = Runner.Spawn(food, position, rotation);

        if(newFood != null)
            currentFood.Add(newFood);

        return newFood;
    }

    async void DespawnFood(S_Food food, float t = 0)
    {
        if(t > 0)
            await Task.Delay(Mathf.RoundToInt(t * 1000));

        if (food == null) return;

        currentFood.Remove(food);
        Runner.Despawn(food.Object);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_FullFoodSpawn()
    {
        Debug.Log("[GameManager] Food list is full and needs to trash some food");
        OnFoodListFull?.Invoke();
    }
    void CleauUp()
    {
        Debug.Log("[GameManager] CleanUp");

        for (int i = currentFood.Count - 1; i >= 0; i--)
        {
            TryDespawnFood(currentFood[i]);
        }
    }
    #endregion

    #region Game States
    void Intermission()
    {
        if(!waitForPlayers || sessionInfo.PlayerCount >= playersRequired)
        {
            Ready = true;
        }
    }

    void Starting()
    {
    }

    void Ongoing()
    {
        GameTime -= Time.fixedDeltaTime;

        if (GameTime <= 0)
        {
            ProgressGameState();
        }
    }

    void Ending()
    {
    }

    public static bool StartGame()
    {
        if (CurrentGameState != GameState.Intermission && ready) return false;

        ProgressGameState();

        return true;
    }

    static async void ProgressGameState(float t = 0)
    {
        if (!isConnected)
        {
            Debug.LogError("[GameManager] do not progress the game state while offline!");
            return;
        }

        if(t > 0)
            await Task.Delay(Mathf.RoundToInt(t * 1000));

        GameState newGameState = (GameState)((int)CurrentGameState + 1);

        if ((int)newGameState > (int)GameState.Ending)
            newGameState = GameState.Intermission;

        Debug.Log("[GameManager] updating game state to: " + newGameState);
        instance.RPC_UpdateGameState(newGameState);
        return;
    }

    [HorizontalLine]
    [InfoBox("The following is Master Client only: ")]
    [Space]
    public UnityEvent<S_GameManager> OnIntermission;
    public UnityEvent<S_GameManager> OnStarting;
    public UnityEvent<S_GameManager> OnOngoing;
    public UnityEvent<S_GameManager> OnEnding;

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_UpdateGameState(GameState state)
    {
        if (gameState == state) return;

        switch (state)
        {
            case GameState.Intermission:
                {
                    OnIntermission?.Invoke(this);
                    break;
                }
            case GameState.Starting:
                {
                    GameTime = startTime * 60;
                    ProgressGameState(startDelay);
                    OnStarting?.Invoke(this);
                    break;
                }
            case GameState.Ongoing:
                {
                    OnOngoing?.Invoke(this);
                    break;
                }
            case GameState.Ending:
                {
                    ProgressGameState(startDelay);

                    CleauUp();

                    OnEnding?.Invoke(this);
                    break;
                }
        }

        gameState = state;
        Debug.Log("[GameManager] successfully updated game state to: " + state);
    }

    #endregion
}
