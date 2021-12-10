using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAppearenceSetter : MonoBehaviour
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

    private void Start()
    {
        if (GameManager.Instance.playingAsBlack)
        {
            FlipSprite();
        }
    }

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
            name = "Black " + name;
        }
        else
        {
            meshMaterialSetter.SetSingleMaterial(whiteMaterial);
            name = "White " + name;
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

    public void MakeVisible(bool b)
    {
        meshRenderer.enabled = b;
        spriteRenderer.enabled = b;
    }

    public void MakeSensePiece()
    {
        meshMaterialSetter.SetSingleMaterial(senseMaterial);
        spriteRenderer.sprite = senseSprite;
        name = name + " SENSE";
    }

    private void FlipSprite()
    {
        spriteRenderer.transform.localEulerAngles = new Vector3(90, 180, 0);
    }
}
