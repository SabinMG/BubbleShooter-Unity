using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpaceOrigin.BubblePop
{
    public class GameControllerBehaviour : MonoBehaviour
    {
        #region public variables
        public BubbleShooter _bubbleShooter;
        public GameConfigSO _gameCofing;
        #endregion

        #region private varibles
        private GameController _gameController;
        #endregion

        #region unity callbacks
        void Awake()
        {
            _gameController = new GameController(Contexts.sharedInstance, _gameCofing);
            _gameController.Initialize();
        } 

        void Update()
        {
            _gameController.Execute();
            _bubbleShooter.Execute();
        }
        #endregion
    }
}
