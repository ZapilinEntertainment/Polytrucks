using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public enum TruckShowState { NoTruck, PlayerTruck, SelectedTruck, Placeholder }
    public class TruckShowModule
    {
        //submodule of truck switch service
        private enum PlaceholderAction { DoNothing, HidePlaceholder, ShowPlaceholder }

        private bool _placeholderActive = false;
        private readonly TruckSwitchService _switchService;
        private GameObject _placeholder;
        public Truck PlayerTruck { get; private set; } = null;
        public Truck ShowingTruck { get; private set; } = null;
        public TruckShowState State { get; private set; } = TruckShowState.NoTruck;

        public TruckShowModule(TruckSwitchService switchService)
        {
            _switchService = switchService;
        }

        public void OnPlayerTruckShown(Truck playerTruck)
        {
            State = TruckShowState.PlayerTruck;
            PlayerTruck = ShowingTruck = playerTruck;
        }
        public void ReturnToPlayerTruck()
        {
            if (State == TruckShowState.SelectedTruck)
            {
                _switchService.CacheTruck(ShowingTruck);
            }
            else
            {
                if (State == TruckShowState.Placeholder)
                {
                    _switchService.CachePlaceholder(_placeholder);
                }
            }
            PlayerTruck.SetVisibility(true);
            ShowingTruck = PlayerTruck;
            State = TruckShowState.PlayerTruck;
        }

        public Truck SwitchToTruck(TruckID id, VirtualPoint point, bool marksAsPlayers = false)
        {
            TruckShowState nextState;
            if (id == TruckID.Undefined) nextState = TruckShowState.Placeholder;
            else
            {
                if (id == PlayerTruck.TruckID) nextState = TruckShowState.PlayerTruck;
                else nextState = TruckShowState.SelectedTruck;
            }


            PlaceholderAction placeholderAction = PlaceholderAction.DoNothing;
            //if (nextState != State)
            {
                switch (State)
                {
                    case TruckShowState.PlayerTruck:
                        {
                            _switchService.SetPlayerVisibility(false);
                            break;
                        }
                    case TruckShowState.SelectedTruck:
                        {
                            if (_placeholderActive)
                            {
                                placeholderAction = PlaceholderAction.HidePlaceholder;
                            }
                            else
                            {
                                _switchService.CacheTruck(ShowingTruck);
                            }
                            break;
                        }
                    case TruckShowState.Placeholder:
                        {
                            placeholderAction = PlaceholderAction.HidePlaceholder;
                            break;
                        }
                }
                switch (nextState)
                {
                    case TruckShowState.PlayerTruck:
                        {
                            ShowingTruck = PlayerTruck;
                            _switchService.SetPlayerVisibility(true);
                            break;
                        }
                    case TruckShowState.SelectedTruck:
                        {
                            if (_switchService.IsTruckUnlocked(id))
                            {
                                ShowingTruck = _switchService.GetTruck(id, point);
                                ShowingTruck.SetVisibility(true);
                            }
                            else placeholderAction = PlaceholderAction.ShowPlaceholder;
                            break;
                        }
                    case TruckShowState.Placeholder:
                        {
                            placeholderAction = PlaceholderAction.ShowPlaceholder;
                            break;
                        }
                    case TruckShowState.NoTruck:
                        {
                            ShowingTruck = null;
                            break;
                        }
                }

                State = nextState;
            }

            switch (placeholderAction)
            {
                case PlaceholderAction.HidePlaceholder:
                    {
                        _switchService.CachePlaceholder(_placeholder);
                        _placeholder = null;
                        _placeholderActive = false;
                        break;
                    }
                case PlaceholderAction.ShowPlaceholder:
                    {
                        ShowingTruck = null;
                        _placeholder = _switchService.GetPlaceholder(point);
                        _placeholder.SetActive(true);
                        _placeholderActive = true;
                        break;
                    }
            }
            if (marksAsPlayers) PlayerTruck = ShowingTruck;
            return ShowingTruck;
        }
    }
}
