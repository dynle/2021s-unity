using GameCanvas;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public sealed class Game : GameBase
{
    const int ENEMY_NUM = 10;
    int[] enemy_x = new int [ENEMY_NUM];
    int[] enemy_y = new int [ENEMY_NUM];
    int[] enemy_speed = new int [ENEMY_NUM];
    bool[] enemy_survived = new bool[ENEMY_NUM];
    int enemy_w = 80;
    int enemy_h = 61;
    int enemy_speed_min = 2;
    int enemy_speed_max = 5;

    int player_x = 360;
    int player_y = 1000;
    int player_dir = 1;
    int player_speed = 8;

    int score =0;

    int gameState = 0;
    int high_score = 0;

    int bullet_speed = -100;
    int bullet_x = 0;
    int bullet_y = 1000;
    bool bullet_onscreen = false;

    int earth_hp = 100;
    int earth_count = 0;
    int stage=1;

    public override void InitGame()
    {
        gc.ChangeCanvasSize(720, 1280);
        gc.SetResolution(720, 1280);

        gc.TryLoad("hs",out high_score);

        resetValue();
    }

    public override void UpdateGame()
    {
        if(gameState == 0){
            if(gc.GetPointerFrameCount(0) == 1){
                gameState =1;
            }
        }

        else if(gameState == 1){

            if(score>high_score){
                high_score = score;
            }

            if(gc.GetPointerFrameCount(0) ==1 ){
                player_dir = -player_dir;
            }

            //stop the flight if contacts with the wall
            if(player_x<=0 || player_x>=550){
                player_speed = 0;
                if(gc.GetPointerFrameCount(0) ==1){
                    if(player_x<=0) player_x +=10;
                    else player_x -=10;
                }
            }else{
                player_speed = 8;
                player_x += player_dir * player_speed;
            }

            // show enemies, check enemy & player, check enemy & bullet
            for(int i =0 ; i < ENEMY_NUM ; i ++ ){
                enemy_y[i] = enemy_y[i] + enemy_speed[i];

                // hide enemies when enemey_surived is changed to false
                if(enemy_survived[i]==false){
                    enemy_x[i]=-100;
                    enemy_y[i]=-100;
                }
                
                // Earth HP
                if(enemy_y[i]> 1280){
                    gc.PlaySound(GcSound.Bling);
                    earth_count++;
                    earth_hp-=20;
                }
                if(earth_hp==0) gameState=2;

                // show new enemies
                if(enemy_y[i]> 1280 || enemy_survived[i]==false){
                    enemy_x[i] = gc.Random(80,640);
                    enemy_y[i] = -gc.Random(100,480);
                    enemy_speed[i] = gc.Random(enemy_speed_min,enemy_speed_max);
                    enemy_survived[i] = true;
                }

                // check whether enemies contact with the player
                if (gc.CheckHitRect (
                    player_x,player_y,110,107,
                    enemy_x[i],enemy_y[i],enemy_w,enemy_h)) {
                        earth_hp=0;
                        gameState =2;
                        gc.Save("hs",high_score);
                }

                // check whether a bullet contacts with an enemy
                if(gc.CheckHitRect(bullet_x,bullet_y,48,36,enemy_x[i],enemy_y[i],enemy_w,enemy_h)){
                    bullet_y=1000;
                    bullet_onscreen=false;
                    enemy_survived[i] = false;
                    score++;

                    // change stage
                    if(score!=0 && score%50==0){
                        enemy_speed_min++;
                        enemy_speed_max++;
                        stage++;
                    }
                }
            }

            // shooting bullets
            if(bullet_onscreen==false){
                bullet_x = player_x+75;
                bullet_onscreen=true;
            }
            if(bullet_y<0){
                bullet_y=1000;
                bullet_onscreen=false;
            }
            bullet_y += bullet_speed;
        }

        else if(gameState == 2){
            // restart the game
            if(gc.GetPointerFrameCount(0) >=120){
                gameState=0;
                player_x = 360;
                player_y = 1000;
                score = 0;
                enemy_speed_min=2;
                enemy_speed_max=5;
                stage = 1;
                resetValue();
            }
        }
    }

    public override void DrawGame()
    {
        if(gameState == 0){
            gc.ClearScreen();
            gc.DrawImage(GcImage.BackgroundFirst,0,0);
            gc.DrawImage(GcImage.BackgroundFirst_Earth,100,500,500,500);
            gc.SetColor(255,255,255);
            gc.SetFontSize(90);
            gc.DrawString("Save the Earth",50,260);
            if(gc.CurrentTimestamp%2==0){
                gc.SetFontSize(60);
                gc.DrawString("Tap screen to start",70,1000);
            }
        }
        else if(gameState == 1){
            gc.DrawImage(GcImage.Background,0,0);
            gc.DrawImage(GcImage.Flighter,player_x,player_y);

            // Enemies
            for(int i =0 ; i < ENEMY_NUM ; i ++ ){
                gc.DrawImage(GcImage.Enemy,enemy_x[i],enemy_y[i]); 
            }

            // Bullets
            gc.DrawImage(GcImage.Bullet,bullet_x,bullet_y);

            if(score!=0 && score%50==0){
                    gc.SetColor(255,0,0);
                    // gc.SetFontSize(100);
                    gc.DrawString("Speed Up!",250,640);
            }

            // bottom bar black
            gc.SetColor(0,0,0);
            gc.FillRect(0,1180,720,40);

            // bottom bar black - EARTH HP yellow
            gc.SetColor(255,255,0);
            gc.SetFontSize(30);
            gc.DrawString("EARTH HP",0,1190);

            // bottom bar black - earth hp bar red
            gc.SetColor(255,0,0);
            gc.FillRect(120,1190,600-earth_count*120,20);
            
            // bottom bar black - earth hp white
            gc.SetColor(255,255,255);
            gc.SetFontSize(40);
            gc.DrawString(earth_hp.ToString(),360,1190);

            // bottom bar white
            gc.SetColor(255,255,255);
            gc.FillRect(0,1220,720,60);

            // bottom bar white - score, stage, high score
            gc.SetColor(0,0,0);
            gc.SetFontSize(40);
            gc.DrawString("SCORE:"+score,80,1240);
            gc.DrawString("STAGE "+stage,300,1240);
            gc.DrawString("HIGH:"+high_score,500,1240);


        }
        else if(gameState == 2){
            gc.SetColor(255,255,255);
            gc.SetFontSize(100);
            gc.DrawString("GAME OVER",140,440);
            gc.SetFontSize(50);
            gc.DrawString("Press 2 secs to restart",70,640);

            // bottom bar black
            gc.SetColor(0,0,0);
            gc.FillRect(0,1180,720,40);

            // bottom bar black - EARTH HP yellow
            gc.SetColor(255,255,0);
            gc.SetFontSize(30);
            gc.DrawString("EARTH HP",0,1190);

            // bottom bar black - earth hp white
            gc.SetColor(255,255,255);
            gc.SetFontSize(40);
            gc.DrawString(earth_hp.ToString(),360,1190);
        }
    }

    public void resetValue(){
        for(int i =0 ; i < ENEMY_NUM ; i ++ ){
            enemy_x[i] = gc.Random(80,640);
            enemy_y[i] = -gc.Random(100,480);
            enemy_speed[i] = gc.Random(enemy_speed_min,enemy_speed_max);
            enemy_survived[i] = true;
        }
            earth_hp = 100;
            earth_count = 0;
    }
}
