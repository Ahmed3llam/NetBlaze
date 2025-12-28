function base64urlToBase64(base64url) {
    let base64 = base64url.replace(/-/g, '+').replace(/_/g, '/');
    while (base64.length % 4) {
        base64 += '=';
    }
    return base64;
}

function toUint8Array(base64url) {
    const base64 = base64urlToBase64(base64url);
    const binaryString = window.atob(base64);
    const len = binaryString.length;
    const bytes = new Uint8Array(len);
    for (let i = 0; i < len; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }
    return bytes;
}

function arrayBufferToBase64URL(buffer) {
    const bytes = new Uint8Array(buffer);
    let binary = '';
    bytes.forEach(b => binary += String.fromCharCode(b));
    const base64 = window.btoa(binary);
    return base64.replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');
}

window.startWebAuthnRegistration = async (options, dotNetHelper) => {
    try {
        const publicKey = {
            challenge: toUint8Array(options.challenge),
            rp: options.rp,
            user: {
                id: toUint8Array(options.user.id),
                name: options.user.name,
                displayName: options.user.displayName
            },
            pubKeyCredParams: options.pubKeyCredParams,
            authenticatorSelection: options.authenticatorSelection,
            attestation: options.attestation || "none",
            extensions: options.extensions
        };

        const credential = await navigator.credentials.create({ publicKey });

        const response = {
            id: credential.id,
            rawId: arrayBufferToBase64URL(credential.rawId),
            type: credential.type,
            response: {
                clientDataJSON: arrayBufferToBase64URL(credential.response.clientDataJSON),
                attestationObject: arrayBufferToBase64URL(credential.response.attestationObject)
            }
        };

        await dotNetHelper.invokeMethodAsync('OnFidoRegistrationComplete', response);
    }
    catch (err) {
        await dotNetHelper.invokeMethodAsync('OnFidoRegistrationComplete', { error: err.message });
    }
};