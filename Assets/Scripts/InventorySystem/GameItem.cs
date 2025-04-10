using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.InventorySystem
{
    public class GameItem : MonoBehaviour
    {
        [SerializeField] private ItemStack _stack;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Header("Throw Settings")]
        [SerializeField] private float _colliderEnabledAfter = 1f;
        [SerializeField] private float _throwGravity = 2f;
        [SerializeField] private float _minXForce = 3f;
        [SerializeField] private float _maxXForce = 5f;
        [SerializeField] private float _throwYForce = 5f;
        public ItemStack Stack => _stack;

        private Collider2D _collider;
        private Rigidbody2D _rigdbody;

        private void Awake() {
            _collider = GetComponent<Collider2D>();
            _rigdbody = GetComponent<Rigidbody2D>();
        }

        private void Start() {
            SetupGameObject();
            _collider.enabled = false;
            StartCoroutine(EnableCollider(_colliderEnabledAfter));
        }

        private void OnValidate() {
            SetupGameObject();
        }
        private void SetupGameObject()
        {
            if (_stack.Item == null) return;
            SetGameSprite();
            AdjustNumberofItems();
            UpdateGameObjectName();
        }

        private void SetGameSprite()
        {
            _spriteRenderer.sprite = _stack.Item.InGameSprite;
        }

        private void UpdateGameObjectName()
        {
            var name = _stack.Item.Name;
            var number = _stack.IsStackble ? _stack.NumberOfitems.ToString() : "ns";
            gameObject.name = $"{name} ({number})";
        }

        private void AdjustNumberofItems()
        {
            _stack.NumberOfitems = _stack.NumberOfitems;
        }

        public ItemStack Pick()
        {
            Destroy(gameObject);
            return _stack;
        }

        public void Throw(float xDir)
        {
            _rigdbody.gravityScale = _throwGravity;
            var throwXForce = Random.Range(_minXForce, _maxXForce);
            _rigdbody.velocity = new Vector2(Mathf.Sign(xDir) * throwXForce, _throwYForce);
            StartCoroutine(DisableGravity(_throwYForce));
        }

        private IEnumerator DisableGravity(float atYVelocity)
        {
            yield return new WaitUntil(() => _rigdbody.velocity.y < -atYVelocity);
            _rigdbody.velocity = Vector2.zero;
            _rigdbody.gravityScale = 0;

        }

        private IEnumerator EnableCollider(float afterTime)
        {
            yield return new WaitForSeconds(afterTime);
            _collider.enabled = true;
        }

        public void SetStack(ItemStack stack)
        {
            _stack = stack;
        }
    }
}
