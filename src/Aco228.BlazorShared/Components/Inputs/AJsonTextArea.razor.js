function tokenize(text) {
    if (!text) return '';

    const escaped = text
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');

    return escaped.replace(
        /("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g,
        match => {
            let cls;
            if (/^"/.test(match)) {
                cls = /:$/.test(match) ? 'json-key' : 'json-string';
            } else if (/true|false/.test(match)) {
                cls = 'json-boolean';
            } else if (/null/.test(match)) {
                cls = 'json-null';
            } else {
                cls = 'json-number';
            }
            return `<span class="${cls}">${match}</span>`;
        }
    );
}

function autoResize(textarea, maxLines) {
    textarea.style.height = 'auto';

    const computed = window.getComputedStyle(textarea);
    const lineHeight = parseFloat(computed.lineHeight);
    const paddingTop = parseFloat(computed.paddingTop);
    const paddingBottom = parseFloat(computed.paddingBottom);
    const borderTop = parseFloat(computed.borderTopWidth) || 0;
    const borderBottom = parseFloat(computed.borderBottomWidth) || 0;

    const contentHeight = textarea.scrollHeight + borderTop + borderBottom;
    const maxHeight = maxLines * lineHeight + paddingTop + paddingBottom + borderTop + borderBottom;

    if (contentHeight >= maxHeight) {
        textarea.style.height = maxHeight + 'px';
        textarea.style.overflowY = 'auto';
    } else {
        textarea.style.height = contentHeight + 'px';
        textarea.style.overflowY = 'hidden';
    }
}

export function init(textarea, codeEl, initialValue, maxLines) {
    codeEl.innerHTML = tokenize(initialValue) + '\n';
    autoResize(textarea, maxLines);

    textarea.addEventListener('scroll', () => {
        const pre = codeEl.parentElement;
        if (pre) {
            pre.scrollTop = textarea.scrollTop;
            pre.scrollLeft = textarea.scrollLeft;
        }
    });
}

export function updateHighlight(textarea, codeEl, value, maxLines) {
    codeEl.innerHTML = tokenize(value) + '\n';
    autoResize(textarea, maxLines);
}
