using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[System.Serializable]
public class WaveDetails
{
    public GridBuilder nextGrid;
    public EnemyPortal[] newPortals;
    public int basicEnemy;
    public int fastEnemy;
}

public class WaveManager : MonoBehaviour
{
    private UI_InGame inGameUI;

    [SerializeField] private GridBuilder currentGrid;

    [Header ("Wave Details")]
    [SerializeField] private float timeBeetwenWaves = 10;
    [SerializeField] private float waveTimer;
    [SerializeField] private WaveDetails[] levelWaves;
    [SerializeField] private int waveIndex;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;

    private List<EnemyPortal> enemyPortals;
    private bool waveTimerEnabled;
    private bool makingNextWave;
    public bool gameBegun;

    [System.Obsolete]
    private void Awake()
    {
        enemyPortals = new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None));
        inGameUI = FindAnyObjectByType<UI_InGame>();
    }

    private void Update()
    {
        if (gameBegun == false) return;
        HandleWaveTimer();
    }

    [ContextMenu("Activate Wave Manager")]
    public void ActivateWaveManager()
    {
        gameBegun = true;
        EnableWaveTimer(true);
    }

    public WaveDetails[] GetLevelWaves() => levelWaves;

    public void CheckIfWaveCompleted()
    {
        if (AllEnemiesDefeated() == false || makingNextWave) return;

        makingNextWave = true;

        CheckForNewLevelLayout();
        EnableWaveTimer(true);
    }

    private void HandleWaveTimer()
    {
        if (waveTimerEnabled == false) return;

        waveTimer -= Time.deltaTime;
        inGameUI.UpdateWaveTimerUI(waveTimer);

        if (waveTimer <= 0)
            StartNewWave();
    }

    public void StartNewWave()
    {
        waveIndex++;
        GiveEnemiesToPortals();
        EnableWaveTimer(false);
        makingNextWave = false;
    }

    private void GiveEnemiesToPortals()
    {

        List<GameObject> newEnemies = GetNewEnemies();
        int portalIndex = 0;

        if (newEnemies == null) return;

        for (int i = 0; i < newEnemies.Count; i++)
        {
            GameObject enemyToAdd = newEnemies[i];
            EnemyPortal portalToReciveEnemy = enemyPortals[portalIndex];

            portalToReciveEnemy.AddEnemy(enemyToAdd);

            portalIndex++;

            if (portalIndex >= enemyPortals.Count)
            {
                portalIndex = 0;
            }
        }
    }

    private void CheckForNewLevelLayout()
    {
        if (waveIndex >= levelWaves.Length) return;

        WaveDetails nextWave = levelWaves[waveIndex];

        if (nextWave.nextGrid != null)
        {
            UpdateLevelTiles(nextWave.nextGrid);
            EnableNewPortals(nextWave.newPortals);
        }

        currentGrid.UpdateNavMesh();
    }

    private void UpdateLevelTiles(GridBuilder nextGrid)
    {
        List<GameObject> grid = currentGrid.GetTileSetup();
        List<GameObject> newGrid = nextGrid.GetTileSetup();

        for (int i = 0;i < grid.Count;i++)
        {
            TileSlot currentTile = grid[i].GetComponent<TileSlot>();
            TileSlot newTile = newGrid[i].GetComponent<TileSlot>();

            bool shouldBeUpdated = currentTile.GetMesh() != newTile.GetMesh() ||
                                   currentTile.GetMaterial() != newTile.GetMaterial() ||
                                   currentTile.GetAllChilden().Count != newTile.GetAllChilden().Count ||
                                   currentTile.transform.rotation != newTile.transform.rotation;

            if (shouldBeUpdated)
            {
                currentTile.gameObject.SetActive(false);

                newTile.gameObject.SetActive(true);
                newTile.transform.parent = currentGrid.transform;

                grid[i] = newTile.gameObject;
                Destroy(currentTile.gameObject);
            }
        }
    }

    private void EnableWaveTimer(bool enable)
    {
        if (waveTimerEnabled == enable) return;

        waveTimer = timeBeetwenWaves;
        waveTimerEnabled = enable;
        inGameUI.EnableWaveTimer(enable);
    }

    private void EnableNewPortals(EnemyPortal[] newPortals)
    {
        foreach (EnemyPortal portal in newPortals)
        {
            portal.AssingWaveManager(this);
            portal.gameObject.SetActive(true);
            enemyPortals.Add(portal);
        }
    }

    private List<GameObject> GetNewEnemies()
    {
        if (waveIndex >= levelWaves.Length)
        {
            return null;
        }

        List<GameObject> newEnemyList = new List<GameObject>();

        for (int i = 0; i < levelWaves[waveIndex].basicEnemy; i++)
        {
            newEnemyList.Add(basicEnemy);
        }

        for (int i = 0; i < levelWaves[waveIndex].fastEnemy; i++)
        {
            newEnemyList.Add(fastEnemy);
        }

        return newEnemyList;
    }

    private bool AllEnemiesDefeated()
    {
        foreach (EnemyPortal portal in enemyPortals)
        {
            if (portal.GetActiveEnemies().Count > 0) return false;
        }
        return true;
    }
}
