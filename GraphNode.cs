using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieGame.NPC;
using Microsoft.Xna.Framework;

namespace ZombieGame.Levels
{
    public class GraphNode
    {
        private List<GameEntity> entities;
        private List<GraphNode> neighbors;
        private int x;
        private int y;
        private float w;
        private float h;
        private int g_cost;
        private int f_cost;
        private int h_cost;
        private GraphNode prev;

        private GraphNode()
        {
            //do nothing
        }

        public GraphNode(int x, int y, float w, float h)
        {
            entities = new List<GameEntity>();
            neighbors = new List<GraphNode>();
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            g_cost = 0;
            f_cost = 0;
            h_cost = 0;
            prev = null;
        }

        public void addNeighbor(GraphNode neighbor)
        {
            if (!neighbors.Contains(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        public void setGCost(int g_cost)
        {
            this.g_cost = g_cost;
        }

        public void setFCost()
        {
            this.f_cost = g_cost + h_cost;
        }

        public void setHCost(int h_cost)
        {
            this.h_cost = h_cost;
        }

        public void setPrevious(GraphNode prev)
        {
            this.prev = prev;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public int getScreenX()
        {
            return (int)(x * w);
        }

        public int getScreenY()
        {
            return (int)(y * h);
        }

        public Vector2 getPosition()
        {
            return new Vector2(x, y);
        }

        public Vector2 getScreenPosition()
        {
            return new Vector2(getScreenX(), getScreenY());
        }

        public int getGCost()
        {
            return g_cost;
        }

        public int getFCost()
        {
            return f_cost;
        }

        public int getHCost()
        {
            return h_cost;
        }

        public GraphNode getPrevious()
        {
            return prev;
        }

        public List<GraphNode> getNeighbors()
        {
            return new List<GraphNode>(neighbors);
        }

        public List<GameEntity> getEntities()
        {
            return new List<GameEntity>(entities);
        }

        public bool addEntity(GameEntity obj)
        {
            if (entities.Contains(obj))
            {
                return false;
            }

            entities.Add(obj);
            return true;
        }

        public bool removeEntity(GameEntity obj)
        {
            return entities.Remove(obj);
        }

        public Rectangle getScreenBoundingBox()
        {
            return new Rectangle((int)(x * w), (int)(y * h), (int)w, (int)h);
        }

        public Vector2 getCenter()
        {
            return new Vector2((int)((x * w) + (w / 2.0f)), (int)((y * h) + (h / 2.0f))) ;
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            GraphNode c_other = (GraphNode)other;
            return this.x == c_other.getX() &&
                this.y == c_other.getY();
        }

        public override string ToString()
        {
            return "[" + x + "," + y + "]";
        }
    }
}
