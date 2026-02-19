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

export function init(textarea, codeEl, initialValue) {
    codeEl.innerHTML = tokenize(initialValue) + '\n';

    textarea.addEventListener('scroll', () => {
        const pre = codeEl.parentElement;
        if (pre) {
            pre.scrollTop = textarea.scrollTop;
            pre.scrollLeft = textarea.scrollLeft;
        }
    });
}

export function updateHighlight(codeEl, value) {
    codeEl.innerHTML = tokenize(value) + '\n';
}
