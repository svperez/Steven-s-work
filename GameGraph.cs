using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZombieGame.NPC;
using ZombieGame.Menus;
using ZombieGame.Platform;
using Microsoft.Xna.Framework;
using ZombieGame.Items;

namespace ZombieGame.Levels
{
    public class GameGraph
    {
        private int offset_x;
        private int offset_y;
        private int width;
        private int height;
        private float node_w;
        private float node_h;
        private List<GraphNode> graph;

        public GameGraph(int width, int height)
        {
            this.width = width;
            this.height = height;
            node_w = IngameMenu.GAME_WIDTH / (float)width;
            node_h = IngameMenu.GAME_HEIGHT / (float)height;
            offset_x = 0;
            offset_y = 0;

            graph = new List<GraphNode>();
            buildGraphGrid(0, 0);
        }

        public GameGraph(int offset_x, int offset_y, int width, int height, float node_w, float node_h)
        {
            this.offset_x = offset_x;
            this.offset_y = offset_y;
            this.width = width;
            this.height = height;
            this.node_w = node_w;
            this.node_h = node_h;

            graph = new List<GraphNode>();
            buildGraphGrid(offset_x, offset_y);
        }

        /// <summary>
        /// Recursive method used to build the graph nodes.
        /// </summary>
        /// <param name="node_x">Current node x</param>
        /// <param name="node_y">Current node y</param>
        /// <returns>Last accessed node</returns>
        private GraphNode buildGraphGrid(int node_x, int node_y)
        {
            if (node_x < offset_x || node_y < offset_y || node_x >= width || node_y >= height)
            {
                return null;
            }

            foreach (GraphNode n in graph)
            {
                if (n.getX() == node_x && n.getY() == node_y)
                {
                    return n;
                }
            }

            GraphNode node = new GraphNode(node_x, node_y, node_w, node_h);

            graph.Add(node);

            //cardinal directions
            GraphNode left = buildGraphGrid(node_x - 1, node_y);
            GraphNode up = buildGraphGrid(node_x, node_y - 1);
            GraphNode right = buildGraphGrid(node_x + 1, node_y);
            GraphNode down = buildGraphGrid(node_x, node_y + 1);
            //diagonal directions
            GraphNode upper_left = buildGraphGrid(node_x - 1, node_y - 1);
            GraphNode upper_right = buildGraphGrid(node_x + 1, node_y - 1);
            GraphNode lower_left = buildGraphGrid(node_x - 1, node_y + 1);
            GraphNode lower_right = buildGraphGrid(node_x + 1, node_y + 1);

            if (left != null) node.addNeighbor(left);
            if (up != null) node.addNeighbor(up);
            if (right != null) node.addNeighbor(right);
            if (down != null) node.addNeighbor(down);
            if (upper_left != null) node.addNeighbor(upper_left);
            if (upper_right != null) node.addNeighbor(upper_right);
            if (lower_left != null) node.addNeighbor(lower_left);
            if (lower_right != null) node.addNeighbor(lower_right);

            return node;
        }

        /// <summary>
        /// Method used to obtain a node at a certain position from the graph list.
        /// </summary>
        /// <param name="node_x">Node's x coordinate</param>
        /// <param name="node_y">Node's y coordinate</param>
        /// <returns>Node at the given coordinates, null if its out of bounds</returns>
        public GraphNode getNodeFromList(int node_x, int node_y)
        {
            foreach (GraphNode n in graph)
            {
                if (n.getX() == node_x && n.getY() == node_y)
                {
                    return n;
                }
            }

            return null;
        }

        /// <summary>
        /// Method used to obtain a node at a certain screen position from the graph list.  Gives the ability
        /// to use game pixel coordinates to obtain a node.
        /// </summary>
        /// <param name="screen_x">Node's x coordinate on the screen</param>
        /// <param name="screen_y">Node's y coordinate on the screen</param>
        /// <returns>Node at the given coordinates, null if its out of bounds</returns>
        public GraphNode getNodeFromScreen(int screen_x, int screen_y)
        {
            int node_x = (int)(screen_x / node_w);
            int node_y = (int)(screen_y / node_h);

            return getNodeFromList(node_x, node_y);
        }

        public GraphNode searchForEntity(GameEntity obj)
        {
            foreach (GraphNode n in graph)
            {
                if (n.getEntities().Contains(obj))
                {
                    return n;
                }
            }

            return null;
        }

        public GraphNode getClosestZombieNode(GraphNode init_pos)
        {
            //find all position locations for food
            List<GraphNode> options = new List<GraphNode>();
            foreach (GraphNode n in graph)
            {
                foreach (GameEntity e in n.getEntities())
                {
                    if(e.isTargetable() && !e.isEnemy())
                    {
                        options.Add(n);
                    }
                }
            }

            return getClosestNode(init_pos, options);
        }

        public List<GraphNode> getClosestZombiePath(GraphNode init_pos)
        {
            GraphNode target = getClosestZombieNode(init_pos);

            if (target == null)
            {
                return new List<GraphNode>();
            }

            //return the path to the closest item of food
            return getShortestPath(init_pos, target);
        }

        public GraphNode getClosestFoodNode(GraphNode init_pos)
        {
            //find all position locations for food
            List<GraphNode> options = new List<GraphNode>();
            foreach (GraphNode n in graph)
            {
                foreach (GameEntity e in n.getEntities())
                {
                    if (e.isEatable())
                    {
                        options.Add(n);
                        break;
                    }
                }
            }

            //return the path to the closest item of food
            return getClosestNode(init_pos, options);
        }

        public List<GraphNode> getClosestFoodPath(GraphNode init_pos)
        {
            GraphNode target = getClosestFoodNode(init_pos);

            if (target == null)
            {
                return new List<GraphNode>();
            }

            //return the path to the closest item of food
            return getShortestPath(init_pos, target);
        }

        public GraphNode getClosestNode(GraphNode init_pos, List<GraphNode> options)
        {
            //determine which location is closest
            if (options.Count == 0)
            {
                return null;
            }

            GraphNode shortest_location = options[0];
            for (int i = 1; i < options.Count; i++)
            {
                if (MathUtils.getInstance().getDistance(init_pos.getPosition(), options[i].getPosition()) <
                    MathUtils.getInstance().getDistance(init_pos.getPosition(), shortest_location.getPosition()))
                {
                    shortest_location = options[i];
                }
            }

            return shortest_location;
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public int getOffsetX()
        {
            return offset_x;
        }

        public int getOffsetY()
        {
            return offset_y;
        }

        public float getNodeWidth()
        {
            return node_w;
        }

        public float getNodeHeight()
        {
            return node_h;
        }

        public GraphNode getRandomNode()
        {
            if (graph.Count == 0)
            {
                return null;
            }

            Random rand = MathUtils.getInstance().getRandom();
            return graph[rand.Next(graph.Count)];
        }

        /// <summary>
        /// Method used to obtain the shortest path between 2 nodes.  This method uses the A* algorithm to
        /// calculate the shortest path.  If the path doesn't exist, an empty list is returned.
        /// </summary>
        /// <param name="init_pos">Starting node</param>
        /// <param name="final_pos">Ending node</param>
        /// <returns>Path from init_pos to final_pos</returns>
        public List<GraphNode> getShortestPath(GraphNode init_pos, GraphNode final_pos)
        {
            List<GraphNode> closed_set = new List<GraphNode>();
            List<GraphNode> open_set = new List<GraphNode>();

            //reset all the nodes
            foreach (GraphNode n in graph)
            {
                n.setGCost(0);
                n.setHCost((int)MathUtils.getInstance().getDistance(n.getPosition(), final_pos.getPosition())); 
                n.setFCost();
                n.setPrevious(null);
            }

            open_set.Add(init_pos);

            while (open_set.Count > 0)
            {
                GraphNode current = open_set[0];

                foreach (GraphNode n in open_set)
                {
                    if (n.getFCost() < current.getFCost())
                    {
                        current = n;
                    }
                }

                closed_set.Add(current);
                open_set.Remove(current);

                foreach (GraphNode n in current.getNeighbors())
                {
                    if (!closed_set.Contains(n))
                    {
                        if (!open_set.Contains(n))
                        {
                            open_set.Add(n);
                            n.setPrevious(current);
                            n.setGCost(current.getGCost() + 1);
                            n.setFCost();
                        }
                        else
                        {
                            int alt_i = open_set.IndexOf(n);
                            if (n.getGCost() < open_set[alt_i].getGCost())
                            {
                                n.setPrevious(current);
                                n.setGCost(current.getGCost() + 1);
                                n.setFCost();
                                open_set.Remove(n);
                                open_set.Add(n);
                            }
                        }
                    }

                    if(closed_set.Contains(final_pos))
                    {
                        //create path and return it
                        List<GraphNode> path = new List<GraphNode>();
                        Stack<GraphNode> raw_path = new Stack<GraphNode>();

                        //gather the initial path
                        int alt_i = closed_set.IndexOf(final_pos);
                        GraphNode cursor = closed_set[alt_i];
                        while (cursor != init_pos)
                        {
                            raw_path.Push(cursor);
                            cursor = cursor.getPrevious();
                        }

                        //place tiles in the correct order
                        while (raw_path.Count > 0)
                        {
                            path.Add(raw_path.Pop());
                        }

                        return path;
                    }
                }
            }

            return new List<GraphNode>();
        }

        public List<GraphNode> getGraph()
        {
            return graph;
        }
    }
}
