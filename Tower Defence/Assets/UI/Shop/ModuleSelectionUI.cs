using System;
using Abstract;
using MaterialLibrary;
using MaterialLibrary.Hexagons;
using Modules;
using TMPro;
using UI.Glyphs;
using UI.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    /// <summary>
    /// Displays a shop card for a Module
    /// </summary>
    public class ModuleSelectionUI : MonoBehaviour
    {
        public Module module;

        public Hexagons bg;

        public TextMeshProUGUI displayName;
        public TextMeshProUGUI tagline;

        public ModuleIcon icon;

        public TextMeshProUGUI effect;

        public GameObject glyphPrefab;
        public Transform applicableGlyphs;

        /// <summary>
        /// Creates the UI
        /// </summary>
        /// <param name="initModule">The module the card is for</param>
        /// <param name="shop">The shop script</param>
        /// <param name="lookup">The TypeSpriteLookup to get the glyph</param>
        public void Init (Module initModule, Gameplay.Shop shop, TypeSpriteLookup lookup)
        {
            module = initModule;
        
            bg.color = initModule.accentColor;

            displayName.text = initModule.displayName;
            tagline.text = initModule.tagline;
            tagline.color = initModule.accentColor;

            icon.SetData(initModule);
        
            effect.text = initModule.effectText;
            effect.color = initModule.accentColor;
            
            // Adds the any glyph
            if (initModule.GetValidTypes() == null)
            {
                TurretGlyphSo glyphSo = lookup.GetForType(null);
                Transform glyph = Instantiate(glyphPrefab, applicableGlyphs.transform).transform;
                glyph.Find("Body").GetComponent<HexagonSprite>().color = glyphSo.body;
                glyph.Find("Shade").GetComponent<HexagonSprite>().color = glyphSo.shade;
                glyph.Find("Glyph").GetComponent<Image>().sprite = glyphSo.glyph;
            }
            // Adds the glyph for every turret the module supports
            else
            {
                foreach (Type turretType in initModule.GetValidTypes())
                {
                    TurretGlyphSo glyphSo = lookup.GetForType(turretType);
                    Transform glyph = Instantiate(glyphPrefab, applicableGlyphs).transform;
                    glyph.Find("Body").GetComponent<HexagonSprite>().color = glyphSo.body;
                    glyph.Find("Shade").GetComponent<HexagonSprite>().color = glyphSo.shade;
                    glyph.Find("Glyph").GetComponent<Image>().sprite = glyphSo.glyph;
                }
            }
            
            // When the card is clicked, we pick the module
            bg.GetComponent<Button>().onClick.AddListener(delegate { MakeSelection(shop); });
        }

        /// <summary>
        /// Called when the player clicks on the card.
        /// Selects the module and closes the shop
        /// </summary>
        /// <param name="shop"></param>
        private void MakeSelection (Gameplay.Shop shop)
        {
            transform.parent.gameObject.SetActive (false);
            Time.timeScale = 1f;
        
            shop.SpawnNewModule(module);
        }
    }
}
