using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Interfaces;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class DrawbackSupplementViewModel : ViewModelBase
    {
        #region Fields

        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private SaveFile _loadedSave = new();
        [ObservableProperty]
        private Options _loadedOptions = new();
        [ObservableProperty]
        private Options.DrawbackSupplements _loadedSupplement = Options.DrawbackSupplements.Generic;

        [ObservableProperty]
        private bool _uUSupplementSelected = false;

        [ObservableProperty]
        private ObservableCollection<DrawbackSupplementPurchase> _drawbackList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteDrawbackCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveDrawbackUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveDrawbackDownCommand))]
        private DrawbackSupplementPurchase _drawbackSelection = new();
        [ObservableProperty]
        private int _drawbackSelectionIndex = 0;

        [ObservableProperty]
        private ObservableCollection<HouseRuleModel> _houseRuleList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteHouseRuleCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveHouseRuleUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveHouseRuleDownCommand))]
        private HouseRuleModel _houseRuleSelection = new();
        [ObservableProperty]
        private int _houseRuleSelectionIndex = 0;

        [ObservableProperty]
        private int _cPGained = 0;
        [ObservableProperty]
        private int _cPCompanionGained = 0;
        [ObservableProperty]
        private int _cPItemGained = 0;
        [ObservableProperty]
        private int _wPGained = 0;

        [ObservableProperty]
        private ObservableCollection<DrawbackSuspendModel> _drawbackSuspendList = new();

        [ObservableProperty]
        private string _uUMode = "";
        [ObservableProperty]
        private string _uUModeDescription = "";
        [ObservableProperty]
        private string _uURiskLevel = "";
        [ObservableProperty]
        private string _uURiskLevelDescription = "";

        #endregion

        #region Properties
        partial void OnDrawbackSelectionChanged(DrawbackSupplementPurchase value)
        {
            if (value != null)
            {
                ListValidationClass.CheckDrawbackSuspendCount(DrawbackSelection, LoadedSave.JumpList.Count);

                LoadDrawbackSuspends();
                UpdatePointValues();
            }
        }

        partial void OnUUModeChanged(string value) => LoadedSave.UUSupplement.Mode = value;

        partial void OnUUModeDescriptionChanged(string value) => LoadedSave.UUSupplement.ModeDescription = value;

        partial void OnUURiskLevelChanged(string value) => LoadedSave.UUSupplement.RiskLevel = value;

        partial void OnUURiskLevelDescriptionChanged(string value) => LoadedSave.UUSupplement.RiskLevelDescription = value;
        #endregion

        #region Constructor

        public DrawbackSupplementViewModel()
        {

        }

        public DrawbackSupplementViewModel(IDialogService dialogService)
        {
            Messenger.Register<SaveDataChangedMessage>(this, (r, m) =>
            {
                LoadedOptions = LoadedSave.Options;
                LoadedSupplement = LoadedSave.Options.DrawbackSupplementSetting;
                LoadSupplementData();
                LoadSuspendJumpNames();
            });
            Messenger.Register<SaveDataSendMessage>(this, (r, m) =>
            {
                LoadedSave = m.Value;
                LoadedOptions = LoadedSave.Options;
                LoadedSupplement = LoadedOptions.DrawbackSupplementSetting;
                LoadSupplementData();
                LoadSuspendJumpNames();
            });
            Messenger.Register<SupplementChangedMessage>(this, (r, m) =>
            {
                if (m.Value == "Drawback")
                {
                    LoadedSupplement = LoadedOptions.DrawbackSupplementSetting;
                    LoadSupplementData();
                    LoadSuspendJumpNames();
                }
            });

            _dialogService = dialogService;
        }

        #endregion

        #region Methods

        private void LoadSupplementData()
        {
            switch (LoadedSupplement)
            {
                case Options.DrawbackSupplements.Generic:
                    CPGained = LoadedSave.GenericDrawbackSupplement.CPGained;
                    CPItemGained = LoadedSave.GenericDrawbackSupplement.CPItemGained;
                    CPCompanionGained = LoadedSave.GenericDrawbackSupplement.CPCompanionGained;
                    WPGained = LoadedSave.GenericDrawbackSupplement.WPGained;
                    LoadDrawbackList(LoadedSave.GenericDrawbackSupplement.Purchases);
                    LoadHouseRuleList(LoadedSave.GenericDrawbackSupplement.HouseRules);

                    UUSupplementSelected = false;
                    break;
                case Options.DrawbackSupplements.UDS:
                    CPGained = LoadedSave.UniversalDrawbackSupplement.CPGained;
                    CPItemGained = LoadedSave.UniversalDrawbackSupplement.CPItemGained;
                    CPCompanionGained = LoadedSave.UniversalDrawbackSupplement.CPCompanionGained;
                    WPGained = LoadedSave.UniversalDrawbackSupplement.WPGained;
                    LoadDrawbackList(LoadedSave.UniversalDrawbackSupplement.Purchases);
                    LoadHouseRuleList(LoadedSave.UniversalDrawbackSupplement.HouseRules);

                    UUSupplementSelected = false;
                    break;
                case Options.DrawbackSupplements.UU:
                    CPGained = LoadedSave.UUSupplement.CPGained;
                    CPItemGained = LoadedSave.UUSupplement.CPItemGained;
                    CPCompanionGained = LoadedSave.UUSupplement.CPCompanionGained;
                    WPGained = LoadedSave.UUSupplement.WPGained;
                    LoadDrawbackList(LoadedSave.UUSupplement.Purchases);
                    LoadHouseRuleList(LoadedSave.UUSupplement.HouseRules);

                    UUMode = LoadedSave.UUSupplement.Mode;
                    UUModeDescription = LoadedSave.UUSupplement.ModeDescription;
                    UURiskLevel = LoadedSave.UUSupplement.RiskLevel;
                    UURiskLevelDescription = LoadedSave.UUSupplement.RiskLevelDescription;

                    UUSupplementSelected = true;
                    break;
            }
        }

        private void LoadDrawbackList(List<DrawbackSupplementPurchase> drawbackList)
        {
            DrawbackList.Clear();

            foreach (DrawbackSupplementPurchase drawback in drawbackList)
            {
                DrawbackList.Add(drawback);
            }

            if (DrawbackList.Any())
            {
                DrawbackSelection = DrawbackList.Last();
            }
        }

        private void LoadHouseRuleList(List<HouseRuleModel> houseRuleList)
        {
            HouseRuleList.Clear();

            foreach (HouseRuleModel houseRule in houseRuleList)
            {
                HouseRuleList.Add(houseRule);
            }

            if (HouseRuleList.Any())
            {
                HouseRuleSelection = HouseRuleList.Last();
            }
        }

        private void LoadDrawbackSuspends()
        {
            DrawbackSuspendList.Clear();

            DrawbackSuspendList = new(DrawbackSelection.SuspendList);

            LoadSuspendJumpNames();
        }

        private void LoadSuspendJumpNames()
        {
            for (int i = 0; i < DrawbackSuspendList.Count; i++)
            {
                DrawbackSuspendList[i].JumpName = LoadedSave.JumpList[i].Name;
            }
        }

        private void UpdatePointValues()
        {
            int cPTotal = 0;
            int cPItemTotal = 0;
            int cPCompanionTotal = 0;
            int wPTotal = 0;

            foreach (DrawbackSupplementPurchase drawback in DrawbackList)
            {
                cPTotal += drawback.ValueChoicePoints;
                cPItemTotal += drawback.ValueItemPoints;
                cPCompanionTotal += drawback.ValueCompanionPoints;
                wPTotal += drawback.ValueWarehousePoints;
            }

            switch (LoadedSupplement)
            {
                case Options.DrawbackSupplements.Generic:
                    LoadedSave.GenericDrawbackSupplement.CPGained = cPTotal;
                    LoadedSave.GenericDrawbackSupplement.CPItemGained = cPItemTotal;
                    LoadedSave.GenericDrawbackSupplement.CPCompanionGained = cPCompanionTotal;
                    LoadedSave.GenericDrawbackSupplement.WPGained = wPTotal;
                    break;
                case Options.DrawbackSupplements.UDS:
                    LoadedSave.UniversalDrawbackSupplement.CPGained = cPTotal;
                    LoadedSave.UniversalDrawbackSupplement.CPItemGained = cPItemTotal;
                    LoadedSave.UniversalDrawbackSupplement.CPCompanionGained = cPCompanionTotal;
                    LoadedSave.UniversalDrawbackSupplement.WPGained = wPTotal;
                    break;
                case Options.DrawbackSupplements.UU:
                    LoadedSave.UUSupplement.CPGained = cPTotal;
                    LoadedSave.UUSupplement.CPItemGained = cPItemTotal;
                    LoadedSave.UUSupplement.CPCompanionGained = cPCompanionTotal;
                    LoadedSave.UUSupplement.WPGained = wPTotal;
                    break;
            }

            CPGained = cPTotal;
            CPItemGained = cPItemTotal;
            CPCompanionGained = cPCompanionTotal;
            WPGained = wPTotal;
        }

        #endregion

        #region Commands

        [RelayCommand]
        private void NewHouseRule()
        {
            HouseRuleModel houseRule = new($"House Rule #{HouseRuleList.Count + 1}");

            switch (LoadedSupplement)
            {
                case Options.DrawbackSupplements.Generic:
                    LoadedSave.GenericDrawbackSupplement.HouseRules.Add(houseRule);
                    break;
                case Options.DrawbackSupplements.UDS:
                    LoadedSave.UniversalDrawbackSupplement.HouseRules.Add(houseRule);
                    break;
                case Options.DrawbackSupplements.UU:
                    LoadedSave.UUSupplement.HouseRules.Add(houseRule);
                    break;
            }

            HouseRuleList.Add(houseRule);
            HouseRuleSelection = HouseRuleList.Last();

            DeleteHouseRuleCommand.NotifyCanExecuteChanged();
            MoveHouseRuleUpCommand.NotifyCanExecuteChanged();
            MoveHouseRuleDownCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteHouseRule))]
        private void DeleteHouseRule()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this House Rule? " +
                "This action cannot be reversed."))
            {
                int index = HouseRuleSelectionIndex;
                switch (LoadedSupplement)
                {
                    case Options.DrawbackSupplements.Generic:
                        LoadedSave.GenericDrawbackSupplement.HouseRules.RemoveAt(index);
                        break;
                    case Options.DrawbackSupplements.UDS:
                        LoadedSave.UniversalDrawbackSupplement.HouseRules.RemoveAt(index);
                        break;
                    case Options.DrawbackSupplements.UU:
                        LoadedSave.UUSupplement.HouseRules.RemoveAt(index);
                        break;
                }
                HouseRuleList.RemoveAt(index);
                DeleteHouseRuleCommand.NotifyCanExecuteChanged();
                MoveHouseRuleUpCommand.NotifyCanExecuteChanged();
                MoveHouseRuleDownCommand.NotifyCanExecuteChanged();

                HouseRuleSelectionIndex = 0;
            }

        }

        private bool CanDeleteHouseRule() => HouseRuleList.Any() && HouseRuleSelection != null;

        [RelayCommand(CanExecute = nameof(CanMoveHouseRuleUp))]
        private void MoveHouseRuleUp()
        {
            int index = HouseRuleSelectionIndex;
            HouseRuleList.SwapCollectionItems(index, index - 1);
            switch (LoadedSupplement)
            {
                case Options.DrawbackSupplements.Generic:
                    LoadedSave.GenericDrawbackSupplement.HouseRules.SwapListItems(index, index - 1);
                    break;
                case Options.DrawbackSupplements.UDS:
                    LoadedSave.UniversalDrawbackSupplement.HouseRules.SwapListItems(index, index - 1);
                    break;
                case Options.DrawbackSupplements.UU:
                    LoadedSave.UUSupplement.HouseRules.SwapListItems(index, index - 1);
                    break;
            }
            HouseRuleSelectionIndex = index - 1;

            DeleteHouseRuleCommand.NotifyCanExecuteChanged();
            MoveHouseRuleUpCommand.NotifyCanExecuteChanged();
            MoveHouseRuleDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveHouseRuleUp() => HouseRuleSelectionIndex > 0 && HouseRuleSelection != null;

        [RelayCommand(CanExecute = nameof(CanMoveHouseRuleDown))]
        private void MoveHouseRuleDown()
        {
            int index = HouseRuleSelectionIndex;
            HouseRuleList.SwapCollectionItems(index, index + 1);
            switch (LoadedSupplement)
            {
                case Options.DrawbackSupplements.Generic:
                    LoadedSave.GenericDrawbackSupplement.HouseRules.SwapListItems(index, index + 1);
                    break;
                case Options.DrawbackSupplements.UDS:
                    LoadedSave.UniversalDrawbackSupplement.HouseRules.SwapListItems(index, index + 1);
                    break;
                case Options.DrawbackSupplements.UU:
                    LoadedSave.UUSupplement.HouseRules.SwapListItems(index, index + 1);
                    break;
            }
            HouseRuleSelectionIndex = index + 1;

            DeleteHouseRuleCommand.NotifyCanExecuteChanged();
            MoveHouseRuleUpCommand.NotifyCanExecuteChanged();
            MoveHouseRuleDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveHouseRuleDown() => HouseRuleSelectionIndex < HouseRuleList.Count - 1 && HouseRuleSelection != null;

        [RelayCommand]
        private void NewDrawback()
        {
            DrawbackSupplementPurchase drawback = new($"Drawback #{DrawbackList.Count + 1}", LoadedSave.JumpList.Count);

            switch (LoadedSupplement)
            {
                case Options.DrawbackSupplements.Generic:
                    LoadedSave.GenericDrawbackSupplement.Purchases.Add(drawback);
                    break;
                case Options.DrawbackSupplements.UDS:
                    LoadedSave.UniversalDrawbackSupplement.Purchases.Add(drawback);
                    break;
                case Options.DrawbackSupplements.UU:
                    LoadedSave.UUSupplement.Purchases.Add(drawback);
                    break;
            }
            DrawbackList.Add(drawback);
            DrawbackSelection = DrawbackList.Last();

            DeleteDrawbackCommand.NotifyCanExecuteChanged();
            MoveDrawbackDownCommand.NotifyCanExecuteChanged();
            MoveDrawbackUpCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteDrawback))]
        private void DeleteDrawback()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Drawback? " +
                "This action cannot be reversed."))
            {
                int index = DrawbackSelectionIndex;
                switch (LoadedSupplement)
                {
                    case Options.DrawbackSupplements.Generic:
                        LoadedSave.GenericDrawbackSupplement.Purchases.RemoveAt(index);
                        break;
                    case Options.DrawbackSupplements.UDS:
                        LoadedSave.UniversalDrawbackSupplement.Purchases.RemoveAt(index);
                        break;
                    case Options.DrawbackSupplements.UU:
                        LoadedSave.UUSupplement.Purchases.RemoveAt(index);
                        break;
                }
                DrawbackList.RemoveAt(index);

                if (DrawbackList.Any())
                {
                    DrawbackSelection = DrawbackList.First();
                }

                DeleteDrawbackCommand.NotifyCanExecuteChanged();
                MoveDrawbackDownCommand.NotifyCanExecuteChanged();
                MoveDrawbackUpCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteDrawback() => DrawbackList.Any() && DrawbackSelection != null;

        [RelayCommand(CanExecute = nameof(CanMoveDrawbackUp))]
        private void MoveDrawbackUp()
        {
            int index = DrawbackSelectionIndex;
            DrawbackList.SwapCollectionItems(index, index - 1);
            switch (LoadedSupplement)
            {
                case Options.DrawbackSupplements.Generic:
                    LoadedSave.GenericDrawbackSupplement.Purchases.SwapListItems(index, index - 1);
                    break;
                case Options.DrawbackSupplements.UDS:
                    LoadedSave.UniversalDrawbackSupplement.Purchases.SwapListItems(index, index - 1);
                    break;
                case Options.DrawbackSupplements.UU:
                    LoadedSave.UUSupplement.Purchases.SwapListItems(index, index - 1);
                    break;
            }
            DrawbackSelectionIndex = index - 1;

            DeleteDrawbackCommand.NotifyCanExecuteChanged();
            MoveDrawbackDownCommand.NotifyCanExecuteChanged();
            MoveDrawbackUpCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveDrawbackUp() => DrawbackSelectionIndex > 0 && DrawbackSelection != null;

        [RelayCommand(CanExecute = nameof(CanMoveDrawbackDown))]
        private void MoveDrawbackDown()
        {
            int index = DrawbackSelectionIndex;
            DrawbackList.SwapCollectionItems(index, index + 1);
            switch (LoadedSupplement)
            {
                case Options.DrawbackSupplements.Generic:
                    LoadedSave.GenericDrawbackSupplement.Purchases.SwapListItems(index, index + 1);
                    break;
                case Options.DrawbackSupplements.UDS:
                    LoadedSave.UniversalDrawbackSupplement.Purchases.SwapListItems(index, index + 1);
                    break;
                case Options.DrawbackSupplements.UU:
                    LoadedSave.UUSupplement.Purchases.SwapListItems(index, index + 1);
                    break;
            }
            DrawbackSelectionIndex = index + 1;

            DeleteDrawbackCommand.NotifyCanExecuteChanged();
            MoveDrawbackDownCommand.NotifyCanExecuteChanged();
            MoveDrawbackUpCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveDrawbackDown() => DrawbackSelectionIndex < DrawbackList.Count - 1 && DrawbackSelection != null;

        #endregion
    }
}
