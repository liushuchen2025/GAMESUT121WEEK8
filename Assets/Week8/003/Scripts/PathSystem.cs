using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSystem : MonoBehaviour {

    public enum SeedType { RANDOM, CUSTOM }
    [Header("Random Related Stuff")]
    public SeedType seedType = SeedType.RANDOM;
    System.Random random;
    public int seed = 0;

    public GameObject blockPrefab;
    public List<GameObject> blockList = new List<GameObject>();
    public GameObject lootToSpawn;
    public List<GameObject> lootList = new List<GameObject>();

    [Space]
    public bool animatedPath;
    public List<MyGridCell> gridCellList = new List<MyGridCell>();
    public int pathLength = 10;
    [Range(1.0f, 10.0f)]
    public float cellSize = 1.0f;

    public Transform startLocation;

    // Start is called before the first frame update
    void Start() {

    }

    void SetSeed() {
        if (seedType == SeedType.RANDOM) {
            random = new System.Random();
        }
        else if (seedType == SeedType.CUSTOM) {
            random = new System.Random(seed);
        }
    }

    void CreatePath() {

        gridCellList.Clear();
        Vector2 currentPosition = startLocation.transform.position;
        gridCellList.Add(new MyGridCell(currentPosition));

        for (int i = 0; i < pathLength; i++) {

            int n = random.Next(100);

            if (n.IsBetween(0, 49)) {
                currentPosition = new Vector2(currentPosition.x + cellSize, currentPosition.y);
            }
            else {
                currentPosition = new Vector2(currentPosition.x, currentPosition.y + cellSize);
            }

            gridCellList.Add(new MyGridCell(currentPosition));

        }
    }

    IEnumerator CreatePathRoutine() {

        gridCellList.Clear();
        for (int i = 0; i<blockList.Count; i++)
        {
           Destroy(blockList[i]);
        }
        for (int i = 0; i < lootList.Count; i++)
        {
            Destroy(lootList[i]);
        }
        Vector2 currentPosition = startLocation.transform.position;
        gridCellList.Add(new MyGridCell(currentPosition));
        GameObject b = Instantiate(blockPrefab, currentPosition, Quaternion.identity);
        blockList.Add(b);

        for (int i = 0; i < pathLength; i++) {

            int n = random.Next(100);
            int l = random.Next(100);

            if (n.IsBetween(0, 49)) {
                currentPosition = new Vector2(currentPosition.x + cellSize, currentPosition.y);
                b = Instantiate(blockPrefab, currentPosition, Quaternion.identity);
                blockList.Add(b);
            }
            else {
                currentPosition = new Vector2(currentPosition.x, currentPosition.y + cellSize);
                b = Instantiate(blockPrefab, currentPosition, Quaternion.identity);
                blockList.Add(b);
            }

            if (l.IsBetween(0, 8))
            {
                GameObject loot = Instantiate(lootToSpawn, currentPosition, Quaternion.identity);
                lootList.Add(loot);
            }
            
            gridCellList.Add(new MyGridCell(currentPosition));
            yield return null;
        }
    }



    private void OnDrawGizmos() {
        for (int i = 0; i < gridCellList.Count; i++) {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(gridCellList[i].location, Vector3.one * cellSize);
            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            Gizmos.DrawCube(gridCellList[i].location, Vector3.one * cellSize);
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SetSeed();

            if (animatedPath)
                StartCoroutine(CreatePathRoutine());
            else
                CreatePath();
        }
    }
}
