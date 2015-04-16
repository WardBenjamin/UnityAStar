using UnityEngine;

public static class Pathfinder
{
    /// <summary>
    /// Method that switfly finds the best path from start to end.
    /// </summary>
    /// <returns>The starting breadcrumb traversable via .next to the end or null if there is no path</returns>    
    public static BreadCrumb FindPath(Vector2 start, Vector2 end)
    {
        //note we just flip start and end here so you don't have to.            
        return FindPathReversed(end, start);
    }

    /// <summary>
    /// Method that switfly finds the best path from start to end. Doesn't reverse outcome
    /// </summary>
    /// <returns>The end breadcrump where each .next is a step back)</returns>
    private static BreadCrumb FindPathReversed(Vector2 start, Vector2 end)
    {
        var world = TileManager.instance;

        MinHeap<BreadCrumb> openList = new MinHeap<BreadCrumb>();
        BreadCrumb[,] brWorld = new BreadCrumb[(int)world.size.x, (int)world.size.y];
        BreadCrumb node;
        Vector2 temp;
        int cost;
        int diff;

        BreadCrumb current = new BreadCrumb(start);
        current.cost = 0;

        BreadCrumb finish = new BreadCrumb(end);
        brWorld[(int)current.position.x, (int)current.position.y] = current;
        openList.Insert(current);

        while (openList.Count > 0)
        {
            //Find best item and switch it to the 'closedList'
            current = openList.RemoveRoot();
            current.onClosedList = true;

            //Find neighbours
            for (int i = 0; i < surrounding.Length; i++)
            {
                temp = new Vector2(current.position.x + surrounding[i].x, current.position.y + surrounding[i].y);
                if (world.TileIsPassable((int)temp.x, (int)temp.y))
                {
                    //Check if we've already examined a neighbour, if not create a new node for it.
                    if (brWorld[(int)temp.x, (int)temp.y] == null)
                    {
                        node = new BreadCrumb(temp);
                        brWorld[(int)temp.x, (int)temp.y] = node;
                    }
                    else
                    {
                        node = brWorld[(int)temp.x, (int)temp.y];
                    }

                    //If the node is not on the 'closedList' check it's new score, keep the best
                    if (!node.onClosedList)
                    {
                        diff = 0;
                        if (current.position.x != node.position.x)
                        {
                            diff += 1;
                        }
                        if (current.position.y != node.position.y)
                        {
                            diff += 1;
                        }

                        int distance = (int)Mathf.Pow(Mathf.Max(Mathf.Abs(end.x - node.position.x), Mathf.Abs(end.y - node.position.y)), 2);
                        //int distance1 = (int)Mathf.Pow(Vector2.Distance(new Vector2(node.position.x, node.position.y), new Vector2(end.x, end.y)), 2);
                        cost = current.cost + diff + distance;
                        //cost = current.cost + diff + (int)Mathf.Pow(Vector2.Distance(node.position, end), 2);


                        if (cost < node.cost)
                        {
                            node.cost = cost;
                            node.next = current;
                        }

                        //If the node wasn't on the openList yet, Insert it 
                        if (!node.onOpenList)
                        {
                            //Check to see if we're done
                            if (node.Equals(finish))
                            {
                                node.next = current;
                                return node;
                            }
                            node.onOpenList = true;
                            openList.Insert(node);
                        }
                    }
                }
            }
        }
        return null; //no path found
    }

    private static Vector2[] surrounding = new Vector2[]
    {                         
		new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0,-1)
	};
}