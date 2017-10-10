using UnityEngine;

public class RopeController : MonoBehaviour
{
    public GameObject fragmentPrefab;

    int fragmentNum = 80;
    GameObject[] fragments;

    float activeFragmentNum = 80;

    Vector3 interval = new Vector3(0f, 0f, 0.25f);

    float[] xPositions;
    float[] yPositions;
    float[] zPositions;

    CatmullRomSpline splineX;
    CatmullRomSpline splineY;
    CatmullRomSpline splineZ;

    int splineFactor = 4;

    void Start()
    {
        fragments = new GameObject[fragmentNum];

        Vector3 position = Vector3.zero;

        for (int i = 0; i < fragmentNum; i++)
        {
            fragments[i] = Instantiate(fragmentPrefab, position, Quaternion.identity);
            fragments[i].transform.SetParent(transform);

            var joint = fragments[i].GetComponent<SpringJoint>();
            if (i > 0)
            {
                joint.connectedBody = fragments[i - 1].GetComponent<Rigidbody>();
            }

            position += interval;
        }

        var renderer = GetComponent<LineRenderer>();
        renderer.positionCount = (fragmentNum - 1) * splineFactor + 1;

        xPositions = new float[fragmentNum];
        yPositions = new float[fragmentNum];
        zPositions = new float[fragmentNum];

        splineX = new CatmullRomSpline(xPositions);
        splineY = new CatmullRomSpline(yPositions);
        splineZ = new CatmullRomSpline(zPositions);
    }

    void Update()
    {
        float vy = Input.GetAxisRaw("Vertical") * 20f * Time.deltaTime;
        activeFragmentNum = Mathf.Clamp(activeFragmentNum + vy, 0, fragmentNum);

        for (int i = 0; i < fragmentNum; i++)
        {
            if (i <= fragmentNum - activeFragmentNum)
            {
                fragments[i].GetComponent<Rigidbody>().position = Vector3.zero;
                fragments[i].GetComponent<Rigidbody>().isKinematic = true;
            }
            else
            {
                fragments[i].GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    void LateUpdate()
    {
        // Copy rigidbody positions to the line renderer
        LineRenderer renderer = GetComponent<LineRenderer>();

        // No interpolation
        //for (int i = 0; i < fragmentNum; i++)
        //{
        //    renderer.SetPosition(i, fragments[i].transform.position);
        //}

        int i;
        for (i = 0; i < fragmentNum; i++)
        {
            Vector3 position = fragments[i].transform.position;
            xPositions[i] = position.x;
            yPositions[i] = position.y;
            zPositions[i] = position.z;
        }

        for (i = 0; i < (fragmentNum - 1) * splineFactor + 1; i++)
        {
            renderer.SetPosition(i, new Vector3(
                splineX.GetValue(i / (float)splineFactor),
                splineY.GetValue(i / (float)splineFactor),
                splineZ.GetValue(i / (float)splineFactor)));
        }
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 100, 100), "" + activeFragmentNum);
    }
}
