const listeners = new Map();
let initialized = false;
let preventableKeys = [];

function ensureInit() {
    if (initialized) return;
    document.addEventListener('keydown', async (e) => {
        if (e.ctrlKey || preventableKeys.includes(e.key))
        {
            e.preventDefault();   
        } 
        else if (isTypingContext(e.target, e.key)) 
        {
            return; // let the input handle it normally
        }
        
        for (const dotNetRef of listeners.values()) {
            await dotNetRef.invokeMethodAsync('HandleKeyDown', e.key, e.ctrlKey, e.shiftKey, e.altKey);
        }
    });
    initialized = true;
}

function isTypingContext(target, key) {
    const isInputElement = target && (
        target.tagName === 'INPUT' ||
        target.tagName === 'TEXTAREA' ||
        target.tagName === 'SELECT' ||
        target.isContentEditable
    );
    if (!isInputElement) return false;

    const isAlphanumeric = key.length === 1 && /[a-zA-Z0-9]/.test(key);
    return isAlphanumeric;
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