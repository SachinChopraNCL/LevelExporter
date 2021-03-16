using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelExporter : MonoBehaviour
{
    GameObject[] allGameObject;
    SaveData data = new SaveData();
    void Start()
    {
        allGameObject =  GameObject.FindGameObjectsWithTag("LevelComponent") as GameObject[];
        for(int i = 0; i < allGameObject.Length; i++)
        {
            LevelComponent levelComponent = new LevelComponent();
            ComponentIdentifier identifier = allGameObject[i].GetComponent<ComponentIdentifier>();
            levelComponent.objectName = identifier.obstacleType.ToString();
            levelComponent.position = allGameObject[i].transform.position;
            levelComponent.rotation = allGameObject[i].transform.rotation;
            levelComponent.scale = allGameObject[i].transform.localScale;
            data.levelComponentData.levelComponents.Add(levelComponent);
        }
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