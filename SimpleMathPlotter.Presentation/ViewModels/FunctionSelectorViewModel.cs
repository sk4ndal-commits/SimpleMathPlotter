using SimpleMathPlotter.Core.Models;
using SimpleMathPlotter.Presentation.Utils;

namespace SimpleMathPlotter.Presentation.ViewModels;

public class FunctionSelectorViewModel : ViewModelBase
{
    private FunctionType _selectedFunctionType = FunctionType.Sin;
    public Array FunctionTypes => Enum.GetValues<FunctionType>();

    public FunctionType SelectedFunctionType
    {
        get => _selectedFunctionType;
        set => SetField(ref _selectedFunctionType, value);
    }
}