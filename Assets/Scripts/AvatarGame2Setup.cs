using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DevDuck
{
    [Serializable]
    public class CharacterInfor
    {
        public Sprite avatar;
        public string name;
    }

    public class AvatarGame2Setup : MonoBehaviour
    {
        public List<CharacterInfor> characters = new List<CharacterInfor>();
        public Image character1Image, character2Image;
        public TextMeshPro character1Name, character2Name;
        public SpriteRenderer character1Sprite, character2Sprite;
        private int character2Index;

        void Start()
        {
            int character1Index = Random.Range(0, characters.Count);
            character1Image.sprite = characters[character1Index].avatar;
            character1Sprite.sprite = characters[character1Index].avatar;
            character1Name.text = characters[character1Index].name;
            do
            {
                character2Index = Random.Range(0, characters.Count);
                character2Image.sprite = characters[character2Index].avatar;
                character2Sprite.sprite = characters[character2Index].avatar;
                character2Name.text = characters[character2Index].name;
            } while (character2Index == character1Index);
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
            }
        }
    }
}