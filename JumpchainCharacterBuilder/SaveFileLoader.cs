using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;

namespace JumpchainCharacterBuilder
{
    /// <summary>
    /// Replaces existing save data in memory with the chosen save data from disk.
    /// </summary>
    public class SaveFileLoader : ObservableRecipient
    {
        public void LoadSave(string filePath, SaveFile saveFile)
        {
            FileAccess.CheckSubdirectoryExists("Saves");

            SaveFile newSave = XmlAccess.ReadObject(filePath);

            if (newSave.SaveVersion < 1.1)
            {
                newSave = SaveMigration.SaveModify(newSave);
            }
            if (newSave.SaveVersion < 1.3)
            {
                newSave = SaveMigration.SaveUpdate(filePath, newSave.SaveVersion, newSave);
            }
            if (newSave.SaveVersion < 1.4)
            {
                newSave = SaveMigration.SaveUpdate(filePath, newSave.SaveVersion, newSave);
            }

            ReplaceSave(saveFile, newSave);
        }

        public void NewSave(SaveFile oldSave)
        {
            ReplaceSave(oldSave, new());
        }

        private void ReplaceSave(SaveFile existingSave, SaveFile newSave)
        {
            existingSave.JumpList = [];
            existingSave.CharacterList = [];
            existingSave.Options = new();
            existingSave.GenericBodyMod = new();
            existingSave.SBBodyMod = new();
            existingSave.EssentialBodyMod = new();
            existingSave.GenericWarehouse = new();
            existingSave.PersonalReality = new();
            existingSave.GenericDrawbackSupplement = new();
            existingSave.UniversalDrawbackSupplement = new();
            existingSave.UUSupplement = new();
            existingSave.ItemCategoryList = [];
            existingSave.PerkCategoryList = [];
            existingSave.UserPerkCategoryList = [];
            existingSave.UserItemCategoryList = [];

            existingSave.Options = newSave.Options;
            existingSave.GenericBodyMod = newSave.GenericBodyMod;
            existingSave.SBBodyMod = newSave.SBBodyMod;
            existingSave.EssentialBodyMod = newSave.EssentialBodyMod;
            existingSave.GenericWarehouse = newSave.GenericWarehouse;
            existingSave.PersonalReality = newSave.PersonalReality;
            existingSave.GenericDrawbackSupplement = newSave.GenericDrawbackSupplement;
            existingSave.UniversalDrawbackSupplement = newSave.UniversalDrawbackSupplement;
            existingSave.UUSupplement = newSave.UUSupplement;
            existingSave.JumpList = newSave.JumpList;
            existingSave.CharacterList = newSave.CharacterList;
            existingSave.ItemCategoryList = newSave.ItemCategoryList;
            existingSave.PerkCategoryList = newSave.PerkCategoryList;
            existingSave.UserItemCategoryList = newSave.UserItemCategoryList;
            existingSave.UserPerkCategoryList = newSave.UserPerkCategoryList;

            existingSave.SaveVersion = newSave.SaveVersion;

            Messenger.Send(new SaveDataChangedMessage("Save Updated"));
        }

    }
}
