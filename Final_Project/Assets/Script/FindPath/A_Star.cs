using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class A_Star : MonoBehaviour
{
    // Variable to store the path for visualization
    public Grid_HomeMade grid;
    public class Node
    {
        public Vector3 position;
        public Node parent;

        public float gCost;
        public float hCost;
        public bool checkFlag;
        public float fCost
        {
            get { return gCost + hCost; }
        }
        public Node(Vector3 pos)
        {
            position = pos;
            checkFlag = false;
        }
        public override string ToString()
        {
            return $"Position: {position}, gCost: {gCost}, hCost: {hCost}, fCost: {fCost}";
        }
        // Override phương thức Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Node other = (Node)obj;
            return position.Equals(other.position);
        }

        // Override phương thức GetHashCode
        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
    }

    public A_Star(Grid gridBase, Tilemap[] tilemaps, float xLeft, float xRight, float yUp, float yDown)
    {
        grid = new Grid_HomeMade(gridBase, tilemaps, xLeft, xRight, yUp, yDown);
    }

    // A* algorithm to find the shortest path
    public List<Vector3> FindPath(Vector3 start, Vector3 goal)
    {


        Vector3[,] map = grid.changeTocoordinates(grid.gridMatrixForTile(grid.tilemap));

        float maxIterations = 500;

        int count = 0;

        if (isWalkable(goal, map) == false || isWalkable(start, map) == false)
        {
            // Debug.Log("Điểm này không đi đc!");
            return null;
        }
        else
        {
            Node Goal = new Node(goal);

            Node current = new Node(start);

            current.gCost = 0;

            int i = 0;

            List<Node> listChecked = new List<Node>();
            listChecked.Add(current);

            while (current.position.x != goal.x || current.position.y != goal.y)
            {
                current.checkFlag = true;
                count++;
                Debug.Log("-------------------------------Lần chạy số : " + i + "-------------------------------");
                List<Node> neiboughs = getNeighbors(current, map, listChecked);
                int num = 0; //cái này để debbug
                foreach (Node neibough in neiboughs)
                {

                    num++;
                    neibough.parent = current;
                    neibough.gCost += current.gCost + 1;
                    neibough.hCost = getDistance(neibough, Goal);
                    Debug.Log("Mình đang check cái này!!!!!: " + neibough.fCost + " => Với tọa dộ là: " + neibough.position);



                }
                listChecked.Add(current);
                current = findMin(neiboughs);
                Debug.Log("Min: " + current.position + " với fCost: " + current.fCost);
                Debug.Log("-------------------------------Lần chạy số : " + i + " Có (" + num + ") hàng xóm -------------------------------");
                i++;
                if (count > maxIterations)
                {
                    Debug.Log("Stopped due to max iteration limit.");
                    break;
                }
            }
            if (current.position == goal)
            {
                List<Vector3> path = ReconstructPath(new Node(start), current);
                Debug.Log("Đường đi được tìm thấy: ");
                foreach (var pos in path)
                {
                    Debug.Log(pos);
                }
                return path; // Kết thúc vì đã tìm được đường đi
            }
            Debug.Log(grid.toString(map));
            Debug.Log("Đây là cha của thằng cuối cùng: " + current.parent.position);
            return null;

        }
    }
    public bool isWalkable(Vector3 position, Vector3[,] map)
    {
        int[,] matrix = grid.gridMatrixForTile(grid.tilemap);
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                if (position == map[row, col] && matrix[row, col] == 1)
                {
                    return false;
                }
                else if (position == map[row, col] && matrix[row, col] == 0)
                {
                    return true;
                }
            }
        }
        return true;
    }

    public Node findMin(List<Node> nodes)
    {
        float min = MathF.Abs((grid.x_right - grid.x_left) * (grid.y_up - grid.y_down));
        Node node = null;
        foreach (Node i in nodes)
        {
            if (i.fCost < min && !i.checkFlag)
            {
                min = i.fCost;
                node = i;
            }
        }
        return node;
    }

    // Get the neighbors of a node (up, down, left, right)
    private List<Node> getNeighbors(Node node, Vector3[,] map, List<Node> listChecked)
    {
        List<Node> nodes = new List<Node>();
        Vector3 v = node.position;
        Vector3 v_up = new Vector3(v.x, v.y + 1, v.z);
        Vector3 v_down = new Vector3(v.x, v.y - 1, v.z);
        Vector3 v_left = new Vector3(v.x - 1, v.y, v.z);
        Vector3 v_right = new Vector3(v.x + 1, v.y, v.z);
        Node node_up = new Node(v_up);
        Node node_down = new Node(v_down);
        Node node_left = new Node(v_left);
        Node node_right = new Node(v_right);

        nodes.Add(node_up);
        nodes.Add(node_down);
        nodes.Add(node_left);
        nodes.Add(node_right);

        for (int i = 0; i < nodes.Count; i++)
        {
            if (!IsInGrid(nodes[i].position) || !isWalkable(nodes[i].position, map) || listChecked.Contains(nodes[i]))
            {
                nodes.RemoveAt(i);
            }
        }

        return nodes;

    }

    // Check if a position is inside the grid
    private bool IsInGrid(Vector3 position)
    {
        if (position.x < grid.x_left || position.x > grid.x_right)
        {
            return false;
        }
        else
        {
            if (position.y < grid.y_down || position.y > grid.y_up)
                return false;
            else
                return true;
        }

    }

    // Reconstruct the path from start to goal by following the parent nodes
    // Reconstruct the path from start to goal by following the parent nodes
    private List<Vector3> ReconstructPath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        // Đi ngược từ node đích về node bắt đầu
        while (currentNode != null && !currentNode.Equals(startNode))
        {
            path.Add(currentNode.position); // Thêm vị trí của node hiện tại vào danh sách đường đi
            currentNode = currentNode.parent; // Di chuyển đến node cha
            // Debug.Log("Đây là vị trị hiện tại: " + currentNode.position);
        }

        // Kiểm tra nếu đã đạt đến startNode
        if (currentNode.Equals(startNode))
        {
            path.Add(startNode.position); // Thêm node bắt đầu vào đường đi
        }
        else
        {
            Debug.LogError("Không thể tái tạo đường đi - không tìm thấy đường đi hợp lệ.");
            return null; // Trả về null nếu không có đường đi hợp lệ
        }

        path.Reverse(); // Đảo ngược danh sách để có đường đi đúng từ start đến goal
        return path;
    }


    // Calculate distance between two nodes (Manhattan distance for grid)
    private float getDistance(Node a, Node b)
    {
        float x = Mathf.Pow((a.position.x - b.position.x), 2);
        float y = Mathf.Pow((a.position.y - b.position.y), 2);
        float z = Mathf.Pow((a.position.z - b.position.z), 2);
        return Mathf.Sqrt(x + y + z);
    }

    void Start()
    {

        Vector3 v = new Vector3(19, 7, 0);
        Debug.Log(IsInGrid(v));
        Node a = new Node(new Vector3(19, 8, 0));
        Node b = new Node(new Vector3(19, 7, 0));

        Node c = new Node(v);
        Debug.Log(getDistance(a, b));

        Vector3 s = new Vector3(0, -5, 0);
        Vector3 g = new Vector3(3, -9, 0);
        FindPath(s, g);

        Debug.Log("Chỗ này đi: " + isWalkable(new Vector3(0, 4, 0), grid.changeTocoordinates(grid.gridMatrixForTile(grid.tilemap))));
        Debug.Log(grid.toString(grid.changeTocoordinates(grid.gridMatrixForTile(grid.tilemap))));

    }
}
