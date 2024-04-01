using UnityEngine;
 
 
[ExecuteInEditMode]
 
public class Anchor : MonoBehaviour
{
    public enum Side
    {
        BottomLeft,
        Left,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        Center,
    }
   
    public Camera uiCamera = null;
    public Side side = Side.Center;
    public Vector3 relativeOffset = Vector3.zero;
   
 
   
    Transform mTrans;
   
 
   
    void Awake() { mTrans = transform; }
 
 
    void Update()
    {
        if (uiCamera != null)
        {
            Rect rect = uiCamera.pixelRect;
            float cx = (rect.xMin + rect.xMax) * 0.5f;
            float cy = (rect.yMin + rect.yMax) * 0.5f;
            float cz = relativeOffset.z;
            Vector3 v = new Vector3(cx, cy, cz);
           
            if (side != Side.Center)
            {
                if (side == Side.Right || side == Side.TopRight || side == Side.BottomRight)
                {
                    v.x = rect.xMax;
                }
                else if (side == Side.Top || side == Side.Center || side == Side.Bottom)
                {
                    v.x = cx;
                }
                else
                {
                    v.x = rect.xMin;
                }
               
                if (side == Side.Top || side == Side.TopRight || side == Side.TopLeft)
                {
                    v.y = rect.yMax;
                }
                else if (side == Side.Left || side == Side.Center || side == Side.Right)
                {
                    v.y = cy;
                }
                else
                {
                    v.y = rect.yMin;
                }
            }
           
            float screenWidth = rect.width;
            float screenHeight = rect.height;
           
            v.x += relativeOffset.x * screenWidth;
            v.y += relativeOffset.y * screenHeight;
           
            if (uiCamera.orthographic)
            {
                v.x = Mathf.RoundToInt(v.x);
                v.y = Mathf.RoundToInt(v.y);
 
            }
 
           
            v = uiCamera.ScreenToWorldPoint(v);
            v.z = relativeOffset.z;
 
            if (mTrans.position != v) mTrans.position = v;
        }
    }
}
 