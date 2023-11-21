# UnDirected
class U_Graph:
	def __init__(self):
		self.graph = {}

	def add_vertex(self, vertex):
		if vertex not in self.graph:
			self.graph[vertex] = []

	def add_edge(self, vertex_1, vertex_2):
		# Add edges between two vertices (undirected graph)
		if vertex_1 in self.graph and vertex_2 in self.graph:
			self.graph[vertex_1].append(vertex_2)
			self.graph[vertex_2].append(vertex_1)

	def __str__(self):
		return str(self.graph)


	def find_star_center(self):
		# Counts edges
		degree = {}
		for vertex, edge in self.graph.items():
			degree[vertex] = len(edge)

		# Total nodes in graph 
		n = len(self.graph)

		# Find the center of star graph
		for node, deg in degree.items():
			if deg == n - 1:
				return node

		return -1


	def valid_path(self, n, edges, source, destination):
		graph = [[] for _ in range(n)]

		for u, v in edges:
			graph[u].append(v)
			graph[v].append(u)


		visited = [False] * n

		def dfs(node):
			if node == destination:
				return True

			visited[node] = True
			for neighbor in graph[node]:
				if not visited[neighbor]:
					if dfs(neighbor):
						return True

			return False
		return dfs(source)


	def count_provinces(self, is_connected):
		def dfs(city):
			visited.add(city)
			for neighbor, connected in enumerate(is_connected[city]):
				if connected and neighbor not in visited:
					dfs(neighbor)

		num_provinces = 0
		n = len(is_connected)
		visited = set()

		for city in range(n):
			if city not in visited:
				dfs(city)
				num_provinces += 1

		return num_provinces


# Undirected
#   A --- B
#    \   /
#      C


u_graph = U_Graph()

u_graph.add_vertex('A')
u_graph.add_vertex('B')
u_graph.add_vertex('C')

u_graph.add_edge('A', 'B')
u_graph.add_edge('B', 'C')
u_graph.add_edge('C', 'A')

print("Undirected:", u_graph)

# Center of star graph
star_graph = U_Graph()
star_graph.add_vertex(1)
star_graph.add_vertex(2)
star_graph.add_vertex(3)
star_graph.add_vertex(4)

star_graph.add_edge(1, 2)
star_graph.add_edge(2, 3)
star_graph.add_edge(4, 2)

center = star_graph.find_star_center()
print("Star Graph Center:", center)


# Find if path exists
find_graph = U_Graph()
find_graph.add_vertex(0)
find_graph.add_vertex(1)
find_graph.add_vertex(2)

find_graph.add_edge(1, 2)
find_graph.add_edge(0, 2)
find_graph.add_edge(1, 0)

find_path = find_graph.valid_path(3, [[0,1],[1,2],[2,0]], 0, 2)
print("Path Exists:", find_path)

find_graph_2 = U_Graph()
find_graph_2.add_vertex(0)
find_graph_2.add_vertex(1)
find_graph_2.add_vertex(2)

find_path_2 = find_graph_2.valid_path(3, [], 0, 2)
print("Path Exists:", find_path_2)


#  Number of Provinces
province_graph = U_Graph()
connections = [[1,1,0],[1,1,0],[0,0,1]]
num_provinces = province_graph.count_provinces(connections)
print("Total Province:", num_provinces)
print("\n")




# Directed
class D_Graph:
	def __init__(self):
		self.graph = {}

	def add_vertex(self, vertex):
		if vertex not in self.graph:
			self.graph[vertex] = []

	def add_edge(self, source, destination):
		#  Add a directed edge from source to destination.
		if source in self.graph and destination in self.graph:
			self.graph[source].append(destination)

	def __str__(self):
		return str(self.graph)


	def find_judge(self):
		n = len(self.graph)
		in_degree = [0] * (n + 1)
		out_degree = [0] * (n + 1)

		for source, destinations in self.graph.items():
			out_degree[source] += len(destinations)
			for destination in destinations:
				in_degree[destination] += 1

		for i in range(1, n + 1):
			if in_degree[i] == n - 1 and out_degree[i] == 0:
				return i

		return - 1


	def can_finish_courses(self, numCourses, prerequisites):
		# Create the graph based on prerequisites
		for course, prereq in prerequisites:
			self.add_vertex(course)
			self.add_vertex(prereq)
			self.add_edge(prereq, course)

		# Helper function for DFS
		def is_cyclic(node, visiting, visited):
			visiting.add(node)
			for neighbor in self.graph.get(node, []):
				if neighbor in visiting or (neighbor not in visited and is_cyclic(neighbor, visiting, visited)):
					return True
			visiting.remove(node)
			visited.add(node)
			return False

		visited = set()
		visiting = set()
		for course in range(numCourses):
			if course not in visited and is_cyclic(course, visiting, visited):
				return False
		return True


# Directed
#    A      
#    |  
#    v 
#    B --> C


d_graph = D_Graph()

d_graph.add_vertex('A')
d_graph.add_vertex('B')
d_graph.add_vertex('C')

d_graph.add_edge('A', 'B')
d_graph.add_edge('B', 'C')

print("Directed:", d_graph)


town_people = D_Graph()

# Add vertices
town_people.add_vertex(1)
town_people.add_vertex(2)

# Add directed edges to represent trust relationships
town_people.add_edge(1, 2)

# Find the town judge
judge = town_people.find_judge()
print("Town Judge:", judge)


#  Course Schedule
course_graph = D_Graph()
num_courses = 2
prerequisites = [[1, 0]]
print("Can Finish Course:", course_graph.can_finish_courses(num_courses, prerequisites))
print("\n")




# Weighted Graph
class Weighted_Graph():
	def __init__(self):
		self.graph = {}

	def add_vertex(self, vertex):
		if vertex not in self.graph:
			self.graph[vertex] = []

	def add_edge(self, source, destination, weight):
		# Add a weighted edge from source to destination.
		if source in self.graph and destination in self.graph:
			self.graph[source].append((destination, weight))

	def __str__(self):
		return str(self.graph)


	def dijkstra(self, start, end):
		# Dictionary to store the shortest distance to each vertex.
		shortest_distance = {vertex: float('inf') for vertex in self.graph}
		shortest_distance[start] = 0

		#  Dictionary to store the previous vertex on the shortest path.
		previous_vertex = {}

		#  Set to keep track of visited vertices:
		unvisited_vertices = set(self.graph)

		while unvisited_vertices:
			current_vertex = min(unvisited_vertices, key=lambda vertex: shortest_distance[vertex])

			for neighbor, weight in self.graph[current_vertex]:
				if weight + shortest_distance[current_vertex] < shortest_distance[neighbor]:
					shortest_distance[neighbor] = weight + shortest_distance[current_vertex]
					previous_vertex[neighbor] = current_vertex

			unvisited_vertices.remove(current_vertex)

		path, current_vertex = [], end
		while current_vertex is not None:
			path.insert(0, current_vertex)
			current_vertex = previous_vertex.get(current_vertex)

		return ' --> '.join(path), shortest_distance[end]


weighted_graph = Weighted_Graph()

weighted_graph.add_vertex('A')
weighted_graph.add_vertex('B')
weighted_graph.add_vertex('C')
weighted_graph.add_vertex('D')
weighted_graph.add_vertex('E')
weighted_graph.add_vertex('F')
weighted_graph.add_vertex('G')

# source, destination, and weight (respectively)
weighted_graph.add_edge('A','B', 2)
weighted_graph.add_edge('A','D', 3)
weighted_graph.add_edge('A','C', 1)
weighted_graph.add_edge('B','F', 4)
weighted_graph.add_edge('D','F', 5)
weighted_graph.add_edge('D','E', 1)
weighted_graph.add_edge('C','D', 6)
weighted_graph.add_edge('E','F', 2)
weighted_graph.add_edge('G','F', 3)

print("Weighted:", weighted_graph)
path, cost = weighted_graph.dijkstra('A','F')
print(f"Shortest Path: '{path}' Total Cost: {cost}")