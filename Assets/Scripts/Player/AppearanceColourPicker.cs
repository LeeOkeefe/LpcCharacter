using Animation;
using UnityEngine;

namespace Player
{
    internal sealed class AppearanceColourPicker : MonoBehaviour
    {
        [SerializeField] private Color[] colours = { Color.white, Color.red, Color.blue, Color.cyan, Color.green, Color.magenta, Color.grey, Color.yellow, Color.black };
        
        private BodyPartAnimator _bodyPart;
        private int _currentColourId;

        private void Awake()
        {
            _bodyPart = GetComponent<BodyPartAnimator>();
        }

        public void NextColour()
        {
            _currentColourId++;

            if (_currentColourId >= colours.Length)
            {
                _currentColourId = 0;
            }

            _bodyPart.UpdateColour(colours[_currentColourId]);
        }
    }
}