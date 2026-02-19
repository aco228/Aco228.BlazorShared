export function autoResize(textarea, maxLines) {
    // Reset to auto so scrollHeight reflects true content height
    textarea.style.height = 'auto';

    const computed = window.getComputedStyle(textarea);
    const lineHeight = parseFloat(computed.lineHeight);
    const paddingTop = parseFloat(computed.paddingTop);
    const paddingBottom = parseFloat(computed.paddingBottom);
    const borderTop = parseFloat(computed.borderTopWidth) || 0;
    const borderBottom = parseFloat(computed.borderBottomWidth) || 0;

    // scrollHeight includes padding but not border; add borders for border-box sizing
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
