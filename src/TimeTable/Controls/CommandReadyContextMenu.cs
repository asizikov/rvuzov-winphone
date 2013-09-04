using System.Windows;
using System.Windows.Data;
using Microsoft.Phone.Controls;

namespace TimeTable.Controls
{
    public class CommandReadyContextMenu : ContextMenu
    {
        //ContextMenu class implicitly adds MenuItems with bound Header property but Command and CommandParameter properties are still not bound.
        //Creating ItemTemplate with menuItem with bound properties will put our MenuItem into implicitly created MenuItem(this is a problem).
        //To avoid this we modify the implicitly created container MenuItem to add a CommandBinding
        protected override DependencyObject GetContainerForItemOverride()
        {
            var item = new MenuItem();

            var headerBinding = new Binding("Header");
            item.SetBinding(MenuItem.HeaderProperty, headerBinding);

            var commandParameter = new Binding("CommandParameter");
            item.SetBinding(MenuItem.CommandParameterProperty, commandParameter);

            var commandBinding = new Binding("Command");
            item.SetBinding(MenuItem.CommandProperty, commandBinding);

            return item;
        }
    }
}
