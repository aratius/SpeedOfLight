using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using OscJack;

namespace Unity.Custom
{

    public struct TrackerData
    {
        public float tx;
        public float ty;
        public float tz;
        public float rx;
        public float ry;
        public float rz;

        public TrackerData(
            float _tx,
            float _ty,
            float _tz,
            float _rx,
            float _ry,
            float _rz
        )
        {
            tx = _tx;
            ty = _ty;
            tz = _tz;
            rx = _rx;
            ry = _ry;
            rz = _rz;
        }

    }

    public class TrackerManager : MonoBehaviour
    {

        [SerializeField] GameObject m_TrackerPrefab;
        List<Tracker> m_TrackerList = new List<Tracker>();
        List<TrackerData> m_TrackerDataList = new List<TrackerData>();

        void Start()
        {
            OscReceiver.Instance.AddCallback("", CheckData);
        }

        void Update()
        {
            for(int i = 0; i < m_TrackerList.Count; i++)
            {
                Tracker tracker = m_TrackerList[i];
                TrackerData trackerData = m_TrackerDataList[i];
                tracker.transform.localPosition = new Vector3(trackerData.tx, trackerData.ty, trackerData.tz);
                tracker.transform.localRotation = Quaternion.Euler(new Vector3(trackerData.rx, trackerData.ry, trackerData.rz));
            }
        }

        void CheckData(string address, OscDataHandle data)
        {
            if(data.GetElementCount() == 0) return;

            // NOTE: アドレスチェック
            Regex regIsTracker = new Regex("tracker[0-9]{0,1}.*");  // TODO: 仕様見てちゃんとする
            if(!regIsTracker.IsMatch(address)) return;

            Debug.Log($"Received a tracker data. data cnt : {data.GetElementCount()}");

            int id = -999;
            // TODO: インデックス取得
            Match matchId = Regex.Match(address, "[0-9]{0,1}");
            if(matchId.Success) {
                try
                {
                    id = Int32.Parse(matchId.Value);
                    Debug.Log($"id : {id}");
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }

            if(id >= 0)
            {
                if(id > m_TrackerDataList.Count - 1) FillTrackers(id + 1);

                TrackerData trackerData = m_TrackerDataList[id];

                // TODO: tx, ty, tz取得
                Regex regTx = new Regex("tracker[0-9]{0,1}:tx");  // TODO: 仕様見てちゃんとする
                Regex regTy = new Regex("tracker[0-9]{0,1}:ty");  // TODO: 仕様見てちゃんとする
                Regex regTz = new Regex("tracker[0-9]{0,1}:ty");  // TODO: 仕様見てちゃんとする
                Regex regRx = new Regex("tracker[0-9]{0,1}:rx");  // TODO: 仕様見てちゃんとする
                Regex regRy = new Regex("tracker[0-9]{0,1}:ry");  // TODO: 仕様見てちゃんとする
                Regex regRz = new Regex("tracker[0-9]{0,1}:ry");  // TODO: 仕様見てちゃんとする

                if(regTx.IsMatch(address)) trackerData.tx = data.GetElementAsFloat(0);
                if(regTy.IsMatch(address)) trackerData.ty = data.GetElementAsFloat(0);
                if(regTz.IsMatch(address)) trackerData.tz = data.GetElementAsFloat(0);
                if(regRx.IsMatch(address)) trackerData.rx = data.GetElementAsFloat(0);
                if(regRy.IsMatch(address)) trackerData.ry = data.GetElementAsFloat(0);
                if(regRz.IsMatch(address)) trackerData.rz = data.GetElementAsFloat(0);

            }

        }

        void FillTrackers(int length)
        {
            for(int i = 0; i < length; i++)
            {
                // new collection
                if(i > m_TrackerDataList.Count - 1) {
                    m_TrackerDataList.Add(new TrackerData(0,0,0,0,0,0));
                    GameObject trackerGO = Instantiate(m_TrackerPrefab, this.transform);
                    Tracker tracker = trackerGO.GetComponent<Tracker>();
                    m_TrackerList.Add(tracker);
                }
            }
        }

    }

}
