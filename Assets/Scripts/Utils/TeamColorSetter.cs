using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColorSetter : MonoBehaviour
{
    public Material blackMaterial;
    public Material whiteMaterial;
    public Material senseMaterial;
    public Sprite blackSprite;
    public Sprite whiteSprite;
    public Sprite senseSprite;
    public MaterialSetter meshMaterialSetter;
    public SpriteRenderer spriteRenderer;
    public MeshRenderer meshRenderer;
    
    public void SetColorByTeam(TeamColor teamColor)
    {
        SetMeshMaterial(teamColor);
        SetSprite(teamColor);
    }

    private void SetMeshMaterial(TeamColor teamColor)
    {
        if (teamColor == TeamColor.Black)
        {
            meshMaterialSetter.SetSingleMaterial(blackMaterial);
        }
        else
        {
            meshMaterialSetter.SetSingleMaterial(whiteMaterial);
        }
    }

    private void SetSprite(TeamColor teamColor)
    {
        if (teamColor == TeamColor.Black)
        {
            spriteRenderer.sprite = blackSprite;
        }
        else
        {
            spriteRenderer.sprite = whiteSprite;
        }
    }

    public void MakeInvisible()
    {
        meshRenderer.enabled = false;
        spriteRenderer.enabled = false;
    }

    public void MakeSensePiece()
    {
        meshMaterialSetter.SetSingleMaterial(senseMaterial);
        spriteRenderer.sprite = senseSprite;
    }
}
