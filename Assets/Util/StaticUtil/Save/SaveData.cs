using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    [Serializable]
    public class SaveData
    {
        [NonSerialized]
        private string _FilePath;

        public int HighScore;

        public SaveData() { }

        public SaveData(string filePath)
        {
            _FilePath = filePath;
        }

        public void LoadGame()
        {
            if (File.Exists(_FilePath))
            {
                SaveData loaded = DeserializeSaveData();
                LoadFrom(loaded);

                Debug.Log("Game data loaded!");
            }
            else
                Debug.Log("No save data!");
        }

        private SaveData DeserializeSaveData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream file = File.Open(_FilePath, FileMode.Open))
            {
                SaveData loaded = (SaveData)bf.Deserialize(file);
                return loaded;
            }
        }

        private void LoadFrom(SaveData loaded)
        {
            HighScore = loaded.HighScore;
        }

        public void SaveGame()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(_FilePath);

            bf.Serialize(file, this);
            file.Close();
        }
    }
}
