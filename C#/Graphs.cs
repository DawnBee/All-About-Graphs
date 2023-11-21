using System;
using System.Linq;
using System.Collections.Generic;

namespace Graph_Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Undirected Graph */
            UGraph<string> uGraph = new UGraph<string>();

            uGraph.AddVertex("A");
            uGraph.AddVertex("B");
            uGraph.AddVertex("C");

            uGraph.AddEdge("A", "B");
            uGraph.AddEdge("B", "C");

            Console.WriteLine(uGraph);

            // Center of star graph
            UGraph<int> starGraph = new UGraph<int>();
            starGraph.AddVertex(1);
            starGraph.AddVertex(2);
            starGraph.AddVertex(3);
            starGraph.AddVertex(4);

            starGraph.AddEdge(1, 2);
            starGraph.AddEdge(2, 3);
            starGraph.AddEdge(4, 2);

            int? center = starGraph.FindStarCenter();
            Console.WriteLine($"Star Graph Center: {center}");

            // Checks if Path exists
            UGraph<int> findGraph = new UGraph<int>();
            findGraph.AddVertex(0);
            findGraph.AddVertex(1);
            findGraph.AddVertex(2);

            findGraph.AddEdge(1, 2);
            findGraph.AddEdge(0, 2);
            findGraph.AddEdge(1, 0);

            int[][] edges = { new int[] { 0, 1 }, new int[] { 1, 2 }, new int[] { 2, 0 } };
            bool findPath = findGraph.ValidPath(3, edges, 0, 2);
            Console.WriteLine($"Path Exists: {findPath}");

            UGraph<int> findGraph2 = new UGraph<int>();
            findGraph2.AddVertex(0);
            findGraph2.AddVertex(1);
            findGraph2.AddVertex(2);

            int[][] edges2 = new int[][] { };
            bool findPath2 = findGraph2.ValidPath(3, edges2, 0, 2);
            Console.WriteLine($"Path Exists: {findPath2}");

            // Number of Provinces
            UGraph<int> provinceGraph = new UGraph<int>();
            int[][] connections = new int[][] { new int[] { 1, 1, 0 }, new int[] { 1, 1, 0 }, new int[] { 0, 0, 1 } };
            int numProvinces = provinceGraph.CountProvinces(connections);
            Console.WriteLine($"Total Province: {numProvinces}");
            Console.WriteLine("\n");


            /* Directed Graph */
            DGraph dGraph = new DGraph();

            dGraph.AddVertex(10);
            dGraph.AddVertex(20);
            dGraph.AddVertex(30);

            dGraph.AddEdge(10, 20);
            dGraph.AddEdge(20, 30);

            Console.WriteLine(dGraph);

            // Create a directed graph for town people
            DGraph townPeople = new DGraph();

            townPeople.AddVertex(1);
            townPeople.AddVertex(2);

            townPeople.AddEdge(1, 2);

            // Find the town judge
            int judge = townPeople.FindJudge();
            Console.WriteLine($"Town Judge: {judge}");

            // Course Schedule
            DGraph courseGraph = new DGraph();
            int numCourses = 2;
            int[][] prerequisites = new int[][] { new int[] { 1, 0 } };
            Console.WriteLine($"Can Finish Course: {courseGraph.CanFinishCourses(numCourses, prerequisites)}");
            Console.WriteLine("\n");


            /* Weighted Graph */
            WeightedGraph<string> weightedGraph = new WeightedGraph<string>();

            weightedGraph.AddVertex("A");
            weightedGraph.AddVertex("B");
            weightedGraph.AddVertex("C");
            weightedGraph.AddVertex("D");
            weightedGraph.AddVertex("E");
            weightedGraph.AddVertex("F");
            weightedGraph.AddVertex("G");

            // Add edges with weights
            weightedGraph.AddEdge("A", "B", 2);
            weightedGraph.AddEdge("A", "D", 3);
            weightedGraph.AddEdge("A", "C", 1);
            weightedGraph.AddEdge("B", "F", 4);
            weightedGraph.AddEdge("D", "F", 5);
            weightedGraph.AddEdge("D", "E", 1);
            weightedGraph.AddEdge("C", "D", 6);
            weightedGraph.AddEdge("E", "F", 2);
            weightedGraph.AddEdge("G", "F", 3);

            Console.WriteLine(weightedGraph);
        }
    }

    // Undirected
    class UGraph<T>
    {
        private Dictionary<T, List<T>> graph = new Dictionary<T, List<T>>();

        public void AddVertex(T vertex)
        {
            if (!graph.ContainsKey(vertex))
            {
                graph[vertex] = new List<T>();
            }
        }

        public void AddEdge(T vertex1, T vertex2)
        {
            if (graph.ContainsKey(vertex1) && graph.ContainsKey(vertex2))
            {
                graph[vertex1].Add(vertex2);
                graph[vertex2].Add(vertex1);
            }
        }

        public override string ToString()
        {
            string result = "Undirected: { ";
            foreach (var kvp in graph)
            {
                result += $"'{kvp.Key}': [{string.Join(", ", kvp.Value.Select(v => $"'{v}'"))}], ";
            }
            result = result.TrimEnd(' ', ',') + " }";
            return result;
        }

        public int? FindStarCenter()
        {
            // Counts edges
            Dictionary<T, int> degree = new Dictionary<T, int>();
            foreach (var vertex in graph.Keys)
            {
                degree[vertex] = graph[vertex].Count;
            }

            // Total nodes in the graph
            int n = graph.Count;

            // Find the center of the star graph
            foreach (var node in degree)
            {
                if (node.Value == n - 1)
                {
                    if (node.Key is int intValue)
                    {
                        return intValue;
                    }
                }
            }

            return null; // Return null when there's no center
        }

        public bool ValidPath(int n, int[][] edges, int source, int destination)
        {
            List<int>[] graph = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                graph[i] = new List<int>();
            }

            foreach (var edge in edges)
            {
                int u = edge[0];
                int v = edge[1];
                graph[u].Add(v);
                graph[v].Add(u);
            }

            bool[] visited = new bool[n];

            bool Dfs(int node)
            {
                if (node == destination)
                {
                    return true;
                }

                visited[node] = true;
                foreach (var neighbor in graph[node])
                {
                    if (!visited[neighbor])
                    {
                        if (Dfs(neighbor))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            return Dfs(source);
        }

        public int CountProvinces(int[][] isConnected)
        {
            HashSet<int> visited = new HashSet<int>(); // Declare visited set here

            void Dfs(int city)
            {
                visited.Add(city);
                for (int neighbor = 0; neighbor < isConnected[city].Length; neighbor++)
                {
                    if (isConnected[city][neighbor] == 1 && !visited.Contains(neighbor))
                    {
                        Dfs(neighbor);
                    }
                }
            }

            int numProvinces = 0;
            int n = isConnected.Length;

            for (int city = 0; city < n; city++)
            {
                if (!visited.Contains(city))
                {
                    Dfs(city);
                    numProvinces++;
                }
            }

            return numProvinces;
        }

    }


    // Directed
    public class DGraph
    {
        private Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();

        public void AddVertex(int vertex)
        {
            if (!graph.ContainsKey(vertex))
            {
                graph[vertex] = new List<int>();
            }
        }

        public void AddEdge(int source, int destination)
        {
            if (graph.ContainsKey(source) && graph.ContainsKey(destination))
            {
                graph[source].Add(destination);
            }
        }

        public override string ToString()
        {
            string result = "Directed: { ";
            foreach (var kvp in graph)
            {
                result += $"'{kvp.Key}': [{string.Join(", ", kvp.Value.Select(v => $"'{v}'"))}], ";
            }
            result = result.TrimEnd(' ', ',') + " }";
            return result;
        }

        public int FindJudge()
        {
            int n = graph.Count;
            int[] inDegree = new int[n + 1];
            int[] outDegree = new int[n + 1];

            foreach (var source in graph.Keys)
            {
                outDegree[source] += graph[source].Count;
                foreach (var destination in graph[source])
                {
                    inDegree[destination]++;
                }
            }

            for (int i = 1; i <= n; i++)
            {
                if (inDegree[i] == n - 1 && outDegree[i] == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool CanFinishCourses(int numCourses, int[][] prerequisites)
        {
            // Create the graph based on prerequisites
            foreach (var prereq in prerequisites)
            {
                int course = prereq[1];
                int preReqCourse = prereq[0];

                AddVertex(course);
                AddVertex(preReqCourse);
                AddEdge(preReqCourse, course);
            }

            // Helper function for DFS
            bool IsCyclic(int node, HashSet<int> visiting, HashSet<int> visited)
            {
                visiting.Add(node);
                foreach (var neighbor in graph.ContainsKey(node) ? graph[node] : new List<int>())
                {
                    if (visiting.Contains(neighbor) || (!visited.Contains(neighbor) && IsCyclic(neighbor, visiting, visited)))
                    {
                        return true;
                    }
                }
                visiting.Remove(node);
                visited.Add(node);
                return false;
            }

            var visited = new HashSet<int>();
            var visiting = new HashSet<int>();
            for (int course = 0; course < numCourses; course++)
            {
                if (!visited.Contains(course) && IsCyclic(course, visiting, visited))
                {
                    return false;
                }
            }
            return true;
        }

    }


    // Weighted Graph
    public class WeightedGraph<T>
    {
        private Dictionary<T, List<Tuple<T, int>>> graph = new Dictionary<T, List<Tuple<T, int>>>();

        public void AddVertex(T vertex)
        {
            if (!graph.ContainsKey(vertex))
            {
                graph[vertex] = new List<Tuple<T, int>>();
            }
        }

        public void AddEdge(T source, T destination, int weight)
        {
            if (graph.ContainsKey(source) && graph.ContainsKey(destination))
            {
                graph[source].Add(new Tuple<T, int>(destination, weight));
            }
        }

        public override string ToString()
        {
            string result = "Weighted: { ";
            foreach (var kvp in graph)
            {
                var edges = string.Join(", ", kvp.Value.Select(t => $"('{t.Item1}', {t.Item2})"));
                result += $"'{kvp.Key}': [ {edges} ], ";
            }
            result = result.TrimEnd(' ', ',') + " }";
            return result;
        }
    }
}
