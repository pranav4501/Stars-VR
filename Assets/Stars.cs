

using UnityEngine;
using System.Globalization;
using System.Collections.Generic;

public class Stars : MonoBehaviour
{
    public TextAsset csvFile; // Assign your CSV file in the Unity Inspector
    public GameObject spherePrefab; // Assign your sphere prefab in the Unity Inspector
    public TextAsset constFile;
    private Dictionary<int, Vector3> starPositionsByHIP = new Dictionary<int, Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        // Check if the prefab and file are assigned
        if (spherePrefab == null || csvFile == null)
        {
            Debug.LogError("Prefab or CSV File is not assigned!");
            return;
        }

        addStars();
        CreateConstellations();
    }

    private void addStars()
    {
        string[] lines = csvFile.text.Split('\n');
        NumberFormatInfo format = new NumberFormatInfo();
        format.NumberDecimalSeparator = ".";

        for (int i = 0; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            if (values.Length < 5) continue;
            if (float.TryParse(values[3], NumberStyles.Any, format, out float x) &&
                float.TryParse(values[4], NumberStyles.Any, format, out float y) &&
                float.TryParse(values[5], NumberStyles.Any, format, out float z))
            {
                Vector3 position = new Vector3(x, z, y);
                Instantiate(spherePrefab, position, Quaternion.identity);
                if (int.TryParse(values[1], out int hipId))
                {
                    starPositionsByHIP.Add(hipId, position);
                }
            }
        }
    }

    private void CreateConstellations(){
        string[] lines = constFile.text.Split('\n');
        {
            string[] values = lines[59].Split(' ');
            for (int i = 3; i < values.Length-1; i+=2)
            {   
                if (int.TryParse(values[i], out int hipId1) && int.TryParse(values[i+1], out int hipId2))
                {
                    
                    Vector3 position1 = starPositionsByHIP[hipId1];
                    Vector3 position2 = starPositionsByHIP[hipId2];

                    // Create a line between the two stars
                    GameObject line = new GameObject("Line");
                    LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, position1);
                    lineRenderer.SetPosition(1, position2);
                    lineRenderer.startWidth = 0.1f;
                    lineRenderer.endWidth = 0.1f;
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.startColor = Color.white;
                    lineRenderer.endColor = Color.white;
                    
                }
            }
        }
    }

}