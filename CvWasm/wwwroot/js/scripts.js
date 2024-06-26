﻿scrollToInput = () => {
    window.scrollTo({ left: 0, top: document.body.scrollHeight, behavior: 'instant' });
};

downloadFile = (fileName, pdfData) => {
    const linkSource = `data:application/pdf;base64,${pdfData}`;
    const downloadLink = document.createElement("a");
    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
    downloadLink.remove();
}