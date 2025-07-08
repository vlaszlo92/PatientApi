using System.Windows.Controls;
using System.Windows.Input;
using AssistantClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AssistantClient.Views;

public partial class PatientView : UserControl
{
    public PatientView()
    {
        InitializeComponent();
        DataContext = App.Services.GetRequiredService<PatientViewModel>();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        FocusManager.SetFocusedElement(this, this);
    }
}
