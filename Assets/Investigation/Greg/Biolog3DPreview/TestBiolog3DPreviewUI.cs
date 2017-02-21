using UnityEngine;
using System.Collections;
using Squid;

public class TestBiolog3DPreviewUI : GuiRenderer
{
    public RenderTexture BiologRenderTexture;
    private UnityRenderer _renderer;
    private ImageControl _biolog3DPreview;

    protected override void Awake()
    {
        base.Awake();

        _renderer = GuiHost.Renderer as UnityRenderer;


        _renderer.InsertTexture(BiologRenderTexture, "biolog3DPreview");
        _biolog3DPreview = (ImageControl)Desktop.GetControl("3DPreview");

        _biolog3DPreview.Texture = "biolog3DPreview";
        _biolog3DPreview.TextureRect = new Rectangle(0, 0, BiologRenderTexture.width, BiologRenderTexture.height);
        
    }
}
