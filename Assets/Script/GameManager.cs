using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public bool CanMove = false;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private GameObject _gridElement;
    [SerializeField] private Transform _perant;
    [SerializeField] private List<GameObject> _goImage = new List<GameObject>();
    [SerializeField] private List<Transform> _gridElements = new List<Transform>();
    [SerializeField] private GameObject _lastPazzle;
    [SerializeField] private Button _nextLvl;
    [SerializeField] private Text _timer;
    [SerializeField] private AudioClip _startGame;
    private PazzleElement[,] _pazzle = new PazzleElement[3, 3];
    private GridElement[,] _grid = new GridElement[3, 3];
    [SerializeField] private List<GridElement> _elementsForRandom;
    [SerializeField] private Sprite Music;
    [SerializeField] private Sprite NoMusic;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Button MusicButton;
    [SerializeField] private RandomEnum _randomEnum;

    [SerializeField] private int _elements;
    [SerializeField] private float _specX;
    [SerializeField] private float _specY;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private bool _developerMode;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _pazzle = new PazzleElement[_elements, _elements];
        _grid = new GridElement[_elements, _elements];
        PastImage();
        Grid();
        StartCoroutine(GridElementCreate());
        _audio.clip = _startGame;
        _audio.Play();
        MusicCheck();
    }

    private void PastImage()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (_goImage[i] != null)
            {
                _goImage[i].GetComponent<SpriteRenderer>().sprite = sprites[i];
            }
            else
            {
                _lastPazzle.GetComponent<SpriteRenderer>().sprite = sprites[sprites.Length - 1];
            }

        }
    }

    private void Grid()
    {
        int counter = 1;
        float specY = _specY;
        for (float x = 0; x < _elements; x++)
        {
            float specX = -_specX;
            for (float y = 0; y < _elements; y++)
            {
                GameObject obj = Instantiate(_gridElement);
                obj.transform.position = new Vector3(specX, specY);
                obj.transform.SetParent(_perant);
                _gridElements.Add(obj.transform);
                GridElement element = obj.GetComponent<GridElement>();
                element.Id = counter;
                element.X = specX;
                element.Y = specY;
                element.GridX = (int)x;
                element.GridY = (int)y;
                _grid[(int)x, (int)y] = obj.GetComponent<GridElement>();
                specX += _specX;
                counter++;
                _elementsForRandom.Add(element);
            }
            specY -= _specY;
        }
    }
        
    
    private IEnumerator GridElementCreate()
    {
        yield return new WaitForSeconds(2.5f);
        int counter = 0;
        float specY = _specY;
        for (float x = 0; x < _elements; x++)
        {
            float specX = -_specX;
            for (float y = 0; y < _elements; y++)
            {
                GameObject currentObj = _goImage[counter];
                if (currentObj != null)
                {
                    GameObject obj = Instantiate(currentObj, new Vector2(specX, specY), Quaternion.identity, _gridElements[counter]);
                    obj.GetComponent<PazzleElement>().X = (int)x;
                    obj.GetComponent<PazzleElement>().Y = (int)y;
                    _pazzle[(int)x, (int)y] = obj.GetComponent<PazzleElement>();
                }
                counter++;
                specX += _specX;
                yield return new WaitForSeconds(.1f);
            }
            specY -= _specY;
        }
        CheckGrid();
        for (int i = 0; i < _randomEnum.Types.Count; i++)
        {
            if (!_developerMode)
            {
                StartCoroutine(RandomPuzzle((int)_randomEnum.Types[i]));
                yield return new WaitForSeconds(0.2f);
            }
        }
        StartCoroutine(Timer());
    }

   private IEnumerator RandomPuzzle(int index)
    {
        yield return null;
        GridElement grid = _elementsForRandom[_elementsForRandom.Count-1];
        foreach (var obj in _elementsForRandom)
        {
            if (obj.Free)
            {
                grid = obj;
            }
        }

        switch (index){ 
            case 0://up
                int x = grid.GridX + 1;
                PazzleMoving(x,grid.GridY, FindPuzzle(x, grid.GridY));
                break;
            case 1://right
                int y = grid.GridY - 1;
                PazzleMoving(y, grid.GridY, FindPuzzle(grid.GridX, y));
                break;
            case 2://down
                int xx = grid.GridX - 1;
                PazzleMoving(xx, grid.GridY, FindPuzzle(xx, grid.GridY));
                break;
            case 3://left
                int yy = grid.GridY + 1;
                PazzleMoving(yy, grid.GridY, FindPuzzle(grid.GridX, yy));
                break;
        }
   }

    public PazzleElement FindPuzzle(int X,int Y)
    {
        PazzleElement puzzle = null;

        foreach (var item in _elementsForRandom)
        {
            if(item.GridX==X && item.GridY == Y)
            {
                puzzle = item.GetComponentInChildren<PazzleElement>();
            }
        }
        return puzzle;
    }

    public void PazzleMoving(int x,int y,PazzleElement element)
    {
        for (int X = 0; X < _elements; X++)
        {
            if (_grid[X, y].Free)
            {
                if (element.X + 2 == _grid[X, y].GridX|| element.X - 2 == _grid[X, y].GridX) return;

                if (element.X + 3 == _grid[X, y].GridX || element.X - 3 == _grid[X, y].GridX) return;
                if (_developerMode)
                {
                    if (element.X + 1 == _grid[X, y].GridX)
                    {
                        _randomEnum.Types.Add(Action.DOWN);
                    }
                    if (element.X - 1 == _grid[X, y].GridX)
                    {
                        _randomEnum.Types.Add(Action.UP);
                    }
                }

                element.gameObject.transform.SetParent(_grid[X, y].transform);
                element.transform.position = _grid[X, y].transform.position;
                element.X = X;
                element.Y = y;
                CheckGrid();
                return;
            }
        }
        for (int Y = 0; Y < _elements; Y++)
        {
            if (_grid[x, Y].Free)
            {
                if (element.Y + 2 == _grid[x, Y].GridY|| element.Y - 2 == _grid[x, Y].GridY) return;

                if (element.Y + 3 == _grid[x, Y].GridY || element.Y - 3 == _grid[x, Y].GridY) return;

                if (_developerMode)
                {
                    if (element.Y + 1 == _grid[x, Y].GridY)
                    {
                        _randomEnum.Types.Add(Action.RIGHT);
                    }
                    if (element.Y - 1 == _grid[x, Y].GridY)
                    {
                        _randomEnum.Types.Add(Action.LEFT);
                    }
                }

                element.gameObject.transform.SetParent(_grid[x, Y].transform);
                element.transform.position = _grid[x, Y].transform.position;
                element.X = x;
                element.Y = Y;
                CheckGrid();
               return;
            }
        }
        
    }

    private void CheckGrid()
    {
        for (int i = 0; i < _elements; i++)
        {
            for (int j = 0; j < _elements; j++)
            {
                if (_grid[i,j].GetComponentInChildren<PazzleElement>())
                {
                    _grid[i, j].Free = false;
                }
                else
                {
                    _grid[i, j].Free = true;
                }
            }
        }
        Result();
    }

    public void Help()
    {
        if (CanMove)
        {
            foreach (Transform obj in _gridElements)
            {
                try
                {
                    obj.GetComponentInChildren<Text>().enabled = !obj.GetComponentInChildren<Text>().enabled;
                }
                catch { }
            }
        }
    }
        
    private void Result()
    {
        if (CanMove)
        {
            int counter = 0;
            foreach (Transform obj in _gridElements)
            {
                if (obj.GetComponentInChildren<PazzleElement>() == null || (obj.GetComponent<GridElement>().Id == obj.GetComponentInChildren<PazzleElement>().Id))
                {
                    counter++;
                    if (counter > ((_elements*_elements)-1))
                    {
                        CanMove = false;
                        GameObject newObj = Instantiate(_lastPazzle, new Vector2(0, 0), Quaternion.identity, _grid[2, 2].transform);
                        newObj.transform.localPosition = new Vector2(0, 0);
                        if (_nextLvl != null)
                        {
                            _nextLvl.gameObject.SetActive(true);
                        }
                        try
                        {
                            FileManager.Instance.result.ResultArray.Add(SceneManager.GetActiveScene().buildIndex);
                            FileManager.Instance.WriteFile();
                            Steam.Instence.GetAchievment(SceneManager.GetActiveScene().buildIndex);
                        }
                        catch { }

                        foreach (Transform newobj in _gridElements)
                        {
                            try
                            {
                                if (newobj.GetComponentInChildren<Text>().enabled == true)
                                {
                                    newobj.GetComponentInChildren<Text>().enabled = false;
                                }
                            }
                            catch { }

                        }
                    }
                }
            }
        }
    }

    private IEnumerator Timer()
    {
        if (_developerMode)
        {
            yield return new WaitForSeconds(_randomEnum.Types.Count * 0.1f);
        }
        TimeSpan myTimeSpan;
        CanMove = true;
        int timeCounter = 0;
        while (CanMove)
        {
            myTimeSpan = TimeSpan.FromSeconds(timeCounter);
            _timer.text = "TIMER " + string.Format("{0:mm:ss}", myTimeSpan);
            timeCounter++;
            yield return new WaitForSeconds(1f);
        }
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SwitchLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void MusicControll()
    {
        audioSource.enabled = !audioSource.enabled;
        if (audioSource.enabled)
        {
            MusicButton.GetComponent<Image>().sprite = Music;
            PlayerPrefs.SetString("Music", "ON");
        }
        else
        {
            MusicButton.GetComponent<Image>().sprite = NoMusic;
            PlayerPrefs.SetString("Music", "OFF");
        }
    }

    private void MusicCheck()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetString("Music") == "ON")
            {
                MusicButton.GetComponent<Image>().sprite = Music;
                audioSource.enabled = true;
            }
            else
            {
                MusicButton.GetComponent<Image>().sprite = NoMusic;
                audioSource.enabled = false;
            }
        }
    }
}