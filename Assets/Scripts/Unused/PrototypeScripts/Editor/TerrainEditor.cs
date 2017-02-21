//using System.Collections.Generic;
//using System.Diagnostics;
//using UnityEditor;
//using UnityEngine;
//using Debug = UnityEngine.Debug;

//[CustomEditor(typeof(CustomTerrainCollider))]
//public class TerrainEditor : Editor
//{
//    private CustomTerrainCollider terrainCollider;
//    private TerrainData terrainData;
//    private float AlphaCutoff;

//    private int?[,] _vertexCache;
//    private int _currentNewIndex;

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        if (GUILayout.Button("Bake Collider"))
//        {
//            Bake();
//        }
//    }

//    private void Bake()
//    {
//        _currentNewIndex = 0;

//        terrainCollider = (target as CustomTerrainCollider);
//        terrainData = terrainCollider.gameObject.GetComponent<Terrain>().terrainData;
//        AlphaCutoff = (float)terrainCollider.AlphaCutoff;

//        if (terrainData.heightmapHeight - 1 != terrainData.alphamapHeight || terrainData.heightmapWidth - 1 != terrainData.alphamapWidth)
//        {
//            Debug.LogError("Error: Baking terrain colliders can only be performed if the control texture dimensions are one less than the heightmap dimensions");
//            return;
//        }

//        float[,,] alphaMaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);


//        _vertexCache = new int?[terrainData.heightmapWidth, terrainData.heightmapHeight];

//        List<Vector3> vertices = new List<Vector3>(65000);
//        List<int> indices = new List<int>(65000);

//        Stopwatch watch = new Stopwatch();
//        watch.Start();

//        int blockSize = 253;
//        int xBlockCount = terrainData.heightmapWidth / blockSize + Mathf.CeilToInt(terrainData.heightmapWidth / blockSize);
//        int yBlockCount = terrainData.heightmapHeight / blockSize + Mathf.CeilToInt(terrainData.heightmapHeight / blockSize);

//        for (int blockX = 0; blockX < xBlockCount; blockX++)
//        {
//            for (int blockY = 0; blockY < yBlockCount; blockY++)
//            {
//                _vertexCache = new int?[terrainData.heightmapWidth, terrainData.heightmapHeight];
//                int startX = blockX * blockSize;
//                int startY = blockY * blockSize;
//                int endX = Mathf.Min(startX + blockSize, terrainData.heightmapWidth - 1);
//                int endY = Mathf.Min(startY + blockSize, terrainData.heightmapHeight - 1);

//                for (int x = startX; x < endX; x++)
//                {
//                    for (int y = startY; y < endY; y++)
//                    {
//                        int x2 = x;
//                        int y2 = y;

//                        int @case = 0;
//                        int p1 = 0;
//                        p1 += (1 - SampleAlphaMap(y2, x2, alphaMaps)) < AlphaCutoff ? 1 : 0;
//                        p1 += (1 - SampleAlphaMap(y2 + 1, x2, alphaMaps)) < AlphaCutoff ? 1 : 0;
//                        p1 += (1 - SampleAlphaMap(y2 + 1, x2 + 1, alphaMaps)) < AlphaCutoff ? 1 : 0;
//                        p1 += (1 - SampleAlphaMap(y2, x2 + 1, alphaMaps)) < AlphaCutoff ? 1 : 0;

//                        if (p1 > 2)
//                        {
//                            @case += 1;
//                        }

//                        int p2 = 0;
//                        p2 += (1 - SampleAlphaMap(y2, x2 + 1, alphaMaps)) < AlphaCutoff ? 1 : 0;
//                        p2 += (1 - SampleAlphaMap(y2 + 1, x2 + 1, alphaMaps)) < AlphaCutoff ? 1 : 0;
//                        p2 += (1 - SampleAlphaMap(y2 + 1, x2 + 2, alphaMaps)) < AlphaCutoff ? 1 : 0;
//                        p2 += (1 - SampleAlphaMap(y2, x2 + 2, alphaMaps)) < AlphaCutoff ? 1 : 0;

//                        if (p2 > 2)
//                        {
//                            @case += 2;
//                        }

//                        int p3 = 0;
//                        p3 += (1 - SampleAlphaMap(y2 + 1, x2 + 1, alphaMaps)) < AlphaCutoff ? 1 : 0;
//                        p3 += (1 - SampleAlphaMap(y2 + 2, x2 + 1, alphaMaps)) < AlphaCutoff ? 1 : 0;
//                        p3 += (1 - SampleAlphaMap(y2 + 2, x2 + 2, alphaMaps)) < AlphaCutoff ? 1 : 0;
//                        p3 += (1 - SampleAlphaMap(y2 + 1, x2 + 2, alphaMaps)) < AlphaCutoff ? 1 : 0;

//                        if (p3 > 2)
//                        {
//                            @case += 4;
//                        }

//                        int p4 = 0;
//                        p4 += (1 - SampleAlphaMap(y2 + 1, x2, alphaMaps)) < AlphaCutoff ? 1 : 0;
//                        p4 += (1 - SampleAlphaMap(y2 + 2, x2, alphaMaps)) < AlphaCutoff ? 1 : 0;
//                        p4 += (1 - SampleAlphaMap(y2 + 2, x2 + 1, alphaMaps)) < AlphaCutoff ? 1 : 0;
//                        p4 += (1 - SampleAlphaMap(y2 + 1, x2 + 1, alphaMaps)) < AlphaCutoff ? 1 : 0;

//                        if (p4 > 2)
//                        {
//                            @case += 8;
//                        }

//                        GenerateTriangles(x, y, @case, vertices, indices);
//                    }
//                }

//                if (vertices.Count != 0)
//                {
//                    Stopwatch goCreationWatch = new Stopwatch();
//                    Debug.Log("Generating objects!");

//                    goCreationWatch.Start();

//                    Mesh collisionMesh = new Mesh();

//                    Vector3[] vertArray = vertices.ToArray();
//                    Debug.Log("Time after vert copy: " + goCreationWatch.ElapsedMilliseconds);
//                    int[] triArray = indices.ToArray();
//                    Debug.Log("Time after tri copy: " + goCreationWatch.ElapsedMilliseconds);
//                    collisionMesh.vertices = vertArray;
//                    Debug.Log("Time after vert assignment: " + goCreationWatch.ElapsedMilliseconds);
//                    collisionMesh.triangles = triArray;
//                    Debug.Log("Time after tri assignment: " + goCreationWatch.ElapsedMilliseconds);



//                    GameObject meshGO = new GameObject("MeshCollider");
//                    meshGO.isStatic = true;
//                    meshGO.transform.parent = terrainCollider.transform;
//                    Debug.Log("Time after go creation: " + goCreationWatch.ElapsedMilliseconds);

//                    MeshCollider meshCollider = meshGO.AddComponent<MeshCollider>();
//                    meshCollider.convex = false;
//                    meshCollider.smoothSphereCollisions = true;
//                    meshCollider.sharedMesh = collisionMesh;
//                    Debug.Log("Time after mesh collider assignment: " + goCreationWatch.ElapsedMilliseconds);


//                    Debug.Log("Time to create mesh collider: " + goCreationWatch.ElapsedMilliseconds);
//                }
//                vertices.Clear();
//                indices.Clear();
//                _vertexCache = new int?[terrainData.heightmapWidth, terrainData.heightmapHeight];
//                _currentNewIndex = 0;
//            }
//        }
//        watch.Stop();
//        Debug.Log("Took " + watch.Elapsed.TotalSeconds + " seconds");
//    }

//    private float SampleAlphaMap(int x, int y, float[,,] alphaMap)
//    {
//        int alphaMapXMin = (int)x - 1;
//        int alphaMapYMin = (int)y - 1;
//        alphaMapXMin = Mathf.Clamp(alphaMapXMin, 0, terrainData.alphamapWidth - 1);
//        alphaMapYMin = Mathf.Clamp(alphaMapYMin, 0, terrainData.alphamapHeight - 1);

//        return alphaMap[alphaMapXMin, alphaMapYMin, 0];
//    }

//    private void GenerateTriangles(int x, int y, int @case, List<Vector3> triangles, List<int> indices)
//    {
//        //Vector3 p1 = GetPosition(x, y);
//        //Vector3 p2 = GetPosition(x + 1, y);
//        //Vector3 p3 = GetPosition(x + 1, y + 1);
//        //Vector3 p4 = GetPosition(x, y + 1);

//        Vector2 p1 = new Vector2(x, y);
//        Vector2 p2 = new Vector2(x + 1, y);
//        Vector2 p3 = new Vector2(x + 1, y + 1);
//        Vector2 p4 = new Vector2(x, y + 1);
//        int indexStart = indices.Count;
//        switch (@case)
//        {
//            // None are alpha'd out.
//            case 0:
//                AddVertex(p1, triangles, indices);
//                AddVertex(p3, triangles, indices);
//                AddVertex(p2, triangles, indices);
//                AddVertex(p1, triangles, indices);
//                AddVertex(p4, triangles, indices);
//                AddVertex(p3, triangles, indices);
//                break;
//            // Only the top left point is alpha'd out
//            case 1:
//                AddVertex(p2, triangles, indices);
//                AddVertex(p3, triangles, indices);
//                AddVertex(p4, triangles, indices);
//                break;
//            // Only the top right point is alpha'd out
//            case 2:
//                AddVertex(p1, triangles, indices);
//                AddVertex(p3, triangles, indices);
//                AddVertex(p4, triangles, indices);
//                break;
//            // Only the bottom right point is alpha'd out
//            case 4:
//                AddVertex(p1, triangles, indices);
//                AddVertex(p4, triangles, indices);
//                AddVertex(p2, triangles, indices);
//                break;
//            // Only the bottom left point is alpha'd out
//            case 8:
//                AddVertex(p1, triangles, indices);
//                AddVertex(p3, triangles, indices);
//                AddVertex(p2, triangles, indices);
//                break;
//        }
//    }

//    private void AddVertex(Vector2 vertexCoordinates, List<Vector3> triangles, List<int> indices)
//    {
//        int x = (int)vertexCoordinates.x;
//        int y = (int)vertexCoordinates.y;
//        if (_vertexCache[x, y].HasValue)
//        {
//            indices.Add(_vertexCache[x, y].Value);
//        }
//        else
//        {
//            triangles.Add(GetPosition(x, y));
//            indices.Add(_currentNewIndex);
//            _vertexCache[x, y] = _currentNewIndex;
//            _currentNewIndex++;
//        }
//    }

//    private Vector3 GetPosition(int x, int y)
//    {
//        Vector3 point = new Vector3();

//        point.x = (float)(x) * terrainData.heightmapScale.x;
//        point.z = (float)(y) * terrainData.heightmapScale.z;
//        point.y = terrainData.GetHeight(x, y);

//        return point;
//    }
//}
