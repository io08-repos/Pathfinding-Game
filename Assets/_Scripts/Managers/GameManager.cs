using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int _cluesFound = 0;
    private const int CLUES_TO_FIND = 8;

    public bool AllCluesFound => _cluesFound >= CLUES_TO_FIND;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddClue() => _cluesFound++;
}
