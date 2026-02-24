using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Spine;
using UnityEngine;

namespace DevDuck
{
    public class PositionGame16Data
    {
        public int level;
        public List<Vector3> listPositions;
    }

    public class SavePositonGame16 : MonoBehaviour
    {
        public int levelPosition, indexLevel;
        public List<Vector3> positions = new List<Vector3>();
        public List<GameObject> positonsObjects = new List<GameObject>();
        public static SavePositonGame16 instance;

        private void Awake()
        {
            instance = this;
        }
        public void SaveToJson()
        {
            PositionGame16Data data = new PositionGame16Data();
            data.level = levelPosition;
            data.listPositions = positions;
            string json = JsonUtility.ToJson(data);
            //  File.WriteAllText(Application.persistentDataPath + "/save.json", json);
            File.WriteAllText($"Assets/Resources/Art/game16/LevelData_16/DataLevel_{data.level}.json", json);
        }
        public void UpdateJson()
        {
            positions.Clear();
            for (int i = 0; i < positonsObjects.Count; i++)
            {
                if (positonsObjects[i].activeSelf)
                {
                    positions.Add(positonsObjects[i].transform.position);
                }
            }
        }

        public PositionGame16Data LoadFromJson()
        {
            PositionGame16Data result = new PositionGame16Data();
            string data = "";
#if UNITY_EDITOR
            data = File.ReadAllText($"Assets/Resources/Art/game16/LevelData_16/DataLevel_{levelPosition}.json");
#else
            data = File.ReadAllText($"Assets/Resources/Art/game16/LevelData_16/DataLevel_{levelPosition}.json");
#endif
            result = JsonUtility.FromJson<PositionGame16Data>(data);
            positions = result.listPositions;
            return result;
        }
        public List<Vector3> LoadLevelPositionGame16(int level)
        {
            PositionGame16Data result = new PositionGame16Data();
            string data = "";
#if UNITY_EDITOR
            if (level >= 5)
            {
                data = File.ReadAllText("Assets/Resources/Art/game16/LevelData_16/DataLevel_5.json");
            }
            else
            {
                data = File.ReadAllText($"Assets/Resources/Art/game16/LevelData_16/DataLevel_{level + 1}.json");
            }
#else
             if (level >= 5)
            {
                data = Resources.Load<TextAsset>($"Art/game16/LevelData_16/DataLevel_5").ToString();
            }
            else
            {
               data = Resources.Load<TextAsset>($"Art/game16/LevelData_16/DataLevel_{level + 1}").ToString();
            }
#endif
            result = JsonUtility.FromJson<PositionGame16Data>(data);
            positions = result.listPositions;
            return positions;
        }
    }
}