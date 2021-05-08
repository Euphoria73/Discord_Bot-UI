using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BotWithUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MessageBox.Show(
                "1.Enter correct Token\n" +
                "2.Choose Bot in Available guilds\n" +
                "3.Choose Available channels\n" +
                "4.Start chatting",
                "Instruction", 
                MessageBoxButton.OK,
                MessageBoxImage.Information);            
        }
    }
}
