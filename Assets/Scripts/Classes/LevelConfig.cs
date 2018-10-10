using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interface
{
    /// <summary>
    /// Game Mode Types
    /// </summary>
    public enum GameTypes
    {
        Tutorial,
        BeatTheClock,
        OutRun,
        GetToTheEnd,
        DefeatTheBoss,
        Collector
    };
    
    /// <summary>
    /// Environment Types
    /// </summary>
    public enum EnvironmentTypes
    {
        CityNight,
        CityDay,
        CitySunset
    };

    /// <summary>
    /// Config for each level setup
    /// </summary>
    public struct LevelConfig
    {
        /// <summary>
        /// Level ID
        /// </summary>
        public int LevelID;

        /// <summary>
        /// Game Type
        /// </summary>
        public GameTypes GameType;

        /// <summary>
        /// Environment type
        /// </summary>
        public EnvironmentTypes EnvironmentType;

        /// <summary>
        /// Length of the level, 0 means infinite
        /// </summary>
        public float LevelLength;

        /// <summary>
        /// How big are the gaps between each new obsticle
        /// </summary>
        public float LevelGapLength;

        /// <summary>
        /// Length of time for Beat the Clock mode
        /// </summary>
        public float StartingTimeLength;

        /// <summary>
        /// Amount of health the boss has for Beat the boss mode
        /// </summary>
        public float BossHealthAmount;

        /// <summary>
        /// Player must be faster than this enemy in order to win
        /// </summary>
        public float OutRunSpeedAverage;

        /// <summary>
        /// Collect the amount of items before reaching the end of the level
        /// </summary>
        public float CollectAmount;


        /// <summary>
        /// Get to the End
        /// </summary>
        /// <param name="gameType"></param>
        /// <param name="environmentType"></param>
        /// <param name="levelLength"></param>
        public LevelConfig(int              levelID,
                           GameTypes        gameType, 
                           EnvironmentTypes environmentType, 
                           float            levelLength, 
                           float            levelGapLength,
                           float            startingTimeLength, 
                           float            bossHealthAmount, 
                           float            outRunSpeedAverage, 
                           float            collectAmount)
        {
            this.LevelID = levelID;
            this.GameType = gameType;
            this.EnvironmentType = environmentType;
            this.LevelLength = levelLength;
            this.LevelGapLength = levelGapLength;
            this.StartingTimeLength = startingTimeLength;
            this.BossHealthAmount = bossHealthAmount;
            this.OutRunSpeedAverage = outRunSpeedAverage;
            this.CollectAmount = collectAmount;
        }
    }
}