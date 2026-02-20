// Keyed by inputId so multiple AFileInput instances on the same page are independent.
const _instances = new Map();

/**
 * Initialise drag-and-drop and paste handling for one AFileInput component.
 * @param {HTMLElement}  dropzoneEl  The visible drop-zone element.
 * @param {string}       inputId     The id of the hidden <input type="file">.
 * @param {DotNetObjectReference} dotnetRef  Blazor callback reference.
 */
export function initialize(dropzoneEl, inputId, dotnetRef) {
    const inputEl = document.getElementById(inputId);
    if (!inputEl) return;

    const instance = { dotnetRef, inputEl, dropzoneEl, dragCounter: 0 };
    _instances.set(inputId, instance);

    // ── Drag events ──────────────────────────────────────────────────────
    instance.onDragEnter = (e) => {
        e.preventDefault();
        if ([...e.dataTransfer.types].includes('Files')) {
            instance.dragCounter++;
            if (instance.dragCounter === 1)
                dotnetRef.invokeMethodAsync('SetDragging', true);
        }
    };

    instance.onDragOver = (e) => {
        e.preventDefault();
        e.dataTransfer.dropEffect = 'copy';
    };

    instance.onDragLeave = (e) => {
        e.preventDefault();
        instance.dragCounter = Math.max(0, instance.dragCounter - 1);
        if (instance.dragCounter === 0)
            dotnetRef.invokeMethodAsync('SetDragging', false);
    };

    instance.onDrop = (e) => {
        e.preventDefault();
        instance.dragCounter = 0;
        dotnetRef.invokeMethodAsync('SetDragging', false);
        const files = e.dataTransfer?.files;
        if (files && files.length > 0) _assignFiles(inputEl, files);
    };

    dropzoneEl.addEventListener('dragenter', instance.onDragEnter);
    dropzoneEl.addEventListener('dragover', instance.onDragOver);
    dropzoneEl.addEventListener('dragleave', instance.onDragLeave);
    dropzoneEl.addEventListener('drop', instance.onDrop);

    // ── Paste (document-level so the user doesn't have to focus the zone) ─
    // Skips the event when focus is inside a text input / textarea.
    instance.onDocumentPaste = (e) => {
        const active = document.activeElement;
        if (active && (
            active.tagName === 'INPUT' ||
            active.tagName === 'TEXTAREA' ||
            active.isContentEditable
        )) return;

        const files = e.clipboardData?.files;
        if (files && files.length > 0) {
            e.preventDefault();
            _assignFiles(inputEl, files);
        }
    };

    document.addEventListener('paste', instance.onDocumentPaste);
}

/**
 * Programmatically open the browser's file picker for the hidden input.
 * @param {string} inputId
 */
export function triggerBrowse(inputId) {
    document.getElementById(inputId)?.click();
}

/**
 * Remove all event listeners registered by this instance.
 * @param {string} inputId
 */
export function dispose(inputId) {
    const instance = _instances.get(inputId);
    if (!instance) return;
    instance.dropzoneEl.removeEventListener('dragenter', instance.onDragEnter);
    instance.dropzoneEl.removeEventListener('dragover', instance.onDragOver);
    instance.dropzoneEl.removeEventListener('dragleave', instance.onDragLeave);
    instance.dropzoneEl.removeEventListener('drop', instance.onDrop);
    document.removeEventListener('paste', instance.onDocumentPaste);
    _instances.delete(inputId);
}

/**
 * Route an external FileList through the hidden <input> so Blazor's
 * InputFile component raises its OnChange callback with proper IBrowserFile
 * instances.
 */
function _assignFiles(inputEl, files) {
    const dt = new DataTransfer();
    for (const file of files) dt.items.add(file);
    inputEl.files = dt.files;
    inputEl.dispatchEvent(new Event('change', { bubbles: true }));
}
