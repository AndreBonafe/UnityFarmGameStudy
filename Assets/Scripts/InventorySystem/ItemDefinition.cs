using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InventorySystem
{
    [CreateAssetMenu(menuName = "Inventory/Item Definition", fileName = "New Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private bool _isStackable;
        [SerializeField] private bool _isTool;
        [SerializeField] private Sprite _inGameSprite;
        [SerializeField] private Sprite _uiSprite;

        public string Name => _name;
        public bool IsStackble => _isStackable;
        public bool IsTool => _isTool;
        public Sprite InGameSprite => _inGameSprite;
        public Sprite UiSprite => _uiSprite;
    }

}
