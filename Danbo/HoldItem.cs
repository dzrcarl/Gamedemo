using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldItem : MonoBehaviour {

    public bool hasPlayer = false;
    public static bool beingCarried = false;
    public bool touched = false;
    public bool isInPot = false;
    public Vector3 snapOffset;

    public Transform player;
    public Transform original;
    public Transform cooker;
    //private Transform snapRef;

    private Transform foodFinder;
    private GameObject food;
    private Vector3 snapCharacterOffSet;

    public Transform colorCtrl;
    private Material material;
    private Color normalColor;
    private Color highlightedColor;
    private float interval;

    // Use this for initialization
    void Start () {
        snapCharacterOffSet = new Vector3(0, 7f, 0);
        // initialize for color highlighting
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
        float dist_player = Vector3.Distance(gameObject.transform.position, player.position);
        if(dist_player <= 7.5f && IsInvision())
        {
            hasPlayer = true;
            interval += Time.deltaTime;
            material.color = Color.Lerp(normalColor, highlightedColor, interval);
        }
        else
        {
            hasPlayer = false;
            material.color = normalColor;
        }
        if (cooker != null && cooker != this) // check if this item has cooker to contain it, exclude itself
        {
            float dist_pot = Vector3.Distance(gameObject.transform.position, cooker.position);
            if (dist_pot <= 7.5f)
            {
                isInPot = true;
                //food = cooker.GetChild(0).gameObject; 
                //Get the food GameObject that belongs to this pan/pot
                GetByTag(cooker, "food");
                food = foodFinder.gameObject;
            }
            else
            {
                isInPot = false;
            }
        }

        snapCharacterOffSet = new Vector3(Mathf.Sin((player.rotation.eulerAngles.y * Mathf.PI)/180) * 4f, 5f, Mathf.Cos((player.rotation.eulerAngles.y * Mathf.PI) / 180) * 4f);
        //Debug.Log(Snaping.readyToSnap);
        //Debug.Log(Snaping.deskName);
        // pick up item
        if (hasPlayer && Input.GetButtonDown("Pick"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = player;
            beingCarried = true;
            player.GetComponent<Animator>().SetBool("isGribbing", true);
            //GetComponent<Rigidbody>().useGravity = false;
            //snap this item to character
            this.transform.position = player.position + snapCharacterOffSet;
            material.color = normalColor;
        }


        // drop down item
        if (beingCarried && Input.GetButtonDown("Drop") && !isInPot)
        {

            if (!Snaping.readyToSnap)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = original;
                beingCarried = false;
                GetComponent<Rigidbody>().useGravity = true;
                player.GetComponent<Animator>().SetBool("isGribbing", false);
            }
            else
            {
                GetComponent<Rigidbody>().isKinematic = false;
                beingCarried = false;
                string pName = Snaping.deskName;
                Debug.Log(pName);
                transform.parent = GameObject.Find(pName).transform;
                Transform snapRef = transform.parent.GetChild(0);
                GetComponent<Rigidbody>().useGravity = true;
                player.GetComponent<Animator>().SetBool("isGribbing", false);
                // snap
                transform.position = snapRef.position + snapOffset;
            }

        }
        else if (beingCarried && isInPot && Input.GetButtonDown("Drop"))
        {
            // drop food in pot
            // remove this food.
            // change pot status to with food
            // player set to idle
            food.GetComponent<MeshRenderer>().enabled = true;
            player.GetComponent<Animator>().SetBool("isGribbing", false);
            Destroy(this.gameObject);
        }
    }

    // get child element by tag
    public void GetByTag(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag) {
                foodFinder = child;
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

        Vector3 point = this.transform.position;

        if (IsINTriangle(point, player.position, f1, f0) || IsINTriangle(point, player.position, f2, f0))
        {
            //Debug.Log("cube in this !!!");
            return true;
        }
        else
        {
            //Debug.Log("cube not in this !!!");
            return false;
        }
    }

}