export function validateFiles(typesAccepted, files) {
    for(const file in files) {
        if(!validateFile(typesAccepted, file)) return false;
    }
}

export function validateFile(typesAccepted, file) {
    return typesAccepted.indexOf(file.type.slice(file.type.indexOf('/') + 1)) !== -1;
}