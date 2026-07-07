const listeners = new Map();
let initialized = false;
let preventableKeys = [];

function ensureInit() {
    if (initialized) return;
    document.addEventListener('keydown', async (e) => {
        if (preventableKeys.includes(e.key))
        {
            console.log('prevenented keys: ' + e.key)
            e.preventDefault();   
        }
        
        for (const dotNetRef of listeners.values()) {
            await dotNetRef.invokeMethodAsync('HandleKeyDown', e.key, e.ctrlKey, e.shiftKey, e.altKey);
        }
    });
    initialized = true;
}

export function setPreventableKeys(dotNetRef) {
    setInterval(async () => {
        const preventables = await dotNetRef.invokeMethodAsync('GetPreventableKeys');
        preventableKeys = preventables;
        console.log('preventable keys: ' + JSON.stringify(preventableKeys))
    }, 800);    
}

export function register(id, dotNetRef) {
    ensureInit();
    setPreventableKeys(dotNetRef);
    listeners.set(id, dotNetRef);
}

export function unregister(id) {
    listeners.delete(id);
}