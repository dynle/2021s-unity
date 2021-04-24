using GameCanvas;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// ゲームクラス。
/// 学生が編集すべきソースコードです。
/// </summary>
public sealed class Game : GameBase
{
    int money;
    const int CARD_TYPE = 11;
    int[] card_count = new int [CARD_TYPE];
    string[] card_name = {"A","B","C","D","E","F","G","H","I","J","外れ"};
    bool isComplete;
    int new_card;
    int possibility;


    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void InitGame()
    {
        resetValue();
    }

    /// <summary>
    /// 動きなどの更新処理
    /// </summary>
    public override void UpdateGame()
    {
        if(gc.GetPointerFrameCount(0)==1 && !isComplete){
            money-=100;
            possibility = gc.Random(0,4);
            //”外れ”の場合
            if(possibility==0){
                new_card=10;
            }
            else if(possibility==1){
                new_card=gc.Random(0,4);
            }
            else{
                new_card=gc.Random(5,9);
            }
            card_count[new_card]++;

            isComplete=false;
            for(int i=0;i<CARD_TYPE/2;i++){
                if(card_count[i] >= 5){
                    isComplete=true;
                }
            }
        }

        if(gc.GetPointerFrameCount(0)>=120){
            resetValue();
        }
    }
    /// <summary>
    /// 描画の処理
    /// </summary>
    public override void DrawGame()
    {
        gc.ClearScreen();
        gc.SetColor(0,0,0);
        gc.SetFontSize(36);
        gc.DrawString("money:"+money,60, 40);

        if(new_card >= 0){
        gc.DrawString("new:"+card_name[new_card],60, 80);
        }

        for(int i=0 ; i< CARD_TYPE ; i++){
            gc.DrawString(card_name[i] + ":" + card_count[i],60, 120+i*40);
        }

        if(isComplete){
            gc.DrawString("complete!!",60, 620);
        }
        else if(!isComplete && money==0){
            gc.DrawString("Fail!! Try again.",60, 620);
        }
    }

    void resetValue(){
        money = 10000;
        for (int i=0;i<CARD_TYPE;i++){
            card_count[i] = 0;
        }
        isComplete = false;
        new_card = -1;
    }
}
