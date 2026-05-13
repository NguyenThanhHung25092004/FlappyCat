using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        float camX = transform.parent.position.x;
        // Tính toán Offset dựa trên tốc độ hiện tại của Player
        // Dùng Time.time giúp nó trôi liên tục không bao giờ khựng
        float offset = (camX * scrollSpeed) / transform.localScale.x;

        // Gán giá trị vào Material để hình ảnh tự cuộn
        meshRenderer.material.mainTextureOffset = new Vector2(offset, 0);
    }
}