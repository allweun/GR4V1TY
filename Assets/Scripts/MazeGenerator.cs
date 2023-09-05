/* ----------------------------
 * 2D Maze Generator for Unity.
 * Uses Deapth-First searching 
 * and Recursive Backtracking.
 * ----------------------------
 * Generates a 2x2 centre room in 
 * the middle of the Maze.
 * ----------------------------
 * Author: c00pala
 * ~13/05/2018~
 * ---------------------------- */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour {

    #region Variables:
    // ------------------------------------------------------
    // User defined variables - set in editor:
    // ------------------------------------------------------
    [Header("Maze generation values:")]
    [Tooltip("How many cells tall is the maze. MUST be an even number. " +
        "If number is odd, it will be reduced by 1.\n\n" +
        "Minimum value of 4.")]
    public int mazeRows;
    [Tooltip("How many cells wide is the maze. Must be an even number. " +
        "If number is odd, it will be reduced by 1.\n\n" +
        "Minimum value of 4.")]
    public int mazeColumns;

    [Header("Maze object variables:")]
    [Tooltip("Cell prefab object.")]
    [SerializeField]
    private GameObject cellPrefab;

    [Tooltip("If you want to disable the main sprite so the cell has no background, set to TRUE. This will create a maze with only walls.")]
    public bool disableCellSprite;

    // ------------------------------------------------------
    // System defined variables - You don't need to touch these:
    // ------------------------------------------------------

    // Variable to store size of centre room. Hard coded to be 2.
    private int centreSize = 2;

    // Dictionary to hold and locate all cells in maze.
    private Dictionary<Vector2, Cell> allCells = new Dictionary<Vector2, Cell>();
    // List to hold unvisited cells.
    private List<Cell> unvisited = new List<Cell>();
    // List to store 'stack' cells, cells being checked during generation.
    private List<Cell> stack = new List<Cell>();

    // Array will hold 4 centre room cells, from 0 -> 3 these are:
    // Top left (0), top right (1), bottom left (2), bottom right (3).
    private Cell[] centreCells = new Cell[4];

    // Cell variables to hold current and checking Cells.
    private Cell currentCell;
    private Cell checkCell;

    // Array of all possible neighbour positions.
    private Vector2[] neighbourPositions = new Vector2[] { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, -1) };

    // Size of the cells, used to determine how far apart to place cells during generation.
    private float cellSize;

    private GameObject mazeParent;
    #endregion


    public Transform player;
    public Transform endTrigger;
    public bool generateNew = false;
    public Transform topLeft;
    public Transform bottomRight;
    public GameObject coinPrefab;
    public GameObject[] cellPrefabs;
    public Transform catcher;
    public GameObject mazeTiles;
    public GameObject coinTiles;
    public Slider coinSlider;
    public bool nextLevel;

    public void Update(){
        if(generateNew){
            generateNew = false;
            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
            //foreach(GameObject coin in coins){
            //    Destroy(coin);
            //}
            for(int i = 0; i < coins.Length; i++){
                Destroy(coins[i]);
            }
            GenerateMaze(mazeRows, mazeColumns);
        }
        if(nextLevel){
            PlayerPrefs.SetInt("LevelIndex",PlayerPrefs.GetInt("LevelIndex")+1);
            NextLevel();
            nextLevel=false;
        }
    }

    /* This Start run is an example, you can delete this when 
     * you want to start calling the maze generator manually. 
     * To generate a maze is really easy, just call the GenerateMaze() function
     * pass a rows value and columns value as parameters and the generator will
     * do the rest for you. Enjoy!
     */
    private void Start()
    {
        mazeRows = ((2 * PlayerPrefs.GetInt("LevelIndex")) / 30) + 5;
        mazeColumns  = mazeRows;
        PlayerPrefs.SetFloat("magnify", mazeRows);
        GenerateMaze(mazeRows, mazeColumns);
    }
    int levleIndex(int rows){
        if(PlayerPrefs.GetInt("LevelIndex")%20==0){
                if(PlayerPrefs.GetInt("LevelIndex")==0)
                    rows=4;
                else
                    rows=rows+2;
            }else{
                rows = mazeRows;
            }
            return rows;
    }
    public void NextLevel(){
        // eski değer   mazeRows = ((2 * PlayerPrefs.GetInt("LevelIndex")) / 30) + 8;
        //mazeRows = ((2 * PlayerPrefs.GetInt("LevelIndex")) / 30) + 5;
        mazeRows = levleIndex(mazeRows);
        mazeColumns  = mazeRows;
        PlayerPrefs.SetFloat("magnify", mazeRows);
        GenerateMaze(mazeRows, mazeColumns);
    }

    private void GenerateMaze(int rows, int columns)
    {
        if (mazeParent != null) DeleteMaze();

        mazeRows = rows;
        mazeColumns = columns;
        CreateLayout();
    }

    // Creates the grid of cells.
    public void CreateLayout()
    {
        cellPrefab = cellPrefabs[PlayerPrefs.GetInt("LevelIndex")%7];
        InitValues();

        // Set starting point, set spawn point to start.
        Vector2 startPos = new Vector2(-(cellSize * (mazeColumns / 2)) + (cellSize / 2), -(cellSize * (mazeRows / 2)) + (cellSize / 2));
        Vector2 spawnPos = startPos;

        for (int x = 1; x <= mazeColumns; x++)
        {
            for (int y = 1; y <= mazeRows; y++)
            {
                GenerateCell(spawnPos, new Vector2(x, y));

                // Increase spawnPos y.
                spawnPos.y += cellSize;
            }

            // Reset spawnPos y and increase spawnPos x.
            spawnPos.y = startPos.y;
            spawnPos.x += cellSize;
        }

        CreateCentre();
        RunAlgorithm();
        MakeExit();

        topLeft.transform.position = new Vector2((-(mazeColumns*cellSize))/2 + (cellSize/2),((mazeRows*cellSize))/2 + (cellSize/2) - cellSize);
        bottomRight.transform.position = new Vector2(((mazeColumns*cellSize))/2 - (cellSize/2), (-(mazeRows*cellSize))/2 + (cellSize/2));
        for(float i = topLeft.transform.position.x; i <= bottomRight.transform.position.x; i += cellSize){
            for(float j = topLeft.transform.position.y; j >= bottomRight.transform.position.y; j -= cellSize){
                Instantiate(coinPrefab, new Vector2(i, j), Quaternion.identity);
            }
        }
        GameObject[] tempCoinArray = GameObject.FindGameObjectsWithTag("Coin");
        coinSlider.maxValue = tempCoinArray.Length;
        //foreach(GameObject tempCoin in tempCoinArray){
        //    tempCoin.transform.parent = coinTiles.transform;
        //}
        for(int i = 0; i < tempCoinArray.Length; i++){
            tempCoinArray[i].transform.parent = coinTiles.transform;
        }
    }

    // This is where the fun stuff happens.
    public void RunAlgorithm()
    {
        // Get start cell, make it visited (i.e. remove from unvisited list).
        unvisited.Remove(currentCell);

        // While we have unvisited cells.
        while (unvisited.Count > 0)
        {
            List<Cell> unvisitedNeighbours = GetUnvisitedNeighbours(currentCell);
            if (unvisitedNeighbours.Count > 0)
            {
                // Get a random unvisited neighbour.
                checkCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                // Add current cell to stack.
                stack.Add(currentCell);
                // Compare and remove walls.
                CompareWalls(currentCell, checkCell);
                // Make currentCell the neighbour cell.
                currentCell = checkCell;
                // Mark new current cell as visited.
                unvisited.Remove(currentCell);
            }
            else if (stack.Count > 0)
            {
                // Make current cell the most recently added Cell from the stack.
                currentCell = stack[stack.Count - 1];
                // Remove it from stack.
                stack.Remove(currentCell);
            }
        }
    }

    public void MakeExit()
    {
        // Create and populate list of all possible edge cells.
        List<Cell> edgeCells = new List<Cell>();

        foreach (KeyValuePair<Vector2, Cell> cell in allCells)
        {
            if (cell.Key.x == 0 || cell.Key.x == mazeColumns || cell.Key.y == 0 || cell.Key.y == mazeRows)
            {
                edgeCells.Add(cell.Value);
            }
        }

        // Get edge cell randomly from list.
        Cell newCell = edgeCells[Random.Range(0, edgeCells.Count)];

        // Remove appropriate wall for chosen edge cell.
        //Sol
        if (newCell.gridPos.x == 0) {
            RemoveWall(newCell.cScript, 1);
            endTrigger.transform.position = new Vector2((newCell.gridPos.x - cellSize) - (mazeRows/2 * cellSize) - (cellSize/2) - (cellSize/4), newCell.gridPos.y - (mazeColumns/2 * cellSize) - (cellSize/2)+0.35f);
            endTrigger.transform.localScale = new Vector3(1, 2, 1);
            catcher.transform.position = new Vector2(endTrigger.transform.position.x - 2, endTrigger.transform.position.y);
            catcher.transform.rotation = Quaternion.Euler(0 ,0 ,180);
        }
        //Sag
        else if (newCell.gridPos.x == mazeColumns) {
            RemoveWall(newCell.cScript, 2);
            endTrigger.transform.position = new Vector2((newCell.gridPos.x + cellSize) - (mazeRows/2 * cellSize) - (cellSize/2) + (cellSize/4)+0.35f, newCell.gridPos.y - (mazeColumns/2 * cellSize) - (cellSize/2));
            endTrigger.transform.localScale = new Vector3(1, 2, 1);
            catcher.transform.position = new Vector2(endTrigger.transform.position.x + 2, endTrigger.transform.position.y);
            catcher.transform.rotation = Quaternion.Euler(0 ,0 ,0);
        }
        //Ust
        else if (newCell.gridPos.y == mazeRows) {
            RemoveWall(newCell.cScript, 3);
            endTrigger.transform.position = new Vector2(newCell.gridPos.x - (mazeRows/2 * cellSize) - (cellSize/2), (newCell.gridPos.y + cellSize) - (mazeColumns/2 * cellSize) - (cellSize/2) + (cellSize/4)+0.35f);
            endTrigger.transform.localScale = new Vector3(2, 1, 1);
            catcher.transform.position = new Vector2(endTrigger.transform.position.x, endTrigger.transform.position.y + 2);
            catcher.transform.rotation = Quaternion.Euler(0 ,0 ,90);
        }
        //Alt
        else {
            RemoveWall(newCell.cScript, 4);
            endTrigger.transform.position = new Vector2(newCell.gridPos.x - (mazeRows/2 * cellSize) - (cellSize/2), (newCell.gridPos.y - cellSize) - (mazeColumns/2 * cellSize) - (cellSize/2) - (cellSize/4)+1);
            endTrigger.transform.localScale = new Vector3(2, 1, 1);
            catcher.transform.position = new Vector2(endTrigger.transform.position.x, endTrigger.transform.position.y - 2);
            catcher.transform.rotation = Quaternion.Euler(0 ,0 ,270);
        }

        player.position = new Vector2(0, 0);

        //Debug.Log("Maze generation finished.");
    }

    public List<Cell> GetUnvisitedNeighbours(Cell curCell)
    {
        // Create a list to return.
        List<Cell> neighbours = new List<Cell>();
        // Create a Cell object.
        Cell nCell = curCell;
        // Store current cell grid pos.
        Vector2 cPos = curCell.gridPos;

        foreach (Vector2 p in neighbourPositions)
        {
            // Find position of neighbour on grid, relative to current.
            Vector2 nPos = cPos + p;
            // If cell exists.
            if (allCells.ContainsKey(nPos)) nCell = allCells[nPos];
            // If cell is unvisited.
            if (unvisited.Contains(nCell)) neighbours.Add(nCell);
        }

        //for(int i = 0; i < neighbourPositions.Length; i++){
        //    Vector2 nPos = cPos + neighbourPositions[i];
        //    if (allCells.ContainsKey(nPos)) nCell = allCells[nPos];
        //    if (unvisited.Contains(nCell)) neighbours.Add(nCell);
        //}

        return neighbours;
    }

    // Compare neighbour with current and remove appropriate walls.
    public void CompareWalls(Cell cCell, Cell nCell)
    {
        // If neighbour is left of current.
        if (nCell.gridPos.x < cCell.gridPos.x)
        {
            RemoveWall(nCell.cScript, 2);
            RemoveWall(cCell.cScript, 1);
        }
        // Else if neighbour is right of current.
        else if (nCell.gridPos.x > cCell.gridPos.x)
        {
            RemoveWall(nCell.cScript, 1);
            RemoveWall(cCell.cScript, 2);
        }
        // Else if neighbour is above current.
        else if (nCell.gridPos.y > cCell.gridPos.y)
        {
            RemoveWall(nCell.cScript, 4);
            RemoveWall(cCell.cScript, 3);
        }
        // Else if neighbour is below current.
        else if (nCell.gridPos.y < cCell.gridPos.y)
        {
            RemoveWall(nCell.cScript, 3);
            RemoveWall(cCell.cScript, 4);
        }
    }

    // Function disables wall of your choosing, pass it the script attached to the desired cell
    // and an 'ID', where the ID = the wall. 1 = left, 2 = right, 3 = up, 4 = down.
    public void RemoveWall(CellScript cScript, int wallID)
    {
        if (wallID == 1) {
            cScript.wallL.SetActive(false);
            Destroy(cScript.wallL);
        }else if (wallID == 2) {
            cScript.wallR.SetActive(false);
            Destroy(cScript.wallR);
        }else if (wallID == 3) {
            cScript.wallU.SetActive(false);
            Destroy(cScript.wallU);
        }else if (wallID == 4) {
            cScript.wallD.SetActive(false);
            Destroy(cScript.wallD);
        }
    }

    public void CreateCentre()
    {
        // Get the 4 centre cells using the rows and columns variables.
        // Remove the required walls for each.
        centreCells[0] = allCells[new Vector2((mazeColumns / 2), (mazeRows / 2) + 1)];
        RemoveWall(centreCells[0].cScript, 4);
        RemoveWall(centreCells[0].cScript, 2);
        centreCells[1] = allCells[new Vector2((mazeColumns / 2) + 1, (mazeRows / 2) + 1)];
        RemoveWall(centreCells[1].cScript, 4);
        RemoveWall(centreCells[1].cScript, 1);
        centreCells[2] = allCells[new Vector2((mazeColumns / 2), (mazeRows / 2))];
        RemoveWall(centreCells[2].cScript, 3);
        RemoveWall(centreCells[2].cScript, 2);
        centreCells[3] = allCells[new Vector2((mazeColumns / 2) + 1, (mazeRows / 2))];
        RemoveWall(centreCells[3].cScript, 3);
        RemoveWall(centreCells[3].cScript, 1);

        // Create a List of ints, using this, select one at random and remove it.
        // We then use the remaining 3 ints to remove 3 of the centre cells from the 'unvisited' list.
        // This ensures that one of the centre cells will connect to the maze but the other three won't.
        // This way, the centre room will only have 1 entry / exit point.
        List<int> rndList = new List<int> { 0, 1, 2, 3 };
        int startCell = rndList[Random.Range(0, rndList.Count)];
        rndList.Remove(startCell);
        currentCell = centreCells[startCell];
        foreach(int c in rndList)
        {
            unvisited.Remove(centreCells[c]);
        }

        //for(int i = 0; i < rndList.Count; i++){
        //    unvisited.Remove(centreCells[i]);
        //}
    }

    public void GenerateCell(Vector2 pos, Vector2 keyPos)
    {
        // Create new Cell object.
        Cell newCell = new Cell();

        // Store reference to position in grid.
        newCell.gridPos = keyPos;
        // Set and instantiate cell GameObject.
        newCell.cellObject = Instantiate(cellPrefab, pos, cellPrefab.transform.rotation);
        // Child new cell to parent.
        if (mazeParent != null) newCell.cellObject.transform.parent = mazeParent.transform;
        // Set name of cellObject.
        newCell.cellObject.name = "Cell - X:" + keyPos.x + " Y:" + keyPos.y;
        // Get reference to attached CellScript.
        newCell.cScript = newCell.cellObject.GetComponent<CellScript>();
        // Disable Cell sprite, if applicable.
        if (disableCellSprite) {
            newCell.cellObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(newCell.cellObject.GetComponent<SpriteRenderer>());
        }

        // Add to Lists.
        allCells[keyPos] = newCell;
        unvisited.Add(newCell);
        mazeParent.transform.parent = mazeTiles.transform;
    }

    public void DeleteMaze()
    {
        if (mazeParent != null) Destroy(mazeParent);
        Resources.UnloadUnusedAssets();
    }

    public void InitValues()
    {
        // Check generation values to prevent generation failing.
        if (IsOdd(mazeRows)) mazeRows--;
        if (IsOdd(mazeColumns)) mazeColumns--;

        if (mazeRows <= 3) mazeRows = 4;
        if (mazeColumns <= 3) mazeColumns = 4;

        // Determine size of cell using localScale.
        cellSize = cellPrefab.transform.localScale.x;

        // Create an empty parent object to hold the maze in the scene.
        mazeParent = new GameObject();
        mazeParent.transform.position = Vector2.zero;
        mazeParent.name = "Maze";
    }

    public bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

    public class Cell
    {
        public Vector2 gridPos;
        public GameObject cellObject;
        public CellScript cScript;
    }
}