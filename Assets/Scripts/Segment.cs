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
        /// If there should be an inBetween or not
        /// </summary>
        public bool InBetweenValueSet;

        /// <summary>
        /// Databind
        /// </summary>
        public void DataBind(ref float position, float inBetween)
        {
            this._AdjustPivot = Vector3.zero;
            for (int i = 0; i < this.ListofObstacles.Length; i++)
            {
                if (this.ListofObstacles[i] == null)
                    continue;
                this._AdjustPivot = new Vector3(this._AdjustPivot.x, ((AssetFactory.Instance.FloorSegment.GetComponentInChildren<BoxCollider2D>().size.y * 0.5f) + (this.ListofObstacles[i].GetComponent<BoxCollider2D>().size.y * 0.5f)) * (this.ListofObstacles[i].UpwardsTransition ? 1.0f : -1.0f), 0.0f);
                this.ListofObstacles[i].gameObject.transform.localPosition = this._AdjustPivot;
                this._AdjustPivot += new Vector3(this.ListofObstacles[i].GetComponent<BoxCollider2D>().size.x + (this.InBetweenValueSet ? inBetween : 0.0f), this.ListofObstacles[i].GetComponent<BoxCollider2D>().size.y * 0.5f * (this.ListofObstacles[i].UpwardsTransition ? 1.0f : -1.0f), 0.0f);
                this.ListofObstacles[i].DataBind();

                position += this.ListofObstacles[i].GetComponent<BoxCollider2D>().size.x + (this.InBetweenValueSet ? inBetween : 0.0f);
            }
        }
    }
}