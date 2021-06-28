using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TheJam
{
    public class Map
    {
        public List<Entity> entities;
        public Texture2D background;
        public int frameLength;
        public int currentFrame = 0;
        public int millisLastFrame = 0;
        public SoundEffect BackgroundMusic;
        private SoundEffectInstance soundInstance;
        public int millisplaying;
        public bool loaded = false;
        public WallType wt;
        public Game1 g;
        public enum WallType
        {
            TopLeft,
            Top,
            TopRight,
            Left,
            BottomLeft,
            Bottom,
            BottomRight,
            Right,
            ship,
            Free,
            Blocked
        }

        public Map(List<Entity> entities, Texture2D background, SoundEffect BackgroundMusic, WallType wt, Game1 g)
        {
            this.entities = entities;
            this.background = background;
            this.BackgroundMusic = BackgroundMusic;
            frameLength = 0;
            this.wt = wt;
            this.g = g;
        }
        public Map(List<Entity> entities, Texture2D background, int frameLength, SoundEffect BackgroundMusic, WallType wt, Game1 g)
        {
            this.entities = entities;
            this.background = background;
            this.frameLength = frameLength;
            this.BackgroundMusic = BackgroundMusic;
            this.wt = wt;
            this.g = g;
        }

        public void Update(GameTime gt)
        {
            if (entities.Exists(test => test is Player))
            {
                if (loaded)
                {
                }
                else
                {
                    loaded = true;
                    if (g.currentMusic != BackgroundMusic)
                    {
                        if (g.music != null) g.music.Stop();
                        g.music = BackgroundMusic.CreateInstance();
                        g.music.IsLooped = true;
                        g.music.Play();
                        g.currentMusic = BackgroundMusic;
                    }

                    switch (wt)
                    {
                        case WallType.TopLeft:
                            break;
                        case WallType.Top:
                            break;
                        case WallType.TopRight:
                            break;
                        case WallType.Left:
                            break;
                        case WallType.BottomLeft:
                            break;
                        case WallType.Bottom:
                            break;
                        case WallType.BottomRight:
                            break;
                        case WallType.Right:
                            break;
                        case WallType.ship:
                        case WallType.Free:

                            for (int u = 0; u < 9; u++)
                                entities.Add(new LeaveTile(u, -1, g.nothing, false, g.zoneCoordinates.X, g.zoneCoordinates.Y - 1, u, 7, false, g));
                            for (int u = 0; u < 8; u++)
                                entities.Add(new LeaveTile(-1, u, g.nothing, false, g.zoneCoordinates.X - 1, g.zoneCoordinates.Y, 7, u, false, g));
                            for (int u = 0; u < 9; u++)
                                entities.Add(new LeaveTile(u, 8, g.nothing, false, g.zoneCoordinates.X, g.zoneCoordinates.Y + 1, u, 0, false, g));
                            for (int u = 0; u < 8; u++)
                                entities.Add(new LeaveTile(8, u, g.nothing, false, g.zoneCoordinates.X + 1, g.zoneCoordinates.Y, 0, u, false, g));

                            break;
                        case WallType.Blocked:
                            for (int u = 0; u < 9; u++) entities.Add(new Entity(-1, u, 0, true, g.nothing, g));
                            for (int u = 0; u < 8; u++) entities.Add(new Entity(u, -1, 0, true, g.nothing, g));
                            for (int u = 0; u < 9; u++) entities.Add(new Entity(8, u, 0, true, g.nothing, g));
                            for (int u = 0; u < 8; u++) entities.Add(new Entity(u, 8, 0, true, g.nothing, g));
                            break;
                        default:
                            break;
                    }

                    //setup
                }
            }
            else
            {
                loaded = false;
                entities.RemoveAll(test => (test is LeaveTile && !((LeaveTile)test).permanent) /*|| (test.collision == true && test.sprite == g.nothing && test.depth == 0)*/);
                //unload
            }
        }
    }
}
