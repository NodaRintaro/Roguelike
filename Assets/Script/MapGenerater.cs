using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 区域分割法によるMap自動生成
/// </summary>
public class MapGenerater : MonoBehaviour
{
    //マップ全体の大きさを決める
    [SerializeField,Header("マップ全体の横幅")] static public int _xLength = 50;
    [SerializeField,Header("マップ全体の縦幅")] static public int _zLength = 50;
    [SerializeField,Header("作るエリアの数")] int _areaNum = 4;

    //部屋の大きさの決めるための範囲
    [SerializeField,Header("生成する部屋の大きさの最小値")] int _roomSizeMin = 5;
    [SerializeField,Header("生成する部屋の大きさの最大値")] int _roomSizeMax = 10;
    [SerializeField,Header("生成するエリア大きさの最小値")] int _mapSizeMin = 6;
    [SerializeField, Header("生成するエリア大きさの最大値")] int _mapSizeMax = 6;

    private int _randomPosX, _randomPosZ, _randomRoomSize, _areaSize;

    /// <summary>
    /// 今のx座標の最大値と前回のx座標の最大値
    /// </summary>
    private int _currentMaxAreaSizeX = 1, _keepMinAreaSizeX = 1;
    public void MapGenerate()
    {
        for(int i = 0; i < _areaNum; i++)
        {
            if(i == 0)
            {

            }
        }
    }
}
