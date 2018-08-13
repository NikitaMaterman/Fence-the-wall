using System;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour {
	public enum TowerType
	{
		Empty,
		Magic,
		Turret
	}

	[SerializeField] public GameObject MagicTower;
	[SerializeField] public GameObject TurretTower;
	[SerializeField] public GameObject Projectile;
	[SerializeField] public int Damage = 2;
	[SerializeField] public float Range = 50f;
	[SerializeField] public float AttackSpeed = 2f;
	[SerializeField] public int UpgradeCost = 10;

	// Initialize as 1 so the equation doesn't return zero
	[SerializeField] public int UpgradeCount = 1;

	private int MagicUpgradeCost {
		get { return UpgradeCost * UpgradeCount * 2; }
	}

	private int TurretUpgradeCost {
		get { return UpgradeCost * UpgradeCount; }
	}

	public TowerType Type = TowerType.Empty;

	void Start() {

	}

	void OnMouseDown() {
		WorldManager.Instance.TowerPanel.SetActive(true);
		
		foreach (var button in WorldManager.Instance.TowerPanelButtons) {
			Boolean enableButton = true;

			// Remove previous listeners to avoid multiple upgrades at once
			button.onClick.RemoveAllListeners();

			switch (button.name) {
				case "MagicUpgradeButton":
					if (Type != TowerType.Magic && Type != TowerType.Empty) {
						enableButton = false;
					}

					// Subscribe new listener on our tower
					button.onClick.AddListener(MagicButtonClick);
					button.gameObject.SetActive(enableButton && UpgradeCount < 3);
					break;
				case "TurretUpgradeButton":
					if (Type != TowerType.Turret && Type != TowerType.Empty) {
						enableButton = false;
					}

					// Subscribe new listener on our tower
					button.onClick.AddListener(TurretButtonClick);
					button.gameObject.SetActive(enableButton && UpgradeCount < 3);
					break;
				default:
					button.gameObject.SetActive(true);
					break;
			}

		}

		var buttonText = UpgradeCount == 1 ? "Build" : "Upgrade";
		foreach (var text in WorldManager.Instance.TowerPanel.GetComponentsInChildren<Text>()) {
			switch (text.name) {
				case "MagicUpgradeText":
					text.text = String.Format("{0} magic ({1})", buttonText, MagicUpgradeCost);
					break;
				case "TurretUpgradeText":
					text.text = String.Format("{0} turret ({1})", buttonText, TurretUpgradeCost);
					break;
				case "DamageText":
					text.text = String.Format("Damage: {0}", Damage);
					break;
				default:
					// Keep old value (like "Close")
					break;
			}
		}
	}

	private void MagicButtonClick() {
		SwapTurretAndHandleGold(MagicTower, MagicUpgradeCost);
	}

	private void TurretButtonClick() {
		SwapTurretAndHandleGold(TurretTower, TurretUpgradeCost);
	}

	void SwapTurretAndHandleGold(GameObject newPrefab, int upgradeCost) {
		// Ensure no negative Gold is allowed
		if (WorldManager.Instance.Inventory.Gold - upgradeCost < 0) {
			return;
		}

		Quaternion rotation = gameObject.transform.rotation;
		Vector3 position = gameObject.transform.position;

		// Instantiate the new game object at the old game objects position and rotation.
		GameObject newGameObject = Instantiate(newPrefab, position, rotation);
		
		if (gameObject.transform.parent != null) {
			// Set the new game object parent as the old game objects parent.
			newGameObject.transform.SetParent(gameObject.transform.parent);
		}

		// Ensure the old window isn't still open
		WorldManager.Instance.TowerPanel.SetActive(false);

		// After disabling (ensuring no double clicks are registered)
		WorldManager.Instance.Inventory.Gold -= upgradeCost;

		// Destroy the old game object, immediately, so it takes effect in the editor.
		Destroy(gameObject);
	}
}
