using TMPro;
using Turrets.Upgrades;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeSelectionUI : MonoBehaviour
    {
        public Upgrade upgrade;

        public Hexagons bg;

        public TextMeshProUGUI displayName;
        public TextMeshProUGUI tagline;

        public UpgradeIcon icon;

        public TextMeshProUGUI effect;

        public GameObject glyphPrefab;
        public Transform applicableGlyphs;

        // Called when creating the UI
        public void Init (Upgrade initUpgrade, Shop shop, TypeSpriteLookup lookup)
        {
            upgrade = initUpgrade;
        
            bg.color = initUpgrade.accentColor;

            displayName.text = initUpgrade.displayName;
            tagline.text = initUpgrade.tagline;
            tagline.color = initUpgrade.accentColor;

            icon.SetData(initUpgrade);
        
            effect.text = initUpgrade.effectText;
            effect.color = initUpgrade.accentColor;

            if (initUpgrade.GetValidTypes() == null)
            {
                var glyphSo = lookup.GetForType(null);
                var glyph = Instantiate(glyphPrefab, applicableGlyphs.transform).transform;
                glyph.Find("Body").GetComponent<HexagonSprite>().color = glyphSo.body;
                glyph.Find("Shade").GetComponent<HexagonSprite>().color = glyphSo.shade;
                glyph.Find("Glyph").GetComponent<Image>().sprite = glyphSo.glyph;
            }
            else
            {
                foreach (var turretType in initUpgrade.GetValidTypes())
                {
                    var glyphSo = lookup.GetForType(turretType);
                    var glyph = Instantiate(glyphPrefab, applicableGlyphs).transform;
                    glyph.Find("Body").GetComponent<HexagonSprite>().color = glyphSo.body;
                    glyph.Find("Shade").GetComponent<HexagonSprite>().color = glyphSo.shade;
                    glyph.Find("Glyph").GetComponent<Image>().sprite = glyphSo.glyph;
                }
            }

            bg.GetComponent<Button>().onClick.AddListener(delegate { MakeSelection(shop); });
        }

        // Called when the user clicks on the button
        private void MakeSelection (Shop shop)
        {
            transform.parent.gameObject.SetActive (false);
            Time.timeScale = 1f;
        
            shop.SpawnNewUpgrade(upgrade);
        }
    }
}
