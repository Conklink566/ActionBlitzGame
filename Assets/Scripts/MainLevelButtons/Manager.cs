using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Level;
using Game.UI;
using Game.Gameplay;
using UnityEngine.SceneManagement;

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
        /// Floor Pivot Position
        /// </summary>
        public Vector3 FloorPivotPosition;

        /// <summary>
        /// Checkpoint list
        /// </summary>
        public List<float> CheckPointList = new List<float>();

        /// <summary>
        /// List of different Segments
        /// </summary>
        public GameObject[] ParkourSegmentsList;

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
        /// Pause Game Panel
        /// </summary>
        public GameObject WinGamePanel;

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
                        this.WinGamePanel.SetActive(false);
                        break;
                    case GameState.Pause:
                        this.Player.PlayerState = AnimationType.Idle;
                        this.StartGamePanel.SetActive(false);
                        this.LostGamePanel.SetActive(false);
                        this.PauseGamePanel.SetActive(true);
                        this.WinGamePanel.SetActive(false);
                        break;
                    case GameState.Win:
                        this.Player.PlayerState = AnimationType.Idle;
                        this.StartGamePanel.SetActive(false);
                        this.LostGamePanel.SetActive(false);
                        this.PauseGamePanel.SetActive(false);
                        this.WinGamePanel.SetActive(true);
                        break;
                    case GameState.Play:
                        if(this.Player != null)
                            this.Player.PlayerState = this.Player.PreviousState;
                        this.StartGamePanel.SetActive(false);
                        this.LostGamePanel.SetActive(false);
                        this.PauseGamePanel.SetActive(false);
                        this.WinGamePanel.SetActive(false);
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
        /// Adjust of the camera between the player, min
        /// </summary>
        public Vector3 CameraAdjustmentMin;

        /// <summary>
        /// Adjust of the camera between the player, max
        /// </summary>
        public Vector3 CameraAdjustmentMax;

        /// <summary>
        /// Main Camera
        /// </summary>
        public GameObject MainCamera;

        /// <summary>
        /// Always the time to go faster over time
        /// </summary>
        private float _TimeCurve = 0.0f;

        /// <summary>
        /// For the Obsticales ending, prevent wierd placement at ending
        /// </summary>
        public float EndPadding;

        /// <summary>
        /// For the obsticales starting, prevent placement at the start of the level
        /// </summary>
        public float StartPadding;

        /// <summary>
        /// Pause Game
        /// </summary>
        public bool PauseGame = false;

        /// <summary>
        /// Background movement
        /// </summary>
        public BackgroundMovement BackgroundMovement;

        /// <summary>
        /// Progression Bar
        /// </summary>
        public ProgressionBar ProgressionBar;

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
            AudioManager.Instance.CurrentSceneDisplay = SceneManager.GetSceneByBuildIndex(2).name;
            this.PlayerSpawned = false;
            this.ResetStats();
            this.CreateLevel();
        }

        /// <summary>
        /// Update every frame
        /// </summary>
        private void Update()
        {
            if (this.PlayerFollow != null)
            {
                //this.MainCamera.transform.position = this.PlayerFollow.transform.position + this.CameraAdjustmentMin;
                this.MainCamera.transform.position = new Vector3(this.PlayerFollow.transform.position.x + Mathf.Lerp(this.CameraAdjustmentMin.x, this.CameraAdjustmentMax.x, this.SpeedCurrentModifier / 100.0f), 
                                                                 this.CameraAdjustmentMin.y, 
                                                                 this.CameraAdjustmentMin.z);
                if(this.PlayerFollow.transform.position.x >= FileConfigHandler.Instance.UserConfig.LevelConfig.LevelLength)
                {
                    this.GameState = GameState.Win;
                }

            }
            if (!this._StartGame)
                return;
            if (this.GameState == GameState.Lose ||
                this.GameState == GameState.Win)
            {
                return;
            }
            this.SpeedMeterAdjustment();
            this.ProgressionBar.AdjustMarker(this.PlayerFollow.transform.position.x);
        }

        /// <summary>
        /// Restart Level
        /// </summary>
        public void RestartLevel()
        {
            this.ResetStats();
            this.PlayerSpawn(StartingTypes.Starting);
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
            this.FloorPivotPosition = Vector3.zero;
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
            this.FloorPivotPosition = new Vector3(-this.StartPadding, 0.0f, 0.0f);
            this.PivotPosition = new Vector3(this.StartPadding * 0.25f, 0.0f, 0.0f);
            //Adding ending padding for increased length
            while (this.FloorPivotPosition.x < FileConfigHandler.Instance.UserConfig.LevelConfig.LevelLength + EndPadding)
            {
                GameObject floor = (GameObject)Instantiate(AssetFactory.Instance.FloorSegment);
                floor.transform.position = this.FloorPivotPosition; 
                floor.GetComponent<SegmentLevel>().DataBind(ref this.FloorPivotPosition);
                this._CurrentListOfSegments.Add(floor);
            }
            //Minusing ending padding to reduce obsticales
            while(this.PivotPosition.x < FileConfigHandler.Instance.UserConfig.LevelConfig.LevelLength - this.EndPadding)
            {
                GameObject parkour = (GameObject)Instantiate(this.ParkourSegmentsList[Random.Range(0, this.ParkourSegmentsList.Length)]);
                parkour.transform.position = this.PivotPosition;
                parkour.GetComponent<Segment>().DataBind(ref this.PivotPosition.x, 10.0f);
                this.PivotPosition = new Vector3(this.PivotPosition.x + FileConfigHandler.Instance.UserConfig.LevelConfig.LevelGapLength, this.PivotPosition.y, this.PivotPosition.z);
                this._CurrentListOfSegments.Add(parkour);
            }
            this.PlayerSpawn(StartingTypes.Starting);
        }

        /// <summary>
        /// Spawning Player at location/location type
        /// </summary>
        public void PlayerSpawn(StartingTypes type)
        {
            switch(type)
            {
                case StartingTypes.Starting:
                    if (!this.PlayerSpawned)
                    {
                        this.PlayerSpawned = true;
                        GameObject playerObj = (GameObject)Instantiate(this.PlayerPreFab);
                        this.PlayerFollow = playerObj;
                    }
                    SegmentLevel segment = this._CurrentListOfSegments[0].GetComponent<SegmentLevel>();
                    this.Player.gameObject.transform.position = new Vector3(-this.StartPadding * 0.5f,
                                                                            (segment.FloorSegment.GetComponent<BoxCollider2D>().size.y * 0.5f) + (this.Player.RunningCollider.size.y * 0.5f) + (this.Player.RunningCollider.offset.y * -1.0f),
                                                                            0.0f);
                    break;
                default:
                    break;
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