using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGun : MonoBehaviour
{
    [Range(0, 7)]
    public int terrainEditingRange = 2;
    public float shootDistance = 10f;
    public float isolevelDiff = 10;

    public GameObject shootParticles;

    Camera cam;
    EndlessTerrain endlessTerrain;
    FirstPersonMovement personMovement;

    // Start is called before the first frame update
    void Start()
    {
        shootParticles.SetActive(false);
        cam = GetComponent<Camera>();
        endlessTerrain = FindObjectOfType<EndlessTerrain>();
        personMovement = GetComponentInParent<FirstPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        bool holdingG = Input.GetKey(KeyCode.G);
        bool lmb = Input.GetKey(KeyCode.Mouse0);
        bool rmb = Input.GetKey(KeyCode.Mouse1);
        if (holdingG && (lmb || rmb))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, shootDistance))
            {
                if (hit.transform.GetComponent<Chunk>())
                    if(lmb)
                        ProcessChunk(hit.transform.GetComponent<Chunk>(), hit.point, -isolevelDiff);
                    else
                        ProcessChunk(hit.transform.GetComponent<Chunk>(), hit.point, isolevelDiff);
            }
            shootParticles.SetActive(true);
        }
        else
        {
            shootParticles.SetActive(false);
        }
        int scrollDiff = (int)Input.mouseScrollDelta.y;
        if (scrollDiff != 0)
        {
            if(terrainEditingRange + scrollDiff >= 0 && terrainEditingRange + scrollDiff < 10)
                terrainEditingRange += (int)Input.mouseScrollDelta.y;
        }
    }

    void ProcessChunk(Chunk chunk, Vector3 hitPos, float isolevelDiff)
    {
        personMovement.NotifyTerrainChange(hitPos, terrainEditingRange);
        endlessTerrain.EditChunkPoints(chunk, hitPos, isolevelDiff, terrainEditingRange);
    }

    //private void OnGUI()
    //{
    //    int size = 12;
    //    float posX = cam.pixelWidth / 2 - size / 4;
    //    float posY = cam.pixelHeight / 2 - size / 2;
    //    GUI.Label(new Rect(posX, posY, size, size), "*");
    //}
}
