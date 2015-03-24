using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TextureTiling : MonoBehaviour {
	
	public float textureToMeshZ = 2f;
	public bool OnlyX = false;
	public bool OnlyY = false;

	private Vector3 prevScale = Vector3.one;
	private float prevTextureToMeshZ = -1f;
	private Texture texture;
	
	void Start (){
		this.prevScale = gameObject.transform.lossyScale;
		this.prevTextureToMeshZ = this.textureToMeshZ;
		this.UpdateTiling();
	}
	
	void Update () {
		// If something has changed
		if(gameObject.transform.lossyScale != prevScale || !Mathf.Approximately(this.textureToMeshZ, prevTextureToMeshZ))
			this.UpdateTiling();
		
		// Maintain previous state variables
		this.prevTextureToMeshZ = this.textureToMeshZ;
	}
	
	[ContextMenu("UpdateTiling")]
	void UpdateTiling()
	{
		if(texture == null)
		{
			texture = gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture;
		}
		if(texture != null)
		{
			float planeSizeX = 1f;
			float planeSizeZ = 1f;
			float textureToMeshX = ((float)this.texture.width/this.texture.height)*this.textureToMeshZ;
			Vector2 newScale = new Vector2(planeSizeX*gameObject.transform.lossyScale.x/textureToMeshX, planeSizeZ*gameObject.transform.lossyScale.y/textureToMeshZ);
			if(OnlyX)
				newScale.y = 1.0f;
			else if(OnlyY)
				newScale.x = 1.0f;
			gameObject.GetComponent<Renderer>().sharedMaterial.mainTextureScale = newScale;
			this.prevScale = gameObject.transform.lossyScale;
		}
	}
}