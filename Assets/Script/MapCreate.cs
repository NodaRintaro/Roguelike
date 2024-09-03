using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    //マップ全体の大きさを決める
    [SerializeField,Header("マップ全体の横幅")] static public int _x = 50;//マップ全体の横幅
    [SerializeField,Header("マップ全体の縦幅")] static public int _z = 50;//マップ全体の縦幅
    [SerializeField,Header("作るエリアの数")] int _areaNum = 4;//作るエリアの数

    //部屋の大きさの決めるための範囲
    [SerializeField,Header("生成するエリア大きさの最小値")] int _mapSizeMin = 3;//生成するエリア大きさの最小値
    [SerializeField,Header("生成するエリア大きさの最大値")] int _mapSizeMax = 7;//生成するエリア大きさの最大値

    [SerializeField, Header("今のx座標の最大値")] private int _keepCurrentMaxArea = 1;
    [SerializeField, Header("前回のx座標の最大値")] private int _keepMinAreaSize = 1;
}
