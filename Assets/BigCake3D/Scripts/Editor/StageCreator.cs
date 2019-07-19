using UnityEngine;
using UnityEditor;

public class StageCreator : EditorWindow
{
    #region Variables
    private GameObject[] cakePrefabs = new GameObject[5];
    private GameObject obstacles = null;
    private GameObject toppingPrefab = null;
    private GameObject[] creamLayers = new GameObject[3];

    private int stageCount = 0;
    private int layerCount = 0;

    private Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
    private float cakeStep = 0.25f;
    private float creamStep = 0.35f;

    private int block = 2;
    #endregion

    #region Builtin Methods
    [MenuItem("Tools/Big Cake/Stage Creator")]
    private static void Init()
    {
        StageCreator stageCreator = (StageCreator)GetWindow(typeof(StageCreator));
        stageCreator.Show();
    }

    private void OnGUI()
    {
        GUILayout.Space(20);
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < cakePrefabs.Length; i++)
        {
            cakePrefabs[i] =
                (GameObject)EditorGUILayout.ObjectField("Pandispanya Prefab : ", cakePrefabs[i], typeof(Object), true);
        }
        GUILayout.Space(15);
        obstacles = (GameObject)EditorGUILayout.ObjectField("Obstacles Prefab : ", obstacles, typeof(Object), true);

        toppingPrefab = (GameObject)EditorGUILayout.ObjectField("Topping Prefab : ", toppingPrefab, typeof(Object), true);

        GUILayout.Space(15);
        EditorGUILayout.LabelField("Krema Katmanları");
        for (int i = 0; i < creamLayers.Length; i++)
        {
            creamLayers[i] =
                (GameObject)EditorGUILayout.ObjectField($"Krema Katmanı Prefab {i + 1} : ", creamLayers[i], typeof(Object), true);
        }

        GUILayout.Space(10);
        stageCount = EditorGUILayout.IntField("Var Olan Stage Sayısı : ", stageCount);

        GUILayout.Space(10);
        layerCount = EditorGUILayout.IntField("Pasta Katman Sayısı : ", layerCount);

        GUILayout.Space(25);
        if (GUILayout.Button("Create Stage"))
        {
            CreateStage();
        }
        EditorGUILayout.EndVertical();
    }
    #endregion

    #region Custom Methods
    private void CreateStage()
    {
        GameObject stage = new GameObject();
        GameObject cakeLayers = new GameObject();
        GameObject obstacle = Instantiate(obstacles, startPosition, Quaternion.Euler(0, 0, 0));
        GameObject topping = Instantiate(toppingPrefab);

        cakeLayers.transform.SetParent(stage.transform);
        obstacle.transform.SetParent(stage.transform);

        obstacle.name = "Obstacles";
        cakeLayers.name = "CakeLayers";
        stage.name = "Stage_" + (stageCount + 1);

        for (int i = 0; i < layerCount; i++)
        {
            int cakeIndex = Random.Range(0, cakePrefabs.Length);
            GameObject cake = Instantiate(cakePrefabs[cakeIndex], startPosition, Quaternion.Euler(0, 100, 0));

            int index = Random.Range(0, creamLayers.Length);
            startPosition.y += creamStep;
            GameObject cream = Instantiate(creamLayers[index], startPosition, Quaternion.Euler(0, 100, 0));

            startPosition.y += cakeStep;

            cake.transform.SetParent(cakeLayers.transform);
            cream.transform.SetParent(cakeLayers.transform);
        }
        startPosition.y += cakeStep;
        topping.transform.position = startPosition;
        topping.transform.rotation = Quaternion.Euler(270, 0, 0);
        topping.transform.SetParent(cakeLayers.transform);
        stageCount++;
        if (stageCount % block == 0 && layerCount <= 7)
        {
            layerCount++;
        }
        startPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }
    #endregion
}
