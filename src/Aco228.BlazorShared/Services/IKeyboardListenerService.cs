using Aco228.Common.Extensions;
using Aco228.Common.Models;

namespace Aco228.BlazorShared.Services;

public interface IKeyboardListenerService : ISingleton
{
    void Register(string key, KeyboardListenerModel model, Func<Task> callback);
    void Remove(string key);
    Task TryInvoke(bool isShiftPressed, bool isCtrlPressed, string keyPress);
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
        var evt = Events.FirstOrDefault(x => x.UniqueKey == eventModel.UniqueKey);
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

    public KeyboardListenerModel() { }

    public KeyboardListenerModel(string key, bool isShiftPressed = false, bool isCtrlPressed = false)
    {
        KeyPress = key;
        ShiftPressed = isShiftPressed;
        CtrlPressed = isCtrlPressed;
    }

    public string UniqueKey
        => $"{(ShiftPressed ? "st" : "sf")}-{(CtrlPressed ? "ct" : "cf")}-{KeyPress.ToLowerInvariant()}";
}