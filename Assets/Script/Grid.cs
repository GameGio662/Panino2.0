using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
    public GameObject cubePrefab; // Assicurati di assegnare un prefab di un cubo nell'Inspector

    void Awake()
    {
        DestroyGrid();
        GenerateGrid(5, 5);
    }

    void DestroyGrid()
    {
        // Rimuovi tutti gli oggetti figlio (cubi) dell'oggetto corrente
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    void GenerateGrid(int rows, int columns)
    {
        float spacing = 1.2f; // Spaziatura tra i cubi

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = new Vector3(col * spacing, 0f, row * spacing);
                Instantiate(cubePrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
