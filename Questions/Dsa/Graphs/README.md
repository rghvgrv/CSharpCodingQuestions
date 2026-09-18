# Graphs

A graph is a set of points (nodes) connected by lines (edges): cities and roads, people and friendships, web pages and links. Most graph problems are solved by walking the graph with BFS or DFS.

## Words to know

- **Node** (vertex) and **edge** (connection).
- **Directed**: edges have a direction (A → B). **Undirected**: they go both ways.
- **Weighted**: edges have a cost, such as a distance.
- **Cycle**: a path that comes back to where it started.
- `V` = number of nodes, `E` = number of edges.

## Storing a graph: adjacency list

For each node, keep a list of its neighbors.

```csharp
// Nodes 0..n-1, edges like [0, 1] meaning 0 — 1
List<int>[] graph = new List<int>[n];
for (int i = 0; i < n; i++)
{
    graph[i] = new List<int>();
}
foreach (int[] edge in edges)
{
    graph[edge[0]].Add(edge[1]);
    graph[edge[1]].Add(edge[0]);   // skip this line for a directed graph
}
```

## The two ways to walk a graph

**BFS (breadth-first search)** uses a **queue** and visits nodes in rings: first the neighbors, then the neighbors' neighbors. It finds the **shortest path** when all edges cost the same.

**DFS (depth-first search)** uses recursion or a **stack**. It goes as deep as possible, then backs up. It's good for exploring everything, finding cycles and ordering tasks.

Both need a **visited** set so you don't go around in circles, and both cost `O(V + E)`.

## Algorithm cheat sheet

| Problem | Algorithm | Cost |
|---|---|---|
| Shortest path, equal edge costs | BFS | `O(V + E)` |
| Shortest path, non-negative weights | Dijkstra | `O((V + E) log V)` |
| Shortest path, negative weights | Bellman–Ford | `O(V · E)` |
| All pairs shortest paths | Floyd–Warshall | `O(V³)` |
| Order tasks with dependencies | Topological sort | `O(V + E)` |
| Are two nodes connected? | Union-Find | almost `O(1)` per query |
| Cheapest way to connect all nodes | Kruskal / Prim | `O(E log E)` |

A grid is a graph too: each cell connects to its 4 neighbors.
