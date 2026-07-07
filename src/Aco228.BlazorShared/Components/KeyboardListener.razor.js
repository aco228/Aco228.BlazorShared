const listeners = new Map();
let initialized = false;

function ensureInit() {
    if (initialized) return;
    document.addEventListener('keydown', (e) => {
        for (const dotNetRef of listeners.values()) {
            dotNetRef.invokeMethodAsync('HandleKeyDown', e.key, e.ctrlKey, e.shiftKey, e.altKey);
        }
    });
    initialized = true;
}

export function register(id, dotNetRef) {
    ensureInit();
    listeners.set(id, dotNetRef);
}

export function unregister(id) {
    listeners.delete(id);
}