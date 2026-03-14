using UnityEngine;

public class Clue : MonoBehaviour
{
    private bool _playerInRange = false;

    private void Update()
    {
        if (PlayerController.Interact && _playerInRange)
        {
            GameManager.Instance.AddClue();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerInRange = false;
    }
}
