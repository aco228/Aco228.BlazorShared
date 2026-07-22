using Aco228.Common.Extensions;
using Aco228.Common.Models;

namespace Aco228.BlazorShared.Services;

public interface IKeyboardListenerService : ITransient
{
    void Register(string key, KeyboardListenerModel model, Func<Task> callback);
    void Remove(string key);
    bool Exists(bool isShiftPressed, bool isCtrlPressed, string keyPress, out bool shouldBePrevented);
    Task TryInvoke(bool isShiftPressed, bool isCtrlPressed, string keyPress);
    List<string> GetPreventableKeys();
}

public class KeywordListenerService : IKeyboardListenerService
{
    private ConcurrentList<KeyboardListenerStructModel> Events { get; set; } = new();
    
    public void Register(string key, KeyboardListenerModel model, Func<Task> callback)
    {
        var eventModel = new KeyboardListenerStructModel()
        {
            Callback = callback,
            CallerKey = key,
            Model = model,
            UniqueKey = $"{key}-{model.UniqueKey}"
        };
        var evt = Events.LastOrDefault(x => x.UniqueKey == eventModel.UniqueKey);
        if (evt == null)
        {
            Events.Add(eventModel);
            return;
        }
        
        evt.Callback = callback;
        evt.Model = model;
    }

    public void Remove(string key)
    {
        Events.RemoveOne(x => x.CallerKey == key);
    }

    public bool Exists(bool isShiftPressed, bool isCtrlPressed, string keyPress, out bool shouldBePrevented)
    {
        shouldBePrevented = false;
        var model = new KeyboardListenerModel() { ShiftPressed = isShiftPressed, CtrlPressed = isCtrlPressed, KeyPress = keyPress};
        var evt = Events.FirstOrDefault(x => x.Model.UniqueKey == model.UniqueKey);
        if (evt == null)
            return false;

        shouldBePrevented = evt.Model.Prevent;
        return true;
    }

    public async Task TryInvoke(bool isShiftPressed, bool isCtrlPressed, string keyPress)
    {
        if (Events.Any() == false)
            return;

        var model = new KeyboardListenerModel() { ShiftPressed = isShiftPressed, CtrlPressed = isCtrlPressed, KeyPress = keyPress};
        var evt = Events.FirstOrDefault(x => x.Model.UniqueKey == model.UniqueKey);
        if (evt == null)
            return;
        
        await evt.Callback();
    }

    public List<string> GetPreventableKeys()
        => Events.Where(x => x.Model.Prevent).Select(x => x.Model.KeyPress).Distinct().ToList();


    internal class KeyboardListenerStructModel
    {
        public string UniqueKey { get; set; }
        public string CallerKey { get; set; }
        public KeyboardListenerModel Model { get; set; }
        public Func<Task> Callback { get; set; }
    }
}

public class KeyboardListenerModel
{
    public string KeyPress { get; set; }
    public bool ShiftPressed { get; set; }
    public bool CtrlPressed { get; set; }
    public bool Prevent { get; set; }

    public KeyboardListenerModel() { }

    public KeyboardListenerModel(string key, bool isShiftPressed = false, bool isCtrlPressed = false, bool prevent = false)
    {
        KeyPress = key;
        ShiftPressed = isShiftPressed;
        CtrlPressed = isCtrlPressed;
        Prevent = prevent;
    }

    public string UniqueKey
        => $"{(ShiftPressed ? "st" : "sf")}-{(CtrlPressed ? "ct" : "cf")}-{KeyPress.ToLowerInvariant()}";
}