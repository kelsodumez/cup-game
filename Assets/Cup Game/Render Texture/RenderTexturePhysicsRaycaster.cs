using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RenderTexturePhysicsRaycaster : PhysicsRaycaster
{
    // The image displaying the render texture, used to determine the rect that the texture occupies on screen
    [SerializeField] private RectTransform renderTextureRect;
    
    [Header("Debug")] 
    public bool showDebug = false;
    public Color rayColor = Color.green;
    public float debugDuration = 0f; // 0 = single frame
    
    // Overrides Raycast to now use remapped coordinates.
    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        if (eventCamera == null || eventCamera.activeTexture == null) return;

        // Remap screen-space pointer position into render texture pixel space
        PointerEventData remappedData = new PointerEventData(EventSystem.current);
        remappedData.position = RemapToRenderTextureSpace(eventData.position);
        remappedData.pressPosition = RemapToRenderTextureSpace(eventData.pressPosition);
        
        // Draw a debug ray
        if (showDebug)
        {
            Ray ray = new Ray();
            int displayIndex = 0;
            float distanceToClipPlane = 0f;
            
            if (!ComputeRayAndDistance(remappedData, ref ray, ref displayIndex, ref distanceToClipPlane))
                return;
            
            Debug.DrawRay(ray.origin, ray.direction * distanceToClipPlane, rayColor, debugDuration);
        }
        
        base.Raycast(remappedData, resultAppendList);

        // Preserve original screen position in results so UI layout still works
        for (int i = 0; i < resultAppendList.Count; i++)
        {
            var result = resultAppendList[i];
            result.screenPosition = eventData.position;
            resultAppendList[i] = result;
        }
    }

    private Vector2 RemapToRenderTextureSpace(Vector2 screenPos)
    {
        RenderTexture texture = eventCamera.activeTexture;

        // Get the screen-space rect of the image displaying the render texture
        Vector3[] corners = new Vector3[4];
        renderTextureRect.GetWorldCorners(corners);

        // corners. 0-1-2-3 = BottomLeft-TL-TR-BR
        float rectX = corners[0].x;
        float rectY = corners[0].y;
        float rectW = corners[3].x - corners[0].x;
        float rectH = corners[1].y - corners[0].y;

        // Normalize pointer position within the image rect (0-1 UV)
        float u = (screenPos.x - rectX) / rectW;
        float v = (screenPos.y - rectY) / rectH;

        // Scale UV to render texture pixel dimensions
        return new Vector2(u * texture.width, v * texture.height);
    }
}