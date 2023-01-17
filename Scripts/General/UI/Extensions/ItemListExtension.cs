using Godot;

public static class ItemListExtension {
    public static void AddItemAndShow(this ItemList itemList, string text, Texture icon = null, bool selectable = true) {
        itemList.AddItem(text, icon, selectable);
        if(itemList.GetItemCount() > 0) {
            itemList.Show();
        }
    }

    public static void RemoveItemAndHide(this ItemList itemList, int index) {
        itemList.RemoveItem(index);
        if(itemList.GetItemCount() <= 0) {
            itemList.Hide();
        }
    }
}