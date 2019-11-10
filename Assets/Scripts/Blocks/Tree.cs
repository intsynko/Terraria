using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tree", menuName = "Objects/Tree", order = 51)]
public class Tree : ScriptableObject
{
    public AudioClip woodSound;
    public AudioClip dropSound;
    public AudioClip foliageSound;

    public Sprite Base;
    public Sprite Thrunk;
    public Sprite Crown;


    public Block GetTree(int i, int j, Block b)
    {
        Sprite[,] sprites =  new Sprite[7, 5]{
            {null,Base, Thrunk, null,null},
            {null,null, Thrunk, null,null},
            {null,null, Thrunk, null,null},
            {null,Crown, Thrunk, Crown,null},
            {Crown,Crown, Crown, Crown,Crown},
            {null, Crown, Crown, Crown, null},
            {null, null, Crown, null, null}
        };
        if (sprites[i, j] != null)
            return SettingWood(sprites[i, j], b);
        else
            return null;

    }

    public Block[,] GetTree()
    {
       return new Block[5,2]{
            {SettingWood(Thrunk), SettingWood(Base) },
            {SettingWood(Thrunk), null},
            {SettingWood(Thrunk), null},
            {SettingWood(Thrunk), null},
            {SettingWood(Crown), null}
        };
    }


    public Block SettingWood(Sprite sprite, Block block)
    {
        if (sprite == Crown)
        {
            block.DropUnits = 0;
            block.name = "foliage";
            block.drilSound = foliageSound;
        }
        else
        {
            block.name = "tree";
            block.drilSound = woodSound;
            block.DropUnits = 1;
        }
        block.dropSound = dropSound;
        block.plane = 1;
        block.phisicsLayer = 2;
        block.renderLayer = 2;
        block.DropSprite = sprite;
        return block;
    }


    public Block SettingWood(Sprite sprite)
    {
        return new Block { DropUnits=1, name="tree", drilSound=woodSound,
            dropSound = dropSound, plane = 1, phisicsLayer = 2, renderLayer = 2, DropSprite = sprite};
    }
}
