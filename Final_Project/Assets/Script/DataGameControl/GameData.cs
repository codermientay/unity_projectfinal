using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance; // Singleton Instance

    public List<PositionOfScene> positionOfScenes = new List<PositionOfScene>(); // Danh sách tọa độ

    private void Awake()
    {
        // Tạo Singleton để giữ nguyên dữ liệu
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Không phá hủy object này khi chuyển cảnh
        }
        else
        {
            Destroy(gameObject); // Đảm bảo chỉ có một Instance duy nhất
        }
    }

    // Thêm tọa độ mới
    public void AddPosition(Vector3 position, int sceneIndex)
    {
        PositionOfScene existingScene = positionOfScenes.Find(scene => scene.index == sceneIndex);
        if (existingScene != null)
        {
            existingScene.playerPosition = position; // Cập nhật tọa độ nếu cảnh đã tồn tại
        }
        else
        {
            positionOfScenes.Add(new PositionOfScene(position, sceneIndex)); // Thêm cảnh mới
        }
    }

    // Lấy tọa độ theo index cảnh
    public Vector3 GetPosition(int sceneIndex)
    {
        PositionOfScene scene = positionOfScenes.Find(scene => scene.index == sceneIndex);
        return scene != null ? scene.playerPosition : Vector3.zero;
    }

    // Xóa dữ liệu cảnh
    public void RemovePosition(int sceneIndex)
    {
        positionOfScenes.RemoveAll(scene => scene.index == sceneIndex);
    }
}

[System.Serializable]
public class PositionOfScene
{
    public Vector3 playerPosition; // Lưu tọa độ player
    public int index; // Chỉ số của cảnh

    public PositionOfScene(Vector3 position, int sceneIndex)
    {
        playerPosition = position;
        index = sceneIndex;
    }
}