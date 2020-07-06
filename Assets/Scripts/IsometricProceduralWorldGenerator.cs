using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricProceduralWorldGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Generate 20x20
        StartCoroutine(this.Generate(10, 10));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("SCREENSHOT TAKEN: " + Application.persistentDataPath);
            ScreenCapture.CaptureScreenshot("screenshot.png");
        }
    }

    /**
     * <summary>
     * Generate a procedural isometric world
     * </summary>
     *
     * <param name="sizeX"></param>
     * <param name="sizeY"></param>
     * 
     * <returns>
     * IEnumerator null
     * </returns>
     */
    private IEnumerator Generate (int sizeX, int sizeY)
    {
        sizeX = (sizeX * 2) - 1;
        Debug.Log("Generating...");

        // The initial starting points
        int startX = 0;
        int startY = 0;

        float offsetX = 1.675f;
        float offsetY = 0.95f;

        float zIndex = 0;

        startX = -sizeX;
        startY = -sizeY;

        int seed = 256;

        // While we have not procedurally generated in both axis (x and y)
        // Keep generating new tile until we have reached both in size of x and y.
        for (float x = startX; x < sizeX; x = x + offsetX)
        {
            for (float y = startY; y < sizeY; y = y + offsetY)
            {
                float nx = x / sizeX;
                float ny = y / sizeY;

                float noise = Mathf.PerlinNoise(nx + seed, ny + seed);
                this.CreateNewTileAt(x, y, zIndex, noise);

                zIndex++;
                yield return null;
            }

            zIndex = 0;
            yield return null;
        }

        zIndex = 0.5f;

        for (float x = startX + offsetX / 2; x < sizeX; x = x + offsetX)
        {
            for (float y = startY + offsetY / 2; y < sizeY; y = y + offsetY)
            {
                float nx = x / sizeX;
                float ny = y / sizeY;

                float noise = Mathf.PerlinNoise(nx + seed, ny + seed);
                this.CreateNewTileAt(x, y, zIndex, noise);

                zIndex++;
                yield return null;
            }

            zIndex = 0.5f;

            yield return null;
        }

        yield return null;
    }

    /**
     * <summary>
     * Create a new tile at a given coordinates
     * </summary>
     * 
     * <param name="x"></param>
     * <param name="y"></param>
     * <param name="z"></param>
     * <param name="noise"></param>
     * 
     * <returns>
     * void
     * </returns>
     */
    private void CreateNewTileAt (float x, float y, float z, float noise)
    {
        GameObject tile;
        // Water tile
        if (noise < 0.2f)
        {
            tile = MonoBehaviour.Instantiate(Resources.Load("Prefabs/Water Tile") as GameObject);
        }
        // Sand tile
        else if (noise < 0.3f)
        {
            tile = MonoBehaviour.Instantiate(Resources.Load("Prefabs/Sand Tile") as GameObject);
        }
        // Default tile
        else
        {
            tile = MonoBehaviour.Instantiate(Resources.Load("Prefabs/Default Tile") as GameObject);
        }

        Vector3 targetPosition = new Vector3(x, y, z);
        tile.transform.position = targetPosition;
    }
}
