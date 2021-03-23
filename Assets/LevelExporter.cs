using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelExporter : MonoBehaviour
{
    GameObject[] allGameObject;
    List<GameObject> allFloors = new List<GameObject>();
    public int[,] level;
    float lowX;
    float lowZ;
    float highX;
    float highZ;

    float zDiff;
    float xDiff;

    SaveData data = new SaveData();
    void Start()
    {
        
        allGameObject =  GameObject.FindGameObjectsWithTag("LevelComponent") as GameObject[];
        for (int i = 0; i < allGameObject.Length; i++)
        {
            ComponentIdentifier identifier = allGameObject[i].GetComponent<ComponentIdentifier>();
            if (identifier.obstacleType == ComponentIdentifier.LevelComponentType.Plane)
            {
                allFloors.Add(allGameObject[i]);
            }

        }

        lowX = allFloors[0].transform.position.x;
        lowZ = allFloors[0].transform.position.z;
        highX = allFloors[0].transform.position.x;
        highZ = allFloors[0].transform.position.z;

        for (int i = 0; i < allGameObject.Length; i++)
        {
            LevelComponent levelComponent = new LevelComponent();
            ComponentIdentifier identifier = allGameObject[i].GetComponent<ComponentIdentifier>();
            levelComponent.objectName = identifier.obstacleType.ToString();
            levelComponent.position = allGameObject[i].transform.position;
            levelComponent.rotation = allGameObject[i].transform.rotation;
            levelComponent.scale = allGameObject[i].transform.localScale;
            data.levelComponentData.levelComponents.Add(levelComponent);

            if(identifier.obstacleType == ComponentIdentifier.LevelComponentType.Plane)
            {
                if (allGameObject[i].transform.position.x < lowX) { lowX = allGameObject[i].transform.position.x; }
                if (allGameObject[i].transform.position.x > highX) { highX = allGameObject[i].transform.position.x; }
                if (allGameObject[i].transform.position.z < lowZ) { lowZ = allGameObject[i].transform.position.z; }
                if (allGameObject[i].transform.position.z > highZ) {  highZ = allGameObject[i].transform.position.z; }
            }
        }



        highX += 25;
        highZ += 25;
        lowX -= 25;
        lowZ -= 25; 

        xDiff = highX - lowX;
        zDiff = highZ - lowZ;

        int xDim = (int) xDiff / 5;
        int zDim = (int) zDiff / 5;

        level = new int[xDim, zDim];

        for (int i = 0; i < level.GetLength(1); i++)
        {
            for (int j = 0; j < level.GetLength(0); j++)
            {
                level[j,i] = 0;
            }
        }
        
        foreach (GameObject floor in allFloors)
        {
            int xPos = (int) (floor.transform.position.x + -lowX) / 5;
            int zPos = (int) (floor.transform.position.z + -lowZ) / 5;
            level[xPos,zPos] = 1;
            for (int j = -5; j < 5; j++)
            {
                for (int i = -5; i < 5; i++)
                {
                    level[xPos + i, zPos + j] = 1;
                }
            }
        }

        foreach (GameObject obj in allGameObject)
        {
            LevelComponent levelComponent = new LevelComponent();
            ComponentIdentifier identifier = obj.GetComponent<ComponentIdentifier>();
            if (identifier.obstacleType == ComponentIdentifier.LevelComponentType.Coin || identifier.obstacleType == ComponentIdentifier.LevelComponentType.Speed || identifier.obstacleType == ComponentIdentifier.LevelComponentType.Jump || identifier.obstacleType == ComponentIdentifier.LevelComponentType.Finishline)
            {
                int xPos = (int)(obj.transform.position.x + -lowX) / 5;
                int zPos = (int)(obj.transform.position.z + -lowZ) / 5;
                if (identifier.obstacleType == ComponentIdentifier.LevelComponentType.Speed) { level[xPos, zPos] = 4; }
                if (identifier.obstacleType == ComponentIdentifier.LevelComponentType.Jump) { level[xPos, zPos] = 4; }
                if (identifier.obstacleType == ComponentIdentifier.LevelComponentType.Coin) { level[xPos, zPos] = 3; }
                if (identifier.obstacleType == ComponentIdentifier.LevelComponentType.Finishline) { level[xPos, zPos] = 2; }
            }
        }

        StreamWriter sr = new StreamWriter("grid.txt");
        sr.WriteLine(xDim.ToString());
        sr.WriteLine(zDim.ToString());
        sr.WriteLine(lowX);
        sr.WriteLine(lowZ);
        string output = "";
        for (int i = 0; i < level.GetLength(1); i++)
        {
            for (int j = 0; j < level.GetLength(0); j++)
            {
                output += level[j, i].ToString(); 
            }
            sr.WriteLine(output);
            output = "";
        }
        sr.Close();
        data.SaveIntoJson();
  }

}

public class SaveData : MonoBehaviour
{
    public LevelComponentData levelComponentData = new LevelComponentData();

    public void SaveIntoJson()
    {
        string levelComponent = JsonUtility.ToJson(levelComponentData, true);
        Debug.Log(Application.persistentDataPath);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + ".json", levelComponent);

    }
}

[System.Serializable]
public class LevelComponentData
{    
    public List<LevelComponent> levelComponents = new List<LevelComponent>(); 

}

[System.Serializable]
public class LevelComponent
{
    public string objectName;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale; 
}
