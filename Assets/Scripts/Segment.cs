using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;

namespace Game.Level
{
    public class Segment : MonoBehaviour
    {
        /// <summary>
        /// Lsit of obstacles
        /// </summary>
        public ObsticleStacking[] ListofObstacles;

        /// <summary>
        /// Adjust pivot for the next obstacle
        /// </summary>
        private Vector3 _AdjustPivot;

        /// <summary>
        /// Where a player can spawn
        /// </summary>
        public GameObject SpawningPoint;

        /// <summary>
        /// Databind
        /// </summary>
        public void DataBind()
        {
            for (int i = 0; i < this.ListofObstacles.Length; i++)
            {
                if (this.ListofObstacles[i] == null)
                    continue;
                this._AdjustPivot = new Vector3(0.0f, this.GetComponent<BoxCollider2D>().size.y * 0.5f * (this.ListofObstacles[i].UpwardsTransition ? 1.0f : -1.0f));
                this._AdjustPivot += new Vector3(0.0f, this.ListofObstacles[i].GetComponent<BoxCollider2D>().size.y * 0.5f * (this.ListofObstacles[i].UpwardsTransition ? 1.0f : -1.0f));
                this.ListofObstacles[i].gameObject.transform.localPosition = new Vector3(this.ListofObstacles[i].gameObject.transform.localPosition.x, this._AdjustPivot.y, this.ListofObstacles[i].gameObject.transform.localPosition.z);
                this.ListofObstacles[i].DataBind();
            }

            if(this.SpawningPoint != null &&
               !Manager.Instance.PlayerSpawned)
            {
                Manager.Instance.PlayerSpawned = true;
                GameObject player = (GameObject)Instantiate(Manager.Instance.PlayerPreFab, this.SpawningPoint.transform.position, Quaternion.identity);
                Manager.Instance.PlayerFollow = player;
            }
        }
    }
}