using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PrceduralBridge : MonoBehaviour
{
    [SerializeField] private float _bridgeWidth = 3;
    [SerializeField] private float _bridgelenghtOffSet = 3;
    [SerializeField] private float _bridgeHeight = 3;

    [SerializeField] private int _bridgeSegment = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        Vector3 perpendicular1 = transform.position+transform.right*_bridgeWidth;
        Vector3 perpendicular2 = transform.position-transform.right*_bridgeWidth;
        Vector3 a= Vector3.zero;
        Vector3 b= Vector3.zero;
        Vector3 c= Vector3.zero;
        Vector3 d= Vector3.zero;
        
        Vector3 a2= Vector3.zero;
        Vector3 b2= Vector3.zero;
        Vector3 c2= Vector3.zero;
        Vector3 d2= Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(new Ray(perpendicular1, transform.forward), out  hit)) { a = hit.point; }
        if (Physics.Raycast(new Ray(perpendicular1, -transform.forward), out  hit)) { b = hit.point; }
        if (Physics.Raycast(new Ray(perpendicular2, transform.forward), out  hit)) { c = hit.point; }
        if (Physics.Raycast(new Ray(perpendicular2, -transform.forward), out  hit)) { d = hit.point; }
        
        Debug.DrawLine(perpendicular1, transform.position, Color.yellow);
        Debug.DrawLine(perpendicular2, transform.position, Color.yellow);
        
        Debug.DrawLine(perpendicular1, a, Color.green);
        Debug.DrawLine(perpendicular1, b, Color.green);
        Debug.DrawLine(perpendicular2, c, Color.green);
        Debug.DrawLine(perpendicular2, d, Color.green);
        
        if (Physics.Raycast(new Ray(a+(a-perpendicular1).normalized*_bridgelenghtOffSet+new Vector3(0,10,0), -Vector3.up), out  hit)) { a2 = hit.point; }
        if (Physics.Raycast(new Ray(b+(b-perpendicular1).normalized*_bridgelenghtOffSet+new Vector3(0,10,0), -Vector3.up), out  hit)) { b2 = hit.point; }
        if (Physics.Raycast(new Ray(c+(c-perpendicular2).normalized*_bridgelenghtOffSet+new Vector3(0,10,0), -Vector3.up), out  hit)) { c2 = hit.point; }
        if (Physics.Raycast(new Ray(d+(d-perpendicular2).normalized*_bridgelenghtOffSet+new Vector3(0,10,0), -Vector3.up), out  hit)) { d2 = hit.point; }
        
        Debug.DrawLine(perpendicular1, a2, Color.green);
        Debug.DrawLine(perpendicular1, b2, Color.green);
        Debug.DrawLine(perpendicular2, c2, Color.green);
        Debug.DrawLine(perpendicular2, d2, Color.green);
        

        Vector3 h = Vector3.Lerp(a,b,0.5f)+ transform.up*_bridgeHeight;
        Vector3 h2= Vector3.Lerp(c,d,0.5f)+ transform.up*_bridgeHeight;
        
        Debug.DrawLine(perpendicular1, h, Color.brown);
        Debug.DrawLine(perpendicular2, h2, Color.brown);
        
        CalculateArcheBridge(a2,h,b2);
        CalculateArcheBridge(c2,h2,d2);
        CalculateArcheBridge(a,h,b);
        CalculateArcheBridge(c,h2,d);

    }

    private void CalculateArcheBridge(Vector3 a, Vector3 h , Vector3 b) {
        float t1;
        float t0;
        Vector3 p1;
        Vector3 p2;
        for (int i = 1; i < _bridgeSegment; i++) {
            t1= (1f/_bridgeSegment)*i;
            t0= (1f/_bridgeSegment)*(i-1);
            p1 = GetBezier(a, h, b, t0);
            p2 = GetBezier(a, h, b, t1);
            Debug.DrawLine(p2, p1, Color.blue);
        }
        t1= 1;
        t0= (1f/_bridgeSegment)*(_bridgeSegment-1);
        p1 = GetBezier(a, h, b, t0);
        p2 = GetBezier(a, h, b, t1);
        Debug.DrawLine(p2, p1, Color.blue);
    }
    
    private Vector3 GetBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t) {
        Vector3 pa =Vector3.Lerp(p0, p1, t);
        Vector3 pb = Vector3.Lerp(p1, p2, t);
        return Vector3.Lerp(pa, pb, t);
    }
}