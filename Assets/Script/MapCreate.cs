using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        public bool nullData;
    }

    [SerializeField, Header("1つのグリッドの大きさ")] private int _gridSize;

    public int GridSize => _gridSize;

    //マップ全体の大きさを決める
    [SerializeField, Header("マップ全体の横幅")] public int _xLength = 50;
    [SerializeField, Header("マップ全体の縦幅")] public int _zLength = 50;
    [SerializeField, Header("作るエリアの数")] int _areaNum = 4;

    //部屋の大きさの決めるための範囲
    [SerializeField, Header("生成する部屋の大きさの最小値")] int _roomSizeMin = 5;
    [SerializeField, Header("生成する部屋の大きさの最大値")] int _roomSizeMax = 10;
    [SerializeField, Header("生成するエリア大きさの最小値")] int _areaSizeMin = 7;

    [SerializeField, Header("エリアを分割する時の初期の分割し始める初期の中心座標")]
    (int x, int z) _startPos = (1, 1);

    [SerializeField, Header("部屋の床となるオブジェクト")]
    private GameObject _roomTile;

    [SerializeField, Header("部屋や通路以外のMapを埋めるオブジェクト")]
    private GameObject _dontWalkTile;

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

    //エリアをつなぐみちのData
    private List<PosData> _loadData = new List<PosData>();

    //区画ごとのKeyの名前
    private string _a = "A";
    private string _b = "B";
    private string _fKey = "firstKey";

    //最終的に出来上がった区画のKeyのみを保存するList
    private List<string> _keyList = new List<string>();

    //ランダムな座標
    private int _randomPos;

    //道を作る際に道同士をつなぐ座標
    private (int x, int y, int z) _loadLinkPosA, _loadLinkPosB;

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

        //TODO:歩けない床を作る       
        DontWalkGroundCreate();
    }


    /// <summary>
    /// 部屋を作るためのエリアを作る
    /// </summary>
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


    /// <summary>
    /// 先ほど作ったエリア内に部屋を作る
    /// </summary>
    private void RoomCreate()
    {
        //部屋を作る
        foreach (var key in _keyList)
        {
            //ランダムに部屋の大きさを決める 
            _randomRoomSizeMinX = Random.Range(_areaData[key].xMinPos, _areaData[key].xMinPos + ((_areaData[key].xMaxPos - _areaData[key].xMinPos) / 2) - _roomSizeMin) + 1;
            _randomRoomSizeMaxX = Random.Range(_areaData[key].xMinPos + ((_areaData[key].xMaxPos - _areaData[key].xMinPos) / 2) + _roomSizeMin, _areaData[key].xMaxPos) - 1;
            _randomRoomSizeMinZ = Random.Range(_areaData[key].zMinPos, _areaData[key].zMinPos + ((_areaData[key].zMaxPos - _areaData[key].zMinPos) / 2) - _roomSizeMin) + 1;
            _randomRoomSizeMaxZ = Random.Range(_areaData[key].zMinPos + ((_areaData[key].zMaxPos - _areaData[key].zMinPos) / 2) + _roomSizeMin, _areaData[key].zMaxPos) - 1;

            //上記で決めた大きさをもとに床となるオブジェクトを生成する
            BuildTile(_randomRoomSizeMinX, _randomRoomSizeMaxX, _randomRoomSizeMinZ, _randomRoomSizeMaxZ, _roomTile);

            //for (int i = _randomRoomSizeMinX; _randomRoomSizeMaxX >= i; i++)
            //{
            //    for (int j = _randomRoomSizeMinZ; _randomRoomSizeMaxZ >= j; j++)
            //    {
            //        Instantiate(_roomTile, new Vector3(i * _gridSize, 0, j * _gridSize), Quaternion.identity);
            //   }
            //}

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


    /// <summary>
    /// 道を作る
    /// </summary>
    private void LoadCreate()
    {
        //エリアに隣接している通路のキーを入れる
        string loadkey = null;

        //通路から一番近い距離にある部屋
        string nearRoomA = null;
        string nearRoomB = null;

        //通路を作る
        foreach (var key in _dividePosData.Keys)
        {
            nearRoomA = null;
            nearRoomB = null;

            //分割線が縦にエリアを分割していた場合
            if (_dividePosData[key].xMinPos == _dividePosData[key].xMaxPos)
            {
                //分割線と一番近い位置にある部屋を２つ探す
                foreach (var roomKey in _keyList)
                {
                    if (_dividePosData[key].zMinPos < _roomData[roomKey].zMinPos && _dividePosData[key].zMaxPos > _roomData[roomKey].zMaxPos)
                    {
                        if (_dividePosData[key].xMinPos < _roomData[roomKey].xMinPos)
                        {
                            if (nearRoomA == null)
                            {
                                nearRoomA = roomKey;
                            }
                            else if (_roomData[nearRoomA].xMinPos > _roomData[roomKey].xMinPos)
                            {
                                nearRoomA = roomKey;
                            }
                        }
                        else if (_dividePosData[key].xMinPos > _roomData[roomKey].xMinPos)
                        {
                            if (nearRoomB == null)
                            {
                                nearRoomB = roomKey;
                            }
                            else if (_roomData[nearRoomB].xMaxPos < _roomData[roomKey].xMaxPos)
                            {
                                nearRoomB = roomKey;
                            }
                        }
                    }
                }

                //分割線と部屋をつなげる

                _randomPos = Random.Range(_roomData[nearRoomA].zMinPos, _roomData[nearRoomA].zMaxPos);

                for (int i = _dividePosData[key].xMaxPos; i < _roomData[nearRoomA].xMaxPos; i++)
                {
                    Instantiate(_roomTile, new Vector3(i * _gridSize, 0, _randomPos * _gridSize), Quaternion.identity);
                    if (i == _dividePosData[key].xMaxPos)
                        _loadLinkPosA = (i, 0, _randomPos);
                }

                _loadData.Add(new PosData
                {
                    xMinPos = _dividePosData[key].xMaxPos,
                    zMinPos = _randomPos,
                    xMaxPos = _roomData[nearRoomA].xMaxPos,
                    zMaxPos = _randomPos
                });

                _randomPos = Random.Range(_roomData[nearRoomB].zMinPos, _roomData[nearRoomB].zMaxPos);

                for (int i = _dividePosData[key].xMaxPos; i > _roomData[nearRoomB].xMaxPos; i--)
                {
                    Instantiate(_roomTile, new Vector3(i * _gridSize, 0, _randomPos * _gridSize), Quaternion.identity);
                    if (i == _dividePosData[key].xMaxPos)
                        _loadLinkPosB = (i, 0, _randomPos);
                }

                _loadData.Add(new PosData
                {
                    xMinPos = _dividePosData[key].xMaxPos,
                    zMinPos = _randomPos,
                    xMaxPos = _roomData[nearRoomB].xMaxPos,
                    zMaxPos = _randomPos
                });

                if (_loadLinkPosA.z >= _loadLinkPosB.z)
                {
                    for (int i = _loadLinkPosB.z + 1; i < _loadLinkPosA.z; i++)
                    {
                        Instantiate(_roomTile, new Vector3(_loadLinkPosB.x * _gridSize, 0, i * _gridSize), Quaternion.identity);
                    }

                    for (int i = _dividePosData[key].zMinPos; i < _loadLinkPosB.z; i++)
                    {
                        Instantiate(_dontWalkTile, new Vector3(_loadLinkPosB.x * _gridSize, 0, i * _gridSize), Quaternion.identity);
                    }
                    for (int i = _loadLinkPosA.z + 1; i <= _dividePosData[key].zMaxPos; i++)
                    {
                        Instantiate(_dontWalkTile, new Vector3(_loadLinkPosA.x * _gridSize, 0, i * _gridSize), Quaternion.identity);
                    }
                }
                else if (_loadLinkPosA.z < _loadLinkPosB.z)
                {
                    for (int i = _loadLinkPosA.z + 1; i < _loadLinkPosB.z; i++)
                    {
                        Instantiate(_roomTile, new Vector3(_loadLinkPosB.x * _gridSize, 0, i * _gridSize), Quaternion.identity);
                    }

                    for (int i = _dividePosData[key].zMinPos; i < _loadLinkPosA.z; i++)
                    {
                        Instantiate(_dontWalkTile, new Vector3(_loadLinkPosA.x * _gridSize, 0, i * _gridSize), Quaternion.identity);
                    }
                    for (int i = _loadLinkPosB.z + 1; i <= _dividePosData[key].zMaxPos; i++)
                    {
                        Instantiate(_dontWalkTile, new Vector3(_loadLinkPosB.x * _gridSize, 0, i * _gridSize), Quaternion.identity);
                    }
                }
            }

            //分割線が横にエリアを分割していた場合
            else if (_dividePosData[key].zMinPos == _dividePosData[key].zMaxPos)
            {
                //分割線と一番近い位置にある部屋を２つ探す
                foreach (var roomKey in _keyList)
                {
                    if (_dividePosData[key].xMinPos < _roomData[roomKey].xMinPos && _dividePosData[key].xMaxPos > _roomData[roomKey].xMaxPos)
                    {
                        if (_dividePosData[key].zMinPos < _roomData[roomKey].zMinPos)
                        {
                            if (nearRoomA == null)
                            {
                                nearRoomA = roomKey;
                            }
                            else if (_roomData[nearRoomA].zMinPos > _roomData[roomKey].zMinPos)
                            {
                                nearRoomA = roomKey;
                            }
                        }
                        else
                        {
                            if (nearRoomB == null)
                            {
                                nearRoomB = roomKey;
                            }
                            else if (_roomData[nearRoomB].zMaxPos < _roomData[roomKey].zMaxPos)
                            {
                                nearRoomB = roomKey;
                            }
                        }
                    }
                }

                _randomPos = Random.Range(_roomData[nearRoomA].xMinPos, _roomData[nearRoomA].xMaxPos);

                for (int i = _dividePosData[key].zMaxPos; i < _roomData[nearRoomA].zMaxPos; i++)
                {
                    Instantiate(_roomTile, new Vector3(_randomPos * _gridSize, 0, i * _gridSize), Quaternion.identity);
                    if (i == _dividePosData[key].zMaxPos)
                        _loadLinkPosA = (_randomPos, 0, i);
                }

                _loadData.Add(new PosData
                {
                    xMinPos = _randomPos,
                    zMinPos = _dividePosData[key].zMaxPos,
                    xMaxPos = _randomPos,
                    zMaxPos = _roomData[nearRoomA].zMaxPos
                });

                _randomPos = Random.Range(_roomData[nearRoomB].xMinPos, _roomData[nearRoomB].xMaxPos);

                for (int i = _dividePosData[key].zMaxPos; i > _roomData[nearRoomB].zMaxPos; i--)
                {
                    Instantiate(_roomTile, new Vector3(_randomPos * _gridSize, 0, i * _gridSize), Quaternion.identity);
                    if (i == _dividePosData[key].zMaxPos)
                        _loadLinkPosB = (_randomPos, 0, i);
                }

                _loadData.Add(new PosData
                {
                    xMinPos = _randomPos,
                    zMinPos = _dividePosData[key].zMaxPos,
                    xMaxPos = _randomPos,
                    zMaxPos = _roomData[nearRoomB].zMaxPos
                });

                if (_loadLinkPosA.x >= _loadLinkPosB.x)
                {
                    for (int i = _loadLinkPosB.x + 1; i < _loadLinkPosA.x; i++)
                    {
                        Instantiate(_roomTile, new Vector3(i * _gridSize, 0, _loadLinkPosA.z * _gridSize), Quaternion.identity);
                    }

                    for (int i = _dividePosData[key].xMinPos; i < _loadLinkPosB.x; i++)
                    {
                        Instantiate(_dontWalkTile, new Vector3(i * _gridSize, 0, _loadLinkPosB.z * _gridSize), Quaternion.identity);
                    }
                    for (int i = _loadLinkPosA.x + 1; i <= _dividePosData[key].xMaxPos; i++)
                    {
                        Instantiate(_dontWalkTile, new Vector3(i * _gridSize, 0, _loadLinkPosA.z * _gridSize), Quaternion.identity);
                    }
                }
                else if (_loadLinkPosA.x < _loadLinkPosB.x)
                {
                    for (int i = _loadLinkPosA.x + 1; i < _loadLinkPosB.x; i++)
                    {
                        Instantiate(_roomTile, new Vector3(i * _gridSize, 0, _loadLinkPosB.z * _gridSize), Quaternion.identity);
                    }

                    for (int i = _dividePosData[key].xMinPos; i < _loadLinkPosA.x; i++)
                    {
                        Instantiate(_dontWalkTile, new Vector3(i * _gridSize, 0, _loadLinkPosA.z * _gridSize), Quaternion.identity);
                    }
                    for (int i = _loadLinkPosB.x + 1; i <= _dividePosData[key].xMaxPos; i++)
                    {
                        Instantiate(_dontWalkTile, new Vector3(i * _gridSize, 0, _loadLinkPosA.z * _gridSize), Quaternion.identity);
                    }
                }
            }
        }
    }

    private void DontWalkGroundCreate()
    {
        PosData loadPos;

        foreach(var areaKey in _keyList)
        {
            //エリアの左上の部分を埋める
            BuildTile(_areaData[areaKey].xMinPos, _roomData[areaKey].xMinPos - 1, _roomData[areaKey].zMaxPos + 1, _areaData[areaKey].zMaxPos, _dontWalkTile);

            //エリアの右上の部分を埋める
            BuildTile(_roomData[areaKey].xMaxPos + 1, _areaData[areaKey].xMaxPos, _roomData[areaKey].zMaxPos + 1, _areaData[areaKey].zMaxPos, _dontWalkTile);

            //エリアの左下の部分を埋める
            BuildTile(_areaData[areaKey].xMinPos, _roomData[areaKey].xMinPos - 1, _areaData[areaKey].zMinPos, _roomData[areaKey].zMinPos - 1, _dontWalkTile);

            //エリアの右下の部分を埋める
            BuildTile(_roomData[areaKey].xMaxPos + 1, _areaData[areaKey].xMaxPos, _areaData[areaKey].zMinPos, _roomData[areaKey].zMinPos - 1, _dontWalkTile);

            //エリア上部の部分を埋める
            loadPos = SearchLoad(_roomData[areaKey].xMinPos, _roomData[areaKey].xMaxPos, _roomData[areaKey].zMinPos, _areaData[areaKey].zMaxPos);
            if (loadPos.nullData)
                BuildTile(_roomData[areaKey].xMinPos, _roomData[areaKey].xMaxPos, _roomData[areaKey].zMaxPos + 1, _areaData[areaKey].zMaxPos, _dontWalkTile);
            else
            {
                BuildTile(_roomData[areaKey].xMinPos, loadPos.xMinPos - 1, _roomData[areaKey].zMaxPos + 1, _areaData[areaKey].zMaxPos, _dontWalkTile);
            }
        }
    }

    /// <summary>
    /// 指定された範囲をTileで埋める
    /// </summary>
    private void BuildTile(int xMin ,int xMax ,int zMin ,int zMax ,GameObject Tile)
    {
        for(int x = xMin; x <= xMax; x++)
        {
            for (int z = zMin; z <= zMax; z++)
            {
                Instantiate(Tile, new Vector3(x * _gridSize, 0, z * _gridSize), Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// エリアないに道があるかを探す
    /// </summary>
    private PosData SearchLoad(int xMin, int xMax, int zMin, int zMax)
    {
        foreach (var item in _loadData)
        {
            if(xMin <= item.xMinPos && xMax >= item.xMaxPos && zMin <= item.xMinPos && zMax >= item.zMaxPos)
            {
                return item;
            } 
        }
        return new PosData{nullData = true};
    }
}
