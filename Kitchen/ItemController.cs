using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

    public Transform player;
    private Transform snapRef;
    private Transform objFinder;    // used in GetByType()
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
		
	}

    public bool BeingCarried()
    {
        return true;
    }

    private bool MatchWithDesk()
    {
        if (deskTag == "utilitiesOnly")
        {
            int count = player.childCount;
            string compTag = player.GetChild(count - 1).tag;
            if (compTag == "utility")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private bool IsInVision()
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
    }    // check if item is within the vision of the character
                                    // true only when item is in the vision trangle
    private float TriangleArea(float v0x, float v0y, float v1x, float v1y, float v2x, float v2y)
    {
        return Mathf.Abs((v0x * v1y + v1x * v2y + v2x * v0y
            - v1x * v0y - v2x * v1y - v0x * v2y) / 2f);
    }   // form the triangle area of the vision
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
    }   // check if item is in sight

    private bool RangeCheck(Transform target)
    {
        //snapRef = transform.GetChild(0);
        float dist_player = Vector3.Distance(target.position, player.position);

        if (dist_player <= 7.5f && IsInVision())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void HighlightColor()
    {
        snapRef = transform.GetChild(0);
        if (RangeCheck(snapRef))
        {
            interval += Time.deltaTime;
            material.color = Color.Lerp(normalColor, highlightedColor, interval);
        }
        else
        {
            material.color = normalColor;
        }
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
}
