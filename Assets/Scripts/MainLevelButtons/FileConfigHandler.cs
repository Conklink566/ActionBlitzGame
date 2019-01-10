using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Game.Interface
{
    public enum SceneNames
    {
        LoadingAssets       = 0,
        MainMenu            = 1,
        LoadingLevel        = 2,
        MainLevel           = 3
    };
    
    public class FileConfigHandler : MonoBehaviour
    {
        /// <summary>
        /// UserConfig
        /// </summary>
        public UserConfig UserConfig { get; set; }

        /// <summary>
        /// Get this instance
        /// </summary>
        public static FileConfigHandler Instance;

        /// <summary>
        /// Awake this instance
        /// </summary>
        private void Awake()
        {
            if(FileConfigHandler.Instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                FileConfigHandler.Instance = this;
            }
            else if(FileConfigHandler.Instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Save File
        /// </summary>
        public static void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/userData.dat");

            formatter.Serialize(file, FileConfigHandler.Instance.UserConfig);
            file.Close();
        }

        /// <summary>
        /// Load File
        /// </summary>
        public static void Load()
        {
            if(File.Exists(Application.persistentDataPath + "/userData.dat"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/userData.dat", FileMode.Open);
                FileConfigHandler.Instance.UserConfig = (UserConfig)formatter.Deserialize(file);
                file.Close();
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = File.Create(Application.persistentDataPath + "/userData.dat");

                FileConfigHandler.Instance.UserConfig = new UserConfig();
                FileConfigHandler.Instance.UserConfig.CharacterConfig = new CharacterConfig(0, 0, 0, 0);
                FileConfigHandler.Instance.UserConfig.LevelConfig = AssetFactory.Instance.LevelConfigList[0];

                formatter.Serialize(file, FileConfigHandler.Instance.UserConfig);
                file.Close();
            }
        }

        /// <summary>
        /// Delete file
        /// </summary>
        public static void DeleteFile()
        {
            if (File.Exists(Application.persistentDataPath + "/userData.dat"))
            {
                File.Delete(Application.persistentDataPath + "/userData.dat");
            }
        }
    }
}