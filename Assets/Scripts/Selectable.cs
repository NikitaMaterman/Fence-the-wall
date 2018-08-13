using UnityEngine;

public class Selectable : MonoBehaviour {
	[SerializeField]
	public Shader MouseOverShader;

	[SerializeField]
	public Shader MouseExitShader;

	[SerializeField]
	public Color OutlineColor = Color.white;

	void Start () {
		if (MouseExitShader == null) {
			MouseExitShader = Shader.Find("Diffuse");
		}
		if (MouseOverShader == null) {
			MouseOverShader = Shader.Find("Custom/Outline");
		}
	}
	
	void OnMouseOver() {
		// Ensure all renderers in the children are given the new shader
		foreach (var renderer in GetComponentsInChildren<MeshRenderer>()) {
			renderer.material.shader = MouseOverShader;
			var propertyId = Shader.PropertyToID("_OutlineColor");
			renderer.material.SetColor(propertyId, OutlineColor);
			renderer.material.SetFloat("_Outline", 0.1f);
		}
	}
	
	void OnMouseExit() {
		// Reset renderers in children
		foreach (var renderer in GetComponentsInChildren<MeshRenderer>()) {
			renderer.material.shader = MouseExitShader;
		}
	}
}
