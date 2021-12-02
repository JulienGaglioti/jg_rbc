using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColorSetter : MonoBehaviour
{
    public Material blackMaterial;
    public Material whiteMaterial;
    public Sprite blackSprite;
    public Sprite whiteSprite;
    public MaterialSetter meshMaterialSetter;
    public SpriteRenderer spriteRenderer;
    
    public void SetColor(TeamColor color)
    {
        SetMeshMaterial(color);
        SetSprite(color);
    }

    private void SetMeshMaterial(TeamColor color)
    {
        if (color == TeamColor.Black)
        {
            meshMaterialSetter.SetSingleMaterial(blackMaterial);
        }
        else
        {
            meshMaterialSetter.SetSingleMaterial(whiteMaterial);
        }
    }

    private void SetSprite(TeamColor color)
    {
        if (color == TeamColor.Black)
        {
            spriteRenderer.sprite = blackSprite;
        }
        else
        {
            spriteRenderer.sprite = whiteSprite;
        }
    }
}
