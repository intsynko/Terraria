using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour, IObject
{
    public float DropUnits = 0;
    public Sprite DropSprite;
    public string name = "block";
    public AudioClip drilSound;
    public AudioClip dropSound;
    public int plane = 0;
    public int phisicsLayer = 1;
    public int renderLayer = 3;
    public bool isTexture = false;


    public List<Condition> conditions { get; private set; }
    public Condition CurrentCondition { get; private set; }

    private bool isPressed = false;
    private float timeStart;
    protected GrounGenerator grounGenerator;
    public AudioSource audioSource;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbody2D;
    protected SpriteRenderer spriteRenderer;


    public Block()
    {
        CurrentCondition = Condition.Material;
    }


    protected virtual void OnMouseDown()
    {
        isPressed = true;
        timeStart = Time.time;
        audioSource.clip = drilSound;
    }

    protected virtual void OnMouseExit()
    {
        isPressed = false;
    }

    protected virtual void OnMouseUp()
    {
        isPressed = false;
    }

    protected virtual void Start()
    {
        if (isTexture)
        {
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(GetComponent<Rigidbody2D>());
        }
        switch(plane)
        {
            case 0: renderLayer = 3; phisicsLayer = 1; transform.position += new Vector3(0, 0, -0.1f); break;
            case 1: renderLayer = 2; phisicsLayer = 2; break;
        }
        //audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        grounGenerator = transform.parent.GetComponent<GrounGenerator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer.sprite = DropSprite;
        grounGenerator.SetElementAtPosition(plane, (int)transform.position.x, (int)transform.position.y);
        gameObject.layer = LayerMask.NameToLayer("Layer"+ phisicsLayer);
        GetComponent<SpriteRenderer>().sortingLayerName = "Layer" + renderLayer;
    }

    protected virtual void Update()
    {
        if(CurrentCondition == Condition.Material)
        if (isPressed && !isTexture)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
            if (Time.time - timeStart >= DropUnits) Drop();
        }
    }

    protected virtual void Drop()
    {
        spriteRenderer.sprite = DropSprite;
        rigidbody2D.isKinematic = false;
        boxCollider2D.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Layer2");
        transform.localScale = transform.localScale * 0.5f;
        CurrentCondition = Condition.Dropped;
        grounGenerator.DelElementAtPosition(plane, (int)transform.position.x, (int)transform.position.y);
    }

    public IEnumerator EnterPlayer(Transform playerTransform)
    {
        rigidbody2D.isKinematic = true;
        boxCollider2D.enabled = false;
        while ((playerTransform.position - transform.position).magnitude > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, 10f * Time.deltaTime);
            yield return null;
        }
        spriteRenderer.enabled = false;
        while (audioSource.isPlaying)
            yield return null;
        audioSource.clip = dropSound;
        audioSource.Play();
        while (audioSource.isPlaying)
            yield return null;
        //Destroy(gameObject);
    }

    public virtual void Instanse(int x, int y)
    {
        CurrentCondition = Condition.Material;
        transform.position = new Vector2(x, y);
        rigidbody2D.velocity = new Vector2();
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = DropSprite;
        gameObject.layer = LayerMask.NameToLayer("Layer" + phisicsLayer);
        transform.localScale = transform.localScale * 2f;
        boxCollider2D.enabled = true;
        grounGenerator.SetElementAtPosition(plane, x, y);
    }
}
