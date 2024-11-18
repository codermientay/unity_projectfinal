using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid_HomeMade : MonoBehaviour
{
    // Tham chiếu đến Grid và Tilemap
    public Grid grid;
    public Tilemap[] tilemap;
    public float x_left, x_right, y_up, y_down;
    public List<Vector3> tilePositions = new List<Vector3>();
    public Vector3 cellSize;
    public Grid_HomeMade()
    {

    }

    public Grid_HomeMade(Grid grid, Tilemap[] tilemap, float x_left, float x_right, float y_up, float y_down)
    {
        this.grid = grid;
        this.tilemap = tilemap;
        this.x_left = x_left;
        this.x_right = x_right;
        this.y_up = y_up;
        this.y_down = y_down;
        this.cellSize = grid.cellSize;
    }

    public int[,] gridMatrixForAMap(Tilemap tilemap)
    {
        // Tính số hàng và cột dựa trên tọa độ và kích thước ô
        int numRows = Mathf.CeilToInt((y_up - y_down) / cellSize.y);
        int numCols = Mathf.CeilToInt((x_right - x_left) / cellSize.x);

        // Khởi tạo mảng 2 chiều
        int[,] matrix = new int[numRows, numCols];

        // Lấp đầy mảng với chỉ số hoặc giá trị tương ứng
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                // Chuyển đổi tọa độ ô sang tọa độ thế giới
                Vector3 worldPosition = new Vector3(x_left + col * cellSize.x, y_down + row * cellSize.y, 0);
                Vector3Int cellPosition = grid.WorldToCell(worldPosition);
                matrix[row, col] = CheckForTile(tilemap, cellPosition);
            }

        }


        return matrix;
    }
    public int[,] gridMatrixForTile(Tilemap[] tilemap)
    {


        int numRows = Mathf.CeilToInt((y_up - y_down) / cellSize.y);
        int numCols = Mathf.CeilToInt((x_right - x_left) / cellSize.x);

        // Khởi tạo mảng 2 chiều
        int[,] matrix = new int[numRows, numCols];
        int[,] matrix_total = new int[numRows, numCols];
        int[,] matrix_total_temp = new int[numRows, numCols];
        // Lấp đầy mảng với chỉ số hoặc giá trị tương ứng
        for (int i = 0; i < tilemap.Length; i++)
        {
            matrix_total_temp = gridMatrixForAMap(tilemap[i]);
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    Vector3 worldPosition = new Vector3(x_left + col * cellSize.x, y_down + row * cellSize.y, 0);
                    Vector3Int cellPosition = grid.WorldToCell(worldPosition);
                    matrix_total[row, col] = (matrix_total[row, col] + matrix_total_temp[row, col] >= 1) ? 1 : 0;
                    if (matrix_total[row, col] == 1)
                    {
                        tilePositions.Add(worldPosition);
                    }
                }
            }

        }

        return matrix_total;
    }

    public Vector3[,] changeTocoordinates(int[,] matrix)
    {
        int numRows = matrix.GetLength(0);
        int numCols = matrix.GetLength(1);
        Vector3[,] worldCoordinates = new Vector3[numRows, numCols];

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {

                // Convert grid coordinates to world coordinates
                float worldX = x_left + col * cellSize.x;
                float worldY = y_down + row * cellSize.y;
                worldCoordinates[row, col] = new Vector3(worldX, worldY, 0);

            }
        }

        return worldCoordinates;
    }
    public String toString(Vector3[,] arr)
    {
        String s = String.Empty;
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                s += "(" + arr[i, j].x + ")," + "(" + arr[i, j].y + ")," + "(" + arr[i, j].z + ")" + "\n";
            }
        }
        return s;
    }

    // Hàm kiểm tra xem ô tại vị trí cell có tile hay không
    public int CheckForTile(Tilemap tilemap, Vector3Int cellPosition)
    {
        TileBase tile = tilemap.GetTile(cellPosition);

        if (tile != null)
        {
            // Kiểm tra xem Tilemap có CompositeCollider2D không
            CompositeCollider2D compositeCollider = tilemap.GetComponent<CompositeCollider2D>();
            if (compositeCollider != null)
            {
                // Nếu có Composite Collider, trả về 1

                return 1;
            }
            else
            {

                return 0;
            }
        }
        else
        {
            return 0;
        }
    }

    // Vẽ điểm màu đỏ trong ô mục tiêu
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red; // Draw existing tiles
    //     foreach (Vector3 position in tilePositions)
    //     {
    //         Vector3Int cellPosition = grid.WorldToCell(position);
    //         Gizmos.DrawSphere(grid.GetCellCenterWorld(cellPosition), 0.2f);
    //     }

    //     // Draw the A* path in green
    //     // if (aStar != null && aStar.path != null)
    //     // {
    //     //     Gizmos.color = Color.green;
    //     //     foreach (Vector2Int position in aStar.path)
    //     //     {
    //     //         Vector3 worldPosition = grid.GetCellCenterWorld((Vector3Int)position);
    //     //         Gizmos.DrawSphere(worldPosition, 0.2f); // Size of the spheres
    //     //     }
    //     // }
    // }

    public static implicit operator Grid(Grid_HomeMade v)
    {
        throw new NotImplementedException();
    }
    // void Start()
    // {
    //     // Lấy kích thước cell của Grid
    //     Vector3 cellSize = grid.cellSize;
    //     // Debug.Log("Cell Size: " + cellSize);

    //     // Các tọa độ thế giới mà bạn muốn kiểm tra
    //     Vector3 worldPosition = new Vector3(-19, 9, 0);
    //     Vector3 worldPosition1 = new Vector3(-19, 8, 0);

    //     // Chuyển đổi tọa độ thế giới sang tọa độ của ô trong Grid
    //     Vector3Int cellPosition = grid.WorldToCell(worldPosition);
    //     Vector3Int cellPosition1 = grid.WorldToCell(worldPosition1);

    //     // In ra vị trí của các ô trong Grid
    //     // Debug.Log("Cell Position: " + cellPosition);
    //     // Debug.Log("Cell Position 1: " + cellPosition1);

    //     // Kiểm tra xem ô tại vị trí đó có tile hay không
    //     // gridMatrixForAMap(x_left, x_right, y_up, y_down, cellSize, tilemap[1], grid);
    //     int[,] grid_test = gridMatrixForTile(tilemap);
    //     // Chuyển sâng tọa độ 
    //     Vector3[,] coordinates = changeTocoordinates(gridMatrixForTile(tilemap));
    //     // Gán ô mục tiêu để vẽ

    //     // Example: find the path
    //     Debug.Log(toString(coordinates));
    //     Debug.Log(CheckForTile(tilemap[1], new Vector3Int(0,4,0)));
    //     Debug.Log(CheckForTile(tilemap[0], new Vector3Int(0,4,0)));
    // }
}
