using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    //マップ全体の大きさを決める
    [SerializeField] static public int _x = 50;//マップ全体の横幅
    [SerializeField] static public int _z = 50;//マップ全体の縦幅
    [SerializeField] int _areaNum = 4;//作るエリアの数

    //部屋の大きさの決めるための範囲
    [SerializeField] int _mapSizeMin = 3;//生成するエリア大きさの最小値
    [SerializeField] int _mapSizeMax = 7;//生成するエリア大きさの最大値
}
