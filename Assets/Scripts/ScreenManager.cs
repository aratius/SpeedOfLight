using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using OscJack;
using TMPro;

namespace Unity.Custom
{

  public struct ScreenData
  {
    public float tx;
    public float ty;
    public float tz;
    public float w;
    public float h;
    public float d;

    public ScreenData(
        float _tx,
        float _ty,
        float _tz,
        float _w,
        float _h,
        float _d
    )
    {
      tx = _tx;
      ty = _ty;
      tz = _tz;
      w = _w;
      h = _h;
      d = _d;
    }

  }

  public class ScreenManager : MonoBehaviour
  {

    [SerializeField] GameObject m_ScreenPrefab;
    [SerializeField] TrackerManager m_TrackerManager;
    List<Screen> m_ScreenList = new List<Screen>();
    List<ScreenData> m_ScreenDataList = new List<ScreenData>();
    Screen m_SelectedScreen;

    public int length => m_ScreenList.Count;

    public Screen Get(int index)
    {
      return m_ScreenList[index];
    }

    void Start()
    {
      OscReceiver.Instance.AddCallback("", CheckData);
    }

    void Update()
    {
      for (int i = 0; i < m_ScreenList.Count; i++)
      {
        Screen screen = m_ScreenList[i];
        ScreenData screenData = m_ScreenDataList[i];
        screen.UpdateInfo(
          new Vector3(screenData.w, screenData.h, screenData.d),
          new Vector3(screenData.tx, screenData.ty, screenData.tz)
        );
      }

      if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // 左クリック
      {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Rayを生成
        List<GameObject> hitObjects = new List<GameObject>();
        bool isHit = false;
        foreach (RaycastHit hit in Physics.RaycastAll(ray))
        {
          GameObject go = hit.collider.gameObject;
          if (go.tag == "ScreenBody") hitObjects.Add(go);
        }
        if (hitObjects.Count > 0)
        {
          GameObject closest = hitObjects[0];
          float distMin = Vector3.Distance(Camera.main.transform.position, closest.transform.position);
          foreach (GameObject hitObject in hitObjects)
          {
            float dist = Vector3.Distance(Camera.main.transform.position, closest.transform.position);
            if (dist < distMin)
            {
              distMin = dist;
              closest = hitObject;
            }
          }
          if (m_SelectedScreen != null)
          {
            m_SelectedScreen.SetOutline(m_SelectedScreen.cameraEnabled ? OutlineType.Camera : OutlineType.Null);
          }
          m_SelectedScreen = closest.transform.parent.gameObject.GetComponent<Screen>();
          m_SelectedScreen.SetOutline(OutlineType.Focus);
        }
        else
        {
          if (m_SelectedScreen != null)
          {
            m_SelectedScreen.SetOutline(m_SelectedScreen.cameraEnabled ? OutlineType.Camera : OutlineType.Null);
          }
          m_SelectedScreen = null;
        }
      }

      int applyDisplayIndex = -999;
      if (Input.GetKeyDown(KeyCode.Alpha0)) applyDisplayIndex = 0;
      else if (Input.GetKeyDown(KeyCode.Alpha1)) applyDisplayIndex = 1;
      else if (Input.GetKeyDown(KeyCode.Alpha2)) applyDisplayIndex = 2;
      else if (Input.GetKeyDown(KeyCode.Alpha3)) applyDisplayIndex = 3;
      else if (Input.GetKeyDown(KeyCode.Alpha4)) applyDisplayIndex = 4;
      else if (Input.GetKeyDown(KeyCode.Alpha5)) applyDisplayIndex = 5;
      else if (Input.GetKeyDown(KeyCode.Alpha6)) applyDisplayIndex = 6;
      else if (Input.GetKeyDown(KeyCode.Alpha7)) applyDisplayIndex = 7;
      else if (Input.GetKeyDown(KeyCode.Alpha8)) applyDisplayIndex = 8;
      if (applyDisplayIndex != -999 && m_SelectedScreen != null)
      {
        m_SelectedScreen.SetCamera(applyDisplayIndex);
        m_SelectedScreen.SetOutline(OutlineType.Camera);
      }

    }

    async void CheckData(string address, OscDataHandle data)
    {
      if (data.GetElementCount() == 0) return;


      // NOTE: アドレスチェック
      Regex regIsScreen = new Regex("/screen[0-9]{1,2}.*");  // TODO: 仕様見てちゃんとする
      if (!regIsScreen.IsMatch(address)) return;

      // Debug.Log($"Received a screen data. data cnt : {data.GetElementCount()}");

      int id = -999;
      // TODO: インデックス取得
      Match matchId = Regex.Match(address, "[0-9]{1,2}");
      if (matchId.Success)
      {
        try
        {
          id = Int32.Parse(matchId.Value);
        }
        catch (FormatException e)
        {
          Console.WriteLine(e.Message);
        }
      }

      int index = id - 1;

      if (index >= 0)
      {

        if (index > m_ScreenDataList.Count - 1) await FillScreens(index + 1);

        ScreenData screenData = m_ScreenDataList[index];

        // TODO: tx, ty, tz取得
        Regex regTx = new Regex("/screen[0-9]{1,2}:tx");
        Regex regTy = new Regex("/screen[0-9]{1,2}:ty");
        Regex regTz = new Regex("/screen[0-9]{1,2}:tz");
        Regex regW = new Regex("/screen[0-9]{1,2}:w");
        Regex regH = new Regex("/screen[0-9]{1,2}:h");
        Regex regD = new Regex("/screen[0-9]{1,2}:d");

        if (regTx.IsMatch(address)) screenData.tx = data.GetElementAsFloat(0);
        else if (regTy.IsMatch(address)) screenData.ty = data.GetElementAsFloat(0);
        else if (regTz.IsMatch(address)) screenData.tz = -data.GetElementAsFloat(0);  // NOTE: OpenVRと座標系そのものが逆だと思う
        else if (regW.IsMatch(address)) screenData.w = data.GetElementAsFloat(0);
        else if (regH.IsMatch(address)) screenData.h = data.GetElementAsFloat(0);
        else if (regD.IsMatch(address)) screenData.d = data.GetElementAsFloat(0);

        m_ScreenDataList[index] = screenData;
      }

    }

    async UniTask FillScreens(int length)
    {
      await UniTask.WaitForFixedUpdate();

      for (int i = 0; i < length; i++)
      {
        // new collection
        if (i > m_ScreenDataList.Count - 1)
        {
          m_ScreenDataList.Add(new ScreenData(0, 0, 0, 0, 0, 0));
          GameObject screenGO = Instantiate(m_ScreenPrefab, this.transform);
          screenGO.name = $"Screen_{i}";
          Screen screen = screenGO.GetComponent<Screen>();
          ScreenData screenData = m_ScreenDataList[i];
          screen.Init(
            screenGO.name,
            new Vector3(screenData.w, screenData.h, screenData.d),
            m_TrackerManager.Get(i).gameObject,
            new Vector3(screenData.tx, screenData.ty, screenData.tz)
          );
          m_ScreenList.Add(screen);
        }
      }
    }

  }

}
