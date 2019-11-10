using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ground : Block
{
    public Vector2 RangeOfGrassCount = new Vector2(7,10);

    public Sprite SpriteUp;
    public Sprite SpriteLeft;
    public Sprite SpriteUpLeft;
    public Sprite SpriteRight;
    public Sprite SpriteUpRight;
    public Sprite SpriteMiddle;
    public Sprite SpriteUpLeftRight;
    public Sprite SpriteLeftRight;
    public Sprite SpriteDown;
    public Sprite SpriteDownLeft;
    public Sprite SpriteDownRight;
    public Sprite SpriteDownLeftRight;
    public Sprite SpriteDownLeftUp;
    public Sprite SpriteDownRightUp;
    public Sprite SpriteDownUp;
    public Sprite SpriteLeftRightDownUp;
    
    

    public bool up;
    public bool left;
    public bool right;
    public bool down;



    protected override void Start()
    {
        base.Start();
        StartCoroutine(controlGrass());
        
    }
    

    IEnumerator controlGrass()
    {
        while (true)
        {

            yield return new WaitForSeconds(Random.Range(RangeOfGrassCount.x, RangeOfGrassCount.y));
            if (CurrentCondition == Condition.Dropped) break;

            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            
             up = grounGenerator.IsElementAtPosition(plane, x, y +1);
             left = grounGenerator.IsElementAtPosition(plane, x - 1, y);
             right = grounGenerator.IsElementAtPosition(plane, x + 1, y);
             down = grounGenerator.IsElementAtPosition(plane, x, y - 1);

            if (up)
            {
                if (down)
                { 
                    if(!left && !right) { spriteRenderer.sprite = SpriteLeftRight;  continue; }
                    if (!right)         { spriteRenderer.sprite = SpriteRight;      continue; }
                    if (!left)          { spriteRenderer.sprite = SpriteLeft;       continue; }
                    if (left && right)  { spriteRenderer.sprite = SpriteMiddle;     continue; }
                }
                else
                {
                    if (!left && !right){ spriteRenderer.sprite = SpriteDownLeftRight;  continue; }
                    if (!right)         { spriteRenderer.sprite = SpriteDownRight;      continue; }
                    if (!left)          { spriteRenderer.sprite = SpriteDownLeft;       continue; }
                    if (left && right)  { spriteRenderer.sprite = SpriteDown;  continue; }
                }
            }
            else
            {
                if (down)
                {
                    if (!left && !right) { spriteRenderer.sprite = SpriteUpLeftRight; continue; }
                    if (!right) { spriteRenderer.sprite = SpriteUpRight; continue; }
                    if (!left) { spriteRenderer.sprite = SpriteUpLeft; continue; }
                    if (left && right) { spriteRenderer.sprite = SpriteUp; continue; }
                }
                else
                {
                    if (!left && !right) { spriteRenderer.sprite = SpriteLeftRightDownUp; continue; }
                    if (!right) { spriteRenderer.sprite = SpriteDownRightUp; continue; }
                    if (!left) { spriteRenderer.sprite = SpriteDownLeftUp; continue; }
                    if (left && right) { spriteRenderer.sprite = SpriteDownUp; continue; }
                }

            }
        }

    }
    

    public override void Instanse(int x, int y)
    {
        base.Instanse(x, y);
        StartCoroutine(controlGrass());
    }
}
