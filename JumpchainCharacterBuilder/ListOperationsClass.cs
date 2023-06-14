using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JumpchainCharacterBuilder
{
    public static class ListOperationsClass
    {
        /// <summary>
        /// Swaps two items in a List<typeparamref name="T"/>. Assumes
        /// that the user has already done any needed checks pre-swap.
        /// </summary>
        /// <typeparam name="T">Represents the type of the List.</typeparam>
        /// <param name="list">Represents the list that used this extension.</param>
        /// <param name="pos1">Represents the index of the first item.</param>
        /// <param name="pos2">Represents the index of the second item.</param>
        public static void SwapListItems<T>(this List<T> list, int pos1, int pos2)
        {
            (list[pos1], list[pos2]) = (list[pos2], list[pos1]);
        }

        /// <summary>
        /// Swaps two items in an ObservableCollection<typeparamref name="T"/>. Assumes
        /// that the user has already done any needed checks pre-swap.
        /// </summary>
        /// <typeparam name="T">Represents the type of the ObservableCollection</typeparam>
        /// <param name="list">Represents the ObservableCollection that used this
        /// extension.</param>
        /// <param name="pos1">Represents the index of the first item.</param>
        /// <param name="pos2">Represents the index of the second item.</param>
        public static void SwapCollectionItems<T>(this ObservableCollection<T> list, int pos1, int pos2)
        {
            (list[pos1], list[pos2]) = (list[pos2], list[pos1]);
        }
    }
}
