using System.Collections;
using System.Collections.Generic;
using RPG.UI;
using UnityEngine;

namespace RPG.UI
{
    public class CharacterWindowView : UIBaseView
    {
        private CharacterWindowViewModel VM => ViewModel as CharacterWindowViewModel;
        [SerializeField] private CharacterBinder _characterBinder;
        private bool _eventsBound = false;
        protected override void BindEvents()
        {
            if (VM == null)
            {
                _eventsBound = false;
                return;
            }
    
            if (_eventsBound)
            {
                return;
            }

            ReFreshCharacterUI();
        }
    
        protected override void UnbindEvents()
        {
            if (! _eventsBound)
                return;
            
        }
        
        private void ReFreshCharacterUI()
        {
            if (VM == null)
            {
                return;
            }
            var data = VM.CurrentCharacterData;
                
            if (data == null) 
            {
                _characterBinder?.SetModel(null);
                return;
            }
            _characterBinder?.SetModel(data);
            
            
        }
        
        private void ClearItemUI()
        {
            _characterBinder?.SetModel(null);
        }
    }
}

