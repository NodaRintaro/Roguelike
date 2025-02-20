using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 区域分割法によるMap自動生成
/// </summary>
public class MapGenerator : MonoBehaviour
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
    [SerializeField, Header("マップ全体の横幅")] public int _fieldLengthX = 50;
    [SerializeField, Header("マップ全体の縦幅")] public int _fieldLengthZ = 50;
    [SerializeField, Header("作るエリアの数")] int _areaNum = 4;

    //部屋の大きさの決めるための範囲
    [SerializeField, Header("生成する部屋の大きさの最小値")] int _roomSizeMin = 5;
    [SerializeField, Header("生成する部屋の大きさの最大値")] int _roomSizeMax = 10;
    [SerializeField, Header("生成するエリア大きさの最小値")] int _areaSizeMin = 7;

    [SerializeField, Header("エリアを分割する時の初期の分割し始める初期の中心座標")]
    private const int _startPosX = 1;
    private const int _startPosZ = 1;

    [SerializeField, Header("部屋の床となるオブジェクト")]
    private GameObject _roomTile;

    [SerializeField, Header("部屋や通路以外のMapを埋めるオブジェクト")]
    private GameObject _dontWalkTile;

    [SerializeField, Header("生成したタイルを管理する親オブジェクト")]
    private GameObject _tileCollectObject;

    //それぞれのエリアと部屋の大きさのデータ
    private Dictionary<string, PosData> _areaData
        = new();

    private Dictionary<string, PosData> _roomData
        = new();

    public Dictionary<string, PosData> AreaData => _areaData;

    public Dictionary<string, PosData> RoomData => _roomData;

    //道を作る際に必要となる境界線のデータ
    private Dictionary<string, PosData> _dividePosData
        = new();

    //エリアをつなぐみちのData
    private List<PosData> _loadData = new();

    //区画ごとのKeyの名前
    private string _areaNameA = "_A";
    private string _areaNameB = "_B";
    private string _fKey = "firstKey";

    //最終的に出来上がった区画のKeyのみを保存するList
    private List<string> _keyList = new List<string>();

    public List<string> KeyList => _keyList;

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

    private List<Vector3> _walkPointList = new List<Vector3>();
    private List<Vector3> _goalPointList = new List<Vector3>();

    public List<Vector3> WalkPointList => _walkPointList;

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

        //歩けない床を作る       
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
                _randomPos = Random.Range(_startPosX + _areaSizeMin, _fieldLengthX - _areaSizeMin);

                _areaData.Add(_areaNameA,
                    new PosData
                    {
                        xMinPos = _startPosX,
                        xMaxPos = _randomPos - 1,
                        zMinPos = _startPosZ,
                        zMaxPos = _fieldLengthZ
                    });

                _areaData.Add(_areaNameB,
                    new PosData
                    {
                        xMinPos = _randomPos + 1,
                        xMaxPos = _fieldLengthX,
                        zMinPos = _startPosZ,
                        zMaxPos = _fieldLengthZ
                    });

                _dividePosData.Add(_fKey,
                    new PosData
                    {
                        xMinPos = _randomPos,
                        xMaxPos = _randomPos,
                        zMinPos = _startPosZ,
                        zMaxPos = _fieldLengthZ
                    });

                _keyList.Add(_areaNameA);
                _keyList.Add(_areaNameB);

                Debug.Log("エリア" + _areaNameA + "の座標:" + _areaData[_areaNameA]);
                Debug.Log("エリア" + _areaNameB + "の座標:" + _areaData[_areaNameB]);

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

                    _areaData.Add(_wideArea + _areaNameA,
                        new PosData
                        {
                            xMinPos = _areaData[_wideArea].xMinPos,
                            xMaxPos = _randomPos - 1,
                            zMinPos = _areaData[_wideArea].zMinPos,
                            zMaxPos = _areaData[_wideArea].zMaxPos
                        });

                    _areaData.Add(_wideArea + _areaNameB,
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

                    Debug.Log("エリア" + _wideArea + _areaNameA + "の座標:" + _areaData[_wideArea + _areaNameA]);
                    Debug.Log("エリア" + _wideArea + _areaNameB + "の座標:" + _areaData[_wideArea + _areaNameB]);

                    _keyList.Remove(_wideArea);
                    _keyList.Add(_wideArea + _areaNameA);
                    _keyList.Add(_wideArea + _areaNameB);
                }

                else
                {
                    _randomPos = Random.Range(_areaData[_wideArea].zMinPos + _areaSizeMin, _areaData[_wideArea].zMaxPos - _areaSizeMin);

                    _areaData.Add(_wideArea + _areaNameA,
                        new PosData
                        {
                            xMinPos = _areaData[_wideArea].xMinPos,
                            xMaxPos = _areaData[_wideArea].xMaxPos,
                            zMinPos = _areaData[_wideArea].zMinPos,
                            zMaxPos = _randomPos - 1
                        });

                    _areaData.Add(_wideArea + _areaNameB,
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

                    Debug.Log("エリア" + _wideArea + _areaNameA + "の座標:" + _areaData[_wideArea + _areaNameA]);
                    Debug.Log("エリア" + _wideArea + _areaNameB + "の座標:" + _areaData[_wideArea + _areaNameB]);

                    _keyList.Remove(_wideArea);
                    _keyList.Add(_wideArea + _areaNameA);
                    _keyList.Add(_wideArea + _areaNameB);
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
        //生成したタイルのオブジェクトをいれる変数
        GameObject targetTile = null;

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

                //ランダムな位置から分割線までの道を作る
                _randomPos = Random.Range(_roomData[nearRoomA].zMinPos, _roomData[nearRoomA].zMaxPos);
                for (int xPos = _dividePosData[key].xMaxPos; xPos < _roomData[nearRoomA].xMinPos; xPos++)
                {
                    targetTile = Instantiate(_roomTile, new Vector3(xPos * _gridSize, 0, _randomPos * _gridSize), Quaternion.identity);
                    targetTile.transform.SetParent(_tileCollectObject.transform);

                    if (xPos == _dividePosData[key].xMaxPos)
                    {
                        _loadLinkPosA = (xPos, 0, _randomPos);
                        _walkPointList.Add(new Vector3(xPos, 0, _randomPos));
                    }

                }

                //分割線と部屋をつなぐRoomAの道のデータを保存する
                _loadData.Add(new PosData
                {
                    xMinPos = _dividePosData[key].xMinPos + 1,
                    zMinPos = _randomPos,
                    xMaxPos = _roomData[nearRoomA].xMinPos - 1,
                    zMaxPos = _randomPos
                });


                //ランダムな位置から分割線までの道を作る
                _randomPos = Random.Range(_roomData[nearRoomB].zMinPos, _roomData[nearRoomB].zMaxPos);
                for (int xPos = _dividePosData[key].xMaxPos; xPos > _roomData[nearRoomB].xMaxPos; xPos--)
                {
                    targetTile = Instantiate(_roomTile, new Vector3(xPos * _gridSize, 0, _randomPos * _gridSize), Quaternion.identity);
                    targetTile.transform.SetParent(_tileCollectObject.transform);

                    if (xPos == _dividePosData[key].xMaxPos)
                    {
                        _loadLinkPosB = (xPos, 0, _randomPos);
                        _walkPointList.Add(new Vector3(xPos, 0, _randomPos));
                    }
                }

                //分割線と部屋をつなぐRoomBの道のデータを保存する
                _loadData.Add(new PosData
                {
                    xMinPos = _roomData[nearRoomB].xMaxPos + 1,
                    zMinPos = _randomPos,
                    xMaxPos = _dividePosData[key].xMaxPos - 1,
                    zMaxPos = _randomPos
                });

                //分割線の部分にタイルを敷き詰める
                if (_loadLinkPosA.z >= _loadLinkPosB.z)
                {
                    //道同士を繋げる
                    BuildTile(_loadLinkPosB.x, _loadLinkPosB.x, _loadLinkPosB.z + 1, _loadLinkPosA.z - 1, _roomTile);

                    //分割線の残りの部分を歩けない床で埋める
                    BuildTile(_loadLinkPosB.x, _loadLinkPosB.x, _dividePosData[key].zMinPos, _loadLinkPosB.z - 1, _dontWalkTile);
                    BuildTile(_loadLinkPosB.x, _loadLinkPosB.x, _loadLinkPosA.z + 1, _dividePosData[key].zMaxPos, _dontWalkTile);
                }
                else if (_loadLinkPosA.z < _loadLinkPosB.z)
                {
                    //道同士を繋げる
                    BuildTile(_loadLinkPosB.x, _loadLinkPosB.x, _loadLinkPosA.z + 1, _loadLinkPosB.z - 1, _roomTile);

                    //分割線の残りの部分を歩けない床で埋める
                    BuildTile(_loadLinkPosB.x, _loadLinkPosB.x, _dividePosData[key].zMinPos, _loadLinkPosA.z - 1, _dontWalkTile);
                    BuildTile(_loadLinkPosB.x, _loadLinkPosB.x, _loadLinkPosB.z + 1, _dividePosData[key].zMaxPos, _dontWalkTile);
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

                //ランダムな位置から分割線までの道を作る
                _randomPos = Random.Range(_roomData[nearRoomA].xMinPos, _roomData[nearRoomA].xMaxPos);
                for (int zPos = _dividePosData[key].zMaxPos; zPos < _roomData[nearRoomA].zMinPos; zPos++)
                {
                    targetTile = Instantiate(_roomTile, new Vector3(_randomPos * _gridSize, 0, zPos * _gridSize), Quaternion.identity);
                    targetTile.transform.SetParent(_tileCollectObject.transform);

                    if (zPos == _dividePosData[key].zMaxPos)
                    {
                        _loadLinkPosA = (_randomPos, 0, zPos);
                        _walkPointList.Add(new Vector3(_randomPos, 0, zPos));
                    }
                }

                //分割線と部屋をつなぐRoomA道のデータを保存する
                _loadData.Add(new PosData
                {
                    xMinPos = _randomPos,
                    zMinPos = _dividePosData[key].zMaxPos + 1,
                    xMaxPos = _randomPos,
                    zMaxPos = _roomData[nearRoomA].zMinPos - 1
                });


                //ランダムな位置から分割線までの道を作る
                _randomPos = Random.Range(_roomData[nearRoomB].xMinPos, _roomData[nearRoomB].xMaxPos);
                for (int zPos = _dividePosData[key].zMaxPos; zPos > _roomData[nearRoomB].zMaxPos; zPos--)
                {
                    targetTile = Instantiate(_roomTile, new Vector3(_randomPos * _gridSize, 0, zPos * _gridSize), Quaternion.identity);
                    targetTile.transform.SetParent(_tileCollectObject.transform);

                    if (zPos == _dividePosData[key].zMaxPos)
                    {
                        _loadLinkPosB = (_randomPos, 0, zPos);
                        _walkPointList.Add(new Vector3(_randomPos, 0, zPos));
                    }
                }

                //分割線と部屋をつなぐRoomBの道のデータを保存する
                _loadData.Add(new PosData
                {
                    xMinPos = _randomPos,
                    zMinPos = _roomData[nearRoomB].zMaxPos + 1,
                    xMaxPos = _randomPos,
                    zMaxPos = _dividePosData[key].zMaxPos - 1
                });


                //分割線の部分にタイルを敷き詰める
                if (_loadLinkPosA.x >= _loadLinkPosB.x)
                {
                    //道同士を繋げる
                    BuildTile(_loadLinkPosB.x + 1, _loadLinkPosA.x - 1, _loadLinkPosA.z, _loadLinkPosA.z, _roomTile);

                    //分割線の残りの部分を歩けない床で埋める
                    BuildTile(_dividePosData[key].xMinPos, _loadLinkPosB.x - 1, _loadLinkPosB.z, _loadLinkPosB.z, _dontWalkTile);
                    BuildTile(_loadLinkPosA.x + 1, _dividePosData[key].xMaxPos, _loadLinkPosA.z, _loadLinkPosA.z, _dontWalkTile);
                }
                else if (_loadLinkPosA.x < _loadLinkPosB.x)
                {
                    //道同士を繋げる
                    BuildTile(_loadLinkPosA.x + 1, _loadLinkPosB.x - 1, _loadLinkPosB.z, _loadLinkPosB.z, _roomTile) ;

                    //分割線の残りの部分を歩けない床で埋める
                    BuildTile(_dividePosData[key].xMinPos, _loadLinkPosA.x - 1, _loadLinkPosA.z, _loadLinkPosA.z, _dontWalkTile);
                    BuildTile(_loadLinkPosB.x + 1, _dividePosData[key].xMaxPos, _loadLinkPosA.z, _loadLinkPosA.z, _dontWalkTile);
                }
            }
        }
    }

    /// <summary>
    /// 歩けない床を敷き詰めていく
    /// </summary>
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
            loadPos = SearchLoad(_roomData[areaKey].xMinPos, _roomData[areaKey].xMaxPos, _roomData[areaKey].zMaxPos + 1, _areaData[areaKey].zMaxPos);
            if (loadPos.nullData)
                BuildTile(_roomData[areaKey].xMinPos, _roomData[areaKey].xMaxPos, _roomData[areaKey].zMaxPos + 1, _areaData[areaKey].zMaxPos, _dontWalkTile);
            else
            {
                BuildTile(_roomData[areaKey].xMinPos, loadPos.xMinPos - 1, _roomData[areaKey].zMaxPos + 1, _areaData[areaKey].zMaxPos, _dontWalkTile);
                BuildTile(loadPos.xMinPos + 1, _roomData[areaKey].xMaxPos, _roomData[areaKey].zMaxPos + 1, _areaData[areaKey].zMaxPos, _dontWalkTile);
            }

            //エリア下部の部分を埋める
            loadPos = SearchLoad(_roomData[areaKey].xMinPos, _roomData[areaKey].xMaxPos, _areaData[areaKey].zMinPos, _roomData[areaKey].zMinPos - 1);
            if (loadPos.nullData)
                BuildTile(_roomData[areaKey].xMinPos, _roomData[areaKey].xMaxPos, _areaData[areaKey].zMinPos, _roomData[areaKey].zMinPos - 1, _dontWalkTile);
            else
            {
                BuildTile(_roomData[areaKey].xMinPos, loadPos.xMinPos - 1, _areaData[areaKey].zMinPos, _roomData[areaKey].zMinPos - 1, _dontWalkTile);
                BuildTile(loadPos.xMinPos + 1, _roomData[areaKey].xMaxPos, _areaData[areaKey].zMinPos, _roomData[areaKey].zMinPos - 1, _dontWalkTile);
            }

            //エリアの左の部分を埋める
            loadPos = SearchLoad(_areaData[areaKey].xMinPos, _roomData[areaKey].xMinPos - 1, _roomData[areaKey].zMinPos, _roomData[areaKey].zMaxPos);
            if (loadPos.nullData)
                BuildTile(_areaData[areaKey].xMinPos, _roomData[areaKey].xMinPos - 1, _roomData[areaKey].zMinPos, _roomData[areaKey].zMaxPos, _dontWalkTile);
            else
            {
                BuildTile(_areaData[areaKey].xMinPos, _roomData[areaKey].xMinPos - 1, _roomData[areaKey].zMinPos, loadPos.zMinPos - 1, _dontWalkTile);
                BuildTile(_areaData[areaKey].xMinPos, _roomData[areaKey].xMinPos - 1, loadPos.zMinPos + 1, _roomData[areaKey].zMaxPos, _dontWalkTile);
            }

            //エリアの右の部分を埋める
            loadPos = SearchLoad(_roomData[areaKey].xMaxPos + 1, _areaData[areaKey].xMaxPos, _roomData[areaKey].zMinPos, _roomData[areaKey].zMaxPos);
            if (loadPos.nullData)
                BuildTile(_roomData[areaKey].xMaxPos + 1, _areaData[areaKey].xMaxPos, _roomData[areaKey].zMinPos, _roomData[areaKey].zMaxPos, _dontWalkTile);
            else
            {
                BuildTile(_roomData[areaKey].xMaxPos + 1, _areaData[areaKey].xMaxPos, _roomData[areaKey].zMinPos, loadPos.zMinPos - 1, _dontWalkTile);
                BuildTile(_roomData[areaKey].xMaxPos + 1, _areaData[areaKey].xMaxPos, loadPos.zMinPos + 1, _roomData[areaKey].zMaxPos, _dontWalkTile);
            }
        }
    }

    /// <summary>
    /// 指定された範囲をTileで埋める
    /// </summary>
    private void BuildTile(int xMin ,int xMax ,int zMin ,int zMax ,GameObject Tile)
    {
        GameObject tile;
        for(int x = xMin; x <= xMax; x++)
        {
            for (int z = zMin; z <= zMax; z++)
            {
                tile = Instantiate(Tile, new Vector3(x * _gridSize, 0, z * _gridSize), Quaternion.identity);
                tile.transform.SetParent(_tileCollectObject.transform);
            }
        }
    }

    /// <summary>
    /// エリアないに道があるかを探す
    /// </summary>
    private PosData SearchLoad(int xMin, int xMax, int zMin, int zMax)
    {
        foreach (var loadPos in _loadData)
        {
            if(xMin <= loadPos.xMinPos && xMax >= loadPos.xMaxPos && zMin <= loadPos.zMinPos && zMax >= loadPos.zMaxPos)
            {
                return loadPos;
            } 
        }
        return new PosData {nullData = true};
    }
}
