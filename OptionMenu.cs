using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ZombieGame.Draw;
using ZombieGame.Platform;
using Microsoft.Xna.Framework.Graphics;
using ZombieGame.Levels;
using Microsoft.Xna.Framework.Audio;
using ZombieGame.Document;

namespace ZombieGame.Menus
{
    /// <summary>
    /// Option menu. Changing ingame options such as speed, sound an game difficulty 
    /// 
    /// </summary>
    /// <author>Steven Perez, Eduardo De la Cruz, Jason Kaiser</author>
    public class OptionsMenu : GameMenu
    {
        public const int SCENE_WIDTH = 86;
        public const int SCENE_HEIGHT = 86;

        private List<GUICheckBox> diff_rbs;
        private List<GUICheckBox> sens_rbs;

        public OptionsMenu(Game g)
            : base(g)
        {
            diff_rbs = new List<GUICheckBox>();
            sens_rbs = new List<GUICheckBox>();
           
            GUIButton tool1 = new GUIButton(new Vector2(GameMain.GAME_WIDTH / 2 - 64, (MainMenu.MAINTN_HEIGHT + 10) * 6 ), new Vector2(MainMenu.MAINTN_WIDTH, MainMenu.MAINTN_HEIGHT), "MainMenu", "GO BACK", Color.YellowGreen, ButtonType.STONE2, g);
            tool1.setTextOffset(new Vector2(15, 9));
            tool1.setFont(g.Content.Load<SpriteFont>("Fonts\\GUIFont2"));

            GUICheckBox difficulty = new GUICheckBox(new Vector2(0, 130), new Vector2(0,0), "", "Difficulty", Color.White, g);
            difficulty.setOffset(new Vector2(30, 0));
            difficulty.setFont(g.Content.Load<SpriteFont>("Fonts\\GUIFont2"));

            GUICheckBox easy = new GUICheckBox(new Vector2(200, 130), new Vector2(30, 30), "easy", "Easy", Color.Red, g);
            easy.setOffset(new Vector2(30,0));
            easy.setFont(g.Content.Load<SpriteFont>("Fonts\\GUIFont2"));

            GUICheckBox medium = new GUICheckBox(new Vector2(340, 130), new Vector2(30, 30), "medium", "Medium", Color.Red, g);
            medium.setOffset(new Vector2(30, 0));
            medium.setFont(g.Content.Load<SpriteFont>("Fonts\\GUIFont2"));

            GUICheckBox hard = new GUICheckBox(new Vector2(480, 130), new Vector2(30, 30), "hard", "Hard", Color.Red, g);
            hard.setOffset(new Vector2(30, 0));
            hard.setFont(g.Content.Load<SpriteFont>("Fonts\\GUIFont2"));

            GUICheckBox speed = new GUICheckBox(new Vector2(0, 210), new Vector2(0, 0), "", "Sensitivity", Color.White, g);
            speed.setOffset(new Vector2(30, 0));
            speed.setFont(g.Content.Load<SpriteFont>("Fonts\\GUIFont2"));

            GUICheckBox slow = new GUICheckBox(new Vector2(200, 210), new Vector2(30, 30), "slow", "Slow", Color.Red, g);
            slow.setOffset(new Vector2(30, 0));
            slow.setFont(g.Content.Load<SpriteFont>("Fonts\\GUIFont2"));

            GUICheckBox med = new GUICheckBox(new Vector2(340, 210), new Vector2(30, 30), "medium", "Medium", Color.Red, g);
            med.setOffset(new Vector2(30, 0));
            med.setFont(g.Content.Load<SpriteFont>("Fonts\\GUIFont2"));

            GUICheckBox fast = new GUICheckBox(new Vector2(480, 210), new Vector2(30, 30), "fast", "Fast", Color.Red, g);
            fast.setOffset(new Vector2(30, 0));
            fast.setFont(g.Content.Load<SpriteFont>("Fonts\\GUIFont2"));

            GUICheckBox mute = new GUICheckBox(new Vector2(280, 280), new Vector2(30, 30), "mute", "Mute", Color.Red, g);
            mute.setOffset(new Vector2(30, 0));
            mute.setFont(g.Content.Load<SpriteFont>("Fonts\\GUIFont2"));

            gui.Add(tool1);
            gui.Add(mute);
            gui.Add(difficulty);
            gui.Add(speed);
            diff_rbs.Add(easy);
            diff_rbs.Add(medium);
            diff_rbs.Add(hard);
            
            sens_rbs.Add(slow);
            sens_rbs.Add(med);
            sens_rbs.Add(fast);

            music_layer = g.Content.Load<SoundEffect>("Audio//Music//MainMenuMusic").CreateInstance();
            music_layer.Volume = 0.1f;
            music_layer.IsLooped = true;

            if (!ProfileManager.getInstance().getSelectedProfile().mute_sound)
                music_layer.Play();
            else
                mute.setChecked(true);

            if (ProfileManager.getInstance().getSelectedProfile().current_diff == Profile.Difficulty.Easy)
                easy.setChecked(true);
            else if (ProfileManager.getInstance().getSelectedProfile().current_diff == Profile.Difficulty.Medium)
                medium.setChecked(true);
            else if (ProfileManager.getInstance().getSelectedProfile().current_diff == Profile.Difficulty.Hard)
                hard.setChecked(true);

            if (ProfileManager.getInstance().getSelectedProfile().current_sens == Profile.Sensitivity.Slow)
                slow.setChecked(true);
            else if (ProfileManager.getInstance().getSelectedProfile().current_sens == Profile.Sensitivity.Med)
                med.setChecked(true);
            else if (ProfileManager.getInstance().getSelectedProfile().current_sens ==Profile.Sensitivity.Fast)
                fast.setChecked(true);
        }

        public override void update(List<IController> players)
        {
            foreach (IController c in players)
            {
                if (c.isBtnPressed(CtrlBtns.START))
                {
                    transition = MenuTransition.MAINMENU;
                    music_layer.Stop();
                }

                if (c.isBtnPressed(CtrlBtns.PRIMARY))
                {
                    foreach (GUICheckBox e in diff_rbs)
                    {
                        if (e.onClick(c.getPosition()))
                        {
                            Profile profile = ProfileManager.getInstance().getSelectedProfile();

                            if (e.getID().Equals("easy"))
                            {
                                //set dif in profile
                                profile.current_diff = Profile.Difficulty.Easy;

                                foreach (GUICheckBox cb in diff_rbs)
                                {
                                    cb.setChecked(false);
                                }

                                e.setChecked(true);
                            }
                            else if (e.getID().Equals("medium"))
                            {
                                //set dif in profile
                                profile.current_diff = Profile.Difficulty.Medium;

                                foreach (GUICheckBox cb in diff_rbs)
                                {
                                    cb.setChecked(false);
                                }

                                e.setChecked(true);
                            }
                            else if (e.getID().Equals("hard"))
                            {
                                //set dif in profile
                                profile.current_diff = Profile.Difficulty.Hard;

                                foreach (GUICheckBox cb in diff_rbs)
                                {
                                    cb.setChecked(false);
                                }

                                e.setChecked(true);
                            }

                            break;
                        }
                    }

                    foreach (GUICheckBox f in sens_rbs)
                    {
                        if (f.onClick(c.getPosition()))
                        {
                            Profile profile = ProfileManager.getInstance().getSelectedProfile();

                            if (f.getID().Equals("slow"))
                            {
                                profile.current_sens = Profile.Sensitivity.Slow;

                                foreach (GUICheckBox cb in sens_rbs)
                                {
                                    cb.setChecked(false);
                                }

                                f.setChecked(true);

                                foreach (IController d in players)
                                {
                                    d.updateSensitivity();
                                }
                            }

                            else if (f.getID().Equals("medium"))
                            {
                                profile.current_sens = Profile.Sensitivity.Med;

                                foreach (GUICheckBox cb in sens_rbs)
                                {
                                    cb.setChecked(false);
                                }

                                foreach (IController d in players)
                                {
                                    d.updateSensitivity();
                                }
                                f.setChecked(true);
                            }

                            else if (f.getID().Equals("fast"))
                            {
                                profile.current_sens = Profile.Sensitivity.Fast;

                                foreach (GUICheckBox cb in sens_rbs)
                                {
                                    cb.setChecked(false);
                                }
                                foreach (IController d in players)
                                {
                                    d.updateSensitivity();
                                }

                                f.setChecked(true);
                            }
                        }
                    }

                    //check if any player has pressed a gui element
                    foreach (GUIElement e in gui)
                    {
                        Profile profile = ProfileManager.getInstance().getSelectedProfile();

                        if (e.onClick(c.getPosition()))
                        {
                            if (e.getID().Equals("MainMenu"))
                            {
                                transition = MenuTransition.MAINMENU;
                                music_layer.Stop();
                            }
                            else if (e.getID().Equals("mute"))
                            {
                                profile.mute_sound = !profile.mute_sound;
                                if (!profile.mute_sound)
                                {
                                    music_layer.Play();

                                }
                                else
                                {
                                    music_layer.Stop();

                                }
                            }
                            
                            //something in the gui was pressed, don't let the click impact anything below it
                            break;
                        }
                    }
                }
            }
        }
        

        public override void draw(SpriteBatch scr)
        {
            Rectangle bounds = new Rectangle(0, 0, GameMain.GAME_WIDTH, GameMain.GAME_HEIGHT);
            scr.Draw(g.Content.Load<Texture2D>("Graphics//MainMenuBack"), bounds, Color.White);

            foreach (GUIElement e in gui)
            {
                e.draw(scr);
            }

            foreach (GUIElement e in diff_rbs)
            {
                e.draw(scr);
            }

            foreach (GUIElement e in sens_rbs)
            {
                e.draw(scr);
            }

            scr.Draw(g.Content.Load<Texture2D>("Graphics//logo3"), new Rectangle(200, 26, 250, 80), Color.White);
        }
    }
}
