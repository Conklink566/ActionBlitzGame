using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level
{
    public class ObsticleStacking : MonoBehaviour
    {
        /// <summary>
        /// List of obstacles that are stacked on this object
        /// </summary>
        public GameObject[] StackedObstacles;

        /// <summary>
        /// Adjust pivot for the next obstacle
        /// </summary>
        private Vector3 _AdjustPivot;

        /// <summary>
        /// Upwards transition
        /// </summary>
        public bool UpwardsTransition;

        /// <summary>
        /// Databind
        /// </summary>
        public void DataBind()
        {
            this._AdjustPivot += new Vector3(0.0f, this.GetComponent<BoxCollider2D>().size.y * 0.5f * (this.UpwardsTransition ? 1.0f : -1.0f));
            for(int i = 0; i < this.StackedObstacles.Length; i++)
            {
                if (this.StackedObstacles[i] == null)
                    continue;
                this._AdjustPivot += new Vector3(0.0f, this.StackedObstacles[i].GetComponent<BoxCollider2D>().size.y * 0.5f * (this.UpwardsTransition ? 1.0f : -1.0f));
                this.StackedObstacles[i].transform.localPosition = new Vector3(0.0f, this._AdjustPivot.y, this.StackedObstacles[i].transform.localPosition.z);
                this._AdjustPivot += new Vector3(0.0f, this.StackedObstacles[i].GetComponent<BoxCollider2D>().size.y * 0.5f * (this.UpwardsTransition ? 1.0f : -1.0f));
            }
        }
    }
}