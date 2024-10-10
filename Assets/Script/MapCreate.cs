using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 区域分割法によるMap自動生成
/// </summary>
public class MapCreate : MonoBehaviour
{
    public struct PosData
    {
        public int xMinPos;
        public int zMinPos;
        public int xMaxPos;
        public int zMaxPos;
    }

    [SerializeField,Header("1つのグリッドの大きさ")] private int _gridSize;

    public int GridSize => _gridSize;

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

    [SerializeField, Header("部屋の床となるオブジェクト")]
    private GameObject _roomTile;

    //それぞれのエリアと部屋の大きさのデータ
    private Dictionary<string, PosData> _areaData 
        = new Dictionary<string, PosData>();

    private Dictionary<string, PosData> _roomData
        = new Dictionary<string, PosData>();

    public Dictionary<string, PosData> AreaData => _areaData;

    public Dictionary<string, PosData> RoomData => _roomData;

    //道を作る際に必要となる境界線のデータ
    private Dictionary<string, PosData> _dividePosData
        = new Dictionary<string, PosData>();

    private List<(int xPoint, int zPoint)> _linkPoint = new List<(int xPoint, int zPoint)>();

    //区画ごとのKeyの名前
    private string _a = "A";
    private string _b = "B";
    private string _fKey = "firstKey";

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

    public void Start()
    {
        MapGenerate();
    }

    public void MapGenerate()
    {
        //Dataの初期化
        _roomData = new Dictionary<string, PosData>();
        _areaData = new Dictionary<string, PosData>();
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

                _areaData.Add(_a,
                    new PosData
                    {
                        xMinPos = _startPos.x,
                        xMaxPos = _randomPos - 1, 
                        zMinPos = _startPos.z, 
                        zMaxPos = _zLength 
                    });

                _areaData.Add(_b,
                    new PosData
                    {
                        xMinPos = _randomPos + 1,
                        xMaxPos = _xLength,
                        zMinPos = _startPos.z,
                        zMaxPos = _zLength
                    });

                _dividePosData.Add(_fKey,
                    new PosData
                    {
                        xMinPos = _randomPos,
                        xMaxPos = _randomPos,
                        zMinPos = _startPos.z,
                        zMaxPos = _zLength
                    });

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
                        if ((_areaData[_wideArea].xMaxPos - _areaData[_wideArea].xMinPos) * (_areaData[_wideArea].zMaxPos - _areaData[_wideArea].zMinPos) <
                            (_areaData[key].xMaxPos - _areaData[key].xMinPos) * (_areaData[key].zMaxPos - _areaData[key].zMinPos))
                        {
                            _wideArea = key;
                        }
                    }
                }
                Debug.Log(_wideArea + "を分割します");

                //_wideAreaがXじくに大きければたてにYじくのに大きければ横に分割する
                if (_areaData[_wideArea].xMaxPos - _areaData[_wideArea].xMinPos > _areaData[_wideArea].zMaxPos - _areaData[_wideArea].zMinPos)
                {
                    _randomPos = Random.Range(_areaData[_wideArea].xMinPos + _areaSizeMin, _areaData[_wideArea].xMaxPos - _areaSizeMin);

                    _areaData.Add(_wideArea + _a,
                        new PosData
                        {
                            xMinPos = _areaData[_wideArea].xMinPos,
                            xMaxPos = _randomPos - 1,
                            zMinPos = _areaData[_wideArea].zMinPos,
                            zMaxPos = _areaData[_wideArea].zMaxPos
                        });
                    
                    _areaData.Add(_wideArea + _b,
                        new PosData
                        {
                            xMinPos = _randomPos + 1,
                            xMaxPos = _areaData[_wideArea].xMaxPos,
                            zMinPos = _areaData[_wideArea].zMinPos,
                            zMaxPos = _areaData[_wideArea].zMaxPos
                        });


                    _dividePosData.Add(_wideArea,
                        new PosData
                        {
                            xMinPos = _randomPos,
                            xMaxPos = _randomPos,
                            zMinPos = _areaData[_wideArea].zMinPos,
                            zMaxPos = _areaData[_wideArea].zMaxPos
                        });

                    Debug.Log("エリア" + _wideArea + _a + "の座標:" + _areaData[_wideArea + _a]);
                    Debug.Log("エリア" + _wideArea + _b + "の座標:" + _areaData[_wideArea + _b]);

                    _keyList.Remove(_wideArea);
                    _keyList.Add(_wideArea + _a);
                    _keyList.Add(_wideArea + _b);
                }

                else
                {
                    _randomPos = Random.Range(_areaData[_wideArea].zMinPos + _areaSizeMin, _areaData[_wideArea].zMaxPos - _areaSizeMin);

                    _areaData.Add(_wideArea + _a,
                        new PosData
                        {
                            xMinPos = _areaData[_wideArea].xMinPos,
                            xMaxPos = _areaData[_wideArea].xMaxPos,
                            zMinPos = _areaData[_wideArea].zMinPos,
                            zMaxPos = _randomPos - 1
                        });

                    _areaData.Add(_wideArea + _b,
                        new PosData
                        {
                            xMinPos = _areaData[_wideArea].xMinPos,
                            xMaxPos = _areaData[_wideArea].xMaxPos,
                            zMinPos = _randomPos + 1,
                            zMaxPos = _areaData[_wideArea].zMaxPos
                        });

                    _dividePosData.Add(_wideArea,
                        new PosData
                        {
                            xMinPos = _areaData[_wideArea].xMinPos,
                            xMaxPos = _areaData[_wideArea].xMaxPos,
                            zMinPos = _randomPos,
                            zMaxPos = _randomPos
                        });

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
            _randomRoomSizeMinX = Random.Range(_areaData[key].xMinPos, _areaData[key].xMinPos + ((_areaData[key].xMaxPos - _areaData[key].xMinPos) / 2) - _roomSizeMin) + 1;
            _randomRoomSizeMaxX = Random.Range(_areaData[key].xMinPos + ((_areaData[key].xMaxPos - _areaData[key].xMinPos) / 2), _areaData[key].xMaxPos) - 1;
            _randomRoomSizeMinZ = Random.Range(_areaData[key].zMinPos, _areaData[key].zMinPos + ((_areaData[key].zMaxPos - _areaData[key].zMinPos) / 2) - _roomSizeMin) + 1;
            _randomRoomSizeMaxZ = Random.Range(_areaData[key].zMinPos + ((_areaData[key].zMaxPos - _areaData[key].zMinPos) / 2), _areaData[key].zMaxPos) - 1;

            //上記で決めた大きさをもとに床となるオブジェクトを生成する
            for (int i = _randomRoomSizeMinX; _randomRoomSizeMaxX >= i; i++)
            {
                for (int j = _randomRoomSizeMinZ; _randomRoomSizeMaxZ >= j; j++)
                {
                    Instantiate(_roomTile, new Vector3(i * _gridSize, 0, j * _gridSize), Quaternion.identity);
                }
            }

            //部屋のDataを保存する
            _roomData.Add(key,
                new PosData
                {
                    xMinPos = _randomRoomSizeMinX,
                    xMaxPos = _randomRoomSizeMaxX,
                    zMinPos = _randomRoomSizeMinZ,
                    zMaxPos = _randomRoomSizeMaxZ,
                });
        }
    }

    private void LoadCreate()
    {
        //エリアに隣接している通路のキーを入れる
        string loadkey = null;

        //通路から一番近い距離にある部屋
        string nearRoom = null;

        //通路を作る
        foreach(var key in _keyList)
        {
            if(key == _a || key == _b)
            {
                loadkey = _fKey;
            }
            else
            {
                loadkey = key.Remove(key.Length - 1);
            }

            //たてに分割している場合
            if (_dividePosData[loadkey].xMinPos == _dividePosData[loadkey].xMaxPos)
            {
                //通路を生成し始めるランダムなZ座標
                _randomPos = Random.Range(_roomData[key].zMinPos, _roomData[key].zMaxPos);

                if (_areaData[key].xMaxPos < _dividePosData[loadkey].xMinPos)
                {
                    for(int i = _roomData[key].xMaxPos; i <= _dividePosData[loadkey].xMinPos; i++)
                    {
                        Instantiate(_roomTile,new Vector3(i * _gridSize, 0, _randomPos * _gridSize), Quaternion.identity);
                        if(i == _dividePosData[loadkey].xMinPos)
                        {
                            _linkPoint.Add((i, _randomPos));
                        }
                    }
                }
                else
                {
                    for (int i = _roomData[key].xMinPos; i >= _dividePosData[loadkey].xMinPos; i--)
                    {
                        Instantiate(_roomTile, new Vector3(i * _gridSize, 0, _randomPos * _gridSize), Quaternion.identity);
                        if (i == _dividePosData[loadkey].xMinPos)
                        {
                            _linkPoint.Add((i, _randomPos));
                        }
                    }
                }
                Debug.Log(key.Remove(key.Length - 1));
            }

            //横に分割している場合
            else if(_dividePosData[loadkey].zMinPos == _dividePosData[loadkey].zMaxPos)
            {
                //通路を生成し始めるランダムなZ座標
                _randomPos = Random.Range(_roomData[key].xMinPos, _roomData[key].xMaxPos);

                if (_areaData[key].zMaxPos < _dividePosData[loadkey].zMinPos)
                {
                    for (int i = _roomData[key].zMaxPos; i <= _dividePosData[loadkey].zMinPos; i++)
                    {
                        Instantiate(_roomTile, new Vector3(_randomPos * _gridSize, 0, i * _gridSize), Quaternion.identity);
                        if (i == _dividePosData[loadkey].zMinPos)
                        {
                            _linkPoint.Add((_randomPos, i));
                        }
                    }
                }
                else
                {
                    for (int i = _roomData[key].zMinPos; i >= _dividePosData[loadkey].zMinPos; i--)
                    {
                        Instantiate(_roomTile, new Vector3(_randomPos * _gridSize, 0, i * _gridSize), Quaternion.identity);
                        if (i == _dividePosData[loadkey].zMinPos)
                        {
                            _linkPoint.Add((_randomPos, i));
                        }
                    }
                }
                Debug.Log(key.Remove(key.Length - 1));
            }
        }

        foreach(var key in _dividePosData.Keys) 
        {
            if(_dividePosData[key].xMinPos == _dividePosData[key].xMaxPos)
            {
                for()
            }
            else if(_dividePosData[key].zMinPos == _dividePosData[key].zMaxPos)
            {

            }
        }
    }
}

