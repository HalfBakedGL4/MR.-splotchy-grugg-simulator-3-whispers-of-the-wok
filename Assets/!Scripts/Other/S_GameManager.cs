using Fusion;
using NaughtyAttributes;
using System;
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
    [Networked, SerializeField] GameState gameState { get; set; }
    [SerializeField, Min(1)] int playersRequired = 1;
    [SerializeField] bool waitForPlayers = true;

    [Space]
    public int startTime = 7;
    [Networked, SerializeField] public float GameTime { get; private set; }

    [Space]
    public int startDelay = 5;
    [Networked] float delay { get; set; }

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
        delay = startDelay;
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

        foreach (var item in currentFood)
        {
            Debug.Log("[GameManager] Food: " + item.GetFoodType());
        }
    }

    #region Food
    /// <summary>
    /// used to spawn food
    /// </summary>
    public static S_Food TrySpawnFood(S_Food food, Vector3 position, Quaternion rotation)
    {
        instance.SpawnFood(food, position, rotation);
        return null;
    }
    /// <summary>
    /// used to destroy food
    /// </summary>
    public static void TryDespawnFood(S_Food food)
    {
        instance.DespawnFood(food);
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

        newFood = Runner.Spawn(food.GetComponent<NetworkObject>(), position, rotation).GetComponent<S_Food>();
        currentFood.Add(newFood);

        return newFood;
    }

    void DespawnFood(S_Food food)
    {
        S_Food toDestroy = food.GetComponent<S_Food>();
        if (toDestroy == null) return;

        currentFood.Remove(toDestroy);
        Runner.Despawn(toDestroy.GetComponent<NetworkObject>());
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
        if(!waitForPlayers)
        {
            ProgressGameState();
            return;
        }

        if(sessionInfo.PlayerCount > playersRequired)
        {
            ProgressGameState();
        }
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

        if (GameTime <= 0)
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
        if (decimals > 0)
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
        if (!isConnected)
        {
            Debug.LogWarning("[GameManager] do not start the game while offline.");
            return;
        }

        GameState newGameState = (GameState)((int)CurrentGameState + 1);

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
