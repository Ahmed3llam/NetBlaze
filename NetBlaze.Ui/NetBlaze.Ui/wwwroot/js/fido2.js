// Base64URL Encoding/Decoding helpers
function base64urlToBuffer(base64url) {
    const base64 = base64url.replace(/-/g, '+').replace(/_/g, '/');
    const padLen = (4 - (base64.length % 4)) % 4;
    const padded = base64 + '='.repeat(padLen);
    const binary = atob(padded);
    const bytes = new Uint8Array(binary.length);
    for (let i = 0; i < binary.length; i++) {
        bytes[i] = binary.charCodeAt(i);
    }
    return bytes.buffer;
}

function bufferToBase64url(buffer) {
    const bytes = new Uint8Array(buffer);
    let binary = '';
    for (let i = 0; i < bytes.length; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    const base64 = btoa(binary);
    return base64.replace(/\+/g, '-').replace(/\//g, '_').replace(/=/g, '');
}

function showMessage(message, type = 'info') {
    const statusDiv = document.getElementById('status');
    statusDiv.innerHTML = `<div class="alert alert-${type} text-center">${message}</div>`;
    setTimeout(() => {
        statusDiv.innerHTML = '';
    }, 5000);
}

// Register Device
const registerBtn = document.getElementById('registerBtn');
if (registerBtn) {
    registerBtn.addEventListener('click', async () => {
        try {
            registerBtn.disabled = true;
            registerBtn.textContent = '⏳ جاري التسجيل...';

            // Get options from server
            const response = await fetch('/Home/RegisterStart', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            });

            if (!response.ok) {
                const err = await response.json().catch(() => null);
                throw new Error((err && err.message) || 'فشل الحصول على خيارات التسجيل');
            }

            const options = await response.json();

            // --- Enforce platform authenticator + user verification (added lines) ---
            // ensure excludeCredentials exists (empty array prevents offering other credentials)
            options.excludeCredentials = options.excludeCredentials || [];
            // set user verification required
            options.userVerification = "required";
            // ensure authenticatorSelection object exists and set required fields
            options.authenticatorSelection = options.authenticatorSelection || {};
            options.authenticatorSelection.userVerification = "required";
            options.authenticatorSelection.authenticatorAttachment = "platform";
            // also keep explicit field (some clients expect it on top-level)
            options.authenticatorSelection = {
                authenticatorAttachment: "platform",
                userVerification: "required",
                ...options.authenticatorSelection
            };
            // -----------------------------------------------------------------------

            // Convert base64url to buffers
            options.challenge = base64urlToBuffer(options.challenge);
            options.user.id = base64urlToBuffer(options.user.id);

            if (options.excludeCredentials && options.excludeCredentials.length) {
                options.excludeCredentials = options.excludeCredentials.map(cred => ({
                    ...cred,
                    id: base64urlToBuffer(cred.id)
                }));
            }

            // Create credential
            const credential = await navigator.credentials.create({ publicKey: options });

            if (!credential) {
                throw new Error('فشل إنشاء بيانات الاعتماد');
            }

            // Prepare response
            const attestationResponse = {
                id: credential.id,
                rawId: bufferToBase64url(credential.rawId),
                type: credential.type,
                response: {
                    attestationObject: bufferToBase64url(credential.response.attestationObject),
                    clientDataJSON: bufferToBase64url(credential.response.clientDataJSON)
                }
            };

            // Send to server
            const completeResponse = await fetch('/Home/RegisterComplete', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(attestationResponse)
            });

            const result = await completeResponse.json();

            if (result.success) {
                showMessage(result.message, 'success');
                setTimeout(() => location.reload(), 2000);
            } else {
                throw new Error(result.message || 'فشل التسجيل');
            }

        } catch (error) {
            console.error('Registration error:', error);
            showMessage('❌ ' + (error.message || error), 'danger');
            registerBtn.disabled = false;
            registerBtn.textContent = '📱 تسجيل هذا الجهاز';
        }
    });
}

// Attend (Check-in)
const attendBtn = document.getElementById('attendBtn');
if (attendBtn) {
    attendBtn.addEventListener('click', async () => {
        try {
            attendBtn.disabled = true;
            attendBtn.textContent = '⏳ جاري التحقق...';

            // Get options from server
            const response = await fetch('/Home/AttendStart', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            });

            if (!response.ok) {
                const err = await response.json().catch(() => null);
                throw new Error((err && err.message) || 'فشل الحصول على خيارات الحضور');
            }

            const options = await response.json();

            // --- Enforce user verification + platform attachment on assertion options (added lines) ---
            options.userVerification = "required";
            options.authenticatorSelection = options.authenticatorSelection || {};
            options.authenticatorSelection.userVerification = "required";
            options.authenticatorSelection.authenticatorAttachment = "platform";
            // some callers expect authenticatorSelection at top-level as well
            options.authenticatorSelection = {
                authenticatorAttachment: "platform",
                userVerification: "required",
                ...options.authenticatorSelection
            };
            // -----------------------------------------------------------------------

            // Convert base64url to buffers
            options.challenge = base64urlToBuffer(options.challenge);

            if (options.allowCredentials && options.allowCredentials.length) {
                options.allowCredentials = options.allowCredentials.map(cred => ({
                    ...cred,
                    id: base64urlToBuffer(cred.id)
                }));
            } else {
                // ensure allowCredentials exists (empty => rely on userHandle or cause prompt)
                options.allowCredentials = [];
            }

            // Get credential
            const assertion = await navigator.credentials.get({ publicKey: options });

            if (!assertion) {
                throw new Error('فشل الحصول على بيانات الاعتماد');
            }

            // Prepare response
            const assertionResponse = {
                id: assertion.id,
                rawId: bufferToBase64url(assertion.rawId),
                type: assertion.type,
                response: {
                    authenticatorData: bufferToBase64url(assertion.response.authenticatorData),
                    clientDataJSON: bufferToBase64url(assertion.response.clientDataJSON),
                    signature: bufferToBase64url(assertion.response.signature),
                    userHandle: assertion.response.userHandle ? bufferToBase64url(assertion.response.userHandle) : null
                }
            };

            // Send to server
            const completeResponse = await fetch('/Home/AttendComplete', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(assertionResponse)
            });

            const result = await completeResponse.json();

            if (result.success) {
                showMessage(`✅ ${result.message}<br><strong>الوقت: ${result.time}</strong>`, 'success');
                setTimeout(() => location.reload(), 2000);
            } else {
                throw new Error(result.message || 'فشل تسجيل الحضور');
            }

        } catch (error) {
            console.error('Attendance error:', error);
            showMessage('❌ ' + (error.message || error), 'danger');
            attendBtn.disabled = false;
            attendBtn.textContent = '✋ تسجيل الحضور الآن';
        }
    });
}
