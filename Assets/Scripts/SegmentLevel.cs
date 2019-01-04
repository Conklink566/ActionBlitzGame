using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;

namespace Game.Level
{
    public class SegmentLevel : MonoBehaviour
    {
        /// <summary>
        /// List of Segments
        /// </summary>
        public GameObject FloorSegment;

        /// <summary>
        /// Adjust pivot for the next obstacle
        /// </summary>
        private Vector3 _AdjustPivot;

        /// <summary>
        /// Data Bind
        /// </summary>
        public void DataBind(ref Vector3 position)
        {
            this._AdjustPivot += new Vector3(this.FloorSegment.GetComponent<BoxCollider2D>().size.x * 0.5f, 0.0f);
            this.FloorSegment.transform.localPosition = new Vector3(this._AdjustPivot.x, 0.0f, this.FloorSegment.transform.localPosition.z);
            this._AdjustPivot += new Vector3(this.FloorSegment.GetComponent<BoxCollider2D>().size.x * 0.5f, 0.0f);
            position += new Vector3(this.FloorSegment.GetComponent<BoxCollider2D>().size.x, position.y);
        }
    }
}