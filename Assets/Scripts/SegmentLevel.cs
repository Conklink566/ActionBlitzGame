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
        public Segment[] ListOfSegments;

        /// <summary>
        /// Adjust pivot for the next obstacle
        /// </summary>
        private Vector3 _AdjustPivot;

        /// <summary>
        /// Data Bind
        /// </summary>
        public void DataBind()
        {
            for (int i = 0; i < this.ListOfSegments.Length; i++)
            {
                if (this.ListOfSegments[i] == null)
                    continue;
                this._AdjustPivot += new Vector3(this.ListOfSegments[i].GetComponent<BoxCollider2D>().size.x * 0.5f, 0.0f);
                this.ListOfSegments[i].transform.localPosition = new Vector3(this._AdjustPivot.x, 0.0f, this.ListOfSegments[i].transform.localPosition.z);
                this._AdjustPivot += new Vector3(this.ListOfSegments[i].GetComponent<BoxCollider2D>().size.x * 0.5f, 0.0f);
                Manager.Instance.PivotPosition += new Vector3(this.ListOfSegments[i].GetComponent<BoxCollider2D>().size.x, Manager.Instance.PivotPosition.y);
                this.ListOfSegments[i].GetComponent<Segment>().DataBind();
            }
            Manager.Instance.CheckPointList.Add(Manager.Instance.PivotPosition.x);
        }
    }
}