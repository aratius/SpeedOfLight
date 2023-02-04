using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using OscJack;
using Unity.Custom;

namespace Unity.Custom
{

    public class AreaDebugger : MonoBehaviour
    {

        // 4 corners
        Vector3[] m_Corners = new [] {Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};

        void Start()
        {
            OscReceiver.Instance.AddCallback("", CheckData);
        }

        void FixedUpdate()
        {
            for(int i = 0; i < m_Corners.Length; i++)
            {
                Vector3 corner = m_Corners[i];
                Vector3 nextCorner = i + 1 < m_Corners.Length ? m_Corners[i + 1] : m_Corners[0];
                Debug.DrawLine(corner, nextCorner, Color.blue);
                Debug.Log($"c{i} : {corner}, c{i+1} : {nextCorner}");
            }
        }

        void CheckData(string address, OscDataHandle data)
        {
            if(data.GetElementCount() == 0) return;

            // NOTE: アドレスチェック
            Regex regIsCorner = new Regex("/playarea_corner[0-9]{1,2}.*");  // TODO: 仕様見てちゃんとする
            if(!regIsCorner.IsMatch(address)) return;

            int id = -999;

            Match matchId = Regex.Match(address, "[0-9]");
            if(matchId.Success) {
                try
                {
                    id = Int32.Parse(matchId.Value);
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"{e.Message}, {matchId.Value}");
                }
            }

            int index = id - 1;

            if(index >= 0 && index < m_Corners.Length)
            {
                // TODO: tx, ty, tz取得
                Regex regTx = new Regex("/playarea_corner[0-9]{1,2}:tx");  // TODO: 仕様見てちゃんとする
                Regex regTy = new Regex("/playarea_corner[0-9]{1,2}:ty");  // TODO: 仕様見てちゃんとする
                Regex regTz = new Regex("/playarea_corner[0-9]{1,2}:tz");  // TODO: 仕様見てちゃんとする

                if(regTx.IsMatch(address)) m_Corners[index].x = data.GetElementAsFloat(0);
                else if(regTy.IsMatch(address)) m_Corners[index].y = data.GetElementAsFloat(0);
                else if(regTz.IsMatch(address)) m_Corners[index].z = data.GetElementAsFloat(0);

                // Debug.Log($"corner{id} : {corner}");
            }
        }

    }

}