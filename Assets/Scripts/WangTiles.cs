using System.Collections.Generic;
using UnityEngine;

public class WangTiles : MonoBehaviour
{
    public Texture2D grid;
    public Texture2D dungeon;
    public Texture2D squares;


    private Texture2D textureSource;
    private Texture2D texture;
    private List<int> lastRowUp;
    private List<int> currentRowUp;
    private int lastTileRight;
    private System.Random rnd;

	void Awake()
	{
        rnd = new System.Random();
        lastRowUp = new List<int>();
        currentRowUp = new List<int>();
	    lastTileRight = 1;
	    textureSource = squares;
        texture = new Texture2D(1024, 1024) { filterMode = FilterMode.Point };
        for (var i = 0; i < texture.width/16; i++)
        {
            lastRowUp.Add(1);
        }
        
	    RandomizeTexture();
	}
	
    void RandomizeTexture()
    {
        for (var y = 0; y < texture.height / 16; y++)
        {
            for (var x = 0; x < texture.width / 16; x++)
            {
                texture.SetPixels(x * 16, y * 16, 16, 16, WangTile(lastTileRight, lastRowUp[x]));
            }
            lastRowUp.Clear();
            lastRowUp.AddRange(currentRowUp);
            currentRowUp.Clear();
        }
        texture.Apply();
        renderer.material.mainTexture = texture;
    }

    Color[] WangTile(int left, int down)
    {
        int i, j;
        if (left == 1) i = RandomChoice(new List<int>() { 0, 1 });
        else i = RandomChoice(new List<int> { 2, 3 });
        if (down == 1) j = RandomChoice(new List<int>() { 0, 1 });
        else j = RandomChoice(new List<int> { 2, 3 });

        if (i == 0 || i == 3) lastTileRight = 1;
        if (i == 1 || i == 2) lastTileRight = 2;
        if (j == 0 || j == 3) currentRowUp.Add(1);
        if (j == 1 || j == 2) currentRowUp.Add(2);
        return textureSource.GetPixels(i*16, j*16, 16, 16);
    }
    
    T RandomChoice<T>(IList<T> obj)
    {
        return obj[rnd.Next(0, obj.Count)];
    }

	void Update ()
    {
        if(Input.GetKeyDown("escape")) Application.Quit();
	    if(Input.GetKey("space")) RandomizeTexture();
        if (Input.GetKeyDown("1"))
        {
            textureSource = squares;
            RandomizeTexture();
        }
        if (Input.GetKeyDown("2"))
        {
            textureSource = grid;
            RandomizeTexture();
        }
        if (Input.GetKeyDown("3"))
        {
            textureSource = dungeon;
            RandomizeTexture();
        }
    }
}
