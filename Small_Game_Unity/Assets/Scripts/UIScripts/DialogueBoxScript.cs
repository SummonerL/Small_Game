using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxScript : MonoBehaviour
{
    // reference to the player (for positioning)
    private GameObject _playerCharacter;

    // initialize _playerCharacter via singleton
    private void Awake() {
        _playerCharacter = PlayerSingleton.Instance.gameObject;
    }

    public void ShowDialogueBox(Camera activeCamera) { // passed from ui controller
        transform.position = activeCamera.WorldToScreenPoint(_playerCharacter.transform.position);
        gameObject.SetActive(true);
    }

    public void HideDialogueBox() {
        gameObject.SetActive(false);
    }
}
