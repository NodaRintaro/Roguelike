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

    [SerializeField] int _gridSize = 1;

    [SerializeField, Header("エリアを分割する時の初期の分割し始める初期の中心座標")]
    (int x, int z) _startPos = (1, 1);

    [SerializeField, Header("部屋の床となるオブジェクト")]
    private GameObject _roomTile;

    //それぞれのエリアと部屋の大きさのデータ
    private Dictionary<string, (int xMin, int xMax, int zMin, int zMax)> _areaData 
        = new Dictionary<string, (int xMin, int xMax, int zMin, int zMax)>();

    private Dictionary<string, (int xMin, int xMax, int zMin, int zMax)> _roomData
        = new Dictionary<string, (int xMin, int xMax, int zMin, int zMax)>();


    //道を作る際に必要となる境界線と道同士をつなげる始点のデータ
    private Dictionary<string, (int startPointX, int startPointZ, int goalPointX, int goalPointZ)> _loadData
        = new Dictionary<string, (int startPointX, int startPointZ, int goalPointX, int goalPointZ)>();

    private List<(int xPoint, int zPoint)> _linkPoint = new List<(int xPoint, int zPoint)>();

    //区画ごとのKeyの名前
    private string _a = "A";
    private string _b = "B";

    //エリアの分割線につける
    private string _road = "R";

    //最終的に出来上がった区画のKeyのみを保存するList
    private List<string> _keyList = new List<string>();
    private List<(int x,int z)> _roadLinkPoint = new List<(int x,int z)>();

    //ランダムな座標
    private int _randomPos;

    //一番大きいエリア
    private string _wideArea = null;

    //部屋を作る際のランダムな最小座標と最大座標
    private int _randomRoomSizeMinX;
    private int _randomRoomSizeMaxX;
    private int _randomRoomSizeMinZ;
    private int _randomRoomSizeMaxZ;

    //生成する道の本数
    private int _randomRoadNum;

    //ランダムな数値
    private int _randomNum;

    public void Start()
    {
        MapGenerate();
    }

    public void MapGenerate()
    {
        //Dataの初期化
        _roomData = new Dictionary<string, (int xMin, int xMax, int zMin, int zMax)>();
        _areaData = new Dictionary<string, (int xMin, int xMax, int zMin, int zMax)>();
        _keyList = new List<string>();

        //エリアの作成
        AreaCreate();

        //分割したエリアに部屋を生成する
        RoomCreate();

        //道を作る
        LoadCreate();

        //todo:作った道同士をつなげる
    }

    private void AreaCreate()
    {
        //エリアを分割する
        for (int i = 1; i < _areaNum; i++)
        {
            //最初のエリアAとBを作る
            if (i == 1)
            {
                _randomPos = Random.Range(_startPos.x + _areaSizeMin, _xLength - _areaSizeMin);

                _areaData.Add(_a, (_startPos.x, _randomPos - 1, _startPos.z, _zLength));
                _areaData.Add(_b, (_randomPos + 1, _xLength, _startPos.z, _zLength));
                _loadData.Add("firstDivide", (_randomPos, _startPos.z, _randomPos, _zLength));

                _keyList.Add(_a);
                _keyList.Add(_b);

                Debug.Log("エリア" + _a + "の座標:" + _areaData[_a]);
                Debug.Log("エリア" + _b + "の座標:" + _areaData[_b]);

            }

            //エリアが複数存在する場合ひとつのエリアを選択して分割
            else
            {
                _wideArea = null;

                //分割する一番大きなエリアを選択
                foreach (string key in _keyList)
                {
                    if (_wideArea == null)
                    {
                        _wideArea = key;
                    }
                    else if (_wideArea != null)
                    {
                        if ((_areaData[_wideArea].xMax - _areaData[_wideArea].xMin) * (_areaData[_wideArea].zMax - _areaData[_wideArea].zMin) <
                            (_areaData[key].xMax - _areaData[key].xMin) * (_areaData[key].zMax - _areaData[key].zMin))
                        {
                            _wideArea = key;
                        }
                    }
                }
                Debug.Log(_wideArea + "を分割します");

                //_wideAreaがXじくに大きければたてにYじくのに大きければ横に分割する
                if (_areaData[_wideArea].xMax - _areaData[_wideArea].xMin > _areaData[_wideArea].zMax - _areaData[_wideArea].zMin)
                {
                    _randomPos = Random.Range(_areaData[_wideArea].xMin + _areaSizeMin, _areaData[_wideArea].xMax - _areaSizeMin);

                    _areaData.Add(_wideArea + _a, (_areaData[_wideArea].xMin, _randomPos - 1, _areaData[_wideArea].zMin, _areaData[_wideArea].zMax));
                    _areaData.Add(_wideArea + _b, (_randomPos + 1, _areaData[_wideArea].xMax, _areaData[_wideArea].zMin, _areaData[_wideArea].zMax));
                    _loadData.Add(_wideArea, (_randomPos, _areaData[_wideArea].zMin , _randomPos, _areaData[_wideArea].zMax));

                    Debug.Log("エリア" + _wideArea + _a + "の座標:" + _areaData[_wideArea + _a]);
                    Debug.Log("エリア" + _wideArea + _b + "の座標:" + _areaData[_wideArea + _b]);

                    _keyList.Remove(_wideArea);
                    _keyList.Add(_wideArea + _a);
                    _keyList.Add(_wideArea + _b);
                }
                else
                {
                    _randomPos = Random.Range(_areaData[_wideArea].zMin + _areaSizeMin, _areaData[_wideArea].zMax - _areaSizeMin);

                    _areaData.Add(_wideArea + _a, (_areaData[_wideArea].xMin, _areaData[_wideArea].xMax, _areaData[_wideArea].zMin, _randomPos - 1));
                    _areaData.Add(_wideArea + _b, (_areaData[_wideArea].xMin, _areaData[_wideArea].xMax, _randomPos + 1, _areaData[_wideArea].zMax));
                    _loadData.Add(_wideArea, (_areaData[_wideArea].xMin, _randomPos, _areaData[_wideArea].xMax, _randomPos));

                    Debug.Log("エリア" + _wideArea + _a + "の座標:" + _areaData[_wideArea + _a]);
                    Debug.Log("エリア" + _wideArea + _b + "の座標:" + _areaData[_wideArea + _b]);

                    _keyList.Remove(_wideArea);
                    _keyList.Add(_wideArea + _a);
                    _keyList.Add(_wideArea + _b);
                }
            }
        }
    }

    private void RoomCreate()
    {
        //部屋を作る
        foreach (var key in _keyList)
        {
            //ランダムに部屋の大きさを決める 
            _randomRoomSizeMinX = Random.Range(_areaData[key].xMin, _areaData[key].xMin + ((_areaData[key].xMax - _areaData[key].xMin) / 2) - _roomSizeMin) + 1;
            _randomRoomSizeMaxX = Random.Range(_areaData[key].xMin + ((_areaData[key].xMax - _areaData[key].xMin) / 2), _areaData[key].xMax) - 1;
            _randomRoomSizeMinZ = Random.Range(_areaData[key].zMin, _areaData[key].zMin + ((_areaData[key].zMax - _areaData[key].zMin) / 2) - _roomSizeMin) + 1;
            _randomRoomSizeMaxZ = Random.Range(_areaData[key].zMin + ((_areaData[key].zMax - _areaData[key].zMin) / 2), _areaData[key].zMax) - 1;

            //上記で決めた大きさをもとに床となるオブジェクトを生成する
            for (int i = _randomRoomSizeMinX; _randomRoomSizeMaxX >= i; i++)
            {
                for (int j = _randomRoomSizeMinZ; _randomRoomSizeMaxZ >= j; j++)
                {
                    Instantiate(_roomTile, new Vector3(i * _gridSize, 0, j * _gridSize), Quaternion.identity);
                }
            }

            //部屋のDataを保存する
            _roomData.Add(key, (_randomRoomSizeMinX, _randomRoomSizeMaxX, _randomRoomSizeMinZ, _randomRoomSizeMaxZ));
        }
    }

    private void LoadCreate()
    {
        //通路を作る
        foreach(var key in _keyList)
        {
            //エリアに隣接している通路
            var loadkey = key.Remove(key.Length - 1);

            //たてに分割している場合
            if (_loadData[loadkey].startPointX == _loadData[loadkey].goalPointX)
            {
                //通路を生成し始めるランダムなZ座標
                _randomPos = Random.Range(_roomData[key].zMin, _roomData[key].zMax);

                if (_areaData[key].xMax < _loadData[loadkey].startPointX)
                {
                    for(int i = _roomData[key].xMax; i <= _loadData[loadkey].startPointX; i++)
                    {
                        Instantiate(_roomTile,new Vector3(i * _gridSize, 0, _randomPos * _gridSize), Quaternion.identity);
                        if(i == _loadData[loadkey].startPointX)
                        {
                            _linkPoint.Add((i, _randomPos));
                        }
                    }
                }
                else
                {
                    for (int i = _roomData[key].xMin; i >= _loadData[loadkey].startPointX; i--)
                    {
                        Instantiate(_roomTile, new Vector3(i * _gridSize, 0, _randomPos * _gridSize), Quaternion.identity);
                        if (i == _loadData[loadkey].startPointX)
                        {
                            _linkPoint.Add((i, _randomPos));
                        }
                    }
                }
                Debug.Log(key.Remove(key.Length - 1));
            }
            //横に分割している場合
            else if(_loadData[loadkey].startPointZ == _loadData[loadkey].goalPointZ)
            {
                //通路を生成し始めるランダムなZ座標
                _randomPos = Random.Range(_roomData[key].xMin, _roomData[key].xMax);

                if (_areaData[key].zMax < _loadData[loadkey].startPointZ)
                {
                    for (int i = _roomData[key].zMax; i <= _loadData[loadkey].startPointZ; i++)
                    {
                        Instantiate(_roomTile, new Vector3(_randomPos * _gridSize, 0, i * _gridSize), Quaternion.identity);
                        if (i == _loadData[loadkey].startPointZ)
                        {
                            _linkPoint.Add((_randomPos, i));
                        }
                    }
                }
                else
                {
                    for (int i = _roomData[key].zMin; i >= _loadData[loadkey].startPointZ; i--)
                    {
                        Instantiate(_roomTile, new Vector3(_randomPos * _gridSize, 0, i * _gridSize), Quaternion.identity);
                        if (i == _loadData[loadkey].startPointZ)
                        {
                            _linkPoint.Add((_randomPos, i));
                        }
                    }
                }
                Debug.Log(key.Remove(key.Length - 1));
            }
        }

        foreach(var key in _loadData.Keys)
        {

        }
    }
}
