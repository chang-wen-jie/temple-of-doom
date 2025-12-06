using TempleOfDoom.Logic.Models.Entities;

namespace TempleOfDoom.Logic.Models.Doors;

public class BaseDoor : Door
{
    private bool _isOpen = true;
    public override bool IsOpen => _isOpen;

    public override bool CanEnter(Player player)
    {
        return _isOpen;
    }

    public override void Open()
    {
        if (_isOpen) return;
        _isOpen = true;
        TwinDoor?.Open();
    }

    public override void Close()
    {
        if (!_isOpen) return;
        _isOpen = false;
        TwinDoor?.Close();
    }
}