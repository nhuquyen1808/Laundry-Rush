using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevDuck
{
    public class CameraColliderGame4 : ScreenEdgeColliders
    {
        public override void AddCollider()
        {
            //Vector2's for the corners of the screen
            Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0 , cam.nearClipPlane));
            Vector2 topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, cam.nearClipPlane));
            Vector2 topLeft = new Vector2(bottomLeft.x, topRight.y);
            Vector2 bottomRight = new Vector2(topRight.x , bottomLeft.y );

            //Update Vector2 array for edge collider
            edgePoints[0] = bottomLeft;
            edgePoints[1] = topLeft;
            edgePoints[2] = topRight;
            edgePoints[3] = bottomRight;
            edgePoints[4] = bottomLeft;

            edge.points = edgePoints;
        }
    }
}
