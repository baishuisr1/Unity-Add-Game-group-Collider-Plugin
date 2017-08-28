using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

public class AddCollider : EditorWindow
{
    [MenuItem("Window/辅助功能")]
    static void Window()
    {
        AddCollider newWindow = EditorWindow.GetWindow<AddCollider>("辅助功能");
        newWindow.Show();
    }


    private string zoom = "100";

    void OnGUI()
    {
        GUILayout.Label("碰撞器缩放_百分比(默认100%)");
        zoom = GUILayout.TextField(zoom);
        if (GUILayout.Button("添加碰撞体积"))
        {
            float zoomFloat = Convert.ToSingle(zoom)/100;
            if (zoomFloat > 1.5)
            {
                Debug.Log("请输入1-150的数");
                return;
            }
            if (zoomFloat < 0.1)
            {
                Debug.Log("请输入1-150的数");
                return;
            }
            GameObject gameObject = Selection.activeGameObject;
            if (gameObject == null)
            {
                Debug.Log("请选择一个物体");
                return;
            }

            Vector3 vector3;
            vector3 = gameObject.transform.position;
            gameObject.transform.position =new Vector3(0,0,0);

            float Y_Max = -5000;
            float Y_Mix = 5000;

            float X_Max = -5000;
            float X_Mix = 5000;

            float Z_Max = -5000;
            float Z_Mix = 5000;

            foreach (Transform VARIABLE in gameObject.transform)
            {
                BoxCollider boxCollider = VARIABLE.transform.gameObject.AddComponent<BoxCollider>();
                Vector3[] vector3s = GetBoxColliderVertexPositions(boxCollider);
                for (int i = 0; i < vector3s.Length; i++)
                {
                    if (vector3s[i].y >= Y_Max)
                    {
                        Y_Max = vector3s[i].y;
                    }

                    if (vector3s[i].y <= Y_Mix)
                    {
                        Y_Mix = vector3s[i].y;
                    }

                    if (vector3s[i].x >= X_Max)
                    {
                        X_Max = vector3s[i].x;
                    }

                    if (vector3s[i].x <= X_Mix)
                    {
                        X_Mix = vector3s[i].x;
                    }

                    if (vector3s[i].z >= Z_Max)
                    {
                        Z_Max = vector3s[i].z;
                    }

                    if (vector3s[i].z <= Z_Mix)
                    {
                        Z_Mix = vector3s[i].z;
                    }
                }
            }


            Y_Max *= zoomFloat;
            Y_Mix *= zoomFloat;

            X_Max *= zoomFloat;
            X_Mix *= zoomFloat;

            Z_Max *= zoomFloat;
            Z_Mix *= zoomFloat;


            gameObject.AddComponent<MeshCollider>();
            Mesh mesh = new Mesh();
            mesh.name = gameObject.name;
            int[] triangles = new int[36];
            Vector3[] vertices = new Vector3[24];

            Vector3 verticeUp1 = new Vector3(X_Mix, Y_Max, Z_Max);
            vertices[3] = verticeUp1;
            vertices[9] = verticeUp1;
            vertices[21] = verticeUp1;
            Vector3 verticeUp2 = new Vector3(X_Mix, Y_Max, Z_Mix);
            vertices[5] = verticeUp2;
            vertices[11] = verticeUp2;
            vertices[20] = verticeUp2;
            Vector3 verticeUp3 = new Vector3(X_Max, Y_Max, Z_Mix);
            vertices[4] = verticeUp3;
            vertices[10] = verticeUp3;
            vertices[18] = verticeUp3;
            Vector3 verticeUp4 = new Vector3(X_Max, Y_Max, Z_Max);
            vertices[2] = verticeUp4;
            vertices[8] = verticeUp4;
            vertices[19] = verticeUp4;

            Vector3 verticeDown1 = new Vector3(X_Max, Y_Mix, Z_Max);
            vertices[0] = verticeDown1;
            vertices[14] = verticeDown1;
            vertices[17] = verticeDown1;
            Vector3 verticeDown2 = new Vector3(X_Mix, Y_Mix, Z_Max);
            vertices[1] = verticeDown2;
            vertices[15] = verticeDown2;
            vertices[23] = verticeDown2;
            Vector3 verticeDown3 = new Vector3(X_Mix, Y_Mix, Z_Mix);
            vertices[7] = verticeDown3;
            vertices[13] = verticeDown3;
            vertices[22] = verticeDown3;
            Vector3 verticeDown4 = new Vector3(X_Max, Y_Mix, Z_Mix);
            vertices[6] = verticeDown4;
            vertices[12] = verticeDown4;
            vertices[16] = verticeDown4;

            int currentCount = 0;
            for (int i = 0; i < 24; i = i + 4)
            {
                //三角形1
                triangles[currentCount++] = i;
                triangles[currentCount++] = i + 3;
                triangles[currentCount++] = i + 1;
                //三角形2        
                triangles[currentCount++] = i;
                triangles[currentCount++] = i + 2;
                triangles[currentCount++] = i + 3;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;

            foreach (Transform VARIABLE in gameObject.transform)
            {
                DestroyImmediate(VARIABLE.GetComponent<BoxCollider>());
            }
            gameObject.transform.position = vector3;
        }
    }

    Vector3[] GetBoxColliderVertexPositions(BoxCollider boxcollider)
    {
        var vertices = new Vector3[8];
        //下面4个点
        vertices[0] =
            boxcollider.transform.TransformPoint(boxcollider.center +
                                                 new Vector3(boxcollider.size.x, -boxcollider.size.y, boxcollider.size.z)*
                                                 0.5f);
        vertices[1] =
            boxcollider.transform.TransformPoint(boxcollider.center +
                                                 new Vector3(-boxcollider.size.x, -boxcollider.size.y,
                                                     boxcollider.size.z)*0.5f);
        vertices[2] =
            boxcollider.transform.TransformPoint(boxcollider.center +
                                                 new Vector3(-boxcollider.size.x, -boxcollider.size.y,
                                                     -boxcollider.size.z)*0.5f);
        vertices[3] =
            boxcollider.transform.TransformPoint(boxcollider.center +
                                                 new Vector3(boxcollider.size.x, -boxcollider.size.y,
                                                     -boxcollider.size.z)*0.5f);
        //上面4个点
        vertices[4] =
            boxcollider.transform.TransformPoint(boxcollider.center +
                                                 new Vector3(boxcollider.size.x, boxcollider.size.y, boxcollider.size.z)*
                                                 0.5f);
        vertices[5] =
            boxcollider.transform.TransformPoint(boxcollider.center +
                                                 new Vector3(-boxcollider.size.x, boxcollider.size.y, boxcollider.size.z)*
                                                 0.5f);
        vertices[6] =
            boxcollider.transform.TransformPoint(boxcollider.center +
                                                 new Vector3(-boxcollider.size.x, boxcollider.size.y,
                                                     -boxcollider.size.z)*0.5f);
        vertices[7] =
            boxcollider.transform.TransformPoint(boxcollider.center +
                                                 new Vector3(boxcollider.size.x, boxcollider.size.y, -boxcollider.size.z)*
                                                 0.5f);

        return vertices;
    }
}