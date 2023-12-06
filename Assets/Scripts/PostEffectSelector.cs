using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;
using Cysharp.Threading.Tasks;

namespace Unity.Custom
{
    public class PostEffectSelector : MonoBehaviour
    {
        private int m_Index = -1;
        [SerializeField] PostEffect[] m_PostEffects;

        // Start is called before the first frame update
        void Start()
        {
            OscReceiver.Instance.AddCallback("/posteffectId", CheckData);
        }

        async void CheckData(string address, OscDataHandle data)
        {
            
            int index = data.GetElementAsInt(0);
            int count = data.GetElementCount();
            m_Index = index;
            await UniTask.Delay(1000);

            Debug.Log($"post effect {count} {m_Index}");
            for(int i = 0; i < m_PostEffects.Length; i++) {
                m_PostEffects[i].enabled = i == m_Index;
            }
        }
    }

}