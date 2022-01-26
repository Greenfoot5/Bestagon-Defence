using Abstract.Data;
using TMPro;
using Turrets.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ModuleSelectionUI : MonoBehaviour
    {
        public Module Module;

        public Hexagons bg;

        public TextMeshProUGUI displayName;
        public TextMeshProUGUI tagline;

        public ModuleIcon icon;

        public TextMeshProUGUI effect;

        public GameObject glyphPrefab;
        public Transform applicableGlyphs;

        // Called when creating the UI
        public void Init (Module initModule, Shop shop, TypeSpriteLookup lookup)
        {
            Module = initModule;
        
            bg.color = initModule.accentColor;

            displayName.text = initModule.displayName;
            tagline.text = initModule.tagline;
            tagline.color = initModule.accentColor;

            icon.SetData(initModule);
        
            effect.text = initModule.effectText;
            effect.color = initModule.accentColor;

            if (initModule.GetValidTypes() == null)
            {
                var glyphSo = lookup.GetForType(null);
                var glyph = Instantiate(glyphPrefab, applicableGlyphs.transform).transform;
                glyph.Find("Body").GetComponent<HexagonSprite>().color = glyphSo.body;
                glyph.Find("Shade").GetComponent<HexagonSprite>().color = glyphSo.shade;
                glyph.Find("Glyph").GetComponent<Image>().sprite = glyphSo.glyph;
            }
            else
            {
                foreach (var turretType in initModule.GetValidTypes())
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

        // Called when the player clicks on the button
        private void MakeSelection (Shop shop)
        {
            transform.parent.gameObject.SetActive (false);
            Time.timeScale = 1f;
        
            shop.SpawnNewModule(Module);
        }
    }
}
