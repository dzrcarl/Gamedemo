using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snaping : MonoBehaviour {

    public static bool readyToSnap = false;
    public bool wPlayer = false;
    public static string deskName = null;

    public Transform player;
    private Transform snapRef;
    private Transform objFinder;
    private string deskTag;
    private string itemTag;

    public Transform colorCtrl;
    private Material material;
    private Color normalColor;
    private Color highlightedColor;
    private float interval;


    // Use this for initialization
    void Start () {
        // initialize for color highlighting
        deskTag = gameObject.tag;
 
        material = colorCtrl.GetComponent<MeshRenderer>().material;
        normalColor = material.color;
        highlightedColor = new Color(
            normalColor.r * 1.5f,
            normalColor.g * 1.5f,
            normalColor.b * 1.5f
        );
    }
	
	// Update is called once per frame
	void Update () {
        // range check
        snapRef = transform.GetChild(0);
        float dist_player = Vector3.Distance(snapRef.transform.position, player.position);
        wPlayer = HasPlayer(dist_player);
        if (wPlayer)
        {
            deskName = gameObject.name;
            if (TheyMatch())
            {
                readyToSnap = true;
                //Debug.Log("Match!");
            }
            else
            {
                //Debug.Log("Does not Match!");
                readyToSnap = false;
            }
        }
        else
        {
            Debug.Log(gameObject.name + "No player");
            readyToSnap = false;
        }

    }

    private bool HasPlayer(float dist_player)
    {
        bool withPlayer = false;
        if (dist_player <= 7.5f && IsInvision())
        {
            withPlayer = true;
            interval += Time.deltaTime;
            material.color = Color.Lerp(normalColor, highlightedColor, interval);
        }
        else
        {
            withPlayer = false;
            material.color = normalColor;
        }
        return withPlayer;
    }

    // get child element by tag
    public void GetByTag(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                objFinder = child;
                //return child;
            }
        }
    }

    // check if item is in the trangular area that represent the vision of the character
    private float TriangleArea(float v0x, float v0y, float v1x, float v1y, float v2x, float v2y)
    {
        return Mathf.Abs((v0x * v1y + v1x * v2y + v2x * v0y
            - v1x * v0y - v2x * v1y - v0x * v2y) / 2f);
    }

    bool IsINTriangle(Vector3 point, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        float x = point.x;
        float y = point.z;

        float v0x = v0.x;
        float v0y = v0.z;

        float v1x = v1.x;
        float v1y = v1.z;

        float v2x = v2.x;
        float v2y = v2.z;

        float t = TriangleArea(v0x, v0y, v1x, v1y, v2x, v2y);
        float a = TriangleArea(v0x, v0y, v1x, v1y, x, y) + TriangleArea(v0x, v0y, x, y, v2x, v2y) + TriangleArea(x, y, v1x, v1y, v2x, v2y);

        if (Mathf.Abs(t - a) <= 0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // check if item in within character's vision
    public bool IsInvision()
    {
        float distance = 7.5f;
        Quaternion r = player.rotation;
        Vector3 f0 = (player.position + (r * Vector3.forward) * distance);
        Debug.DrawLine(player.position, f0, Color.red);

        Quaternion r0 = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y - 30f, player.rotation.eulerAngles.z);
        Quaternion r1 = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y + 30f, player.rotation.eulerAngles.z);

        Vector3 f1 = (player.position + (r0 * Vector3.forward) * distance);
        Vector3 f2 = (player.position + (r1 * Vector3.forward) * distance);

        Debug.DrawLine(player.position, f1, Color.red);
        Debug.DrawLine(player.position, f2, Color.red);

        Debug.DrawLine(f0, f1, Color.red);
        Debug.DrawLine(f0, f2, Color.red);

        Vector3 point = snapRef.transform.position;

        if (IsINTriangle(point, player.position, f1, f0) || IsINTriangle(point, player.position, f2, f0))
        {
            // Debug.Log("cube in this !!!");
            return true;
        }
        else
        {
            // Debug.Log("cube not in this !!!");
            return false;
        }
    }

    // check if two objects are snapable
    public bool TheyMatch()
    {
        if (deskTag == "utilitiesOnly")
        {
            int count = player.childCount;
            string compTag = player.GetChild(count-1).tag;
            if(compTag == "utility")
            {
                return true;
            }
        }
        else if(deskTag != "utilitiesOnly")
        {
            return true;
        }
        return false;
    }
}