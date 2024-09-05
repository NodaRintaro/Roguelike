using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 区域分割法によるMap自動生成
/// </summary>
public class MapGenerater : MonoBehaviour
{
    //マップ全体の大きさを決める
    [SerializeField,Header("マップ全体の横幅")] static public int _xLength = 50;//マップ全体の横幅
    [SerializeField,Header("マップ全体の縦幅")] static public int _zLength = 50;//マップ全体の縦幅
    [SerializeField,Header("作るエリアの数")] int _areaNum = 4;//作るエリアの数

    //部屋の大きさの決めるための範囲
    [SerializeField,Header("生成するエリア大きさの最小値")] int _roomSizeMin = 3;//生成するエリア大きさの最小値
    [SerializeField,Header("生成するエリア大きさの最大値")] int _roomSizeMax = 7;//生成するエリア大きさの最大値

    private int _randomPosX, _randomPosZ, _randomRoomSize;//横と縦の中心点とへ部屋の大きさ

    public void MapGenerate()
    {
        int areaSize;//分割するエリアの大きさ

        int currentMaxAreaSize = 1;//今のx座標の最大値
        int keepMinAreaSize = 1;//前回のx座標の最大値

        areaSize = _xLength / _areaNum;//分割する大きさを決める

        for(int i = 0; i < _areaNum; i++)
        {
            keepMinAreaSize = currentMaxAreaSize;//前回の最大値を保存する
            if(i == 0)
            {
                currentMaxAreaSize = areaSize;
                _randomPosX = Random.Range(_roomSizeMin + keepMinAreaSize, currentMaxAreaSize);
            }//最初の区画だった場合
            else if(i == _areaNum - 1)
            {
                currentMaxAreaSize = _xLength - 1;
                Debug.Log($"{"前回の最大の幅:" + currentMaxAreaSize} {"今回の最大の幅:" + keepMinAreaSize}");
                _randomPosX = Random.Range(keepMinAreaSize, currentMaxAreaSize);
            }
            else
            {
                currentMaxAreaSize += areaSize;
                Debug.Log($"{"前回の最大の幅:" + currentMaxAreaSize} {"今回の最大の幅:" + keepMinAreaSize}");
            }
        }
    }
}
