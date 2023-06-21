using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Panteon.UI
{
    public class InfiniteScrollView : ScrollRect
    {
        private List<RectTransform> _views;
        private bool _up = false;
        private GridLayoutGroup _gridLayoutGroup;
        private bool _hold = false;

        float heighTreshold = 150f;

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (_gridLayoutGroup == null)
            {
                _gridLayoutGroup = content.GetComponent<GridLayoutGroup>();
                heighTreshold = _gridLayoutGroup.cellSize.y + _gridLayoutGroup.spacing.y;
            }

            if (_views == null || _views.Count == 0)
            {
                _views = new List<RectTransform>();
                for (int i = 0; i < transform.childCount; i++)
                {
                    _views.Add(transform.GetChild(i) as RectTransform);
                }
            }
            base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            _hold = false;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            _hold = true;
            if ((eventData.pressPosition - eventData.position).y < 0)
            {
                _up = true;
            }
            else if ((eventData.pressPosition - eventData.position).y > 0)
            {
                _up = false;
            }
            base.OnDrag(eventData);
        }

        protected override void SetContentAnchoredPosition(Vector2 position)
        {
            if (_hold)
            {
                base.SetContentAnchoredPosition(position);
                return;
            }

            if (_up)
            {
                if (position.y > 400f)
                {
                    position.y -= heighTreshold;
                    SendObjects();
                }
            }
            else
            {
                if (position.y < 400f)
                {
                    position.y += heighTreshold;
                    SendObjects();
                }
            }
            base.SetContentAnchoredPosition(position);
        }

        public override void OnScroll(PointerEventData data)
        {
            base.OnScroll(data);
        }

        private void SendObjects()
        {
            if (_up)
            {
                for (int i = 0; i < 2; i++)
                {
                    content.GetChild(0).SetAsLastSibling();
                }
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    content.GetChild(content.childCount - 1).SetAsFirstSibling();
                }
            }
            UpdatePrevData();
            UpdateBounds();
        }
    }
}
