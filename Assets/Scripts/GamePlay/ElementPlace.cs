using Configs;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.GamePlay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ElementPlace : MonoBehaviour, IPointerDownHandler
    {
        public event Action<string> OnElementClick = delegate { };

        public string ElementName => _elementName;
        public bool IsActive { get; set; } = false;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteRenderer _selfRenderer;

        [Header("Animation")]
        [SerializeField] private float _wrongShakeStrength = 0.1f;
        [SerializeField] private float _wrongAnimSpeed = 1f;
        [SerializeField] private ParticleSystem _rightParticle;

        private string _elementName;

        public void Spawn(Element element) => ChangeElement(element);

        public void Spawn(Element element, float animSpawnSpeed)
        {
            Spawn(element);
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, animSpawnSpeed);
        }

        private void OnDestroy() { DOTween.Kill(transform); }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsActive)
                return;

            Debug.Log(_elementName);
            OnElementClick?.Invoke(_elementName);
        }

        public void ChangeElement(Element element)
        {
            // change rotation
            _spriteRenderer.transform.rotation = Quaternion.Euler(Vector3.zero);
            _spriteRenderer.transform.Rotate(Vector3.back * element.RotationClockwise);

            // calculate and set scale
            var oldSize = _spriteRenderer.sprite.bounds.size.x > _spriteRenderer.sprite.bounds.size.y ?
                _spriteRenderer.sprite.bounds.size.x : _spriteRenderer.sprite.bounds.size.y;
            _spriteRenderer.sprite = element.Icon;
            var newSize = _spriteRenderer.sprite.bounds.size.x > _spriteRenderer.sprite.bounds.size.y ?
                _spriteRenderer.sprite.bounds.size.x : _spriteRenderer.sprite.bounds.size.y;
            _spriteRenderer.transform.localScale = (oldSize / newSize) * _spriteRenderer.transform.localScale;

            // set name
            _elementName = element.Name;
        }

        public Vector2 GetSize() { return _selfRenderer.sprite.bounds.size; }

        public void DoOnRightChoose()
        { 
            _rightParticle.Play(true);
            ShakeRenderer();
        }

        public void DoOnWrongChoose() =>
            ShakeRenderer();

        private void ShakeRenderer()
        {
            DOTween.Kill(_spriteRenderer, true);

            Sequence shakeSequence = DOTween.Sequence()
                .Append(_spriteRenderer.transform.DOShakePosition(
                    duration: _wrongAnimSpeed,
                    strength: _wrongShakeStrength).SetEase(Ease.InBounce));
        }
    }
}
