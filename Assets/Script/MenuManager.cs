using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MenuManager : MonoBehaviour {
    public static MenuManager Instance;

    public Image[] ResultImage;
    public Button[] ButtonsLevel;
    public Sprite ResultTrue;

    [SerializeField] private GameObject[] _canvas;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject[] _galleryPrefabs;
    
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else { Destroy(gameObject); }
        
    }

    private void Start()
    {
        FileManager.Instance.PathOperation();
        FileManager.Instance.ChangeResult();
        Gallery();
    }

    public void ChangeCanvas(int index)
    {
        foreach (var obj in _canvas)
        {
            obj.SetActive(false);
        }
        _canvas[index].SetActive(true);
    }

    public void Gallery()
    {
        for (int i = 0; i < FileManager.Instance.result.ResultArray.Count; i++)
        {
            Instantiate(_galleryPrefabs[i], transform.position, Quaternion.identity,_content.transform);
        }
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

