using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FileManager : MonoBehaviour {
    public static FileManager Instance;

    public Result result = new Result();
    private string _path;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PathOperation();
        ChangeResult();
    }

    public void PathOperation()
    {
        _path = Path.Combine(Application.dataPath, "Result.json");
        if (File.Exists(_path))
        {
            result = JsonUtility.FromJson<Result>(File.ReadAllText(_path));
        }
        else
        {
            File.WriteAllText(_path, JsonUtility.ToJson(result));
        }
    }

    public void ChangeResult()
    {
        int temp;
        for (int i = 0; i < result.ResultArray.Count; i++)
        {
            temp = result.ResultArray[i];
            MenuManager.Instance.ResultImage[temp-1].sprite = MenuManager.Instance.ResultTrue;
            MenuManager.Instance.ButtonsLevel[temp-1].interactable = true;
            Steam.Instence.GetAchievment(temp - 1);
        }
    }

    public void WriteFile()
    {
        File.WriteAllText(_path, JsonUtility.ToJson(result));
    }
}
public class Result
{
    public List<int> ResultArray = new List<int>();
}
