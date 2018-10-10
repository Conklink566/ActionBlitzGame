using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interface
{
    public class AssetFactory : MonoBehaviour
    {
        /// <summary>
        /// Get this instance
        /// </summary>
        public static AssetFactory Instance;

        /// <summary>
        /// List of LevelConfigs
        /// </summary>
        public LevelConfig[] LevelConfigList =
        {
            new LevelConfig(0,  GameTypes.Tutorial,     EnvironmentTypes.CityNight,  500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(1,  GameTypes.GetToTheEnd,  EnvironmentTypes.CityNight,  500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(2,  GameTypes.OutRun,       EnvironmentTypes.CityNight,  500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(3,  GameTypes.BeatTheClock, EnvironmentTypes.CityNight,  500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(4,  GameTypes.Collector,    EnvironmentTypes.CityNight,  500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(5,  GameTypes.GetToTheEnd,  EnvironmentTypes.CityNight,  500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(6,  GameTypes.GetToTheEnd,  EnvironmentTypes.CityDay,    500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(7,  GameTypes.OutRun,       EnvironmentTypes.CityDay,    500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(8,  GameTypes.BeatTheClock, EnvironmentTypes.CityDay,    500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(9,  GameTypes.Collector,    EnvironmentTypes.CityDay,    500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(10, GameTypes.GetToTheEnd,  EnvironmentTypes.CityDay,    500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(11, GameTypes.GetToTheEnd,  EnvironmentTypes.CitySunset, 500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(12, GameTypes.OutRun,       EnvironmentTypes.CitySunset, 500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(13, GameTypes.BeatTheClock, EnvironmentTypes.CitySunset, 500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(14, GameTypes.Collector,    EnvironmentTypes.CitySunset, 500.0f, 50.0f, 0,0,0,0),
            new LevelConfig(15, GameTypes.GetToTheEnd,  EnvironmentTypes.CitySunset, 500.0f, 50.0f, 0,0,0,0),
        };

        /// <summary>
        /// List of Head Icons for Character Editor
        /// </summary>
        public Sprite[] HeadIcons;

        /// <summary>
        /// List of Body Icons for Character Editor
        /// </summary>
        public Sprite[] BodyIcons;

        /// <summary>
        /// List of Leg Icons for Character Editor
        /// </summary>
        public Sprite[] LegIcons;

        /// <summary>
        /// 0-2 is the moving background, 3 is the background, 4 is the light source
        /// </summary>
        public Sprite[] CityNightBackground;

        /// <summary>
        /// 0-2 is the moving background, 3 is the background, 4 is the light source
        /// </summary>
        public Sprite[] CityDayBackground;

        /// <summary>
        /// 0-2 is the moving background, 3 is the background, 4 is the light source
        /// </summary>
        public Sprite[] CitySunsetBackground;

        /// <summary>
        /// Awake this instance
        /// </summary>
        private void Awake()
        {
            if (AssetFactory.Instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                AssetFactory.Instance = this;
            }
            else if (AssetFactory.Instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// Find the environment array based off the selected environment type
        /// </summary>
        /// <param name="environmentType"></param>
        /// <returns></returns>
        public Sprite[] BackgroundType(EnvironmentTypes environmentType)
        {
            switch (environmentType)
            {
                case EnvironmentTypes.CityNight:
                    return this.CityNightBackground;
                case EnvironmentTypes.CityDay:
                    return this.CityDayBackground;
                case EnvironmentTypes.CitySunset:
                    return this.CitySunsetBackground;
                default:
                    throw new System.Exception(string.Format("Invalid: {0} has not been set properly in the BackgroundType property.", environmentType.ToString()));
            }
        }
    }
}