
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class RenderTextureRaycast : MonoBehaviour
{
    [SerializeField] protected Camera UICamera;
    [SerializeField] protected RenderTexture _renderTex;
    [SerializeField] protected RectTransform RawImageRectTrans;

    [SerializeField] protected Camera RenderToTextureCamera;
    [SerializeField] private InputAction _clickAction;


    void Awake()
    {
        _clickAction.performed += ctx => OnPointerClick();
        _clickAction.Enable();
    }

    public void OnPointerClick()
    {
        Vector2 clickLocation = Mouse.current.position.ReadValue();
        // RaycastHit hit;
        // Ray ray = Camera.main.ScreenPointToRay(clickLocation);
        // Debug.Log(ray);
        // // do we hit our portal plane?
        // if (Physics.Raycast(ray, out hit)) 
        // {
        //     Debug.Log(hit.collider.gameObject);
            
            
        //     var localPoint = hit.textureCoord;
        //     // convert the hit texture coordinates into camera coordinates
        //     Vector2 screenPos = new Vector2(localPoint.x * RenderToTextureCamera.pixelWidth, localPoint.y * RenderToTextureCamera.pixelHeight);
        //     Debug.Log(screenPos);
        //     Ray portalRay = RenderToTextureCamera.ScreenPointToRay(screenPos);
        //     RaycastHit portalHit;
        //     // test these camera coordinates in another raycast test
        //     if(Physics.Raycast(portalRay, out portalHit))
        //     {
        //         // Debug.Log(portalHit.collider.gameObject);
        //         try
        //         {
        //             portalHit.collider.gameObject.GetComponent<ClickableObject>().OnMouseDown();
        //         }
        //         catch
        //         {
        //             Debug.LogWarning($"Failed to send OnMouseDown trigger to {portalHit.collider.gameObject.name}");
        //         }
        //     }
        // }
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(RawImageRectTrans, clickLocation, UICamera, out localPoint);
        Vector2 normalizedPoint = Rect.PointToNormalized(RawImageRectTrans.rect, localPoint);

        var renderRay = RenderToTextureCamera.ViewportPointToRay(normalizedPoint);
        if (Physics.Raycast(renderRay, out var raycastHit))
        {
            Debug.Log("Hit: " + raycastHit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("No hit object");
        }
    }
}