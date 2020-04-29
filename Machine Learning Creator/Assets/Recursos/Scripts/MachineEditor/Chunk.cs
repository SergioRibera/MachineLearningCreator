using UnityEngine;

public enum DirChunk { center, left, rigth, forward, back }
public class Chunk : MonoBehaviour
{
    public int width = 10;
    public DirChunk dir;

    private void Start()
    {
        transform.localScale = new Vector3(width, 1, width);
    }

    private void FixedUpdate()
    {
        if (Camera.main.transform.position.y <= transform.position.y)
            transform.rotation = Quaternion.Euler(new Vector3(180, 0, 0));
        else
            transform.rotation = Quaternion.identity;
    }
    public Chunk SetDir(DirChunk d)
    {
        dir = d;
        return this;
    }
}