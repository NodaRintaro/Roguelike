using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 区域分割法によるMap自動生成
/// </summary>
public class MapGenerater : MonoBehaviour
{
    //マップ全体の大きさを決める
    [SerializeField,Header("マップ全体の横幅")] public int _xLength = 50;
    [SerializeField,Header("マップ全体の縦幅")] public int _zLength = 50;
    [SerializeField,Header("作るエリアの数")] int _areaNum = 4;

    //部屋の大きさの決めるための範囲
    [SerializeField,Header("生成する部屋の大きさの最小値")] int _roomSizeMin = 5;
    [SerializeField,Header("生成する部屋の大きさの最大値")] int _roomSizeMax = 10;
    [SerializeField,Header("生成するエリア大きさの最小値")] int _areaSizeMin = 7;

    [SerializeField, Header("エリアを分割する時の初期の分割し始める初期の中心座標")]
    (int x, int z) _startPos = (1, 1);

    //それぞれのエリアと部屋の大きさのデータ
    Dictionary<string, (int xMin, int xMax, int zMin, int zMax)> _areaData 
        = new Dictionary<string, (int xMin, int xMax, int zMin, int zMax)>();

    Dictionary<string, (int xMin, int xMax, int zMin, int zMax)> _roomData 
        = new Dictionary<string, (int xMin, int xMax, int zMin, int zMax)>();

    //部屋を生成し始める中心座標
    private int _randomRoomPosX;
    private int _randomRoomPosZ;

    //部屋の横と縦の大きさ
    private int _randomRoomSizeX;
    private int _randomRoomSizeY;

    //分割した時のエリアの大きさ
    private int _areaSizeX;
    private int _areaSizeZ;

    //区画ごとのKeyの名前
    private string _a = "A";
    private string _b = "B";

    //最終的に出来上がった区画のKeyのみを保存するList
    private List<string> _keyList = new List<string>();

    //エリアを分割するランダムな座標
    private int _randomDividePos;

    //一番大きいエリア
    private string _wideArea = null;

    public void Start()
    {
        MapGenerate();
    }

    public void MapGenerate()
    {
        for(int i = 0; i < _areaNum; i++)
        {
            if(i == 0)
            {
                _randomDividePos = Random.Range(_startPos.x + _areaSizeMin, _xLength - _areaSizeMin);

                _areaData.Add(_a, (_startPos.x, _randomDividePos - 1, _startPos.z, _zLength));
                _areaData.Add(_b, (_randomDividePos + 1, _xLength, _startPos.z, _zLength));

                _keyList.Add(_a);
                _keyList.Add(_b);

                Debug.Log("エリア" + _a + "の座標:" + _areaData[_a]);
                Debug.Log("エリア" + _b + "の座標:" + _areaData[_b]);
            }//最初のエリアAとBを作る
            else
            {
                _wideArea = null;
                foreach(string key in _keyList)
                {
                    if(_wideArea == null)
                    {
                        _wideArea = key;
                    }
                    else
                    {
                        if (_areaData[_wideArea].xMax - _areaData[_wideArea].xMin * _areaData[_wideArea].zMax - _areaData[_wideArea].zMin <
                            _areaData[key].xMax - _areaData[key].xMin * _areaData[key].zMax - _areaData[key].zMin)
                        {
                            _wideArea = key;
                        }
                    }
                }
                if (_areaData[_wideArea].xMax - _areaData[_wideArea].xMin > _areaData[_wideArea].zMax - _areaData[_wideArea].zMin)
                {
                    //エリアを分割する座標を決める
                    _randomDividePos = Random.Range(_areaData[_wideArea].xMin + _areaSizeMin, _areaData[_wideArea].xMax - _areaSizeMin);

                    //
                    _areaData.Add(_wideArea + _a, (_areaData[_wideArea].xMin, _randomDividePos - 1, _areaData[_wideArea].zMin, _areaData[_wideArea].zMax));
                    _areaData.Add(_wideArea + _b, (_randomDividePos + 1, _areaData[_wideArea].xMax, _areaData[_wideArea].zMin, _areaData[_wideArea].zMax));

                    _keyList.Remove(_wideArea);
                    _keyList.Add(_wideArea + _a);
                    _keyList.Add(_wideArea + _b);

                    Debug.Log("エリア" + _wideArea + _a + "の座標:" + _areaData[_wideArea + _a]);
                    Debug.Log("エリア" + _wideArea + _b + "の座標:" + _areaData[_wideArea + _b]);
                }
                else
                {
                    //エリアを分割する座標を決める
                    _randomDividePos = Random.Range(_areaData[_wideArea].zMin + _areaSizeMin, _areaData[_wideArea].zMax - _areaSizeMin);

                    _areaData.Add(_wideArea + _a,_areaData[_wideArea].xMin, _areaData[_wideArea].xMax, _areaData[_wideArea].zMin,)
                }
            }//最初のエリアAとBをもとに子エリアを増やす

        }
    }
}
