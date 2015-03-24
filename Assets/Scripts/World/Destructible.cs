using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

	private Texture2D maskTexture;

	void Start ()
	{
		Material material = this.gameObject.GetComponent<Renderer>().material;
		maskTexture = new Texture2D(32, 32, TextureFormat.ARGB32, false);
		for(int x = 0; x < 32; x++)
			for(int y = 0; y < 32; y++)
				maskTexture.SetPixel(x, y, Color.white);
		maskTexture.Apply();
		material.SetTexture("_MaskTex", this.maskTexture);
	}
	
	void Update ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(mouseRay, Mathf.Infinity);

			if(hit.collider != null && hit.collider.transform == this.transform)
			{
				for(int x= 0; x < 20; x++)
				{
					for(int y = 0; y < 20; y++)
					{
						this.maskTexture.SetPixel(x, y, Color.black);
					}
				}
				maskTexture.Apply();
			}
		}
	}
}
