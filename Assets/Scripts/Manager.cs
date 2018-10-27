using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Level;
using Game.UI;
using Game.Gameplay;

namespace Game.Interface
{
    /// <summary>
    /// Game states
    /// </summary>
    public enum GameState
    {
        None            = 0,
        Play            = 1,
        Pause           = 2,
        Lose            = 3,
        Win             = 4
    };

    /// <summary>
    /// Starting Types
    /// </summary>
    public enum StartingTypes
    {
        Starting            = 0,
        RunThrough          = 1,
        JumpThruBuilding    = 2
    };


    public class Manager : MonoBehaviour
    {
        /// <summary>
        /// Get this instance
        /// </summary>
        public static Manager Instance;

        /// <summary>
        /// Speed Meter
        /// </summary>
        public SpeedMeter SpeedMeter;

        /// <summary>
        /// Pivot Position
        /// </summary>
        public Vector3 PivotPosition;

        /// <summary>
        /// Checkpoint list
        /// </summary>
        public List<float> CheckPointList = new List<float>();

        /// <summary>
        /// List of different Segments
        /// </summary>
        public GameObject[] ParkourSegmentsList;

        /// <summary>
        /// List of Different Starting Segments
        /// </summary>
        public GameObject[] StartingParkourSegmentsList;

        /// <summary>
        /// Start Game Panel
        /// </summary>
        public GameObject StartGamePanel;

        /// <summary>
        /// Lsot game panel
        /// </summary>
        public GameObject LostGamePanel;

        /// <summary>
        /// Pause Game Panel
        /// </summary>
        public GameObject PauseGamePanel;

        /// <summary>
        /// Speed ducking requirement
        /// </summary>
        public float SpeedDuckRequirement;

        /// <summary>
        /// Start game
        /// </summary>
        private bool _StartGame = false;

        /// <summary>
        /// StartGame property
        /// </summary>
        public bool StartGame
        {
            get
            {
                return this._StartGame;
            }
            set
            {
                this._StartGame = value;
                this.StartGamePanel.SetActive(!this._StartGame);
                if (this.Player != null && this._StartGame)
                    this.Player.PlayerState = AnimationType.Run;
            }
        }

        /// <summary>
        /// Player Spawned
        /// </summary>
        public bool PlayerSpawned { get; set; }

        /// <summary>
        /// Max amount of segments spawned ahead of player
        /// </summary>
        public int MaxSegmentsSpawned = 3;

        /// <summary>
        /// Current Segment the player is on
        /// </summary>
        private int _CurrentSegment = 0;

        /// <summary>
        /// Speed Magnifier per percentage boost
        /// </summary>
        public float SpeedMagnifier = 1.0f;

        /// <summary>
        /// Duration of how the speed gets to the target area
        /// </summary>
        public float SpeedDurationSpeed = 1.0f;

        /// <summary>
        /// Speed Current modifier
        /// </summary>
        private float _SpeedTargetModifier = 0.0f;


        /// <summary>
        /// Speed Current Modifier
        /// </summary>
        public float SpeedTargetModifier
        {
            get
            {
                return this._SpeedTargetModifier;
            }
            set
            {
                this._SpeedTargetModifier = value;
            }
        }

        /// <summary>
        /// Speed Current modifier
        /// </summary>
        public float _SpeedCurrentModifier;

        /// <summary>
        /// Speed Current Modifier
        /// </summary>
        public float SpeedCurrentModifier
        {
            get
            {
                return this._SpeedCurrentModifier;
            }
            set
            {
                this._SpeedCurrentModifier = value;
            }
        }
        /// <summary>
        /// Lsit of segments currently spawned
        /// </summary>
        private List<GameObject> _CurrentListOfSegments = new List<GameObject>();

        /// <summary>
        /// Player PreFab
        /// </summary>
        public GameObject PlayerPreFab;

        /// <summary>
        /// Player component
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Follow player
        /// </summary>
        private GameObject _PlayerFollow;

        /// <summary>
        /// Game state
        /// </summary>
        private GameState _GameState = GameState.None;

        /// <summary>
        /// Game State property
        /// </summary>
        public GameState GameState
        {
            get
            {
                return this._GameState;
            }
            set
            {
                if (this._GameState == value)
                    return;
                this._GameState = value;
                switch(this._GameState)
                {
                    case GameState.Lose:
                        this.Player.PlayerState = AnimationType.Faint;
                        this.StartGamePanel.SetActive(false);
                        this.LostGamePanel.SetActive(true);
                        this.PauseGamePanel.SetActive(false);
                        break;
                    case GameState.Pause:
                        this.Player.PlayerState = AnimationType.Idle;
                        this.StartGamePanel.SetActive(false);
                        this.LostGamePanel.SetActive(false);
                        this.PauseGamePanel.SetActive(true);
                        break;
                    case GameState.Win:
                        break;
                    case GameState.Play:
                        if(this.Player != null)
                            this.Player.PlayerState = this.Player.PreviousState;
                        this.StartGamePanel.SetActive(false);
                        this.LostGamePanel.SetActive(false);
                        this.PauseGamePanel.SetActive(false);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Camera follows this player
        /// </summary>
        public GameObject PlayerFollow
        {
            get
            {
                return this._PlayerFollow;
            }
            set
            {
                this._PlayerFollow = value;
                this.Player = this._PlayerFollow.GetComponent<Player>();
            }
        }

        /// <summary>
        /// Adjust of the camera between the player
        /// </summary>
        public Vector3 CameraAdjustment;

        /// <summary>
        /// Main Camera
        /// </summary>
        public GameObject MainCamera;

        /// <summary>
        /// Always the time to go faster over time
        /// </summary>
        private float _TimeCurve = 0.0f;

        /// <summary>
        /// Pause Game
        /// </summary>
        public bool PauseGame = false;

        /// <summary>
        /// Background movement
        /// </summary>
        public BackgroundMovement BackgroundMovement;

        /// <summary>
        /// Awake this instance
        /// </summary>
        private void Awake()
        {
            Manager.Instance = this;
            this.BackgroundMovement.ChangeEnvironment(AssetFactory.Instance.BackgroundType(FileConfigHandler.Instance.UserConfig.LevelConfig.EnvironmentType));
        }

        /// <summary>
        /// Start this instance
        /// </summary>
        private void Start()
        {
            this.ResetStats();
            this.CreateLevel();
        }

        /// <summary>
        /// Update every frame
        /// </summary>
        private void Update()
        {
            this.MainCamera.transform.position = this.PlayerFollow.transform.position + this.CameraAdjustment;
            if (!this._StartGame)
                return;
            if (this.GameState == GameState.Lose)
            {
                return;
            }
            this.SpeedMeterAdjustment();
        }

        /// <summary>
        /// Restart Level
        /// </summary>
        public void RestartLevel()
        {
            this.ResetStats();
            this.CreateLevel();
        }

        /// <summary>
        /// Speed Meter Adjustment
        /// </summary>
        public void SpeedMeterAdjustment()
        {
            if (this.Player.PlayerState == AnimationType.Run)
            {
                if (this.SpeedCurrentModifier == this.SpeedTargetModifier)
                    return;
                if (this.SpeedCurrentModifier < this.SpeedTargetModifier)
                {
                    this.SpeedCurrentModifier += (Time.deltaTime + (this._TimeCurve / 2.0f)) * this.SpeedDurationSpeed;
                    this._TimeCurve += Time.deltaTime;
                    if (this.SpeedCurrentModifier > this.SpeedTargetModifier)
                    {
                        this.SpeedCurrentModifier = this.SpeedTargetModifier;
                        this._TimeCurve = 0.0f;
                    }
                }
                else if (this.SpeedCurrentModifier > this.SpeedTargetModifier)
                {
                    this.SpeedCurrentModifier -= (Time.deltaTime + (this._TimeCurve / 2.0f)) * this.SpeedDurationSpeed;
                    this._TimeCurve += Time.deltaTime;
                    if (this.SpeedCurrentModifier < this.SpeedTargetModifier)
                    {
                        this.SpeedCurrentModifier = this.SpeedTargetModifier;
                        this._TimeCurve = 0.0f;
                    }
                }
            }
            else if (this.Player.PlayerState == AnimationType.Duck &&
                    !this.Player.Falling)
            {
                this.SpeedCurrentModifier -= Time.deltaTime * this.SpeedDurationSpeed * this.Player.SlideDrag;
                if (this.SpeedCurrentModifier < this.SpeedDuckRequirement)
                    this.Player.PlayerState = AnimationType.Run;
            }
            this.SpeedMeter.AdjustDisplay(this.SpeedCurrentModifier);
        }

        /// <summary>
        /// Reset Stats
        /// </summary>
        public void ResetStats()
        {
            this.PivotPosition = Vector3.zero;
            this.PlayerSpawned = false;
            this._CurrentSegment = 0;
            this.SpeedTargetModifier = 50.0f;
            this.SpeedCurrentModifier = 50.0f;
            this.SpeedMeter.AdjustDisplay(this.SpeedCurrentModifier);
            this.SpeedMeter.AdjustDuckIndicator();
            this.SpeedMeter.AdjustSpeedIndicator(this.SpeedCurrentModifier);
            this.SpeedMeter.Slider.value = this.SpeedCurrentModifier / 10.0f;
            this.GameState = GameState.Play;
            this.StartGame = false;
            this.StartGamePanel.SetActive(true);
        }

        /// <summary>
        /// Clear level and player
        /// </summary>
        public void ClearLevel()
        {
            for(int i = 0; i < this._CurrentListOfSegments.Count; i++)
            {
                if (this._CurrentListOfSegments[i] == null)
                    continue;
                Destroy(this._CurrentListOfSegments[i]);
                this._CurrentListOfSegments[i] = null;
            }
            if(this.Player != null)
            {
                Destroy(this.Player.gameObject);
                this.Player = null;
            }
        }

        /// <summary>
        /// Create level
        /// </summary>
        public void CreateLevel()
        {
            this.ClearLevel();
            this._CurrentListOfSegments = new List<GameObject>();
            float distanceCreated = 0.0f;
            while(distanceCreated < FileConfigHandler.Instance.UserConfig.LevelConfig.LevelLength)
            {

            }
            for (int i = 0; i < this.MaxSegmentsSpawned; i++)
            {
                GameObject parkour = (GameObject)Instantiate(this.ParkourSegmentsList[Random.Range(0, this.ParkourSegmentsList.Length)]);
                parkour.transform.position = this.PivotPosition;
                parkour.GetComponent<SegmentLevel>().DataBind();
                this._CurrentListOfSegments.Add(parkour);
            }
        }
        
        /// <summary>
        /// Display Color
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static Color DisplayColor(float percentage)
        {
            if(percentage < 50.0f)
                return Color.Lerp(Color.red, Color.yellow, (percentage / 100.0f) * 2.0f);
            else
                return Color.Lerp(Color.yellow, Color.green, ((percentage - 50.0f) / 100.0f) * 2.0f);
        }
    }
}